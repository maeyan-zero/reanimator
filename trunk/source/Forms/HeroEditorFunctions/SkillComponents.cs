using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Properties;
using System.IO;

namespace Reanimator.HeroEditorFunctions
{
    public class SkillComponents
    {
        Bitmap _completeSkillPanel;
        Graphics _g;

        Bitmap _skillIconPanel;

        SkillPanel _skillPanel;
        DataTable _skillTable;
        List<SkillControls> _skillControls;

        public List<SkillControls> SkillControls
        {
            get { return _skillControls; }
            set { _skillControls = value; }
        }
        Panel _panel;
                

        public SkillComponents(SkillPanel skillPanel)
        {
            _skillPanel = skillPanel;
            _skillControls = new List<SkillControls>();
            _panel = new Panel();

            _completeSkillPanel = _skillPanel.GetImageFromSkillPanel("skill panel");
            _completeSkillPanel.MakeTransparent(Color.White);

            _skillIconPanel = _skillPanel.GetImageFromSkillPanel("icon panel big");
            _skillIconPanel.MakeTransparent(Color.White);

            _g = Graphics.FromImage(_completeSkillPanel);
        }

        public Panel CreatePanel(ref DataTable skillTable, int characterClassId)
        {
            _skillTable = skillTable;
            int generalSkillTableId = 0;

            string bmpPath = Directory.GetCurrentDirectory() + @"\" + characterClassId + ".bmp";

            bool draw = !File.Exists(bmpPath);
            string select = "skillTab = '" + characterClassId + "'";
            DataRow[] skills = _skillTable.Select(select);

            select = "skillTab = '" + generalSkillTableId + "'";
            DataRow[] generalSkills = _skillTable.Select(select);
            //draw = true;
            Bitmap connectorLns = null;
            Bitmap generalSkl = CreateGeneralSkills(generalSkills, draw);
            Bitmap classSkl = CreateClassSkills(skills, draw);

            if (draw)
            {
                connectorLns = CreateConnectorLines(skills);
                _g.DrawImage(connectorLns, new Point());
                _g.DrawImage(generalSkl, new Point());
                _g.DrawImage(classSkl, new Point());

                //Bitmap b = new Bitmap(_completeSkillPanel.Width, _completeSkillPanel.Height);
                //Graphics g = Graphics.FromImage(b);
                //g.Clear(Color.White);
                //g.DrawImage(connectorLns, new Point());
                //g.DrawImage(classSkl, new Point());
                //g.DrawImage(generalSkl, new Point());
                //b.Save(@"F:\classId" + characterClassId + ".bmp");
                //_completeSkillPanel.Save(@"F:\Panel" + characterClassId + ".bmp");

                _completeSkillPanel.Save(bmpPath);
            }
            else
            {
                _completeSkillPanel = (Bitmap)Bitmap.FromFile(bmpPath);
            }

            _panel.BackgroundImage = _completeSkillPanel;
            _panel.BackgroundImageLayout = ImageLayout.Stretch;
            _panel.Size = _completeSkillPanel.Size;

            return _panel;
        }

        private Bitmap CreateConnectorLines(DataRow[] classSkills)
        {
            Bitmap connectorLineLayer = new Bitmap(_completeSkillPanel.Size.Width, _completeSkillPanel.Size.Height);
            Graphics g = Graphics.FromImage(connectorLineLayer);
            g.Clear(Color.Transparent);

            int xOffset = 52;
            int yOffset = 120 + 24;
            int spacingX = 120;
            int spacingY = 122 - 4;

            foreach (DataRow skill in classSkills)
            {
                foreach (DataRow required in classSkills)
                {
                    int currentSkillIndex = (int)required["index"];
                    int requiredSkillIndex = (int)skill["requiredskills1"];

                    if (currentSkillIndex == requiredSkillIndex)
                    {
                        int x1 = (int)skill["skillPageColumn"];
                        int y1 = (int)skill["skillPageRow"];

                        int x2 = (int)required["skillPageColumn"];
                        int y2 = (int)required["skillPageRow"];

                        Bitmap connector = null;
                        Point drawLocation = new Point(x2 * spacingX + xOffset, y2 * spacingY + yOffset);

                        int distanceX = Math.Abs(x1 - x2);
                        int distanceY = Math.Abs(y1 - y2);

                        if (x1 == x2)
                        {
                            if (distanceY > 0)
                            {
                                connector = _skillPanel.GetImageFromSkillPanel("line_" + distanceY + "d");

                                drawLocation.Y += _skillIconPanel.Size.Height / 2;
                            }
                        }
                        else if (x1 < x2)
                        {
                            connector = _skillPanel.GetImageFromSkillPanel("line_" + distanceX + "l_" + distanceY + "d");

                            drawLocation.X -= _skillIconPanel.Size.Width + 24;
                            drawLocation.Y += _skillIconPanel.Size.Height / 2 - 2;
                        }
                        else if (x1 > x2)
                        {
                            connector = _skillPanel.GetImageFromSkillPanel("line_" + distanceX + "r_" + distanceY + "d");

                            drawLocation.Y += _skillIconPanel.Size.Height / 2;
                        }

                        g.DrawImage(connector, drawLocation);
                        connector.Dispose();
                    }
                }
            }

            g.Dispose();
            return connectorLineLayer;
        }

