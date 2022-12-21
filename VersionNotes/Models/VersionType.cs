using System.ComponentModel;

namespace VersionNotes.Models;

public enum VersionType
{
    [Description("Release")]
    Release,
    [Description("Prerelease")]
    Prerelease
}