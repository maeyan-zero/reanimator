using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginInterface;

namespace PluginViewer
{
  public partial class PluginViewerForm : Form
  {
    public PluginViewerForm()
    {
      InitializeComponent();
    }

    public PluginViewerForm(IPlugin[] pluginList) : this()
    {
      PluginInfo(pluginList);
    }

    private void PluginInfo(IPlugin[] pluginList)
    {
      foreach (IPlugin plug in pluginList)
      {
        ListViewItem item = new ListViewItem(plug.Name);
        item.SubItems.Add(plug.Description);
        lv_pluginList.Items.Add(item);
      }
    }
  }
}
