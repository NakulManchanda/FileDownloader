using FileDownloader.Utils;

namespace FileDownloader.Configuration
{
    public class ConfigSettings : IConfigSettings
    {
        public string FileDownloadPath => Helper.GetAppSetting("FileDownloadPath");  
        public string SourcesPath => Helper.GetAppSetting("SourcesPath");
        public string DownloadStrategy => Helper.GetAppSetting("DownloadStrategy");

    }
}
