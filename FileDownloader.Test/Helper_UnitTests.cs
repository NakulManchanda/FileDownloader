using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.IO;

namespace FileDownloader.Test
{
    [TestFixture]
    public class Helper_UnitTests
    {

        private static string ExecutingDirPath = AppDomain.CurrentDomain.BaseDirectory;

        [Test]
        public void TryToDeleteFile_Null_ReturnFalse_NoExceptionThrown()
        {
            String filename = null;
            Assert.AreEqual(false, Helper.TryToDeleteFile(filename));
        }

        [Test]
        public void TryToDeleteFile_Blank_ReturnFalse_NoExceptionThrown()
        {
            String filename = "";
            Assert.AreEqual(false, Helper.TryToDeleteFile(filename));
        }

        //Invalid Filename characters - \/:*?"<>|
        [Test]
        [TestCase("fi|lename.txt")]
        [TestCase("fi>lename.txt")]
        [TestCase("fi<lename.txt")]
        [TestCase("fi\"lename.txt")]
        [TestCase("fi?lename.txt")]
        [TestCase("fi*lename.txt")]
        [TestCase("fi:lename.txt")]
        [TestCase("fi/lename.txt")]
        [TestCase("fi\\\\lename.txt")]
        public void TryToDeleteFile_IllegalChar_ReturnFalse_NoExceptionThrown(string filename)
        {
            
            Assert.AreEqual(false, Helper.TryToDeleteFile(filename));
        }

        [Test]
        [TestCase("MyTestFile.txt")]
        public void TryToDeleteFile_ExistingFile_ReturnTrueOnFileListing(string filename)
        {
            string filepath = Path.Combine(ExecutingDirPath, filename);

            Assert.AreEqual(false, File.Exists(filepath));
            using (File.Create(filepath)) { };
            
            //System.GC.Collect();
            //System.GC.WaitForPendingFinalizers();

            Assert.AreEqual(true,File.Exists(filepath));
            Assert.AreEqual(true, Helper.TryToDeleteFile(filepath));
            Assert.AreEqual(false, File.Exists(filepath));
        }


        [Test]
        [TestCase("filename.txt")]
        public void ValidateFilePath_SimpleValidFileName_ReturnTrueNoException(string filepath)
        {
            Assert.AreEqual(true, Helper.ValidateFilePath(filepath));
        }


        [Test]
        [TestCase("Dir\\filename.txt")]
        public void ValidateFilePath_WithDirFileName_ReturnTrueNoException(string filepath)
        {
            Assert.AreEqual(true, Helper.ValidateFilePath(filepath));
        }

        [Test]
        [TestCase("filena?me.txt")]
        public void ValidateFilePath_IllegalCharFileName_ReturnFalseNoException(string filepath)
        {
            Assert.AreEqual(false, Helper.ValidateFilePath(filepath));
        }


        [Test]
        [TestCase("NoExistAppConfig")]
        public void GetAppSetting_NoExist_Throws(string settingStr)
        {
            ActualValueDelegate<object> testDelegate = () => Helper.GetAppSetting(settingStr);
            Assert.That(testDelegate, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCase("ValidSettingValidPath")]
        public void GetAppSetting_Exist_ReturnCorrectValueFromConfig(string settingStr)
        {
            Assert.AreEqual("AppPath\\sourceUrls.txt", Helper.GetAppSetting(settingStr));
        }


        

        [Test]
        [TestCase("Dir\\filename.txt")]
        public void GetPathFromConfigString_NoReplacementValidString_ReturnSameValueBack(string settingValue)
        {
            Assert.AreEqual(settingValue, Helper.GetPathFromConfigString(settingValue));
        }


        [Test]
        [TestCase("AppPath\\sourceUrls.txt", "sourceUrls.txt")]
        public void GetPathFromConfigString_AppDataReplacementValidString_ReturnValueAfterReplacement(string settingValue,string segment)
        {
            Assert.AreEqual(Path.Combine(ExecutingDirPath,segment), Helper.GetPathFromConfigString(settingValue));
        }

        [Test]
        [TestCase("AppPath\\sourceUrl?s.txt")]
        [TestCase("sourceUrl?s.txt")]
        public void GetPathFromConfigString_IllegalPath_Throws(string settingValue)
        {
            ActualValueDelegate<object> testDelegate = () => Helper.GetPathFromConfigString(settingValue);
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [TestCase("")]
        public void GetPathFromConfigString_Blank_Throws(string settingValue)
        {
            ActualValueDelegate<object> testDelegate = () => Helper.GetPathFromConfigString(settingValue);
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [TestCase(null)]
        public void GetPathFromConfigString_Null_Throws(string settingValue)
        {
            ActualValueDelegate<object> testDelegate = () => Helper.GetPathFromConfigString(settingValue);
            Assert.That(testDelegate, Throws.TypeOf<ArgumentNullException>());
        }



        [Test]
        [TestCase("NoExistAppConfig")]
        public void TryGetLocationFromConfig_NotExistsSetting_ReturnsFalseNoException(string settingStr)
        {
            string location;
            Assert.AreEqual(false, Helper.TryGetLocationFromConfig(settingStr, out location));
        }

        [Test]
        [TestCase("ValidSettingValidPath")]
        public void TryGetLocationFromConfig_ExistsSetting_ReturnsTrueNoException(string settingStr)
        {
            string location;
            Assert.AreEqual(true, Helper.TryGetLocationFromConfig(settingStr, out location));
        }

        [Test]
        [TestCase("ValidSettingInValidIllegalCharPath")]
        public void TryGetLocationFromConfig_ExistsSettingButIllegalFilePath_ReturnsFalseNoException(string settingStr)
        {
            string location;
            Assert.AreEqual(false, Helper.TryGetLocationFromConfig(settingStr, out location));
        }

       

    }
}
