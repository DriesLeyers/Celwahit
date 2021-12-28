using Celwahit.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Interfaces
{
    public enum Direction
    {
        Idle,
        Right,
        Left,
        Falling,
        Jumping,
        Crouching
    };

    interface IGameObject
    {
        public void Update(GameTime gameTime, List<Bullet> bullets);

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
