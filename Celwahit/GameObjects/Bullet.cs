using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Celwahit.GameObjects
{
    public class Bullet : Sprite
    {
        private float timer;
        public bool isRemoved;
        public float LifeSpan = 0f;
        public bool isFlipped = false;

        public Rectangle collisionRectangle;

        public Bullet(Texture2D texture = null) : base(texture)
        {
            this.collisionRectangle.Width = texture.Width;
            this.collisionRectangle.Height = texture.Height;
        }

        public override void Update(GameTime gameTime, List<Bullet> bullets)
        {
            this.collisionRectangle.X = (int)position.X;
            this.collisionRectangle.Y = (int)position.Y;
            timer = 0;

            if (timer > LifeSpan)
                isRemoved = true;
            if (isFlipped)
                position -= _velocity;
            else
                position += _velocity;

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].isRemoved)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isFlipped)
            {
                spriteBatch.Draw(_texture, position, null, Color.White, _rotation, origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            spriteBatch.Draw(_texture, position, null, Color.White, _rotation, origin, 1, SpriteEffects.None, 0);
        }

        public void Collision()
        {
            isRemoved = true;
        }

    }
}
