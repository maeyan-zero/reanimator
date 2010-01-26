using System;
using System.Text;
using System.Windows.Forms;

namespace PluginInterface
{
  public interface IPlugin
  {
    string Name { get; set; }
    IPluginHost Host { get; set; }
    MenuStrip HostMenu { get; set; }
    string HGLDirectory { get; set; }
    void InitializePlugIn();
  }

  public interface IPluginHost
  {
    bool Register(IPlugin ipi);
    void ShowMessage(string message);
  }
}
