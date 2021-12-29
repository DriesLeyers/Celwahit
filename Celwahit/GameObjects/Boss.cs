using Celwahit.AnimationGameObjects;
using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Celwahit.GameObjects
{
    class Boss : EnemyObject
    {
        Animation gettingReadyAnimation;
        Animation shootingAnimation;

        private bool cooldown = false;
        private bool isActive = false;

        //Direction direction;

        public Boss(Texture2D idleBoss, Texture2D walkingBoss, Texture2D gettingReadyBoss, Texture2D shootingBoss, Texture2D bullet, Texture2D healthBar, int startPlaceX, int startPlaceY)
        {
            idleAnimation = BossAnimationBuilder.IdleAnimation(idleBoss);
            walkingAnimation = BossAnimationBuilder.WalkingAnimation(walkingBoss);
            gettingReadyAnimation = BossAnimationBuilder.GettingReadyAnimation(gettingReadyBoss);
            shootingAnimation = BossAnimationBuilder.ShootingAnimation(shootingBoss);

            this.healthBar = healthBar;
            this.blueprintBullet = new Bullet(bullet);

            direction = Direction.Idle;

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, shootingBoss.Bounds.Width, shootingBoss.Bounds.Height);
            hasJumped = true;

            position = new Vector2(startPlaceX, startPlaceY);
            velocity = new Vector2(0, 0);
            Health = 500;
            MaxHealth = 500;
            velocity.Y += 3f;
        }

        public override void Update(GameTime gameTime, Player player, List<Bullet> bullets, bool playerDead)
        {
            base.Update(gameTime, player, bullets, playerDead);
            shootingAnimation.Update(gameTime, 12);
            SetBulletData(30, 30);

            if (gameTime.TotalGameTime.Seconds % 3 >= 0 && gameTime.TotalGameTime.Seconds % 3 < 2)
            {
                if (gameTime.TotalGameTime.Milliseconds % 200 == 0 && !isShooting)
                {
                    Shoot(bullets);
                    //Debug.WriteLine("Boss: Shoot");
                }

                if (gameTime.TotalGameTime.Milliseconds % 200 != 0 && isShooting)
                {
                    isShooting = false;
                    //Debug.WriteLine("Boss: no shoot"); 
                }
            }
            if (isActive)
                SetDirectionToPlayer(player, 330);

            Ideling(gameTime, player, 900);
        }

        private void Ideling(GameTime gameTime,Player player, int distance)
        {
            float sPosX = this.position.X;
            float pPosX = player.Positition.X;

            float sPosY = this.position.Y;
            float pPosY = player.Positition.Y;

            if (Math.Abs(pPosX - sPosX) <= distance)
            {
                isActive = true;
                Debug.WriteLine("isActive");
            }
            else
            {
                Debug.WriteLine("isInactive");
                velocity.X = 1;
                if (gameTime.TotalGameTime.Seconds % 6 >= 0 && gameTime.TotalGameTime.Seconds % 6 < 5)
                {
                    velocity.X *= -1;
                }
                isActive = false;
            }
        }

        protected void DrawHealthBar(SpriteBatch spriteBatch)
        {
            float percentHealth = (float)Health / MaxHealth;
            int barWidth = (int)(healthBar.Width * percentHealth) / 10;

            var barPos = new Vector2(position.X - barWidth / 4, position.Y - 20);
            spriteBatch.Draw(healthBar, barPos, new Rectangle(0, 0, barWidth, 8), Color.White);
        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(isActive)
                DrawHealthBar(spriteBatch);

            if (isShooting)
            {
                if(playerFlipped)
                    spriteBatch.Draw(shootingAnimation.Texture, position, shootingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
                if (!playerFlipped)
                    spriteBatch.Draw(shootingAnimation.Texture, position, shootingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 1f);
            }
            else if(!isShooting)
            {
                if (direction == Direction.Left)
                {
                    spriteBatch.Draw(walkingAnimation.Texture, position, walkingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 1f);
                }
                else if (direction == Direction.Right)
                {
                    spriteBatch.Draw(walkingAnimation.Texture, position, walkingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
                }
                else if (direction == Direction.Idle && !playerFlipped)
                {
                    spriteBatch.Draw(idleAnimation.Texture, position, idleAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 1f);
                }
                else if (direction == Direction.Idle && playerFlipped)
                {
                    spriteBatch.Draw(idleAnimation.Texture, position, idleAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
                }
            }
            //draw de rest
        }

    }


}
