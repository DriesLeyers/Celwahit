using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Interfaces
{
    interface ICollisionGameObject
    {
        public Rectangle CollisionRect { get; set; }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset);
    }
}
