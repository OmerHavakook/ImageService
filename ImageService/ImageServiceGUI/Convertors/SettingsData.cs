using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Convertors
{
    public class SettingsData
    {
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }
        public List<string> Handlers { get; set; }

        public SettingsData(string outputDir, string sourceName, string logName, int thumbnailSize, List<string> handlers)
        {
            this.OutputDir = outputDir;
            this.SourceName = sourceName;
            this.LogName = logName;
            this.ThumbnailSize = thumbnailSize;
            this.Handlers = handlers;
        }

    }
}
