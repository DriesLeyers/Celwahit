using Celwahit.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Celwahit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerBody;
        Texture2D playerLegs;

        Texture2D temp;

        Player player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerLegs = Content.Load<Texture2D>("PlayerLegsSheet");
            playerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Walking");

            temp = Content.Load<Texture2D>("Fiolina_Bot_Walking");

            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            player = new Player(playerBody, playerLegs);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            player.Draw(_spriteBatch, gameTime);
            _spriteBatch.Draw(temp, new Vector2(0, 18), new Rectangle(0,50,384,30),Color.White);

            _spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
