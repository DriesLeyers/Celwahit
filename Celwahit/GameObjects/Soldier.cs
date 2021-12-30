using Celwahit.AnimationGameObjects;
using Celwahit.Collisions;
using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Celwahit.GameObjects
{
    class Soldier : EnemyObject
    {
        Texture2D walkingSoldierTexture;

        public Soldier(Texture2D idleSoldier, Texture2D walkingSoldier, int startPlaceX, int startPlaceY, Texture2D bullet, Texture2D healthbar)
        {
            this.blueprintBullet = new Bullet(bullet);
            this.walkingSoldierTexture = walkingSoldier;

            walkingAnimation = SoldierAnimationBuilder.WalkingAnimation(walkingSoldier);
            idleAnimation = SoldierAnimationBuilder.IdleAnimation(idleSoldier);

            this.healthBar = healthbar;

            direction = Direction.Idle;

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, idleSoldier.Bounds.Width, idleSoldier.Bounds.Height);
            hasJumped = true;

            position = new Vector2(startPlaceX, 250);
            velocity = new Vector2(0, 0);

            velocity.Y += 3f;

        }

        public override void Update(GameTime gameTime, Player player, List<Bullet> bullets, bool playerDead)
        {
            base.Update(gameTime, player, bullets, playerDead);

            SetBulletData(10,17);
            if(gameTime.TotalGameTime.Seconds > 0.5)
            {
                if (gameTime.TotalGameTime.Seconds % 2 == 0 && !isShooting)
                {
                    isShooting = true;
                    if (!playerDead)
                        Shoot(bullets);
                }

                if (gameTime.TotalGameTime.Seconds % 2 != 0 && isShooting)
                    isShooting = false;

                SetDirectionToPlayer(player, 75);
            }
            
        }

        protected void DrawHealthBar(SpriteBatch spriteBatch)
        {
            float percentHealth = (float)Health / MaxHealth;
            int barWidth = (int)(healthBar.Width * percentHealth) / 10;

            var barPos = new Vector2(position.X - barWidth / 4, position.Y - 25);
            spriteBatch.Draw(healthBar, barPos, new Rectangle(0, 0, barWidth, 8), Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawHealthBar(spriteBatch);

            if (direction == Direction.Right)
            {
                spriteBatch.Draw(walkingAnimation.Texture, position ,walkingAnimation.CurrentFrame.SourceRect, Color.White);
            }
            else if(direction == Direction.Left)
            {
                spriteBatch.Draw(walkingAnimation.Texture, position , walkingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
            else if (direction == Direction.Idle && !playerFlipped)
            {
                spriteBatch.Draw(idleAnimation.Texture, position, idleAnimation.CurrentFrame.SourceRect, Color.White);
            }
            else if (direction == Direction.Idle && playerFlipped)
            {
                spriteBatch.Draw(idleAnimation.Texture, position, idleAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
        }
    }
}
