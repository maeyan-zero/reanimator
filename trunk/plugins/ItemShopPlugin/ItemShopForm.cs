using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ItemShopPlugin
{
  public partial class ItemShopForm : Form
  {
    public ItemShopForm()
    {
      InitializeComponent();

      MessageBox.Show("Each player starts  with a fixed amount of points he can spend on modifying his character.\nTo get new points, the player has to sell stuff/use Palladium");
    }
  }
}
