using FileDownloader.Processor;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public class SequentialFileDownloader : AbstractFileDownloader
    {
        public SequentialFileDownloader(string downloadLocation, string[] sources) : base(downloadLocation, sources)
        {
        }

        public override void ProcessAllSources()
        {

            foreach (var src in Sources)
            {
                ProcessSource(DownloadLocation, src);
            }
        }
    }
}
