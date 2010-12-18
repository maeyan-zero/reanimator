using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class LevelRulesEditor : Form
    {
        private readonly LevelRulesFile _levelRules = new LevelRulesFile();

        public LevelRulesEditor()
        {
            InitializeComponent();

            const String path = @"D:\Games\Hellgate London\data\background\city\rule_pmt04.drl";
            //const String path = @"D:\Games\Hellgate London\data\background\sewers\bsb_rule_07.drl";
            //const String path = @"D:\Games\Hellgate London\data\background\catacombs\ctd_template.drl";
            byte[] levelRulesBytes = File.ReadAllBytes(path);
            _levelRules.ParseFileBytes(levelRulesBytes);
        }

        private void _LevelRulesEditor_Paint(object sender, PaintEventArgs e)
        {
            const float scale = 2.5f;
            const float fontSize = 10;
            const float blockSize = 40;
            Graphics g = e.Graphics;
            Font fnt = new Font("Verdana", fontSize);
            Color[] colors = new[] { Color.DeepSkyBlue, Color.DarkSeaGreen, Color.Goldenrod, Color.Cornsilk };

            foreach (LevelRulesFile.LevelRule levelRule in _levelRules.LevelRules)
            {
                if (levelRule.StaticRooms != null)
                {
                    int r = 0;
                    foreach (LevelRulesFile.Room room in levelRule.StaticRooms)
                    {
                        float xPos = room.xPosition * scale + 550;
                        float yPos = room.yPosition * scale + 300;

                        Matrix myMatrix = new Matrix();
                        myMatrix.RotateAt(room.rotation * 180 / (float)Math.PI, new PointF(xPos, yPos));
                        g.Transform = myMatrix;

                        Color color = colors[0];

                        SolidBrush solidBrush = new SolidBrush(color);
                        Pen pn = new Pen(Color.Black);
                        g.FillRectangle(solidBrush, xPos, yPos, blockSize, blockSize);
                        g.DrawRectangle(pn, xPos, yPos, blockSize, blockSize);

                        float strX = xPos;
                        float strY = yPos;
                        if (r % 2 == 0)
                        {
                            strY += blockSize;
                        }
                        else
                        {
                            strY -= (fontSize + 6);
                        }

                        g.Transform = new Matrix();
                        g.DrawString(room.RoomName, fnt, new SolidBrush(Color.Black), strX, strY);
                        r++;
                    }
                }

                if (levelRule.Rules == null) continue;
                int i = 0;
                foreach (LevelRulesFile.Room[] levelRules in levelRule.Rules)
                {
                    int r = 0;
                    foreach (LevelRulesFile.Room room in levelRules)
                    {
                        float yPos = room.xPosition * scale + 650 - i * 350;
                        float xPos = room.yPosition * scale + 200;

                        Matrix myMatrix = new Matrix();
                        myMatrix.RotateAt(room.rotation * 180 / (float)Math.PI, new PointF(xPos, yPos));
                        g.Transform = myMatrix;

                        Color color = colors[i % colors.Length];

                        SolidBrush solidBrush = new SolidBrush(color);
                        Pen pn = new Pen(Color.Black);
                        g.FillRectangle(solidBrush, xPos, yPos, blockSize, blockSize);
                        g.DrawRectangle(pn, xPos, yPos, blockSize, blockSize);


                        float strX = xPos;
                        float strY = yPos;
                        if (r % 2 == 0)
                        {
                            strY += blockSize;
                        }
                        else
                        {
                            strY -= (fontSize + 6);
                        }

                        //g.Transform = new Matrix();
                        g.DrawString(room.RoomName, fnt, new SolidBrush(Color.Black), strX, strY);
                        r++;
                    }
                    i++;
                }
            }
        }
    }
}
