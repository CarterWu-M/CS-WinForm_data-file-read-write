using System;
using System.Runtime.InteropServices;
using System.Text;

namespace INI_file_study.Utilities
{
    internal class IniFile
    {
        public string Path { get; set; }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public IniFile(string path)
        {
            Path = path;
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        public string Read(string section, string key)
        {
            var retVal = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, "", retVal, 255, Path);
            if (0 == ret)
            {
                // Check for last error if needed
                int errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"Error reading INI file: {errorCode} (0x{errorCode:X})");
            }
            return retVal.ToString();
        }
    }
}
