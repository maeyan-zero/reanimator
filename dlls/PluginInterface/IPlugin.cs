using System;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;

namespace PluginInterface
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }

        Form Parent { set; }
        IPluginHost Host { set; }
        MenuStrip HostMenu { set; }

        void InitializePlugIn(bool showMessageWhenSuccesfullyLoaded);
    }

    public interface IPluginHost
    {
        bool Register(IPlugin ipi);
        void ShowMessage(string message);
        void ShowMessage(string message, string title);

        string GetHGLDirectory();
        string GetRootDir();
        string GetClientDir();
        ExcelTables GetExcelTables();
        IPlugin[] GetPluginList();
    }
}
