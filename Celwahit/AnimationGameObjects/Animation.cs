using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    public class Animation
    {
        public AnimationFrame CurrentFrame { get; set; }
        private List<AnimationFrame> frames;

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

        public void UpdateBody(GameTime gameTime)
        {
            CurrentFrame = frames[counter];
            frameMovement += CurrentFrame.SourceRect.Width * gameTime.ElapsedGameTime.TotalSeconds;

            if (frameMovement >= CurrentFrame.SourceRect.Width / 8)
            {
                counter++;
                frameMovement = 0;
            }
               
            if (counter >= frames.Count)
                counter = 0;
        }

        public void UpdateLegs(GameTime gameTime)
        {
            CurrentFrame = frames[counter];
            frameMovement += CurrentFrame.SourceRect.Width * gameTime.ElapsedGameTime.TotalSeconds;

            if (frameMovement >= CurrentFrame.SourceRect.Width / 12)
            {
                counter++;
                frameMovement = 0;
            }

            if (counter >= frames.Count)
                counter = 0;
        }
    }
}
