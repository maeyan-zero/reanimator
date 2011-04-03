using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public delegate void AnimationEnded();
    public delegate void NewFrame();
    public class AnimationHandler
    {
        public event AnimationEnded AnimationEndedEvent = delegate { };
        public event NewFrame NewFrameEvent = delegate { };

        List<Bitmap> frames;
        bool loop;
        bool reverse;
        bool pause;
        Timer timer;
        int currentFrame;

        public void AddFrame(Bitmap bitmap)
        {
            frames.Add(bitmap);
        }

        public void AddFrame(List<Bitmap> bitmap)
        {
            frames.AddRange(bitmap.ToArray());
        }

        public void ClearFrames()
        {
            frames.Clear();
        }

        public int Speed
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }
        
        public bool Reverse
        {
            get { return reverse; }
            set { reverse = value; }
        }

        public bool Pause
        {
            get { return pause; }
            set { pause = value; }
        }

        public void Start()
        {
            pause = false;
            timer.Start();
        }

        public Bitmap GetCurrentFrame()
        {
            Bitmap bitmap = frames[currentFrame];

            return bitmap;
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            currentFrame = 0;
            Stop();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!pause)
            {
                NewFrameEvent();

                CalculateNextFrameIndex();
            }
        }

        private void CalculateNextFrameIndex()
        {
            int position;

            if (reverse)
            {
                position = currentFrame - 1;

                if (loop)
                {
                    currentFrame = position < 0 ? frames.Count - 1 : position;
                }
                else
                {
                    currentFrame = position < 0 ? 0 : position;

                    if (currentFrame == 0)
                    {
                        AnimationEndedEvent();
                    }
                }
            }
            else
            {
                position = currentFrame + 1;

                if (loop)
                {
                    currentFrame = position > frames.Count - 1 ? 0 : position;
                }
                else
                {
                    currentFrame = position > frames.Count - 1 ? frames.Count - 1 : position;

                    if (currentFrame == frames.Count - 1)
                    {
                        AnimationEndedEvent();
                    }
                }
            }
        }

        public AnimationHandler()
        {
            frames = new List<Bitmap>();
            currentFrame = 0;
            loop = false;
            reverse = false;
            pause = false;

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);

            this.AnimationEndedEvent += new AnimationEnded(AnimationHandler_AnimationEndedEvent);
        }

        void AnimationHandler_AnimationEndedEvent()
        {
            timer.Stop();
        }
    }
}
