using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionNotes.Models
{
    public enum ReleaseNoteType
    {
        [Description("Added")]
        Added,
        [Description("Bug Fix")]
        BugFix,
        [Description("Changed")]
        Changed,
        [Description("Updated")]
        Updated,
        [Description("Deferred")]
        Deferred
    }

    public class UpdateFormat
    {
        public string Version { get; set; } = String.Empty;
        public string ReleaseDate { get; set; } = String.Empty;
        public List<ReleaseNote> ReleaseNotes { get; set; } = new();
        public string DownloadLink { get; set; } = String.Empty;
    }

    public class ReleaseNote
    {
        public ReleaseNoteType NoteType { get; set; } = ReleaseNoteType.Added;
        public string Note { get; set; } = String.Empty;
    }
}
