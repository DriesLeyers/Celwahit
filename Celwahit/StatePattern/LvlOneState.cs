using Celwahit;
using Celwahit.Collisions;
using Celwahit.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Celwahit.Scenes
{
    public class LvlOneState : SceneState
    {
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

        public LvlOneState(Game1 game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch) : base(game, graphics, spriteBatch)
        {
            LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
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
            background.Draw(_spriteBatch, tmep[0]);

            if (!playerDead)
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

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            map = new Map();

            Tiles.Content = Content;

            int[,] mapArray = makeMap();

            gameSettings = new GameSettings(_graphics);
            map.Generate(mapArray, 32);

            walkingPlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Walking");
            walkingPlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Walking");

            bulletTexture = Content.Load<Texture2D>("SgBullet");

            idlePlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Idle");
            idlePlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Idle");

            idleSoldier = Content.Load<Texture2D>("Soldier_Idle");
            walkingSoldier = Content.Load<Texture2D>("Soldier_Walking");

            backgroundTexture = Content.Load<Texture2D>("plx-5");
            skyboxTexture = Content.Load<Texture2D>("Mission1_Background3");

            idleBoss = Content.Load<Texture2D>("Idle_Boss");
            walkingBoss = Content.Load<Texture2D>("Walking_Boss");
            shootingBoss = Content.Load<Texture2D>("Shooting_Boss");
            gettingReadyBoss = Content.Load<Texture2D>("Get_Ready_Boss");

            InitializeGameObjects();
        }

        public override void Update(GameTime gameTime)
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
                            //TODO Game Over Screen

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

        private void InitializeGameObjects()
        {
            player = new Player(walkingPlayerBody, walkingPlayerLegs, idlePlayerBody, idlePlayerLegs, bulletTexture);
            soldier = new Soldier(idleSoldier, walkingSoldier, 150, 0, bulletTexture);

            boss = new Boss(idleBoss, walkingBoss, gettingReadyBoss, shootingBoss, 2000, 0);

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
