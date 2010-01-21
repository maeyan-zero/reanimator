using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace Reanimator
{
    public class Config
    {
        DataSet configSet;
        string szFileName;

        public string this[string szIndex]
        {
            get
            {
                return GetItemString(ItemArrayIndex(szIndex));
            }

            set
            {
                SetItemString(szIndex, value);
            }
        }

        public Config(string szFile)
        {
            configSet = new DataSet();
            szFileName = szFile;
            FileStream configFile = null;

            try
            {
                configFile = new FileStream(szFileName, FileMode.Open);
                configSet.ReadXml(configFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load config file!\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            finally
            {
                if (configFile != null)
                {
                    configFile.Close();
                }

                try
                {
                    configSet.Tables["config"].RowChanged += new DataRowChangeEventHandler(Config_Changed);
                    configSet.Tables["config"].ColumnChanged += new DataColumnChangeEventHandler(Config_Changed);
                }
                catch (Exception)
                {
                }
            }
        }

        private void Config_Changed(object sender, EventArgs e)
        {
            Save();
        }

        public bool Save()
        {
            FileStream configFile = null;

            try
            {
                configFile = new FileStream(szFileName, FileMode.Create, FileAccess.Write);
                configSet.WriteXml(configFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to save config file!\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            finally
            {
                if (configFile != null)
                {
                    configFile.Close();
                }
            }

            return true;
        }

        private int ItemArrayIndex(string szColumn)
        {
            try
            {
                return configSet.Tables["config"].Columns[szColumn].Ordinal;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private string GetItemString(int index)
        {
            try
            {
                return (string)configSet.Tables["config"].Rows[0].ItemArray[index];
            }
            catch (Exception)
            {
                return "NOT FOUND";
            }
        }

        private bool SetItemString(string szIndex, string szString)
        {
            int index = ItemArrayIndex(szIndex);
            if (index == -1)
            {
                DataColumn dataColumn = configSet.Tables["config"].Columns.Add(szIndex);
                index = dataColumn.Ordinal;
            }

            try
            {
                // why doesn't this work? bleh
                // configSet.Tables["config"].Rows[0].ItemArray[index] = szString;

                object[] itemArray = configSet.Tables["config"].Rows[0].ItemArray;
                itemArray[index] = szString;
                configSet.Tables["config"].Rows[0].ItemArray = itemArray;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
