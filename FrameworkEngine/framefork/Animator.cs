using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubla
{
    public class Animator
    {
        private Timer timer;
        private int maxLengthFrame;
        private int frame;
        private Texture[] frames;

        private bool loop;
        private bool stop;
        private bool pauseFrame;
        private List<int> futureFrames = new List<int>();
        private bool nextFrame;

        public Animator(int[] posFrame, float speed, bool loop, string nameTexture, Vector2i splitSpriteSizeTexture, bool smoothTexture) {
            Vector2i[] frames = new Vector2i[posFrame.Length];
            Texture[] framesTexture = new Texture[posFrame.Length / 2];
            int j = 0;
            for (int i = 0; i < frames.Length;i += 2)
            {
                frames[j] = new Vector2i(posFrame[i], posFrame[i + 1]);
                framesTexture[j] = new Texture("assets\\" + nameTexture, new IntRect(splitSpriteSizeTexture.X * posFrame[i + 1], splitSpriteSizeTexture.Y * posFrame[i], splitSpriteSizeTexture.X, splitSpriteSizeTexture.Y));
                framesTexture[j].Smooth = smoothTexture;
                j++;

            }
            this.frames = framesTexture;
            
            this.timer = new Timer(speed);
            this.maxLengthFrame = j;
            this.loop = loop;
        }

        public void Update()
        {
            if (stop) return;
            nextFrame = false;

            timer.Add(-Game.SDelta());
            if(timer.GetFloat() < 0)
            {
                foreach(int pauseFrame in futureFrames)
                {
                    if (frame == pauseFrame)
                    {
                        stop = true;
                        this.pauseFrame = true;
                    }
                }
                if (!stop) frame++;
                if (frame >= maxLengthFrame && !stop)
                {
                    if (!loop) stop = true;
                    frame = 0;
                }
                if(!stop) timer.Reset();
                nextFrame = true;
                Console.WriteLine("test 0," + frame);
            }
        }

        public void Pause()
        {
            stop = true;
        }

        public void AddFuturePause(int frame)
        {
            futureFrames.Add(frame);
        }

        public void RemoveFuturePause(int frame)
        {
            futureFrames.RemoveAt(frame);
        }

        public void Resume()
        {
            stop = false;
            if (pauseFrame)
            {
                frame++;
                pauseFrame = false;
            }
        }

        public void Dispose()
        {
            foreach(Texture texture in frames)
            {
                texture.Dispose();
            }
            futureFrames.Clear();
        }

        public int Frame
        {
            get { return frame; }
        }

        public bool PauseAnim
        {
            get { return stop; }
            set { stop = value; }
        }

        public Texture FrameTexture
        {
            get { try { return frames[frame]; } catch { return frames[0]; } }
        }

        public bool NextFrame
        {
            get { return nextFrame; }
        }
    }
}
