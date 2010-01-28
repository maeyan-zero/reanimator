using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;

namespace PluginInterface
{
  public abstract class GenericPlugin : IPlugin
  {
    protected string name;
    protected string description;

    protected Form parent;
    protected IPluginHost host;
    protected MenuStrip hostMenu;

    public GenericPlugin()
    {
      name = "Generic Plugin";
      description = "A generic plugin structure to derive from.";
    }

    public string Name
    {
      get { return name; }
    }

    public string Description
    {
      get { return description; }
    }

    public Form Parent
    {
      set { parent = value; }
    }

    public IPluginHost Host
    {
      set
      {
        this.host = value;
        host.Register(this);
      }
    }

    public MenuStrip HostMenu
    {
      set { hostMenu = value; }
    }

    public virtual void InitializePlugIn(bool showMessageWhenSuccesfullyLoaded)
    {
      if (showMessageWhenSuccesfullyLoaded)
      {
        host.ShowMessage("Plugin " + name + "successfully initialized!");
      }
    }
  }
}
