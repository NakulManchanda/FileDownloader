using FileDownloader.DTO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Test
{
    [TestFixture]
    class FileConnInfo_UnitTests
    {
        private const string testDownloadLoc = "Downloads"; 

        [Test]
        [TestCase("myftp://test:test@localhost/a/b/srfc959.txt", testDownloadLoc)]
        public void CustomSchema_FileConnObjProps_ParseAsNormal(String remoteUri, String baseDownloadLoc)
        {
            FileConnInfo fi = new FileConnInfo(remoteUri, baseDownloadLoc);
            string uniqueId = fi.UniqueId;

            Assert.IsNotEmpty(uniqueId);
            Assert.NotNull(uniqueId);

            //Get Combine Strategy into its own class
            string filename = uniqueId + "_" + "srfc959.txt";

            Assert.AreEqual(new Uri(remoteUri), fi.Url);
            Assert.AreEqual(remoteUri, fi.RemoteUrl);
            Assert.AreEqual("myftp", fi.Scheme);
            Assert.AreEqual("localhost",fi.Host);
            Assert.AreEqual(-1, fi.Port);
            Assert.AreEqual("/a/b/srfc959.txt", fi.RemoteFilePath);
            Assert.AreEqual("srfc959.txt", fi.RemoteFilename);


            Assert.AreEqual(baseDownloadLoc, fi.LocalBasePath);

            //TODO: Get Filename strategy out of FileConn class
            Assert.AreEqual(Path.Combine(baseDownloadLoc, filename), fi.LocalFilePath);
            Assert.AreEqual("test", fi.Username);
            Assert.AreEqual("test", fi.Password);
            Assert.AreEqual(filename, fi.Filename);
            Assert.AreEqual(".txt",fi.Extension);
            
        }


        [Test]
        [TestCase("test@localhost/a/b/srfc959.txt", testDownloadLoc)]
        [TestCase("test:test@localhost/a/b/srfc959.txt", testDownloadLoc)]
        public void Constructor_IncompleteUrls_Throws(String remoteUri, String baseDownloadLoc)
        {

            ActualValueDelegate<object> testDelegate =
            () =>
            {
                return new FileConnInfo(remoteUri, baseDownloadLoc);
            };

            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }


        [Test]
        [TestCase("myftp://test:test@localhost/a/b/srfc9*59.txt", testDownloadLoc)]
        [TestCase("myftp://test:test@localhost/a/b/srfc9:59.txt", testDownloadLoc)]
        public void Constructor_InvalidIncompleteUrls_Throws(String remoteUri, String baseDownloadLoc)
        {

            ActualValueDelegate<object> testDelegate =
            () =>
            {
                return new FileConnInfo(remoteUri, baseDownloadLoc);
            };

            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }


        [Test]
        [TestCase("myftp://test:test@localhost/a/b/srfc9*59.txt", "Down<Loads")]
        [TestCase("myftp://test:test@localhost/a/b/srfc9*59.txt", "Down|Loads")]
        public void Constructor_IllegalDownloadPath_Throws(String remoteUri, String baseDownloadLoc)
        {

            ActualValueDelegate<object> testDelegate =
            () =>
            {
                return new FileConnInfo(remoteUri, baseDownloadLoc);
            };

            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }
    }
}
