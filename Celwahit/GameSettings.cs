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
        public GraphicsDevice GraphicsDevice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GraphicsDeviceManager GraphicsManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SpriteBatch SpriteBatch { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ScreenWidth;
        public int ScreenHeight;

        public GameSettings(GraphicsDevice GD, GraphicsDeviceManager GM, SpriteBatch SB)
        {
            GraphicsDevice = GD;
            GraphicsManager = GM;
            SpriteBatch = SB;

            
        }
    }
}
