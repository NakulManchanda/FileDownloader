using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public class SimpleFileSourceProcessor
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(SimpleFileSourceProcessor));

        public static void ProcessAllSources(string downloadLocation, string[] sources)
        {
            if (downloadLocation == null) throw new ArgumentNullException("Download Location can't be null");
            if (sources == null) throw new ArgumentNullException("List of sources can't be null. Nothing to process.");

            foreach (var src in sources)
            {
                ProcessSource(downloadLocation, src);
            }
        }

        public static void ProcessSource( string downloadLocation, string src)
        {
            Log.Info($"Download Start: {src}");
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
