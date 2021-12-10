using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    static class PlayerAnimationBuilder
    {
        #region Player
        #region Idle
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
        #endregion Idle
        #region Walking
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
        #endregion Walking
        #endregion Player
    }
}
