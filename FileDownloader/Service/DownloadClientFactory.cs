using FileDownloader.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Service
{

    public enum SupportedDownloadSchema
    {
        FTP,
        SFTP
    };


    public class DownloadClientFactory
    {
        
        //Get Download client based on schema
        //It returns concrete class based on schema
        //This function should be extended whenever we add concrete class supporting new schema
        public static IDownloader GetDownloadClient(FileConnInfo fileConnInfo)
        {
            IDownloader client = null;

            if (fileConnInfo == null) throw new ArgumentNullException($"Please provide non-null FileConnInfo object");

            SupportedDownloadSchema dc;
            var isSupported = Enum.TryParse(fileConnInfo.Scheme, true, out dc);

            if (!isSupported)
                throw new NotSupportedException($"Not supported {fileConnInfo.Scheme} download schema  ");

            switch (dc)
            {
                case SupportedDownloadSchema.FTP: client = (IDownloader)new FTPClient(fileConnInfo);
                            break;
                case SupportedDownloadSchema.SFTP: client = (IDownloader)new SFTPClient(fileConnInfo);
                             break;
                default: throw new NotSupportedException($"Scheme {dc.ToString()} Not Supported");
            }
                
            
            return client;
        }
    }
}