        private Bitmap CreateClassSkills(DataRow[] classSkills, bool drawBG)
        {
            try
            {
                /*
                 * SKILLTABS:
                 * 
                 * 0 = general skills (sprint, left_weapon, right_weapon, recall...)
                 * 1 = Blademaster
                 * 3 = Guardian
                 * 6 = Evoker
                 * 8 = Summoner
                 * 11 = Marksman
                 * 13 = Engineer
                 */

                //TODO:
                //When adding a skillpoint, check if the (skills) prerequisites were met

                Bitmap skillIconLayer = null;
                Graphics g = null;

                if (drawBG)
                {
                    skillIconLayer = new Bitmap(_completeSkillPanel.Size.Width, _completeSkillPanel.Size.Height);
                    g = Graphics.FromImage(skillIconLayer);
                    g.Clear(Color.Transparent);
                }

                int xOffset = 48;//144;
                int yOffset = 96 + 24;//192;
                int distanceX = 120;
                int distanceY = 122 - 4;

                foreach (DataRow skill in classSkills)
                {
                    int x = (int)skill["skillPageColumn"];
                    int y = (int)skill["skillPageRow"];
                    string skillName = (string)skill["skill"];
                    string iconName = (string)skill["largeIcon"];
                    int maxLevel = (int)skill["maxLevel"];
                    int currentIndex = (int)skill["index"];
                    int requiredSkill = (int)skill["requiredskills1"];

                    Point loc = new Point(x * distanceX + xOffset, y * distanceY + yOffset);

                    if (drawBG)
                    {
                        // draw skillIconPanel
                        g.DrawImage(_skillIconPanel, loc);

                        // draw skillIcon
                        Bitmap image = _skillPanel.GetImageFromSkillTree(iconName);
                        g.DrawImage(image, new Point(loc.X + 4, loc.Y + 4));
                        image.Dispose();
                    }

                    // set controls
                    SkillControls skillControl = new SkillControls((int)skill["code"], skillName);
                    CreateControls(ref skillControl, maxLevel, loc);
                    skillControl.GridPosition = new Point(x, y);
                    skillControl.Size = _skillIconPanel.Size;
                }

                if (g != null)
                {
                    g.Dispose();
                }
                return skillIconLayer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private Bitmap CreateGeneralSkills(DataRow[] generalSkills, bool drawBG)
        {
            Bitmap generalSkillLayer = null;
            Graphics g = null;

            if (drawBG)
            {
                generalSkillLayer = new Bitmap(_completeSkillPanel.Size.Width, _completeSkillPanel.Size.Height);
                g = Graphics.FromImage(generalSkillLayer);
                g.Clear(Color.Transparent);
            }

            int xOffset = 0;
            int yOffset = 0;
            int distanceX = 80;
            int distanceY = 80;

            foreach (DataRow generalSkill in generalSkills)
            {
                int x = (int)generalSkill["skillPageColumn"];
                int y = (int)generalSkill["skillPageRow"];
                string skillName = (string)generalSkill["skill"];
                string iconName = (string)generalSkill["largeIcon"];
                int maxLevel = (int)generalSkill["maxLevel"];

                if (skillName.Contains("Swiftness_Boost"))
                {
                    xOffset = 20;
                    yOffset = 970;

                    Point loc = new Point(x * distanceX + xOffset, y * distanceY + yOffset);
                    // draw skillIconPanel
                    // _g.DrawImage(_skillIconPanel, loc);

                    // draw skillIcon
                    if (drawBG)
                    {
                        Bitmap image = _skillPanel.GetImageFromSkillTree(iconName);
                        g.DrawImage(image, new Point(loc.X + 4, loc.Y + 4));
                        image.Dispose();
                    }

                    // set controls
                    SkillControls skillControl = new SkillControls((int)generalSkill["code"], skillName);
                    CreateControls(ref skillControl, maxLevel, loc);
                }
                else if(drawBG)
                {
                    xOffset = 24;
                    yOffset = 974;

                    Point loc = new Point(x * distanceX + xOffset, y * distanceY + yOffset);

                    Bitmap image = _skillPanel.GetImageFromSkillTree(iconName);

                    g.DrawImage(image, new Point(loc.X, loc.Y));
                    image.Dispose();
                }
            }

            if (g != null)
            {
                g.Dispose();
            }
            return generalSkillLayer;
        }

        private void CreateControls(ref SkillControls skillControl, int maxLevel, Point loc)
        {
            skillControl.MaxLevel = maxLevel;
            skillControl.Location = new Point(loc.X, loc.Y + 4);

            _skillControls.Add(skillControl);
            _panel.Controls.Add(skillControl.AddButton);
            _panel.Controls.Add(skillControl.SubsButton);
            _panel.Controls.Add(skillControl.Label);
        }
    }

    public class SkillControls
    {
        public string _name;
        public int _id;

        int currentLevel;
        int maxLevel;
        public int Index;
        public int RequiredSkillIndex;

        Point _location;
        public Size Size;
        public Point GridPosition;
        public Button AddButton;
        public Button SubsButton;
        public Label Label;

        public SkillControls(int id, string name)
        {
            this._id = id;
            this._name = name;

            AddButton = CreateSkillButton(Resources.buttonAddNormal, Resources.buttonAddHover, Resources.buttonAddClicked, "Add Button");
            AddButton.Location = new Point(70, 4);
            AddButton.Click += new EventHandler(_addButton_Click);

            SubsButton = CreateSkillButton(Resources.buttonSubstractNormal, Resources.buttonSubstractHover, Resources.buttonSubstractClicked, "Substract Button");
            SubsButton.Location = new Point(70, 34);
            SubsButton.Click += new EventHandler(_subsButton_Click);


            Label = new Label();
            Label.BackColor = Color.Transparent;
            Label.ForeColor = Color.White;
            Label.TextAlign = ContentAlignment.TopCenter;
            Label.MaximumSize = new Size(56, 22);
            Label.Size = new Size(56, 22);
            Label.Location = new Point(12, 72);
        }

        public SkillButton CreateSkillButton(Bitmap normal, Bitmap hover, Bitmap clicked, string name)
        {
            SkillButton button = new SkillButton();
            button.Name = name;

            button.SetButtonImages(normal, hover, clicked);

            //button.BackgroundImageLayout = ImageLayout.Stretch;
            //button.FlatStyle = FlatStyle.Flat;
            //button.FlatAppearance.BorderSize = 0;
            //button.FlatAppearance.CheckedBackColor = Color.Black;
            //button.FlatAppearance.MouseDownBackColor = Color.Black;
            //button.FlatAppearance.MouseOverBackColor = Color.Black;

            return button;
        }

        public override string ToString()
        {
            return currentLevel + "/" + maxLevel;
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                currentLevel = value;
                Label.Text = this.ToString();
            }
        }

        public int MaxLevel
        {
            get { return maxLevel; }
            set
            {
                maxLevel = value;
                Label.Text = this.ToString();
            }
        }

        void _addButton_Click(object sender, EventArgs e)
        {
            currentLevel = (currentLevel + 1) > maxLevel ? currentLevel : (currentLevel + 1);
            Label.Text = this.ToString();
        }

        void _subsButton_Click(object sender, EventArgs e)
        {
            currentLevel = (currentLevel - 1) < 0 ? 0 : (currentLevel - 1);
            Label.Text = this.ToString();
        }

        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                AddButton.Location = new Point(value.X + 70, value.Y + 4);
                SubsButton.Location = new Point(value.X + 70, value.Y + 34);
                Label.Location = new Point(value.X + 8, value.Y + 70);
            }
        }
    }
}
