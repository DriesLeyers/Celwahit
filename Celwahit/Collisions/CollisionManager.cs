using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Celwahit.Collisions
{
    static public class CollisionManager
    {
        static public bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Intersects(rect2))
            {
                return true;
            }
            return false;
        }
    }
}
