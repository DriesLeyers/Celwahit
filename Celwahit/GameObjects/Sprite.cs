using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.GameObjects
{
    public class Sprite //: IGameObject
    {
        protected Texture2D _texture;
        protected float _rotation;

        public Vector2 _velocity;
        protected Vector2 origin;
        public Vector2 position;


        public Sprite(Texture2D texture)
        {
            _texture = texture;
            origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }

        public virtual void Update(GameTime gameTime, List<Bullet> bullets)
        {
            
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
