using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.GameObjects
{
    public class CharacterObject : IGameObject, ICollisionGameObject, IGunSound
    {
        public Rectangle CollisionRect { get; set; }
        protected Rectangle _collisionRectangle;

        public SoundEffect gunSound { get; set; }

        protected bool isShooting = false;
        protected bool playerFlipped = false;
        public bool IsFlipped;
        public bool hasJumped;

        public Direction direction;

        public int Health = 100;
        public int MaxHealth = 100;

        protected Vector2 position = new Vector2(64, 384);

        public Vector2 Positition
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 velocity;

        public virtual void Update(GameTime gameTime, List<Bullet> bullets)
        {
            throw new NotImplementedException();
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public virtual void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            throw new NotImplementedException();
        }
    }
}
