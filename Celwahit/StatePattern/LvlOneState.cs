using Celwahit;
using Celwahit.Collisions;
using Celwahit.GameObjects;
using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Celwahit.Scenes
{
    public class LvlOneState : SceneState, IGameObject
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

        Texture2D gameOver;

        #region boss
        Boss boss;
        Texture2D walkingBoss;
        Texture2D idleBoss;
        Texture2D shootingBoss;
        Texture2D gettingReadyBoss;
        Texture2D healthBar;
        Texture2D pHealthBar;
        #endregion

        Texture2D backgroundTexture;
        Texture2D skyboxTexture;

        Texture2D bulletTexture;

        bool soldierDead = false;
        bool bossDead = false;
        bool playerDead = false;


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
            {
                boss.Draw(_spriteBatch, gameTime);
            }

            foreach (Bullet bullet in bulletsPlayer.ToArray())
                bullet.Draw(_spriteBatch, gameTime);

            foreach (Bullet bullet in bulletsSoldier.ToArray())
                bullet.Draw(_spriteBatch, gameTime);

            map.Draw(_spriteBatch);

            if (playerDead)
            {
                player.Positition = new Vector2(0, player.Positition.Y);

                _spriteBatch.Draw(gameOver, new Vector2(0, -240), Color.White);
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


            map.Generate(mapArray, 32, "level1");

            walkingPlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Walking");
            walkingPlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Walking");

            bulletTexture = Content.Load<Texture2D>("SgBullet");

            idlePlayerBody = Content.Load<Texture2D>("Player/Fiolina_Top_Idle");
            idlePlayerLegs = Content.Load<Texture2D>("Player/Fiolina_Bot_Idle");

            idleSoldier = Content.Load<Texture2D>("Soldier_Idle");
            walkingSoldier = Content.Load<Texture2D>("Soldier_Walking");

            gameOver = Content.Load<Texture2D>("GameOver");

            backgroundTexture = Content.Load<Texture2D>("plx-5");
            skyboxTexture = Content.Load<Texture2D>("Mission1_Background3");

            idleBoss = Content.Load<Texture2D>("Idle_Boss");
            walkingBoss = Content.Load<Texture2D>("Walking_Boss");
            shootingBoss = Content.Load<Texture2D>("Shooting_Boss");
            gettingReadyBoss = Content.Load<Texture2D>("Get_Ready_Boss");

            healthBar = Content.Load<Texture2D>("Health_Bar");
            pHealthBar = Content.Load<Texture2D>("P_Health_Bar");


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
                            Game1.ChangeSceneState(new LvlTwoState(Game1, _graphics, _spriteBatch, player));
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
                        }
                        break;
                    }
                }
            }

            //Bij presentatie zeg da wij jumpe door dirty flag pattern.
            //Dus de hasjumped is de dirty flag

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

            if(!bossDead)
            {
                var temp3 = boss.CollisionRect;
                temp3.Height += 6;

                if (!map.CollisionTiles.Any(x => CollisionManager.CheckCollision(x.Rectangle, temp3)) && !boss.hasJumped)
                {
                    boss.hasJumped = true;
                }

                boss.Update(gameTime, player, bulletsSoldier, playerDead);
            }

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && playerDead)
            {
                Game1.ChangeSceneState(new LvlOneState(Game1, _graphics, _spriteBatch));
            }


        }

        private void InitializeGameObjects()
        {
            player = new Player(walkingPlayerBody, walkingPlayerLegs, idlePlayerBody, idlePlayerLegs, bulletTexture, pHealthBar);
            player.gunSound = Content.Load<SoundEffect>("gun1");

            soldier = new Soldier(idleSoldier, walkingSoldier, 500, 0, bulletTexture, healthBar);
            soldier.gunSound = Content.Load<SoundEffect>("gun2");

            boss = new Boss(idleBoss, walkingBoss, gettingReadyBoss, shootingBoss, bulletTexture,healthBar , 1750, 0);
            boss.gunSound = Content.Load<SoundEffect>("gun3");

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
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
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

        public void Update(GameTime gameTime, List<Bullet> bullets)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
