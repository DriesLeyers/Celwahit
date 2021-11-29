using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    public class Animation
    {
        public AnimationFrame CurrentFrame { get; set; }
        private List<AnimationFrame> frames;
        public Texture2D Texture{ get; set; }

        private double frameMovement = 0;
        private int counter;

        public Animation()
        {
            frames = new List<AnimationFrame>();
        }

        public void AddFrame(AnimationFrame animationFrame)
        {
            frames.Add(animationFrame);
            CurrentFrame = frames[0];
        }

        public void Update(GameTime gameTime, int offsetTemp)
        {
            CurrentFrame = frames[counter];
            frameMovement += CurrentFrame.SourceRect.Width * gameTime.ElapsedGameTime.TotalSeconds;

            if (frameMovement >= CurrentFrame.SourceRect.Width / offsetTemp)
            {
                counter++;
                frameMovement = 0;
            }
               
            if (counter >= frames.Count)
                counter = 0;
        }
    }
}
