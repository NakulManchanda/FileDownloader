namespace FileDownloader.Service
{ 
    public interface IDownloader
    {
        void CreateRequest();
        void GetResponse();
    }
}
