using Celwahit;
using Celwahit.Collisions;
using Celwahit.GameObjects;
using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Celwahit.Scenes
{
    public class LvlTwoState : SceneState, IGameState
    {
        Map map;

        GameSettings gameSettings;
        Background background;
        Skybox skybox;

        SoundEffect won;

        #region player
        bool playerWon;
        bool playWinMusic = true;
        Player player;
        Texture2D walkingPlayerBody;
        Texture2D walkingPlayerLegs;
        Texture2D idlePlayerBody;
        Texture2D idlePlayerLegs;
        #endregion player

        Texture2D gameOver;
        Texture2D youWin;

        #region soldier
        #endregion soldier

        Texture2D pHealthBar;

        Texture2D backgroundTexture;
        Texture2D skyboxTexture;

        Texture2D bulletTexture;

        bool playerDead = false;

        public LvlTwoState(Game1 game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch) : base(game, graphics, spriteBatch)
        {
            LoadContent();
        }

        public LvlTwoState(Game1 game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Player player) : base(game, graphics, spriteBatch)
        {
            LoadContent();
            this.player.Health = player.Health;
        }

        public override void Draw(GameTime gameTime)
        {
            Color color = new Color(123, 72, 0);
            GraphicsDevice.Clear(color);

            Matrix matrix;

            if (player.Positition.X < 385)
            {
                matrix = NotFollowPlayer(true);
            }
            else if (player.Positition.X > 1561.5)
            {
                matrix = NotFollowPlayer(false);
            }
            else
            {
                matrix = FollowPlayer();
            }

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: matrix);

            float[] tmep = gameSettings.GetWindowScale();


            skybox.Draw(_spriteBatch, tmep[0]);

            if (!playerDead)
                player.Draw(_spriteBatch, gameTime);

            map.Draw(_spriteBatch);

            if (playerDead)
            {
                player.Positition = new Vector2(0, player.Positition.Y);

                _spriteBatch.Draw(gameOver, new Vector2(0, -240), Color.White);
            }

            if (playerWon)
            {
                player.Positition = new Vector2(2300, player.Positition.Y);
            }

            if (player.Positition.X >= 2250)
            {
                if (playWinMusic)
                {
                    won.Play();
                }
                playWinMusic = false;
                playerWon = true;
                _spriteBatch.Draw(youWin, new Vector2(1175, -240), Color.White);
            }
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            map = new Map();

            Tiles.Content = Content;

            int[,] mapArray = makeMap();

            gameSettings = new GameSettings(_graphics);
            gameSettings.WindowHeight = 480;
            gameSettings.WindowWidth = 800;

            map.Generate(mapArray, 32,"level2");

            won = Content.Load<SoundEffect>("dead");

            youWin = Content.Load<Texture2D>("win");
            gameOver = Content.Load<Texture2D>("GameOver");
            walkingPlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Walking");
            walkingPlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Walking");

            bulletTexture = Content.Load<Texture2D>("SgBullet");

            idlePlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Idle");
            idlePlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Idle");

            backgroundTexture = Content.Load<Texture2D>("plx-5");
            skyboxTexture = Content.Load<Texture2D>("background_Lvl2");

            pHealthBar = Content.Load<Texture2D>("P_Health_Bar");

            InitializeGameObjects();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (CollisionTiles tile in map.CollisionTiles)
            {
                if(tile.typeTile == TypeTiles.Spike)
                {
                    player.isOnSpike(tile.Rectangle, map.Width, map.Height);
                    if(player.Health <= 0)
                    {
                       playerDead = true;
                    }
                }
                else
                {
                    if (!playerDead && CollisionManager.CheckCollision(tile.Rectangle, player.CollisionRect))
                    {
                        player.Collision(tile.Rectangle, map.Width, map.Height);
                    }
                }
            }

            if (!playerDead)
            {
                var temp = player.CollisionRect;
                temp.Height += 6;

                if (!map.CollisionTiles.Any(x => CollisionManager.CheckCollision(x.Rectangle, temp)) && !player.hasJumped)
                {
                    player.hasJumped = true;
                }

                player.Update(gameTime);
            }

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && playerDead)
            {
                Game1.ChangeSceneState(new LvlOneState(Game1, _graphics, _spriteBatch));
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && playerWon)
            {
                Game1.ChangeSceneState(new LvlOneState(Game1, _graphics, _spriteBatch));
            }
        }

        private void InitializeGameObjects()
        {
            player = new Player(walkingPlayerBody, walkingPlayerLegs, idlePlayerBody, idlePlayerLegs, bulletTexture, pHealthBar);

            background = new Background(backgroundTexture);
            skybox = new Skybox(skyboxTexture);

            background = new Background(backgroundTexture);
        }

        private int[,] makeMap()
        {
            int[,] mapArray = new int[,]
            {
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,2,0,0,1,1,2,2,2,2,2,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,2,2,2,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1,2,1,1,1,1,2,1,2,1,2,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,},
            };

            return mapArray;
        }

        private Matrix FollowPlayer()
        {
            var position = Matrix.CreateTranslation(-player.CollisionRect.X - (player.CollisionRect.Width / 2),/* -player.CollisionRect.Y - (player.CollisionRect.Height / 2)*/ 0, 0);

            var offset = Matrix.CreateTranslation(
                gameSettings.WindowWidth / 2,
                gameSettings.WindowHeight / 2, 0);

            return position * offset;
        }

        private Matrix NotFollowPlayer(bool playerIsAtBegin)
        {
            var offset = new Matrix();
            var position = new Matrix();

            if (playerIsAtBegin)
            {
                position = Matrix.CreateTranslation(-800, 0, 0);

                offset = Matrix.CreateTranslation(
                    gameSettings.WindowWidth,
                    gameSettings.WindowHeight / 2, 0);
            }
            else
            {
                position = Matrix.CreateTranslation(-1975, 0, 0);

                offset = Matrix.CreateTranslation(
                    gameSettings.WindowWidth,
                    gameSettings.WindowHeight / 2, 0);
            }


            return position * offset;
        }
    }
}
