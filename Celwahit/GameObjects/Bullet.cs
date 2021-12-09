using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.GameObjects
{
    public class Bullet : Sprite
    {
        private float timer;
        private float LifeSpan = 0f;
        private bool isRemoved;

        public Bullet(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > LifeSpan)
                isRemoved = true;

            position += _velocity;
        }
    }
}
