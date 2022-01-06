using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celwahit.AnimationGameObjects
{
    static class SoldierAnimationBuilder
    {
        public static Animation WalkingAnimation(Texture2D walkingSoldier)
        {
            Animation animation = new Animation();
            animation.Texture = walkingSoldier;
            int numberOfFrames = 12;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 27, 41)));
                moveRectangle_X += 27;
            }

            return animation;
        }

        public static Animation IdleAnimation(Texture2D idleSoldier)
        {
            Animation animation = new Animation();
            animation.Texture = idleSoldier;
            int numberOfFrames = 6;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 27, 38)));
                moveRectangle_X += 27;
            }

            return animation;
        }
    }
}
