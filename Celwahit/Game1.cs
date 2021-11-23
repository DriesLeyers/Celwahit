using Celwahit.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Celwahit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D walkingPlayerBody;
        Texture2D walkingPlayerLegs;

        Texture2D idlePlayerBody;
        Texture2D idlePlayerLegs;

        private Texture2D startButton;
        private Vector2 startButtonPosition;

        private Thread backgroundThread;
        MouseState mouseState;
        MouseState previousMouseState;

        Player player;

        GameState gameState;
        enum GameState
        {
            StartMenu,
            Playing,
            Paused
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            IsMouseVisible = true;
            LoadGame();

            startButtonPosition = new Vector2(450, 554);
            gameState = GameState.StartMenu;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            startButton = Content.Load<Texture2D>("startscherm");

            walkingPlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Walking");
            walkingPlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Walking");

            idlePlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Idle");
            idlePlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Idle");

            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            player = new Player(walkingPlayerBody, walkingPlayerLegs, idlePlayerBody, idlePlayerLegs);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            mouseState = Mouse.GetState();

            KeyboardState keyboardState = Keyboard.GetState();

            if (gameState == GameState.StartMenu && keyboardState.IsKeyDown(Keys.Enter))
            {
                gameState = GameState.Playing;
            }

            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released && gameState == GameState.StartMenu)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (gameState == GameState.StartMenu)
            {
                _spriteBatch.Draw(startButton, new Vector2(0, 0), Color.White);
            }

            if (gameState == GameState.Playing)
            {
                player.Draw(_spriteBatch, gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 375, 100);

                if (mouseClickRect.Intersects(startButtonRect))
                {
                    gameState = GameState.Playing;
                }
            }
        }

        void LoadGame()
        {

        }
    }
}
