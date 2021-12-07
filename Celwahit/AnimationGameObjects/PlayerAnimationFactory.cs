﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.AnimationGameObjects
{
    class AnimationFactory
    {
        #region Player
        #region Idle
        public static Animation IdleAnimationPlayerBody(Texture2D idlePlayerBody)
        {
            Animation animation = new Animation();

            Animation animationBody = new Animation();

            int moveRectangle_X = 0;
            for (int i = 0; i < 4; i++)
            {
                animationBody.AddFrame(new AnimationFrame(new Rectangle(moveRectangle_X, 0, 32, 26)));
                moveRectangle_X += 32;
            }

            return animationBody;

        }
        public static Animation IdleAnimationPlayerLegs(Texture2D idlePlayerLegs)
        {
            Animation animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 28)));
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
        #endregion Player

        #region Soldier

        public static Animation WalkingAnimationSoldier(Texture2D walkingSoldier)
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

        public static Animation IdleAnimationSoldier(Texture2D IdleSoldier)
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