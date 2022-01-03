using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Interfaces
{
    interface IGameSettings
    {

        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public Vector2 StartButtonPos { get; set; }

        public GraphicsDeviceManager Graphics { get; set; }

        public float[] GetWindowScale();

    }
}

