using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Interfaces
{
    interface IGameSettings
    {
        public GraphicsDevice GraphicsDevice { get; set; }

        public GraphicsDeviceManager GraphicsManager { get; set; }

        public SpriteBatch SpriteBatch { get; set; }



    }
}
