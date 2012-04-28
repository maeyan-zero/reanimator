using System;
using Microsoft.Win32;

namespace Revival.Common
{
    public abstract class Config
    {
        const String Key = @"SOFTWARE\Reanimator";
        static readonly RegistryKey RootKey = Registry.CurrentUser.CreateSubKey(Key);
        static readonly RegistryKey Configkey = RootKey.CreateSubKey("config");

        private static T _GetValue<T>(string name, T defaultValue)
        {
            if (typeof(T) == typeof(Boolean))
            {
                Object ret;
                if ((bool)(Object)defaultValue)
                {
                    ret = Configkey.GetValue(name, 1);
                }
                else
                {
                    ret = Configkey.GetValue(name, 0);
                }

                return (T)(Object)((int)ret != 0);
            }

            return (T)Configkey.GetValue(name, defaultValue);
        }

        private static void _SetValue(String name, Object value)
        {
            if (value is String)
            {
                Configkey.SetValue(name, value, RegistryValueKind.String);
            }
            else if (value is int || value is short)
            {
                Configkey.SetValue(name, value, RegistryValueKind.DWord);
            }
            else if (value is long)
            {
                Configkey.SetValue(name, value, RegistryValueKind.QWord);
            }
            else if (value.GetType() == typeof(String[]))
            {
                Configkey.SetValue(name, value, RegistryValueKind.MultiString);
            }
            else if (value is bool)
            {
                _SetValue(name, ((bool) value) ? 1 : 0);
            }

            Configkey.Flush();
        }

        public static string HglDir
        {
            get { return _GetValue("HglDir", @"C:\Program Files\Flagship Studios\Hellgate London"); }
            set { _SetValue("HglDir", value); }
        }

        public static string HglDataDir
        {
            get { return _GetValue("HglDir", @"C:\Program Files\Flagship Studios\Hellgate London\data"); }
            set { _SetValue("HglDataDir", value); }
        }

        public static string SaveDir
        {
            get { return _GetValue("SaveDir", String.Format(@"C:\Users\{0}\Documents\My Games\Hellgate\Save\Singleplayer", Environment.UserName)); }
            set { _SetValue("SaveDir", value); }
        }

        public static string BackupDir
        {
            get { return _GetValue("BackupDir", String.Format(@"C:\Users\{0}\Documents\My Games\Hellgate\Save\Singleplayer\Backup", Environment.UserName)); }
            set { _SetValue("BackupDir", value); }
        }

        public static string ScriptDir
        {
            get { return _GetValue("ScriptDir", @"C:\Program Files\Flagship Studios\Hellgate London\Reanimator\Scripts"); }
            set { _SetValue("ScriptDir", value); }
        }

        public static string GameClientPath
        {
            get { return _GetValue("GameClientPath", @"C:\Program Files\Flagship Studios\Hellgate London\SP_x32\hellgate_sp_dx9_x32.exe"); }
            set { _SetValue("GameClientPath", value); }
        }

        public static int ClientHeight
        {
            get { return _GetValue("ClientHeight", 500); }
            set { _SetValue("ClientHeight", value); }
        }

        public static int ClientWidth
        {
            get { return _GetValue("ClientWidth", 700); }
            set { _SetValue("ClientWidth", value); }
        }

        public static string IntPtrCast
        {
            get { return _GetValue("IntPtrCast", "hex"); }
            set { _SetValue("IntPtrCast", value); }
        }

        public static bool GenerateRelations
        {
            get { return _GetValue("GenerateRelations", true); }
            set { _SetValue("GenerateRelations", value); }
        }

        public static bool LoadTCv4DataFiles
        {
            get { return _GetValue("LoadTCv4DataFiles", false); }
            set { _SetValue("LoadTCv4DataFiles", value); }
        }

        public static String LastDirectory
        {
            get { return _GetValue("LastDirectory", ""); }
            set { _SetValue("LastDirectory", value); }
        }

        public static String TxtEditor
        {
            get { return _GetValue("TxtEditor", "notepad.exe"); }
            set { _SetValue("TxtEditor", value); }
        }

        public static String XmlEditor
        {
            get { return _GetValue("XmlEditor", "notepad.exe"); }
            set { _SetValue("XmlEditor", value); }
        }

        public static String CsvEditor
        {
            get { return _GetValue("CsvEditor", "notepad.exe"); }
            set { _SetValue("CsvEditor", value); }
        }

        public static String StringsLanguage
        {
            get { return _GetValue("StringsLanguage", "english"); }
            set { _SetValue("StringsLanguage", value); }
        }
    }
}
