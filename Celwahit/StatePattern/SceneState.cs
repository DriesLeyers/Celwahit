using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Celwahit.Interfaces;

namespace Celwahit.Scenes
{
    public abstract class SceneState : IGameState
    {
        public Game1 Game1 { get; set; }

        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public ContentManager Content;
        public GraphicsDevice GraphicsDevice;

        protected SceneState(Game1 game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.Game1 = game;
            this._graphics = graphics;
            this._spriteBatch = spriteBatch;
            this.Content = game.Content;
            this.GraphicsDevice = game.GraphicsDevice;
        }

        public abstract void Initialize();

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}