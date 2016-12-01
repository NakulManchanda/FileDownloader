using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace FileDownloader
{
    class Program
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            Log.Info("Start");

            //AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);


            //Get Download location
            String downloadLocation;
            if(!Helper.TryGetLocationFromConfig("FileDownloadPath",out downloadLocation))
            {
                Log.Info("Example app settings string:");
                Log.Info("<add key =\"FileDownloadPath\" value=\"AppPath\\Downloads\"/>");
                return;
            }

            try
            {
                Directory.CreateDirectory(downloadLocation);
            } catch(Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error("Error when creating folder for download location");
                return;
            }
            Log.Info($"Download Location:{downloadLocation}");
  
            //Get Sources location 
            //1) try reading program argument first
            //2) then from a configured file location - one source per line
            string[] sources;
            if(!TryGetSources("SourcesPath", args, out sources))
            {
                Log.Error("Unable to read sources");
                return;
            }

            try
            {
                //Process Sources
                ProcessSources(downloadLocation, sources);
            } catch (Exception)
            {
                return;
            }
            
            Log.Info("Done");

            return;
        }


        /*
        static void OnProcessExit(object sender, EventArgs e)
        {
            Log.Info("Exit");
        }
        */


        public static void ProcessSources(string downloadLocation,string[] sources)
        {
            try
            {
                foreach (var src in sources)
                {
                    Log.Info($"Download Start: {src}");
                    FileConnInfo fInfo = null;
                    try
                    {
                        fInfo = new FileConnInfo(src, downloadLocation);
                        IDownloader downloader = DownloadClientFactory.GetDownloadClient(fInfo);
                        if (downloader != null)
                        {
                            downloader.CreateRequest();
                            downloader.GetResponse();
                        }

                        Log.Info($"Download Done: {src} ");

                    }
                    catch (Exception ex)
                    {
                        //Delete Partially Downloaded File 
                        if (fInfo != null)
                            Helper.TryToDeleteFile(fInfo.LocalFilePath);

                        Log.Error($"Download Error: {src}", ex);
                    }
                    finally
                    {

                    }
                }
            }catch(Exception)
            {
                Log.Error("Error processing sources");
                throw;
            }
        }


        public static bool TryGetSources(String fileSourceSetting, String[] args, out string[] sources)
        {
            sources = null;
            //Check if any arguments passed - read from args
            if (args.Length > 0)
            {
                try
                {
                    Log.Info($"Source Location: Program Arguments");
                    sources = new string[args.Length];
                    Array.Copy(args, sources, args.Length);
                } catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error("Error reading args");
                    return false;
                }
            }
            else //If no args read from specified source location
            {

                String sourceFile;
                if (!Helper.TryGetLocationFromConfig(fileSourceSetting, out sourceFile))
                {
                    Log.Info("Example app settings string:");
                    Log.Info("<add key =\"SourcePath\" value=\"AppPath\\sourceUrls.txt\" />");
                    return false;
                }


                Log.Info($"Source Location: {sourceFile}");

                try
                {
                    sources = (from line in File.ReadAllLines(sourceFile)
                               select line.Trim()).ToArray();
                }
                catch(Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error($"Error reading sources file");
                    return false;
                }
            }

            if (sources == null)
                return false;

            return true;
        }

    }
}