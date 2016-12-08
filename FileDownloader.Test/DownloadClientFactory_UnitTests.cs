using FileDownloader.DTO;
using FileDownloader.Service;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Test
{
    [TestFixture]
    class DownloadClientFactory_UnitTests
    {
        //Supported Types
        //FTP, SFTP
        [Test]
        [TestCase("ftp://test:test@localhost/a/b/srfc959.txt","Downloads")]
        [TestCase("sftp://test:test@localhost/srfc959.txt","Downloads")]
        public void GetDownloadClient_SupportedType_ReturnsIDownload(string url, string downloadLocation)
        {
            var fi = new FileConnInfo(url, downloadLocation);
            var client = DownloadClientFactory.GetDownloadClient(fi);

            Assert.IsInstanceOf<IDownloader>(client);
        }

        [Test]
        [TestCase("mysftp://test:test@localhost/srfc959.txt", "Downloads")]
        public void GetDownloadClient_NotSupportedType_Throws(string url, string downloadLocation)
        {
           ActualValueDelegate<object> testDelegate =
            () =>
            {
                var fi = new FileConnInfo(url, downloadLocation);
                var client = DownloadClientFactory.GetDownloadClient(fi);
                return client;
            };

            Assert.That(testDelegate, Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void GetDownloadClient_NullFieConnInfo_Throws()
        {
            ActualValueDelegate<object> testDelegate =
            () =>
            {
                FileConnInfo fi = null;
                var client = DownloadClientFactory.GetDownloadClient(fi);
                return client;
            };

            Assert.That(testDelegate, Throws.TypeOf<ArgumentNullException>());
        }


    }
}
