using FileDownloader.Processor;
using FileDownloader.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public class ParallelRetryFileDownloader : AbstractFileDownloader
    {
        public ParallelRetryFileDownloader(string downloadLocation, string[] sources) : base(downloadLocation, sources)
        {
        }

        public override void ProcessAllSources()
        {
            Parallel.ForEach(Sources, src => {
                                                Retry.Do(() => ProcessSource(DownloadLocation, src), TimeSpan.FromSeconds(1));
                                             }
                            );            
        }
    }
}
