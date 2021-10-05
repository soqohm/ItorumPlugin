using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Itorum
{
    public static class Logs
    {
        public static void WriteLog(this string[] productInfo, string path)
        {
            if (productInfo.FirstOrDefault() == null) 
                return;
            if (File.Exists(path))
                File.Delete(path);
            File.AppendAllLines(path, productInfo);

        }

        public static void OpenLog(string path)
        {
            if (File.Exists(path))
                Process.Start("notepad.exe", path);
        }
    }
}