using FileDownloader.Test.TestUtils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileDownloader.Test
{
    [TestFixture]
    class NetworkSimulation_Tests
    {
        private static string ExecutingDirPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string DownloadPath = Path.Combine(ExecutingDirPath, "Downloads");

        [Test]
        public void Network_BigFileMiddleFailure_NoFileOnDisk()
        {


            Action enableDisableNetwork = () =>
                                            {
                                                Thread.Sleep(10000);
                                                TestUtils.Helper.EnableOrDisableNetwork(0, "Disable");
                                                Thread.Sleep(10000);
                                            };


            string downloadPath = Path.Combine(DownloadPath, "NetworkFailure");

            Action DownloadBigFile = () => NetworkSimulation_Tests.DownloadBigFile(downloadPath);


            var t1 = Task.Run(enableDisableNetwork);
            var t2 = Task.Run(DownloadBigFile);


            Task.WaitAll(t1,t2);


            TestUtils.Helper.EnableOrDisableNetwork(0, "Enable");

            DirectoryInfo dir = new DirectoryInfo(downloadPath);

            Assert.AreEqual(0, dir.GetFiles().Length);
        }

       



        private static void DownloadBigFile(string downloadLocation)
        {
            //Create file on FTP folder
            string hugeFileRemotePath = "c:/ftp/test/hugeFile.txt";
            File.Delete(hugeFileRemotePath);
            TestUtils.Helper.CreateHugeFile(hugeFileRemotePath, 2048L * 1024*1024);

            string sourceUrl = "ftp://test:test@localhost/hugeFile.txt";

            try { Directory.Delete(downloadLocation, true); } catch { };
            try { Directory.CreateDirectory(downloadLocation); } catch { };

            //Get File From source
            FileDownloader.SimpleFileSourceProcessor.ProcessSource(downloadLocation, sourceUrl);

        }
    }
}
