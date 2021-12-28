using Celwahit.Collisions;
using Celwahit.GameObjects;
using Celwahit.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Celwahit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public SceneState SceneState { get; set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            //_groundRect = new Rectangle(0, 0, 0, 0);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.SceneState = new MenuState(this, _graphics, _spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            this.SceneState.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color color = new Color(43, 87, 84);
            GraphicsDevice.Clear(color);

            this.SceneState.Draw(gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ChangeSceneState(SceneState sceneState)
        {
            this.SceneState = sceneState;
        }

    }
}
