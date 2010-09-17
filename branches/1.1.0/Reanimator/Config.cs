using System;
using Microsoft.Win32;

namespace Reanimator
{
    public abstract class Config
    {
        const string Key = @"SOFTWARE\Reanimator";
        static readonly RegistryKey RootKey = Registry.CurrentUser.CreateSubKey(Key);
        static readonly RegistryKey Configkey = RootKey.CreateSubKey("config");

        private static T GetValue<T>(string name, T defaultValue)
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

                return (T)(Object)((int)ret == 0 ? false : true);
            }

            return (T)Configkey.GetValue(name, defaultValue);
        }

        private static void SetValue(string name, Object value)
        {
            if (value.GetType() == typeof(String))
            {
                Configkey.SetValue(name, value, RegistryValueKind.String);
            }
            else if (value.GetType() == typeof(Int32) || value.GetType() == typeof(Int16))
            {
                Configkey.SetValue(name, value, RegistryValueKind.DWord);
            }
            else if (value.GetType() == typeof(Int64))
            {
                Configkey.SetValue(name, value, RegistryValueKind.QWord);
            }
            else if (value.GetType() == typeof(String[]))
            {
                Configkey.SetValue(name, value, RegistryValueKind.MultiString);
            }
            else if (value.GetType() == typeof(Boolean))
            {
                SetValue(name, ((bool)value) ? 1 : 0);
            }

            Configkey.Flush();
        }

        public static string HellgatePath
        {
            get { return GetValue("HellgatePath", @"C:\Program FileList\Flagship Studios\Hellgate London"); }
            set { SetValue("HellgatePath", value); }
        }

        public static string CharacterSavePath
        {
            get { return GetValue("CharacterSavePath", String.Format(@"C:\Users\{0}\Documents\My Games\Hellgate\Save\Singleplayer", Environment.UserName)); }
            set { SetValue("CharacterSavePath", value); }
        }

        public static string DefaultClientPath
        {
            get { return GetValue("DefaultClientPath", @"C:\Program FileList\Flagship Studios\Hellgate London\SP_x32\hellgate_sp_dx9_x32.exe"); }
            set { SetValue("DefaultClientPath", value); }
        }

        public static int ClientWidth
        {
            get { return GetValue("ClientWidth", 800); }
            set { SetValue("ClientWidth", value); }
        }

        public static int ClientHeight
        {
            get { return GetValue("ClientHeight", 600); }
            set { SetValue("ClientHeight", value); }
        }

        public static int PreferredDataType
        {
            // 0 = numeric
            // 1 = hexidecimal
            get { return GetValue("PreferredDataType", 0); }
            set { SetValue("PreferredDataType", value); }
        }

        public static bool EnableExcelRelations
        {
            get { return GetValue("EnableExcelRelations", true); }
            set { SetValue("EnableExcelRelations", value); }
        }

        public static bool LoadIndexFilesOnStartup
        {
            get { return GetValue("LoadIndexFilesOnStartup", true); }
            set { SetValue("LoadIndexFilesOnStartup", value); }
        }

        public static bool LoadExcelFilesOnStartup
        {
            get { return GetValue("LoadExcelFilesOnStartup", true); }
            set { SetValue("LoadExcelFilesOnStartup", value); }
        }
    }
}