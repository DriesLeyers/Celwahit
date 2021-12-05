using Microsoft.Xna.Framework;

namespace Celwahit.Interfaces
{
    interface IGameSettings
    {

        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public Vector2 StartButtonPos { get; set; }

        public GraphicsDeviceManager Graphics { get; set; }

    }
}