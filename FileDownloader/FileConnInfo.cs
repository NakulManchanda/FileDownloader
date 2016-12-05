using System;
using System.IO;
using System.Linq;

namespace FileDownloader
{
    public class FileConnInfo
    {
        public String UniqueId { get; }
        public Uri Url { get; }

        //URI Derived Info
        public String RemoteUrl { get { return Url.OriginalString; } }

        public String Scheme { get { return Url.Scheme; } }
        public String Host { get { return Url.Host; } }
        public int Port { get { return Url.Port; } }

        public String Username { get; private set; }
        public String Password { get; private set; }

        public String RemoteFilePath { get; }
        public String RemoteFilename { get; }
        public String Extension { get; }

        //Local Paths
        public String LocalBasePath { get; }
        public String LocalFilePath { get; }
        public String Filename { get; }
        

        public FileConnInfo(String remoteUrl, String localBasePath)
        {
            try
            {

                LocalBasePath = localBasePath;
                UniqueId = Guid.NewGuid().ToString();

                Uri resultUri;
                if (!Uri.TryCreate(remoteUrl, UriKind.Absolute, out resultUri))
                {
                    throw new ArgumentException($"Error initializing conn info for {remoteUrl}");
                }

                Url = resultUri;
                RemoteFilePath = Url.AbsolutePath;
                RemoteFilename = Url.Segments.Last();
                Filename = GetFileName(Url, UniqueId);
                Extension = Path.GetExtension(Filename);
                LocalFilePath = Path.Combine(LocalBasePath,Filename);
               
                Username = "";
                Password = "";
                SetUserInfo(Url.UserInfo);
            }
            catch
            {
                throw new ArgumentException($"Error initializing conn info for {remoteUrl} ");
            }

        }

        private String GetFileName(Uri url,String uniqueId)
        {
            if(url==null || uniqueId==null)
            {
                throw new ArgumentNullException("uri or uniqueId is null");
            }

            return uniqueId + "_" + url.Segments.Last();
        }

        private void SetUserInfo(String userInfo)
        {
            if (userInfo == "") return;
            var userinfo = userInfo.Split(':');

            if (userinfo.Length >= 1)
            {
                Username = userinfo[0];
            }

            if (userinfo.Length >= 2)
            {
                Password = userinfo[1];
            }
        }

    }
}
