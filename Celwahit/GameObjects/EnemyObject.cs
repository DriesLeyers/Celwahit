using Celwahit.AnimationGameObjects;
using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Celwahit.GameObjects
{
    public class EnemyObject : CharacterObject
    {

        protected Animation idleAnimation;
        protected Animation walkingAnimation;

        protected Bullet blueprintBullet;

        public void Update(GameTime gameTime, Player player, List<Bullet> bullets, bool playerDead)
        {
            idleAnimation.Update(gameTime, 7);
            walkingAnimation.Update(gameTime, 12);

            if (hasJumped)
            {
                velocity.Y += 0.15f * 1.0f;
                Gravity();
            }

            if (!hasJumped)
            {
                velocity.Y = 0f;
            }
            position += velocity;

            CollisionRect = new Rectangle((int)this.Positition.X, (int)this.Positition.Y, idleAnimation.CurrentFrame.SourceRect.Width, idleAnimation.CurrentFrame.SourceRect.Height);
            _collisionRectangle = CollisionRect;
            direction = Direction.Right;


            SetBulletData(9);

            if (gameTime.TotalGameTime.Seconds % 2 == 0 && !isShooting)
            {
                isShooting = true;
                if (!playerDead)
                    Shoot(bullets);
            }

            if (gameTime.TotalGameTime.Seconds % 2 != 0 && isShooting)
                isShooting = false;

            SetDirectionToPlayer(player, 50);
        }


        #region Movement

        protected void Gravity()
        {
            velocity.Y += 0.15f * 1.0f;
            position.Y += velocity.Y;
        }

        protected void Jump()
        {
            if (!hasJumped)
            {
                position.Y -= 10;
                velocity.Y = -2.5f;
            }
            hasJumped = true;
            position += velocity;
        }

        public virtual void SetDirectionToPlayer(Player player, int distance)
        {
            float sPosX = this.position.X;
            float pPosX = player.Positition.X;

            float sPosY = this.position.Y;
            float pPosY = player.Positition.Y;

            if (pPosX > sPosX)
            {
                playerFlipped = false;
                direction = Direction.Right;
            }
            else if (pPosX < sPosX)
            {
                playerFlipped = true;
                direction = Direction.Left;
            }

            //TODO check op Y-as verschill da em onder u komt te staan

            if (Math.Abs(pPosX - sPosX) >= distance)
            {
                if (direction == Direction.Right)
                    velocity.X = 1.5f;
                if (direction == Direction.Left)
                    velocity.X = -1.5f;
            }
            else
                velocity.X = 0;
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            _collisionRectangle.X = (int)position.X;
            _collisionRectangle.Y = (int)position.Y;

            if (_collisionRectangle.TouchRightOf(newRectangle, velocity))
            {
                Debug.WriteLine("right");
                position.X = newRectangle.X - 32;
                if (!hasJumped)
                    Jump();
            }
            else if (_collisionRectangle.TouchLeftOf(newRectangle, velocity) && velocity.X < 0)
            {
                Debug.WriteLine("left");
                position.X = newRectangle.X + newRectangle.Width;
                if (!hasJumped)
                    Jump();

            }
            else
            if (_collisionRectangle.TouchBottomOf(newRectangle))
            {
                position.Y = newRectangle.Y - 38;
                hasJumped = false;
            }
            else if (_collisionRectangle.TouchTopOf(newRectangle))
                velocity.Y = 1f;

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - _collisionRectangle.Width) position.X = xOffset - _collisionRectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - _collisionRectangle.Height) position.Y = yOffset - _collisionRectangle.Height;
        }
        #endregion

        #region Bullets
        protected void SetBulletData(float bulletOffset)
        {
            this.blueprintBullet.isFlipped = playerFlipped;

            if (IsFlipped)
                this.blueprintBullet.position = new Vector2(this.position.X - 5, this.position.Y + 5 + bulletOffset);
            else
                this.blueprintBullet.position = new Vector2(this.position.X + 35, this.position.Y + 5 + bulletOffset);

            this.blueprintBullet._velocity = new Vector2(7f, 0f);
            this.blueprintBullet.LifeSpan = 3f;
        }
        protected void Shoot(List<Bullet> bullets)
        {
            bullets.Add(AddBullet());
        }
        private Bullet AddBullet()
        {
            var newBullet = this.blueprintBullet.Clone() as Bullet;
            return newBullet;
        }

        #endregion
    }
}
