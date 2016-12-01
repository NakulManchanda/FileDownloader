using log4net;
using System;
using System.Configuration;
using System.IO;


namespace FileDownloader
{
    public static class Helper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Helper));


        //Quickly create new file using - fsutil file createnew hugeFile.txt 2000000000
        //Try to delete file given a path
        public static bool TryToDeleteFile(string f)
        {
            try
            {
                // delete the file.
                File.Delete(f);
                return true;
            }
            catch (Exception ex)
            {
                //Unable to delete
                return false;
            }
        }

        public static bool TryGetLocationFromConfig(string setting, out string location)
        {
            location = null;
            try
            {
                location = Helper.GetAppSetting(setting);
                location = Helper.GetPathFromConfigString(location);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error($"Please configure valid location for {setting} in file app.config");
                return false;
            }
        }

        //Utility function to validate path for illegal character and replace template variable AppData
        //Replace AppData\ to read from App Installed directory
        public static string GetPathFromConfigString(string filelocation)
        {
            if (filelocation == null)
            {
                throw new ArgumentNullException($"filelocation location cannot be null");
            }

            filelocation = filelocation.Replace("AppPath\\", AppDomain.CurrentDomain.BaseDirectory);

            if (!ValidateFilePath(filelocation))
                throw new ArgumentException($"Illegal Path: {filelocation}");


            return filelocation;
        }

        //Get App settings
        public static string GetAppSetting(string settingStr)
        {
            string settingVal;
            settingVal = ConfigurationManager.AppSettings[settingStr];

            if (settingVal == null)
                throw new ArgumentNullException($"Missing {settingStr} - not configured");
            
            return settingVal;
        }

        //Validate File Path for illegal characters
        public static bool ValidateFilePath(string filepath)
        {
            try
            {
               //Basic Path Validation
               //File dont have to exist to create a FileInfo Object
               FileInfo fi = new FileInfo(filepath);
            }
            catch (ArgumentException ex)
            {
                Log.Error("Illegal Path", ex);
                return false;
            }
            catch (PathTooLongException ex)
            {
                Log.Error("Download Path too long", ex);
                return false;
            }

            return true;
        }

       

    }
}
