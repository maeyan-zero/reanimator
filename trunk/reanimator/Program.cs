using System;
using System.Threading;
using System.Windows.Forms;

namespace Reanimator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += ExceptionHandler.ApplicationThreadException;
                Application.ThreadException += ExceptionHandler.ApplicationThreadException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Forms.Reanimator());
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
    }

    static class ExceptionHandler
    {
        private const String ErrorTitle = "Unhandled Exception";

        private const String FatalErrorTitle = "Fatal Error";
        private const String FatalErrorMessage = "An unrecoverable error has occured!\nExiting...";

        public static void ApplicationThreadException(Object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        public static void ApplicationThreadException(Object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        public static void HandleException(Exception ex)
        {
            try
            {
                String errorMessage = "Unhandled Exception:\n\n" + ex.Message + "\n\n" + ex.GetType() + "\n\nStack Trace:\n" + ex.StackTrace;

                DialogResult result = MessageBox.Show(errorMessage, ErrorTitle, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);

                if (result == DialogResult.Abort) Application.Exit();
            }
            catch
            {
                try
                {
                    MessageBox.Show(FatalErrorMessage, FatalErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }
    }
}
