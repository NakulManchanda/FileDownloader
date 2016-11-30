using log4net;
using System;
using Tamir.SharpSsh;

namespace FileDownloader
{
    public class SFTPClient : IDownloader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SFTPClient));

        private FileConnInfo _fileConnInfo;
        private Sftp _sftp;



        public SFTPClient(FileConnInfo fInfo)
        {
            _fileConnInfo = fInfo;
        }

        public void CreateRequest()
        {
            try
            {
                _sftp = new Sftp(_fileConnInfo.Host, _fileConnInfo.Username, _fileConnInfo.Password);
                _sftp.Connect();
            }
            catch (Tamir.SharpSsh.jsch.JSchException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public void GetResponse()
        {
            try
            {
                if (_sftp.Connected)
                {
                    _sftp.Get(_fileConnInfo.RemoteFilePath, _fileConnInfo.LocalFilePath);
                }
            }
            catch (Tamir.SharpSsh.jsch.SftpException ex)
            {
                Log.Error(ex.message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
