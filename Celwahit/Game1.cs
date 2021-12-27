using Celwahit.Collisions;
using Celwahit.GameObjects;
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
        //private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Map map;

        GameSettings gameSettings;
        Background background;
        Skybox skybox;

        List<Bullet> bulletsPlayer = new List<Bullet>();
        List<Bullet> bulletsSoldier = new List<Bullet>();

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

        #region boss
        Boss boss;
        Texture2D walkingBoss;
        Texture2D idleBoss;
        Texture2D shootingBoss;
        Texture2D gettingReadyBoss;
        #endregion

        Texture2D backgroundTexture;
        Texture2D skyboxTexture;

        Texture2D bulletTexture;

        private Texture2D startButton;

        bool soldierDead = false;
        bool bossDead = false;
        bool playerDead = false;

        MouseState mouseState;
        MouseState previousMouseState;

        //Rectangle _groundRect;

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

            //_groundRect = new Rectangle(0, 0, 0, 0);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            map = new Map();

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

            Tiles.Content = Content;

            int[,] mapArray = makeMap();

            map.Generate(mapArray, 32);

            startButton = Content.Load<Texture2D>("startscherm");

            walkingPlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Walking");
            walkingPlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Walking");

            bulletTexture = Content.Load<Texture2D>("SgBullet");

            idlePlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Idle");
            idlePlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Idle");

            idleSoldier = Content.Load<Texture2D>("Soldier_Idle");
            walkingSoldier = Content.Load<Texture2D>("Soldier_Walking");

            backgroundTexture = Content.Load<Texture2D>("plx-5");
            skyboxTexture = Content.Load<Texture2D>("Mission1_Background3");

            idleBoss= Content.Load<Texture2D>("Idle_Boss");
            walkingBoss= Content.Load<Texture2D>("Walking_Boss");
            shootingBoss= Content.Load<Texture2D>("Shooting_Boss");
            gettingReadyBoss = Content.Load<Texture2D>("Get_Ready_Boss");

            InitializeGameObjects();
        }
        private void InitializeGameObjects()
        {
            player = new Player(walkingPlayerBody, walkingPlayerLegs, idlePlayerBody, idlePlayerLegs, bulletTexture);
            soldier = new Soldier(idleSoldier, walkingSoldier, 150, 0, bulletTexture);

            boss = new Boss(idleBoss, walkingBoss, gettingReadyBoss, shootingBoss, 2000, 0);

            background = new Background(backgroundTexture);
            skybox = new Skybox(skyboxTexture);

            background = new Background(backgroundTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();

            if (gameState == GameState.StartMenu && startScreen.CheckIfWantToPlay(previousMouseState))
            {
                gameState = GameState.Playing;
            }

            if (gameState == GameState.Playing)
            {

                foreach (Bullet bullet in bulletsPlayer.ToArray())
                    bullet.Update(gameTime, bulletsPlayer);

                foreach (Bullet bullet in bulletsSoldier.ToArray())
                    bullet.Update(gameTime, bulletsSoldier);

                var soldierRectForBulletHit = soldier.CollisionRect;
                soldierRectForBulletHit.Width -= 2;
                soldierRectForBulletHit.X += 8;

                foreach (CollisionTiles tile in map.CollisionTiles)
                {
                    if (!playerDead && CollisionManager.CheckCollision(tile.Rectangle, player.CollisionRect))
                    {
                        player.Collision(tile.Rectangle, map.Width, map.Height);
                    }

                    if (!soldierDead && CollisionManager.CheckCollision(tile.Rectangle, soldier.CollisionRect))
                        soldier.Collision(tile.Rectangle, map.Width, map.Height);

                    if (!bossDead && CollisionManager.CheckCollision(tile.Rectangle, boss.CollisionRect))
                        boss.Collision(tile.Rectangle, map.Width, map.Height);
                }

                var bossRectForBulletHit = boss.CollisionRect;
                bossRectForBulletHit.Width -= 2;
                bossRectForBulletHit.X += 8;

                foreach (Bullet bullet in bulletsPlayer.ToArray())
                {
                    foreach (CollisionTiles tile in map.CollisionTiles)
                    {
                        if ((CollisionManager.CheckCollision(tile.Rectangle, bullet.collisionRectangle)))
                        {
                            bullet.Collision();
                        }
                        else if (CollisionManager.CheckCollision(soldierRectForBulletHit, bullet.collisionRectangle) && !soldierDead)
                        {
                            bullet.Collision();
                            soldier.Health -= 25;
                            if (soldier.Health == 0)
                            {
                                soldierDead = true;
                                Debug.WriteLine("soldier died");
                            }

                            break;
                        }
                        else if (CollisionManager.CheckCollision(bossRectForBulletHit, bullet.collisionRectangle) && !bossDead)
                        {
                            bullet.Collision();
                            boss.Health -= 25;
                            if (boss.Health == 0)
                            {
                                bossDead = true;
                                Debug.WriteLine("Boss died");
                            }

                            break;
                        }
                    }
                }

                var playerRectForBulletHit = player.CollisionRect;
                playerRectForBulletHit.Width -= 2;
                playerRectForBulletHit.X += 8;

                foreach (Bullet bullet in bulletsSoldier.ToArray())
                {
                    foreach (CollisionTiles tile in map.CollisionTiles)
                    {
                        if ((CollisionManager.CheckCollision(tile.Rectangle, bullet.collisionRectangle)))
                        {
                            bullet.Collision();
                        }
                        else if (CollisionManager.CheckCollision(playerRectForBulletHit, bullet.collisionRectangle) && !playerDead)
                        {
                            bullet.Collision();
                            player.Health -= 25;
                            if (player.Health == 0)
                            {
                                playerDead = true;
                                Debug.WriteLine("player died");
                            }

                            break;
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

                    player.Update(gameTime, bulletsPlayer);
                }

                if (!soldierDead)
                {
                    var temp2 = soldier.CollisionRect;
                    temp2.Height += 6;

                    if (!map.CollisionTiles.Any(x => CollisionManager.CheckCollision(x.Rectangle, temp2)) && !soldier.hasJumped)
                    {
                        soldier.hasJumped = true;
                    }

                    soldier.Update(gameTime, player, bulletsSoldier, playerDead);
                }
                boss.Update(gameTime, player);
            }

            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Matrix matrix;

            Color color = new Color(43, 87, 84);

            GraphicsDevice.Clear(color);
            if (player.Positition.X < 385)
            {
                matrix = NotFollowPlayer(true);
            }else if(player.Positition.X > 1561.5)
            {
                matrix = NotFollowPlayer(false);
            }
            else
            {
                matrix = FollowPlayer();
            }

            if (gameState == GameState.StartMenu)
            {
                startScreen.DrawVectorStartButton(startButton, _spriteBatch);
            }

            if (gameState == GameState.Playing)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: matrix);

                float[] tmep = gameSettings.GetWindowScale();

                skybox.Draw(_spriteBatch, tmep[0]);
                background.Draw(_spriteBatch, tmep[0]);

                if(!playerDead)
                    player.Draw(_spriteBatch, gameTime);

                if (!soldierDead)
                    soldier.Draw(_spriteBatch, gameTime);

                if (!bossDead)
                    boss.Draw(_spriteBatch, gameTime);

                foreach (Bullet bullet in bulletsPlayer.ToArray())
                    bullet.Draw(_spriteBatch, gameTime);

                foreach (Bullet bullet in bulletsSoldier.ToArray())
                    bullet.Draw(_spriteBatch, gameTime);

                map.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
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
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
            };

            return mapArray;
        }
    }
}
