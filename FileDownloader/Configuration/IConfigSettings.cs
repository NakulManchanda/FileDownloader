using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Configuration
{
    public interface IConfigSettings
    {
        string FileDownloadPath { get; }

        string SourcesPath { get; }

        string DownloadStrategy { get; }

    }
}
