using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celwahit.AnimationGameObjects
{
    static class PlayerAnimationBuilder
    {
        public static Animation IdleAnimationBody(Texture2D idlePlayerBody)
        {
            Animation animation = new Animation();
            animation.Texture = idlePlayerBody;
            int numberOfFrames = 4;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 32, 26)));
                moveRectangle_X += 32;
            }

            return animation;
        }

        public static Animation IdleAnimationLegs(Texture2D idlePlayerLegs)
        {
            Animation animation = new Animation();
            animation.Texture = idlePlayerLegs;
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 28)));
            return animation;
        }

        public static Animation WalkingAnimationBody(Texture2D walkingPlayerBody)
        {
            Animation animation = new Animation();
            animation.Texture = walkingPlayerBody;
            int numberOfFrames = 12;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 32, 29)));
                moveRectangle_X += 32;
            }

            return animation;
        }

        public static Animation WalkingAnimationLegs(Texture2D walkingPlayerLegs)
        {
            Animation animation = new Animation();
            animation.Texture = walkingPlayerLegs;
            int numberOfFrames = 12;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 32, 29)));
                moveRectangle_X += 32;
            }

            return animation;
        }
    }
}
