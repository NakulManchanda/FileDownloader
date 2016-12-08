using FileDownloader.DTO;
using FileDownloader.Service;
using FileDownloader.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Processor
{
    public abstract class AbstractFileDownloader
    {
        protected ILog Log
        {
            get { return LogManager.GetLogger(GetType()); }
        }

        public virtual string DownloadLocation { get; protected set; }
        public virtual string[] Sources { get; protected set; }


        protected AbstractFileDownloader()
        {

        }

        public AbstractFileDownloader(String downloadLocation, string[] sources)
        {
             Log.Debug($"constructor abstract file downloader");

            if (downloadLocation == null) throw new ArgumentNullException("Download Location can't be null");
            if (sources == null) throw new ArgumentNullException("List of sources can't be null. Nothing to process.");

            DownloadLocation = downloadLocation;
            Sources = sources;
        }

        public abstract void  ProcessAllSources();

        public virtual void ProcessSource(string downloadLocation, string src)
        {
            Log.Info($"Download Start: {src}");

            if (downloadLocation == null) throw new ArgumentNullException("Download Location can't be null");
            if (src == null) throw new ArgumentNullException("Source can't be null. Nothing to process.");

            FileConnInfo fInfo = null;
            try
            {
                //Get New File Info object
                fInfo = new FileConnInfo(src, downloadLocation);

                //Get New Downloader Client
                IDownloader downloader = DownloadClientFactory.GetDownloadClient(fInfo);

                //Process Download
                if (downloader != null)
                {
                    downloader.CreateRequest();
                    downloader.GetResponse();
                }

                Log.Info($"Download Done: {src} ");

            }
            catch (Exception ex)
            {
                //Delete Partially Downloaded File 
                if (fInfo != null)
                    Helper.TryToDeleteFile(fInfo.LocalFilePath);

                Log.Error($"Download Error: {src}", ex);
            }
        }


    }
}
