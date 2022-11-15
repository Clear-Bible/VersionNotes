using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using VersionNotes.Models;
using VersionNotes.Views;

namespace VersionNotes.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region Member Variables      

        private ShellView View;

        #endregion //Member Variables


        #region Public Properties

        #endregion //Public Properties


        #region Observable Properties

        private string _message = String.Empty;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }



        private ObservableCollection<ReleaseNote> _releaseNotes = new();
        public ObservableCollection<ReleaseNote> ReleaseNotes
        {
            get => _releaseNotes;
            set
            {
                _releaseNotes = value;
                NotifyOfPropertyChange(() => ReleaseNotes);
            }
        }

        private string _versionNum = String.Empty;
        public string VersionNum
        {
            get => _versionNum;
            set
            {
                _versionNum = value;
                NotifyOfPropertyChange(() => VersionNum);

                if (Message != "" && VersionNum != "")
                {
                    Message = "";
                }
            }
        }

        private string _releaseDate = DateTime.Now.Date.ToString();
        public string ReleaseDate
        {
            get => _releaseDate;
            set
            {
                _releaseDate = value;
                NotifyOfPropertyChange(() => ReleaseDate);
            }
        }

        private ReleaseNoteType _addNoteReleaseType;
        public ReleaseNoteType AddNoteReleaseType
        {
            get => _addNoteReleaseType;
            set
            {
                _addNoteReleaseType = value;
                NotifyOfPropertyChange(() => AddNoteReleaseType);
            }
        }

        private string _addNoteText;
        public string AddNoteText
        {
            get => _addNoteText;
            set
            {
                _addNoteText = value;
                NotifyOfPropertyChange(() => AddNoteText);
            }
        }

        private string _downLoadLink = "https://drive.google.com/drive/folders/1x4-4vjZS4AM6HNehp6XX66TZP68Q68f5";
        public string DownLoadLink
        {
            get => _downLoadLink;
            set
            {
                _downLoadLink = value;
                NotifyOfPropertyChange(() => DownLoadLink);
            }
        }

        public string WindowTitle { get; set; } = "VersionNotes App";

        #endregion //Observable Properties


        #region Constructor

        public ShellViewModel()
        {
            Version thisVersion = Assembly.GetEntryAssembly().GetName().Version;
            WindowTitle += $" - Version: {thisVersion.ToString()}";
        }

        protected override void OnViewLoaded(object view)
        {

            if (view is not null)
            {
                View = (ShellView)view;
            }

            base.OnViewLoaded(view);
        }

        #endregion //Constructor


        #region Methods


        public void Process()
        {
            var updateFormat = new ObservableCollection<ReleaseNote>();

            var rtb = View.RichTextBox;

            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string[] rtbLines = textRange.Text.Split(Environment.NewLine);

            foreach (var line in rtbLines)
            {
                if (line.Contains("Version:"))
                {
                    try
                    {
                        // pull out the version number
                        Regex regex = new Regex("\\b(?:[0-9]{1,3}\\.){3}[0-9]{1,3}\\b");
                        var match = regex.Match(line);
                        if (match.Success)
                        {
                            VersionNum = match.Value;
                        }
                        else
                        {
                            VersionNum = "";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if (line.StartsWith("Added"))
                {
                    updateFormat.Add(ProcessLine(line, ReleaseNoteType.Added));
                }
                else if (line.StartsWith("Updated"))
                {
                    updateFormat.Add(ProcessLine(line, ReleaseNoteType.Updated));
                }
                else if (line.StartsWith("Bug Fix"))
                {
                    updateFormat.Add(ProcessLine(line, ReleaseNoteType.BugFix));
                }
                else if (line.StartsWith("Deferred"))
                {
                    updateFormat.Add(ProcessLine(line, ReleaseNoteType.Deferred));
                }
                else if (line.StartsWith("Changed"))
                {
                    updateFormat.Add(ProcessLine(line, ReleaseNoteType.Changed));
                }
                else if (line.StartsWith("Breaking"))
                {
                    updateFormat.Add(ProcessLine(line, ReleaseNoteType.BreakingChange));
                }
            }

            ReleaseNotes.Clear();
            ReleaseNotes = updateFormat;

        }

        private ReleaseNote ProcessLine(string line, ReleaseNoteType noteType)
        {
            ReleaseNote note = new();
            note.NoteType = noteType;

            try
            {
                // parse out the first word
                var str = line.Substring(line.IndexOf(' ')).Trim();
                if (str.StartsWith("-"))
                {
                    str = str.Substring(1).Trim();
                }

                note.Note = str;
            }
            catch (Exception e)
            {
                note.Note = "PARSING ERROR";
            }

            return note;
        }


        public void Cancel()
        {
            this.TryCloseAsync();
        }

        public void ExportReleaseNotes(string Path)
        {
            if (VersionNum == "")
            {
                Message = "ERROR: Add in a Version Number";
                return;
            }


            UpdateFormat updateFormat = new();
            updateFormat.Version = VersionNum;
            updateFormat.ReleaseDate = ReleaseDate;
            updateFormat.DownloadLink = DownLoadLink;
            updateFormat.ReleaseNotes = new List<ReleaseNote>(ReleaseNotes);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(updateFormat, options);
            File.WriteAllText(Path, jsonString);
        }

        public void Delete(ReleaseNote e)
        {
            if (e is not null)
            {
                ReleaseNotes.Remove(e);
            }
        }

        public void AddRecord()
        {
            ReleaseNotes.Add(new ReleaseNote
            {
                NoteType = AddNoteReleaseType,
                Note = AddNoteText
            });
        }


        public void Clear()
        {
            var rtb = View.RichTextBox;
            rtb.Document.Blocks.Clear();

            DownLoadLink = "";
            VersionNum = "";
            ReleaseNotes.Clear();
        }

        public void ImportReleaseNotes(string fileName)
        {
            var json = File.ReadAllText(fileName);

            try
            {
                var updateFormat = JsonSerializer.Deserialize<UpdateFormat>(json);

                ReleaseDate = updateFormat.ReleaseDate;
                DownLoadLink = updateFormat.DownloadLink;
                VersionNum = updateFormat.Version;
                ReleaseNotes = new ObservableCollection<ReleaseNote>(updateFormat.ReleaseNotes);
            }
            catch (Exception e)
            {
                DownLoadLink = "";
                VersionNum = "";
                ReleaseNotes.Clear();
            }

        }

        #endregion // Methods
    }
}
