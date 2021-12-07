using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Celwahit
{
    class GameSettings : IGameSettings
    {

        public int WindowHeight { get ; set ; }
        public int WindowWidth { get ; set ; }
        public Vector2 StartButtonPos { get ; set; }
        public GraphicsDeviceManager Graphics { get; set; }

        private int targetScreenWidth;
        private int targetScreenHeight;

        public GameSettings(GraphicsDeviceManager graphics)
        {
            this.Graphics = graphics;

            WindowHeight = graphics.PreferredBackBufferHeight;
            WindowWidth = graphics.PreferredBackBufferWidth;

            targetScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            targetScreenWidth = Screen.PrimaryScreen.Bounds.Width;

        }

        public float[] GetWindowScale()
        {
            float tempHeight = (float)targetScreenHeight / WindowHeight;
            float tempWidth = (float)targetScreenWidth / WindowWidth;
            float[] tempArray = { tempWidth, tempHeight };
            return tempArray;
        }
    }
}
