using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SuperScan.com.github.sekasin.superscan
{
    public sealed class Config : IConfig
    {
        [Description("Is the Plugin enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug mode.")]
        public bool Debug { get; set; } = false;

        [Description("Scan interval in seconds.")]
        public int ScanInterval { get; set; } = 300;

        [Description("Scan duration in seconds.")]
        public int ScanDuration { get; set; } = 20;

        [Description("C.A.S.S.I.E. voicelines. Result will always be suffixed with scan results.")]
        public Dictionary<string, string> CassieLines { get; set; } = new(){
            { "pre-scan", ".G3 Execute area .G6 anomaly scan protocol" },
            { "result", ".G3 Scan complete . containment .G4 breach confirmed . detected . " }
        };
    }
}