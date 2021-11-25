using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Collisions
{
    interface ICollisionGameObject
    {
        Rectangle CollisionRectangle { get; set; }

        private void UpdateCollision()
        {

        }
    }
}
