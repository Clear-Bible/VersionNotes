using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionNotes.Models
{
    public class UpdateFormat
    {
        public string Version { get; set; } = String.Empty;
        public VersionType VersionType { get; set; } = VersionType.Release;
        public string ReleaseDate { get; set; } = String.Empty;
        public List<ReleaseNote> ReleaseNotes { get; set; } = new();
        public string DownloadLink { get; set; } = String.Empty;
    }

}
