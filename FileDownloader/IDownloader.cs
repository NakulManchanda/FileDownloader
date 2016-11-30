namespace FileDownloader
{
    public interface IDownloader
    {
        void CreateRequest();
        void GetResponse();
    }
}
