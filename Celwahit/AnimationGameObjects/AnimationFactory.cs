using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    class AnimationFactory
    {
        #region Idle
        public static Animation IdleAnimationPlayerBody(Texture2D idlePlayerBody)
        {
            Animation animation = new Animation();





            return animation;
        }
        public static Animation IdleAnimationPlayerLegs(Texture2D idlePlayerLegs)
        {
            Animation animation = new Animation();




            return animation;
        }
        #endregion Idle
        #region Walking
        public static Animation WalkingAnimationPlayerBody(Texture2D walkingPlayerBody)
        {
            Animation animationBody = new Animation();

            int moveRectangle_X = 0;
            for (int i = 0; i < 12; i++)
            {
                animationBody.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 32, 29)));
                moveRectangle_X += 32;
            }

            return animationBody;
        }


        public static Animation WalkingAnimationPlayerLegs(Texture2D walkingPlayerLegs)
        {
            Animation animation = new Animation();

            int moveRectangle_X = 0;
            for (int i = 0; i < 12; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 32, 29)));
                moveRectangle_X += 32;
            }

            return animation;
        }
        #endregion Walking
    }
}
