using System;
using System.Text;
using System.Windows.Forms;

namespace PluginInterface
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string HGLDirectory { get; set; }

        Form Parent { set; }
        IPluginHost Host { get; set; }
        MenuStrip HostMenu { get; set; }
        void InitializePlugIn(bool showMessageWhenSuccesfullyLoaded);
    }

    public interface IPluginHost
    {
        bool Register(IPlugin ipi);
        void ShowMessage(string message);
        void ShowMessage(string message, string title);
        IPlugin[] GetPluginList();
    }
}
