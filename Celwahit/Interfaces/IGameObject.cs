using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Interfaces
{
    /// <summary>
    /// Must be drawn to screen
    /// </summary>
    interface IGameObject
    {
        public void Draw() { } 
        public void Update(SpriteBatch spriteBatch) { }
    }
}
