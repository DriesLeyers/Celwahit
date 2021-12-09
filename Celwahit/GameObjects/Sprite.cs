using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.GameObjects
{
    public class Sprite : IGameObject
    {
        protected Texture2D _texture;
        protected Vector2 _velocity;
        protected float _rotation;

        public Vector2 origin;
        public Vector2 position;


        public Sprite(Texture2D texture)
        {
            _texture = texture;
            origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, position, null, Color.White, _rotation, origin, 1, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
