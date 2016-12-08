using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Processor
{
    public enum SupportedDownloadStrategy
    {
        Sequential,
        Parallel,
        ParallelRetry
    };

    public class DownloadStrategyFactory
    {

        public static AbstractFileDownloader GetDownloader(string strategyStr, string downloadLocation, string[] sources)
        {
            AbstractFileDownloader downloader = null;

            SupportedDownloadStrategy ds;
            var isSupported = Enum.TryParse(strategyStr,true,out ds);

            if (!isSupported)
                throw new NotSupportedException($"Not supported {strategyStr} download strategy  ");

            switch(ds)
            {
                case SupportedDownloadStrategy.Sequential: downloader = new SequentialFileDownloader(downloadLocation,sources);
                    break;
                case SupportedDownloadStrategy.Parallel:
                    downloader = new ParallelFileDownloader(downloadLocation, sources);
                    break;
                case SupportedDownloadStrategy.ParallelRetry:
                    downloader = new ParallelRetryFileDownloader(downloadLocation, sources);
                    break;
                default: throw new NotSupportedException($"Not supported {ds.ToString()} download strategy  ");
            }
            
            return downloader;
        }
    }
}
