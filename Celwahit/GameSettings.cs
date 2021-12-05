using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit
{
    class GameSettings : IGameSettings
    {

        public int WindowHeight { get ; set ; }
        public int WindowWidth { get ; set ; }
        public Vector2 StartButtonPos { get ; set; }
        public GraphicsDeviceManager Graphics { get; set; }

        public GameSettings(GraphicsDeviceManager graphics)
        {
            this.Graphics = graphics;

            WindowHeight = graphics.PreferredBackBufferHeight;
            WindowWidth = graphics.PreferredBackBufferWidth;

        }
    }
}
