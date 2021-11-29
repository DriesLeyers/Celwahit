using Celwahit.AnimationGameObjects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Interfaces
{
    interface IAnimationFactory
    {
        public Animation WalkingAnimation(Texture2D walkingTexture);

        public Animation IdleAnimation(Texture2D idleTexture);
    }
}
