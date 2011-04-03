using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hellgate;
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms
{
    public partial class AnimationTestForm : Form
    {
        AtlasImageLoader loader;
        AnimationHandler handler;
        List<string> animations;

        public AnimationTestForm(FileManager fileManager)
        {
            InitializeComponent();

            animations = new List<string>();
            animations.Add("health anim ");
            animations.Add("power anim ");

            loader = new AtlasImageLoader();
            loader.LoadAtlas(@"data\uix\xml\main_new_atlas.xml", fileManager);
            handler = new AnimationHandler();
            handler.Speed = (int)numericUpDown1.Value;
            handler.NewFrameEvent += new NewFrame(handler_NewFrameEvent);

            comboBox1.DataSource = animations;
        }

        void handler_NewFrameEvent()
        {
            pictureBox1.Image = handler.GetCurrentFrame();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            handler.Reset();
            handler.Start();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            handler.Stop();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            handler.Pause = !handler.Pause;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            handler.Speed = (int)numericUpDown1.Value;
        }

        private void checkBoxLoop_CheckedChanged(object sender, EventArgs e)
        {
            handler.Loop = checkBoxLoop.Checked;
        }

        private void checkBoxReverse_CheckedChanged(object sender, EventArgs e)
        {
            handler.Reverse = checkBoxReverse.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = (string)comboBox1.SelectedItem;

            handler.ClearFrames();

            for (int counter = 1; counter < 17; counter++)
            {
                handler.AddFrame(loader.GetImage(text + counter));
            }
        }
    }
}
