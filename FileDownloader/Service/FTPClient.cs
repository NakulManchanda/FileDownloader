﻿using FileDownloader.DTO;
using log4net;
using System;
using System.IO;
using System.Net;

namespace FileDownloader.Service
{
    public class FTPClient : IDownloader
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(FTPClient));

        private FileConnInfo _fileConnInfo;
        private Boolean UseBinary { get; }
        private Boolean UsePassive { get; }
        private FtpWebRequest _request;



        public FTPClient(FileConnInfo fInfo)
        {
            _fileConnInfo = fInfo;
            if (fInfo.Extension == "zip")
            {
                UseBinary = true;
            }
            else
            {
                UseBinary = false;
            }

            UsePassive = false;
        }

        public void CreateRequest()
        {
            try
            {
                _request = (FtpWebRequest)WebRequest.Create(_fileConnInfo.RemoteUrl);
                _request.Method = WebRequestMethods.Ftp.DownloadFile;
                _request.KeepAlive = true;
                _request.UsePassive = UsePassive;
                _request.UseBinary = UseBinary;
                _request.Credentials = new NetworkCredential(_fileConnInfo.Username, _fileConnInfo.Password);
                //_request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNo‌​Store);

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
                //FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)_request.GetResponse();

                //Stream Open FileStream
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                using (FileStream writer = new FileStream(_fileConnInfo.LocalFilePath, FileMode.Create))
                {

                    long length = response.ContentLength;
                    int bufferSize = 2048;
                    byte[] buffer = new byte[2048];

                    int readCount;
                    readCount = responseStream.Read(buffer, 0, bufferSize);

                    while (readCount > 0)
                    {
                        writer.Write(buffer, 0, readCount);
                        readCount = responseStream.Read(buffer, 0, bufferSize);
                    }
                }
                
                response.Close();
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
