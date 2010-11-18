using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Reanimator.Forms
{
    public partial class PatchForm : Form
    {
        Patches _patches;
        Process _process;

        public PatchForm()
        {
            InitializeComponent();
        }

        private void Patches_Load(object sender, EventArgs e)
        {
            _patches = new Patches();
            StatusTextBox.Text = "No Process Attached.";

            foreach (DictionaryEntry patch in _patches.AvailablePatches)
            {
                AvailablePatchesListBox.Items.Add(patch.Key);
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            _process = _patches.OpenProcess();

            if (_process == null)
            {
                StatusTextBox.Text = "No process or multiple instances found.";
                ProcessNameTextBox.Text = String.Empty;
                ProcessIDTextBox.Text = String.Empty;
                AvailablePatchesListBox.Enabled = false;
                ApplyButton.Enabled = false;
                return;
            }

            StatusTextBox.Text = "Open.";
            ProcessNameTextBox.Text = _process.ProcessName;
            ProcessIDTextBox.Text = _process.Id.ToString();
            AvailablePatchesListBox.Enabled = true;
            ApplyButton.Enabled = true;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (_process != null)
            {
                _process = null;
                _patches.CloseHandle();
                StatusTextBox.Text = "Closed.";
                ProcessNameTextBox.Text = String.Empty;
                ProcessIDTextBox.Text = String.Empty;
                AvailablePatchesListBox.Enabled = false;
                ApplyButton.Enabled = false;
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            string[] availablePatches = new string[AvailablePatchesListBox.Items.Count];
            AvailablePatchesListBox.CheckedItems.CopyTo(availablePatches, 0);

            bool result = _patches.ApplyPatches(availablePatches, ProcessNameTextBox.Text);

            if (result == true)
            {
                MessageBox.Show("Selected patches successfully applied.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There was a problem applying the selected patches.",
                    "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
