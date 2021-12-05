using Celwahit.Collisions;
using Celwahit.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Celwahit
{
    public class Game1 : Game
    {
        //private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GameSettings gameSettings;

        #region player
        Player player;
        Texture2D walkingPlayerBody;
        Texture2D walkingPlayerLegs;
        Texture2D idlePlayerBody;
        Texture2D idlePlayerLegs;
        #endregion player

        #region soldier
        Soldier soldier;
        Texture2D walkingSoldier;
        Texture2D idleSoldier;
        #endregion soldier

        private List<Rectangle> tileList = new List<Rectangle>();
        Texture2D tile;

        private Texture2D startButton;
        private Vector2 startButtonPosition;

        private Thread backgroundThread;
        MouseState mouseState;
        MouseState previousMouseState;

        Rectangle _groundRect;

        GameState gameState;
        enum GameState
        {
            StartMenu,
            Playing,
            Paused
        }

        public Game1()
        {
            gameSettings = new GameSettings(new GraphicsDeviceManager(this));

            _groundRect = new Rectangle(0,300,1280,50);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            base.Initialize();

            gameSettings.Graphics.PreferredBackBufferWidth = 1280;
            gameSettings.Graphics.PreferredBackBufferHeight = 720;
            gameSettings.Graphics.ApplyChanges();

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

            idleSoldier = Content.Load<Texture2D>("Soldier_Idle");
            walkingSoldier = Content.Load<Texture2D>("Soldier_Walking");

            tile = Content.Load<Texture2D>("Tile");

            InitializeGameObjects();
            InitializeTiles();
        }

        private void InitializeGameObjects()
        {
            player = new Player(walkingPlayerBody, walkingPlayerLegs, idlePlayerBody, idlePlayerLegs);
            soldier = new Soldier(idleSoldier, walkingSoldier);
        }

        private void InitializeTiles()
        {
            
            for (int i = 0; i < 50; i++)
            {
                tileList.Add(new Rectangle(i*55,300,55,55));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //TODO: start screen apart in class zettten.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            if (gameState == GameState.StartMenu && keyboardState.IsKeyDown(Keys.Enter))
            {
                gameState = GameState.Playing;
            }
            if (gameState == GameState.StartMenu && previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            if(gameState == GameState.Playing)
            {
                Debug.Write(" " + player.CollisionRect.Y);
                if (CollisionManager.CheckCollision(_groundRect, player.CollisionRect))
                {
                    Debug.Write("hit ground");
                    player.StopJump();
                }

                player.Update(gameTime);
                soldier.Update(gameTime);
            }
            
            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            FollowPlayer();
            //_spriteBatch.Begin();

            if (gameState == GameState.StartMenu)
            {
                _spriteBatch.Draw(startButton, new Vector2(0, 0), Color.White);
            }

            if (gameState == GameState.Playing)
            {
                player.Draw(_spriteBatch, gameTime);
                soldier.Draw(_spriteBatch, gameTime);
                foreach(Rectangle rectangle in tileList)
                {
                    _spriteBatch.Draw(tile, startButtonPosition, rectangle, Color.White);
                    startButtonPosition.X += 20;
                }
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

        private void FollowPlayer()
        {
            var position = Matrix.CreateTranslation(-player.CollisionRect.X - (player.CollisionRect.Width / 2), -player.CollisionRect.Y - (player.CollisionRect.Height / 2), 0);

            var offset = Matrix.CreateTranslation(
                gameSettings.WindowWidth/ 2,
                gameSettings.WindowHeight/ 2, 0);

            var Transform = position * offset;

            _spriteBatch.Begin(transformMatrix: Transform);
            
        }
    }
}
