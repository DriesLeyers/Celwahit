using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 66, 45)));
                moveRectangle_X += 66;
            }

            return animation;
        }

        public static Animation IdleAnimation(Texture2D idleBoss )
        {
            Animation animation = new Animation();
            animation.Texture = idleBoss;
            int numberOfFrames = 3;

            int moveRectangle_X = 0;
            for (int i = 0; i < numberOfFrames; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 64, 46)));
                moveRectangle_X += 64;
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
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 68, 50)));
                moveRectangle_X += 67;
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
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 65, 44)));
                moveRectangle_X += 65;
            }

            return animation;
        }
    }
}
