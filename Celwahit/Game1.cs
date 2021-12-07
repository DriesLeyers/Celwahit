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
        Background background;
        Skybox skybox;

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
        Texture2D backgroundTexture;
        Texture2D skyboxTexture;

        private Texture2D startButton;

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

        StartScreen startScreen;

        public Game1()
        {
            gameSettings = new GameSettings(new GraphicsDeviceManager(this));

            _groundRect = new Rectangle(0, 0, 1280, 50);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            base.Initialize();

            startScreen = new StartScreen(gameSettings);

            IsMouseVisible = true;

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

            backgroundTexture = Content.Load<Texture2D>("plx-5");
            skyboxTexture = Content.Load<Texture2D>("Mission1_Background3");

            tile = Content.Load<Texture2D>("jungle_tileset");

            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            player = new Player(walkingPlayerBody, walkingPlayerLegs, idlePlayerBody, idlePlayerLegs);
            soldier = new Soldier(idleSoldier, walkingSoldier);
<<<<<<< Updated upstream
            background = new Background(backgroundTexture, 384, 216);
            _groundRect.Y = (int)(background.height*gameSettings.GetWindowScale()[0]);
=======
            background = new Background(backgroundTexture);
            skybox = new Skybox(skyboxTexture);
            _groundRect.Y = (int)(background.height*gameSettings.GetWindowScale()[0])-50;
>>>>>>> Stashed changes

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();

            Debug.Write("Mousepos.X: " + mouseState.X + "\n"
            + "Mousepos.Y: " + mouseState.Y + "\n");
            
            if(gameState == GameState.StartMenu)
            {
                if (startScreen.CheckIfWantToPlay(previousMouseState))
                    gameState = GameState.Playing;
            }

            if (gameState == GameState.Playing)
            {
                Debug.Write("\n" + player.CollisionRect.Y + "\n");
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
            Matrix matrix = FollowPlayer();

            if (gameState == GameState.StartMenu)
            {
                startScreen.DrawVectorStartButton(startButton, _spriteBatch);                
            }

            if (gameState == GameState.Playing)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred,null, SamplerState.PointClamp, transformMatrix: matrix);
                //_spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
                float[] tmep = gameSettings.GetWindowScale();
                Debug.Write(tmep);
                skybox.Draw(_spriteBatch, tmep[0]);
                background.Draw(_spriteBatch, tmep[0]);
                player.Draw(_spriteBatch, gameTime);
                soldier.Draw(_spriteBatch, gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
   
        private Matrix FollowPlayer()
        {
            var position = Matrix.CreateTranslation(-player.CollisionRect.X - (player.CollisionRect.Width / 2),/* -player.CollisionRect.Y - (player.CollisionRect.Height / 2)*/ 0, 0);

            var offset = Matrix.CreateTranslation(
                gameSettings.WindowWidth/ 2,
                gameSettings.WindowHeight/ 2, 0);

            var Transform = position * offset;

            return Transform;
            
        }
    }
}
