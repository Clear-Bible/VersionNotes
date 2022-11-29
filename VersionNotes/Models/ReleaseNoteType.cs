using System.ComponentModel;

namespace VersionNotes.Models;

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
    Deferred,
    [Description("Breaking Change")]
    BreakingChange
}