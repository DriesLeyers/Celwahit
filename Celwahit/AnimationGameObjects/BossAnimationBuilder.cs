using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celwahit.AnimationGameObjects
{
    class BossAnimationBuilder
    {
        public static Animation WalkingAnimation(Texture2D walkingBoss)
        {
            Animation animation = new Animation();
            animation.Texture = walkingBoss;
            int numberOfFrames = 8;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 65, 42)));
                moveRectangle_X += 67;
            }

            return animation;
        }

        public static Animation IdleAnimation(Texture2D idleBoss)
        {
            Animation animation = new Animation();
            animation.Texture = idleBoss;
            int numberOfFrames = 3;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 57, 46)));
                moveRectangle_X += 57;
            }

            return animation;
        }

        public static Animation ShootingAnimation(Texture2D shootingBoss)
        {
            Animation animation = new Animation();
            animation.Texture = shootingBoss;
            int numberOfFrames = 11;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 65, 42)));
                moveRectangle_X += 71;
            }

            return animation;
        }

        public static Animation GettingReadyAnimation(Texture2D gettingReadyBoss)
        {
            Animation animation = new Animation();
            animation.Texture = gettingReadyBoss;
            int numberOfFrames = 10;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 65, 42)));
                moveRectangle_X += 65;
            }

            return animation;
        }
    }
}
