using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    public class Animation
    {
        public AnimationFrame CurrentFrame { get; set; }

        private List<AnimationFrame> frames;

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

        public void Update()
        {
            CurrentFrame = frames[counter];
            counter++;

            if (counter >= frames.Count)
                counter = 0;
        }


    }
}
