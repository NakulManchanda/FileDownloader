using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader.Test.TestUtils
{
    class Helper
    {
        public static void CreateHugeFile(string filePath, long fileSize)
        {
            FileStream fs = new FileStream(filePath, FileMode.CreateNew);
            fs.Seek(fileSize, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();
        }


        public static void EnableOrDisableNetwork(int deviceId, string operation)
        {
            var na = NetworkAdapter.GetNetworkAdapterByDeviceId(deviceId);
            na.EnableOrDisableNetworkAdapter(operation);
        }

    }
}
