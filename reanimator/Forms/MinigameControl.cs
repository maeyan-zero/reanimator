using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Resources;
using Hellgate;
using Reanimator.Properties;

namespace Reanimator.Forms
{
    public partial class MinigameControl : UserControl
    {
        AttributeValues[] values;
        AttributeValues[] currentMinigame;
        Random random;
        Difficulty difficulty;
        Unit.StatBlock.Stat.Values[] minigameValues;

        // List of available mini game icons
        private enum MinigameIcons
        {
            d_physical,
            d_fire,
            d_electric,
            d_spectral,
            d_poison,
            d_critical,
            k_necro,
            k_beast,
            k_spectral,
            k_demon,
            f_mod,
            f_armor,
            f_sword,
            f_gun,
            f_magical
        }

        private enum Difficulty
        {
            easy = 1,
            normal = 2,
            hard = 4
        }

        private struct AttributeValues
        {
            public AttributeValues(string value, int attr0, int attr1, MinigameIcons icon)
            {
                Value = value;
                Attr0 = attr0;
                Attr1 = attr1;
                Icon = icon;
            }

            public string Value;
            public int Attr0;
            public int Attr1;
            public MinigameIcons Icon;
        }

        public MinigameControl()
        {
            InitializeComponent();
        }

        public MinigameControl(Unit.StatBlock.Stat.Values[] minigame) : this()
        {
            minigameValues = minigame;
            random = new Random(DateTime.Now.Millisecond);
            currentMinigame = new AttributeValues[3];
            difficulty = Difficulty.normal;

            InitializeAttributes();
            
            //RandomizeMinigame();
            SetCurrentMinigame();
        }

        private void SetCurrentMinigame()
        {
            for (int counter = 0; counter < minigameValues.Length; counter++)
            {
                AttributeValues game = GetMatchingValues(minigameValues[counter].Attribute1, minigameValues[counter].Attribute2);
                SetMinigame(game.Icon, minigameValues[counter].Stat - 1, counter + 1);
            }
        }

        private AttributeValues GetMatchingValues(int attr0, int attr1)
        {
            foreach (AttributeValues value in values)
            {
                if (value.Attr0 == attr0 && value.Attr1 == attr1)
                {
                    return value;
                }
            }

            return values[CheckIfLAreadyExists(0)];
        }

        private void InitializeAttributes()
        {
            values = new AttributeValues[15];

            values[0] = new AttributeValues("Deal physical damage", 0, 1, MinigameIcons.d_physical);
            values[1] = new AttributeValues("Deal fire damage", 0, 2, MinigameIcons.d_fire);
            values[2] = new AttributeValues("Deal electric damage", 0, 3, MinigameIcons.d_electric);
            values[3] = new AttributeValues("Deal spectral damage", 0, 4, MinigameIcons.d_spectral);
            values[4] = new AttributeValues("Deal poison damage", 0, 5, MinigameIcons.d_poison);
            values[5] = new AttributeValues("Deal critical damage", 2, 0, MinigameIcons.d_critical);
            values[6] = new AttributeValues("Kill Necros", 1, 10, MinigameIcons.k_necro);
            values[7] = new AttributeValues("Kill Beasts", 1, 11, MinigameIcons.k_beast);
            values[8] = new AttributeValues("Kill Spectrals", 1, 12, MinigameIcons.k_spectral);
            values[9] = new AttributeValues("Kill Demons", 1, 13, MinigameIcons.k_demon);
            values[10] = new AttributeValues("Find mod", 4, 15, MinigameIcons.f_mod);
            values[11] = new AttributeValues("Find armor", 4, 17, MinigameIcons.f_armor);
            values[12] = new AttributeValues("Find sword", 4, 43, MinigameIcons.f_sword);
            values[13] = new AttributeValues("Find gun", 4, 46, MinigameIcons.f_gun);
            values[14] = new AttributeValues("Find magical item", 5, 0, MinigameIcons.f_magical);
        }

        private void RandomizeMinigame()
        {
            AttributeValues[] minigameGoals = new AttributeValues[3];
            int count = 0;

            while (count < 3)
            {
                // Create a new minigame goal
                int index = random.Next(0, 15);

                // If the collection of goals doesn't contain the current goal...
                int newIndex = CheckIfLAreadyExists(index);

                // ...add it as new goal
                minigameGoals[count] = values[newIndex];
                count++;

                SetMinigame(values[newIndex].Icon, GetNumberOfAchieves((int)values[newIndex].Icon), count);
            }
        }

        private int GetNumberOfAchieves(int index)
        {
            int minRange = 1;
            int maxRange = 1;

            if (index <= 5)
            {
                minRange = 5 * (int)difficulty;
                maxRange = 10 * (int)difficulty;
            }
            else if (index <= 9)
            {
                minRange = 3 * (int)difficulty;
                maxRange = 7 * (int)difficulty;
            }
            else
            {
                minRange = 2 * (int)difficulty;
                maxRange = 4 * (int)difficulty;
            }

            return random.Next(minRange, maxRange + 1);
        }

        private void SetMinigame(MinigameIcons icon, int count, int position)
        {
            if (position > 0 && position < 4)
            {
                Button button = (Button)p_icons.Controls[position - 1];

                SetMinigameInfos(button, (int)icon, count, position - 1);
            }
        }

        private void b_icon_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            AttributeValues value = (AttributeValues)button.Tag;

            int newIndex = CheckIfLAreadyExists(((int)value.Icon + 1) % 15);

            SetMinigameInfos(button, newIndex, GetNumberOfAchieves((int)values[newIndex].Icon), p_icons.Controls.IndexOf(button));
        }

        private void SetMinigameInfos(Button button, int valueIndex, int count, int buttonIndex)
        {
            ResourceManager rm = Resources.ResourceManager;
            button.BackgroundImage = (Bitmap)rm.GetObject(values[valueIndex].Icon.ToString());
            button.Text = count.ToString();
            button.Tag = values[valueIndex];
            currentMinigame[buttonIndex] = values[valueIndex];

            minigameValues[buttonIndex].Attribute1 = values[valueIndex].Attr0;
            minigameValues[buttonIndex].Attribute2 = values[valueIndex].Attr1;
            minigameValues[buttonIndex].Stat = count + 1;
        }

        private int CheckIfLAreadyExists(int newIndex)
        {
            int index = newIndex;

            while (true)
            {
                if (currentMinigame.Contains(values[index]))
                {
                    index = (index + 1) % 15;
                }
                else
                {
                    return index;
                }
            }
        }

        private void b_reset_Click(object sender, EventArgs e)
        {
            RandomizeMinigame();
        }

        private void rb_difficulty_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_easy.Checked)
            {
                this.difficulty = MinigameControl.Difficulty.easy;
            }
            else if (rb_normal.Checked)
            {
                this.difficulty = MinigameControl.Difficulty.normal;
            }
            else if (rb_hard.Checked)
            {
                this.difficulty = MinigameControl.Difficulty.hard;
            }
        }
    }
}
