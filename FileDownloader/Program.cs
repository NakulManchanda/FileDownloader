using FileDownloader.Configuration;
using FileDownloader.Processor;
using FileDownloader.Utils;
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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);

            //Get Configuration
            IConfigSettings configs=new ConfigSettings();

            //Get Downloader
            AbstractFileDownloader fd;
            if (args.Length > 0)
            {
                //get Sources from args
                fd = new MyFileDownloader(configs, args);
            }
            else
            {
                //get sources from filename in configuration
                fd = new MyFileDownloader(configs);
            }

            //Process all downloads
            fd.ProcessAllSources();
           
            Log.Info("Done");

            return;
        }

        static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }

    }
}