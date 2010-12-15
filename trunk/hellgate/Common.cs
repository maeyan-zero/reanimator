using System.Globalization;

namespace Hellgate
{
    public static class Common
    {
        public static string SaveLaunchCommand = "{0}\\Launcher.exe -load\"{1}\""; // 0 = hgldir, 1 = save file(?)
        public static string InstallPathRegLocation = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Flagship Studios\Hellgate London";
        public static string DefaultHellgatePath = @"C:\Program Files\Flagship Studios\Hellgate London";
        public const string DataPath = @"data\";
        public const string DataCommonPath = @"data_common\";
        public static readonly string[] MPFiles = new[] { "mp_hellgate*" };
        public static readonly string[] SPFiles = new[] { "hellgate*", "sp_hellgate*" };
        public static readonly string[] OriginalDats = new[]
        {
            "hellgate000",
            "hellgate_soundmusic000",
            "hellgate_sound000",
            "hellgate_playershigh000",
            "hellgate_movieslow000",
            "hellgate_movieshigh000",
            "hellgate_movies000",
            "hellgate_localized000",
            "hellgate_graphicshigh000",
            "hellgate_bghigh000",
            "mp_hellgate_1.10.180.3416_1.0.86.4580",
            "mp_hellgate_localized_1.10.180.3416_1.0.86.4580",
            "sp_hellgate_1.10.180.3416_1.18074.70.4256",
            "sp_hellgate_localized_1.10.180.3416_1.18074.70.4256",
            "language",
            "region",
            "mvp"
        };
        public static CultureInfo EnglishUSCulture = new CultureInfo("en-US");
    }
}
