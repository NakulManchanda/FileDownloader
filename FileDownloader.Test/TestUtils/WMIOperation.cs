using System.Management;

namespace FileDownloader.Test.TestUtils
{
    class WMIOperation
    {
        public static ManagementObjectCollection WMIQuery(string strwQuery)
        {
            ObjectQuery oQuery = new ObjectQuery(strwQuery);
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
            ManagementObjectCollection oReturnCollection = oSearcher.Get();
            return oReturnCollection;
        }
    }
}
