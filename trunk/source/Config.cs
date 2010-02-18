using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Reanimator
{
    public abstract class Config
    {
        static string key = @"SOFTWARE\Reanimator";
        static RegistryKey rootKey = Registry.CurrentUser.CreateSubKey(key);
        static RegistryKey configkey = rootKey.CreateSubKey("config");

        public static T GetValue<T>(string name, T defaultValue)
        {
            return (T)configkey.GetValue(name, defaultValue);
        }

        public static void SetValue<T>(string name, T value)
        {
            if (typeof(T) == typeof(String))
            {
                configkey.SetValue(name, value, RegistryValueKind.String);
            }
            else if (typeof(T) == typeof(Int32) || typeof(T) == typeof(Int16))
            {
                configkey.SetValue(name, value, RegistryValueKind.DWord);
            }
            else if (typeof(T) == typeof(Int64))
            {
                configkey.SetValue(name, value, RegistryValueKind.QWord);
            }
            else if (typeof(T) == typeof(String[]))
            {
                configkey.SetValue(name, value, RegistryValueKind.MultiString);
            }
            else if (typeof(T) == typeof(Boolean))
            {
                throw new NotImplementedException("else if (typeof(T) == typeof(Boolean))");
            }

            configkey.Flush();
        }

        public static string hglDir
        {
            get { return GetValue<string>("hglDir", "C:\\Program Files\\Flagship Studios\\Hellgate London"); }
            set { SetValue<string>("hglDir", value); }
        }

        public static bool dataDirsRootChecked
        {
            get { return GetValue<int>("dataDirsRootChecked", 1) == 1 ? true : false; }
            set { SetValue<int>("dataDirsRootChecked", value ? 1 : 0); }
        }

        public static string dataDirsRoot
        {
            get { return GetValue<string>("dataDirsRoot", "C:\\Program Files\\Flagship Studios\\Hellgate London"); }
            set { SetValue<string>("dataDirsRoot", value); }
        }

        public static int clientHeight
        {
            get { return GetValue<int>("clientHeight", 500); }
            set { SetValue<int>("clientHeight", value); }
        }

        public static int clientWidth
        {
            get { return GetValue<int>("clientWidth", 700); }
            set { SetValue<int>("clientWidth", value); }
        }

        public static string gameClientPath
        {
            get { return GetValue<String>("gameClientPath", "C:\\Program Files\\Flagship Studios\\Hellgate London\\SP_x64\\hellgate_sp_dx9_x64.exe"); }
            set { SetValue<string>("gameClientPath", value); }
        }

        public static string cacheFilePath
        {
            get { return GetValue<String>("cacheFilePath", @"cache\dataSet.dat"); }
            set { SetValue<string>("cacheFilePath", value); }
        }
    }
}
