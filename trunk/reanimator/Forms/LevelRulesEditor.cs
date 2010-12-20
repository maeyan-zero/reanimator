using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms
{
    public sealed partial class LevelRulesEditor : Form
    {
        private readonly FileManager _fileManager;
        private readonly LevelRulesFile _levelRules = new LevelRulesFile();
        private readonly Dictionary<String, RoomDefinitionFile> _roomDefinitions = new Dictionary<String, RoomDefinitionFile>();
        private readonly Font _font = new Font("Verdana", 8);
        private readonly Color[] _colors = new[] { Color.DeepSkyBlue, Color.DarkSeaGreen, Color.Goldenrod, Color.Cornsilk };

        private const float GraphicsScrollRate = 0.3f;
        private readonly Point _graphicsInitialOffset = new Point(200, 200);


        private int _graphicsOffsetX;
        private int _graphicsOffsetY;
        private float _graphicsScale = 1.0f;

        public LevelRulesEditor(FileManager fileManager, IndexFile.FileEntry fileEntry)
        {
            InitializeComponent();
            DoubleBuffered = true;

            _fileManager = fileManager;
            byte[] fileBytes = fileManager.GetFileBytes(fileEntry);
            _levelRules.ParseFileBytes(fileBytes);

            foreach (LevelRulesFile.LevelRule levelRule in _levelRules.LevelRules)
            {
                if (levelRule.StaticRooms != null)
                {
                    _LoadRooms(levelRule.StaticRooms, fileEntry.RelativeFullPathWithoutPatch);
                }
                else
                {
                    foreach (LevelRulesFile.Room[] levelRules in levelRule.Rules)
                    {
                        _LoadRooms(levelRules, fileEntry.RelativeFullPathWithoutPatch);
                    }
                }
            }
        }

        public LevelRulesEditor(String filePath)
        {
            InitializeComponent();
            DoubleBuffered = true;

            String rootDir = Path.GetDirectoryName(filePath);
            byte[] fileBytes = File.ReadAllBytes(filePath);
            _levelRules.ParseFileBytes(fileBytes);

            foreach (LevelRulesFile.LevelRule levelRule in _levelRules.LevelRules)
            {
                if (levelRule.StaticRooms != null)
                {
                    _LoadRooms(levelRule.StaticRooms, rootDir);
                }
                else
                {
                    foreach (LevelRulesFile.Room[] levelRules in levelRule.Rules)
                    {
                        _LoadRooms(levelRules, rootDir);
                    }
                }
            }
        }

        private void _LoadRooms(IEnumerable<LevelRulesFile.Room> rooms, String rootDir)
        {
            foreach (LevelRulesFile.Room room in rooms)
            {
                // todo: create proper function - we are adding duplicates as RoomName and RoomName[Script]
                if (_roomDefinitions.ContainsKey(room.RoomName)) continue;

                String roomPathName = room.RoomName;
                int indexL = room.RoomName.IndexOf('[');
                if (indexL > 0)
                {
                    roomPathName = roomPathName.Substring(0, indexL);
                }

                byte[] fileBytes;
                if (_fileManager != null)
                {
                    String relativeDirectory = Path.GetDirectoryName(rootDir);
                    String relativePath = Path.Combine(relativeDirectory, roomPathName + RoomDefinitionFile.Extension);
                    fileBytes = _fileManager.GetFileBytes(relativePath);
                }
                else
                {
                    String roomPath = Path.Combine(rootDir, roomPathName + RoomDefinitionFile.Extension);
                    fileBytes = File.ReadAllBytes(roomPath);
                }

                RoomDefinitionFile roomDefinitionFile = new RoomDefinitionFile();
                roomDefinitionFile.ParseFileBytes(fileBytes);
                _roomDefinitions.Add(room.RoomName, roomDefinitionFile);
            }
        }

        private enum PaintType : uint
        {
            Block,
            Outline,
            Text
        }

        private bool _flipWidthHeight;
        private bool _reverseRotation;
        private bool _flipXYOffsets;
        private bool _disableWidthOffset;
        private bool _flipWidthHeightOffset;
        private void _PaintRooms(Graphics g, IEnumerable<LevelRulesFile.Room> rooms, PointF offset, Color color, PaintType paintType)
        {
            int i = 0;
            foreach (LevelRulesFile.Room room in rooms)
            {
                float yPos = room.xPosition * _graphicsScale + offset.X + _graphicsOffsetX;
                float xPos = room.yPosition * _graphicsScale + offset.Y + _graphicsOffsetY;
                if (_flipXYOffsets)
                {
                    yPos = room.yPosition * _graphicsScale + offset.X + _graphicsOffsetX;
                    xPos = room.xPosition * _graphicsScale + offset.Y + _graphicsOffsetY;
                }

                RoomDefinitionFile roomDefinition = _roomDefinitions[room.RoomName];
                float roomWidth = roomDefinition.RoomDefinition.FileHeader.UnknownFloat1 * _graphicsScale;
                float roomHeight = roomDefinition.RoomDefinition.FileHeader.UnknownFloat2 * _graphicsScale;
                if (_flipWidthHeight)
                {
                    roomWidth = roomDefinition.RoomDefinition.FileHeader.UnknownFloat2 * _graphicsScale;
                    roomHeight = roomDefinition.RoomDefinition.FileHeader.UnknownFloat1 * _graphicsScale;
                }

                Matrix myMatrix = new Matrix();
                float rotateDeg = -room.rotation*180.0f/(float) Math.PI;
                if (_reverseRotation) rotateDeg *= -1;

                SolidBrush solidBrush = new SolidBrush(color);
                Pen pn = new Pen(Color.Black);

                if (roomWidth < 0)
                {
                    roomWidth *= -1;
                    if (!_disableWidthOffset)
                    {
                        if (_flipWidthHeightOffset)
                        {
                            xPos -= roomWidth;
                        }
                        else
                        {
                            yPos -= roomWidth;
                        }
                    }
                }
                if (roomHeight < 0)
                {
                    roomHeight *= -1;
                    if (!_disableWidthOffset)
                    {
                        if (_flipWidthHeightOffset)
                        {
                            xPos -= roomHeight;
                        }
                        else
                        {
                            yPos -= roomHeight;
                        }
                    }
                }

                myMatrix.RotateAt(rotateDeg, new PointF(xPos, yPos));
                g.Transform = myMatrix;

                if (paintType ==PaintType.Block)
                {
                    g.FillRectangle(solidBrush, xPos, yPos, roomWidth, roomHeight);
                    
                   // g.Transform = new Matrix();
                    
                   //g.FillRectangle(new SolidBrush(Color.MistyRose), xPos, yPos, roomWidth, roomHeight);
                    //g.DrawString(rotateDeg.ToString(), _font, new SolidBrush(Color.Black), xPos, yPos);
                }
                if (paintType == PaintType.Outline) g.DrawRectangle(pn, xPos, yPos, roomWidth, roomHeight);


                float strX = xPos;
                float strY = yPos;
                //if (i % 2 == 0)
                //{
                //    strY += roomHeight;
                //}
                //else
                //{
                //    strY -= (_font.Size + 6);
                //}

                //g.Transform = new Matrix();
                if (paintType == PaintType.Text) g.DrawString(room.RoomName, _font, new SolidBrush(Color.Black), strX, strY);
                i++;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (LevelRulesFile.LevelRule levelRule in _levelRules.LevelRules)
            {
                if (levelRule.StaticRooms != null)
                {
                    _PaintRooms(e.Graphics, levelRule.StaticRooms, new PointF(_graphicsInitialOffset.X, _graphicsInitialOffset.Y), _colors[0], PaintType.Block);
                    _PaintRooms(e.Graphics, levelRule.StaticRooms, new PointF(_graphicsInitialOffset.X, _graphicsInitialOffset.Y), _colors[0], PaintType.Outline);
                    _PaintRooms(e.Graphics, levelRule.StaticRooms, new PointF(_graphicsInitialOffset.X, _graphicsInitialOffset.Y), _colors[0], PaintType.Text);
                }
                else
                {
                    int i = 0;
                    foreach (LevelRulesFile.Room[] levelRules in levelRule.Rules)
                    {
                        float offsetX = 650 - i*350*_graphicsScale;
                        float offsetY = 200;
                        _PaintRooms(e.Graphics, levelRules, new PointF(offsetX, offsetY), _colors[i % _colors.Length], PaintType.Block);
                        _PaintRooms(e.Graphics, levelRules, new PointF(offsetX, offsetY), _colors[i % _colors.Length], PaintType.Outline);
                        _PaintRooms(e.Graphics, levelRules, new PointF(offsetX, offsetY), _colors[i % _colors.Length], PaintType.Text);
                        i++;
                    }
                }
            }
        }

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    // todo: do we need something in here? don't think so since we have plain background
        //}

        private bool _mouseDown;
        private Point _mousePrevPoint;
        private void _LevelRulesEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _mouseDown = false;
        }

        private void _LevelRulesEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            _mouseDown = true;
            _mousePrevPoint = e.Location;
        }

        private void _LevelRulesEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown) return;

            _graphicsOffsetY += e.Location.X - _mousePrevPoint.X;
            _graphicsOffsetX += e.Location.Y - _mousePrevPoint.Y;
            _mousePrevPoint = e.Location;
            Refresh();
        }

        private void _LevelRulesEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            _graphicsScale += e.Delta > 0 ? GraphicsScrollRate : -GraphicsScrollRate;
            Refresh();
        }

        private void _LevelRulesEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Home) return;

            _graphicsOffsetX = _graphicsInitialOffset.X;
            _graphicsOffsetY = _graphicsInitialOffset.Y;
            Refresh();
        }

        private void flipWidthHeight_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            _flipWidthHeight = flipWidthHeight_checkBox.Checked;
            Refresh();
        }

        private void reverseRotation_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            _reverseRotation = reverseRotation_checkBox.Checked;
            Refresh();
        }

        private void flipXYOffsets_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            _flipXYOffsets = flipXYOffsets_checkBox.Checked;
            Refresh();
        }

        private void disableWidthOffset_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            _disableWidthOffset = disableWidthOffset_checkBox.Checked;
            flipWidthHeightOffset_checkBox.Enabled = !_disableWidthOffset;
            Refresh();
        }

        private void flipWidthHeightOffset_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            _flipWidthHeightOffset = flipWidthHeightOffset_checkBox.Checked;
            Refresh();
        }
    }
}
