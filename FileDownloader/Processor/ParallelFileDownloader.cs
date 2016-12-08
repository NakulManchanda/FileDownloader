using FileDownloader.Processor;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public class ParallelFileDownloader : AbstractFileDownloader
    {
        public ParallelFileDownloader(string downloadLocation, string[] sources) : base(downloadLocation, sources)
        {
        }

        public override void ProcessAllSources()
        {
            Parallel.ForEach(Sources, src => { ProcessSource(DownloadLocation, src); });            
        }
    }
}
