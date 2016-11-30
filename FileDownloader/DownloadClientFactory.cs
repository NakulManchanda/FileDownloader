using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public class DownloadClientFactory
    {
        //Get Download client based on schema
        //It returns concrete class based on schema
        //This function should be extended whenever we add concrete class supporting new schema
        public static IDownloader GetDownloadClient(FileConnInfo fileConnInfo)
        {
            IDownloader client = null;

            if (fileConnInfo == null) return client;

            if (fileConnInfo.Scheme == "ftp")
            {
                client = (IDownloader)new FTPClient(fileConnInfo);
            }
            else if (fileConnInfo.Scheme == "sftp")
            {
                client = (IDownloader)new SFTPClient(fileConnInfo);
            }

            return client;
        }
    }
}
