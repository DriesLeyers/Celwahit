using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit
{
    static class RectangleHelper
    {
        public static bool TouchTopOf(this Rectangle r1, Rectangle r2)
        {
            return r1.Top < r2.Bottom &&
             r1.Bottom > r2.Bottom &&
             r1.Right > r2.Left &&
             r1.Left < r2.Right;
        }

        public static bool TouchBottomOf(this Rectangle r1, Rectangle r2)
        {
            return r1.Bottom > r2.Top &&
              r1.Top < r2.Top &&
              r1.Right > r2.Left &&
              r1.Left < r2.Right;
        }

        public static bool TouchLeftOf(this Rectangle r1, Rectangle r2, Vector2 velocity)
        {
            return r1.Left + velocity.X < r2.Right &&
              r1.Right > r2.Right &&
              r1.Bottom > r2.Top + 10 &&
              r1.Top < r2.Bottom - 10;
        }

        public static bool TouchRightOf(this Rectangle r1, Rectangle r2, Vector2 velocity)
        {
            return r1.Right + velocity.X > r2.Left &&
               r1.Left < r2.Left &&
               r1.Bottom > r2.Top + 10 &&
               r1.Top < r2.Bottom - 10;
        }
    }
}
