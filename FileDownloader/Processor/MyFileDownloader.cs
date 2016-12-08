using FileDownloader.Configuration;
using FileDownloader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Processor
{
    class MyFileDownloader : AbstractFileDownloader
    {
        string _downloadLocation;
        string _sourcesPath;
        string[] _sources;


        public IConfigSettings AppSettings { get; }

        
        public override String DownloadLocation
        {
            get { return _downloadLocation; }
            protected set { _downloadLocation = Helper.GetPathFromConfigString(value); }
        }

        public String SourcesPath
        {
            get { return _sourcesPath; }
            protected set { _sourcesPath = Helper.GetPathFromConfigString(value); }
        }

        public AbstractFileDownloader Strategy { get; set; }


        public override String[] Sources
        {
            get { return _sources; }
            protected set { _sources = value; }
        }


        public MyFileDownloader(IConfigSettings config) 
        {
            Log.Debug($"constructor my file downloader with config only");

            AppSettings = config;
            DownloadLocation = AppSettings.FileDownloadPath;
            Directory.CreateDirectory(DownloadLocation);

            if (Sources==null)
                SetSource(AppSettings.SourcesPath);

            SetStrategy(AppSettings.DownloadStrategy);
        }

        
        public MyFileDownloader(IConfigSettings config, String[] sources): this(config)
        {
            Log.Debug($"constructor my file downloader with sources");
            SetSource(sources);
        }

        private void SetSource(string filepath)
        {
            if (filepath==null) throw new ArgumentNullException("Sources Path not defined");

            SourcesPath = filepath;
            Sources = Helper.GetAllLinesFromFile(SourcesPath);
        }


        private void SetSource(string[] sources)
        {
            if (sources == null || sources.Length>0 ) throw new ArgumentNullException("Sources not defined");

            SourcesPath = String.Empty;
            Sources = sources;
        }

        public void SetStrategy(string strategyStr)
        {
            Strategy = DownloadStrategyFactory.GetDownloader(strategyStr, DownloadLocation, Sources);
        }


        public override void ProcessAllSources()
        {
            if (Strategy == null) return;
            Strategy.ProcessAllSources();
        }

    }
}
