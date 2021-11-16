using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    public class AnimationFrame
    {
        public Rectangle SourceRect { get; set; }

        public AnimationFrame(Rectangle rectangle)
        {
            SourceRect = rectangle;
        }
    }
}
