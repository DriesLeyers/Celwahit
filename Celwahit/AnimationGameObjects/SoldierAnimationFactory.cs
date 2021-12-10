using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    class SoldierAnimationFactory //: IAnimationFactory
    {
        #region Soldier

        public static Animation WalkingAnimation(Texture2D walkingTexture)
        {
            Animation animation = new Animation();

            int moveRectangle_X = 0;
            for (int i = 0; i < 12; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 27, 41)));
                moveRectangle_X += 27;
            }

            return animation;
        }

        public static Animation IdleAnimation(Texture2D idleTexture)
        {
            Animation animation = new Animation();

            int moveRectangle_X = 0;
            for (int i = 0; i < 6; i++)
            {
                animation.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 27, 38)));
                moveRectangle_X += 27;
            }

            return animation;
        }

        #endregion Soldier


    }
}
