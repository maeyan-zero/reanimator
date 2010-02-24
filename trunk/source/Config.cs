using System;
using Microsoft.Win32;

namespace Reanimator
{
    public abstract class Config
    {
        const string Key = @"SOFTWARE\Reanimator";
        static readonly RegistryKey RootKey = Registry.CurrentUser.CreateSubKey(Key);
        static readonly RegistryKey Configkey = RootKey.CreateSubKey("config");

        public static T GetValue<T>(string name, T defaultValue)
        {
            return (T)Configkey.GetValue(name, defaultValue);
        }

        public static void SetValue<T>(string name, T value)
        {
            if (typeof(T) == typeof(String))
            {
                Configkey.SetValue(name, value, RegistryValueKind.String);
            }
            else if (typeof(T) == typeof(Int32) || typeof(T) == typeof(Int16))
            {
                Configkey.SetValue(name, value, RegistryValueKind.DWord);
            }
            else if (typeof(T) == typeof(Int64))
            {
                Configkey.SetValue(name, value, RegistryValueKind.QWord);
            }
            else if (typeof(T) == typeof(String[]))
            {
                Configkey.SetValue(name, value, RegistryValueKind.MultiString);
            }
            else if (typeof(T) == typeof(Boolean))
            {
                throw new NotImplementedException("else if (typeof(T) == typeof(Boolean))");
            }

            Configkey.Flush();
        }

        public static string HglDir
        {
            get { return GetValue("hglDir", "C:\\Program Files\\Flagship Studios\\Hellgate London"); }
            set { SetValue("hglDir", value); }
        }

        public static bool DataDirsRootChecked
        {
            get { return GetValue("dataDirsRootChecked", 1) == 1 ? true : false; }
            set { SetValue("dataDirsRootChecked", value ? 1 : 0); }
        }

        public static string DataDirsRoot
        {
            get { return GetValue("dataDirsRoot", "C:\\Program Files\\Flagship Studios\\Hellgate London"); }
            set { SetValue("dataDirsRoot", value); }
        }

        public static int ClientHeight
        {
            get { return GetValue("clientHeight", 500); }
            set { SetValue("clientHeight", value); }
        }

        public static int ClientWidth
        {
            get { return GetValue("clientWidth", 700); }
            set { SetValue("clientWidth", value); }
        }

        public static string GameClientPath
        {
            get { return GetValue("gameClientPath", "C:\\Program Files\\Flagship Studios\\Hellgate London\\SP_x64\\hellgate_sp_dx9_x64.exe"); }
            set { SetValue("gameClientPath", value); }
        }

        public static string CacheFilePath
        {
            get { return GetValue("cacheFilePath", @"cache\dataSet.dat"); }
            set { SetValue("cacheFilePath", value); }
        }

        public static bool datUnpacked
        {
            get { return GetValue<bool>("datUnpacked", false); }
            set { SetValue<bool>("datUnpacked", value); }
        }
    }
}
