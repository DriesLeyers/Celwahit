using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Collisions
{
    interface ICollisionGameObject
    {
        public Rectangle CollisionRect { get; set; }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset);
    }
}
