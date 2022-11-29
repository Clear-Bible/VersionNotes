using System;

namespace VersionNotes.Models;

public class ReleaseNote
{
    public ReleaseNoteType NoteType { get; set; } = ReleaseNoteType.Added;
    public string Note { get; set; } = String.Empty;
    public string VersionNumber { get; set; } = String.Empty;
}