using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.GameObjects
{
    public class CharacterObject
    {
        public Rectangle CollisionRect { get; set; }
        protected Rectangle _collisionRectangle;

        protected bool isShooting = false;
        protected bool playerFlipped = false;
        public bool IsFlipped;
        public bool hasJumped;

        public Direction direction;

        public int Health = 100;

        protected Vector2 position = new Vector2(64, 384);

        public Vector2 Positition
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 velocity;





    }
}
