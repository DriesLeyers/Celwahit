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
    class Soldier : CharacterObject, IGameObject, ICollisionGameObject
    {
        //public Rectangle CollisionRect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Animation walkingAnimation;
        Animation idleAnimation;

        protected Direction direction;

        Bullet bullet;

        Texture2D walkingSoldierTexture;

        //Vector2 position;
        //Vector2 velocity;

        public int Health = 100;

        bool playerFlipped = false;

        public Soldier(Texture2D idleSoldier, Texture2D walkingSoldier, int startPlaceX, int startPlaceY, Texture2D bullet)
        {
            this.bullet = new Bullet(bullet);
            this.walkingSoldierTexture = walkingSoldier;

            walkingAnimation = SoldierAnimationBuilder.WalkingAnimation(walkingSoldier);
            idleAnimation = SoldierAnimationBuilder.IdleAnimation(idleSoldier);

            direction = Direction.Idle;

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, idleSoldier.Bounds.Width, idleSoldier.Bounds.Height);
            hasJumped = true;

            position = new Vector2(startPlaceX, startPlaceY);
            velocity = new Vector2(0, 0);

            velocity.Y += 3f;

        }

        public Soldier()
        {
        }

        private void Jump()
        {
            Debug.WriteLine("Jump");
            if (!hasJumped)
            {
                position.Y -= 10;
                velocity.Y = -2.5f;
            }
            hasJumped = true;
            position += velocity;
        }

        private void Shoot(List<Bullet> bullets)
        {
                bullets.Add(AddBullet());
        }

        public Bullet AddBullet()
        {
            var newBullet = this.bullet.Clone() as Bullet;
            return newBullet;
        }

        public void Update(GameTime gameTime, Player player, List<Bullet> bullets)
        {
            idleAnimation.Update(gameTime, 7);
            walkingAnimation.Update(gameTime, 12);
            if (hasJumped)
            {
                Gravity();
                velocity.Y += 0.15f * 1.0f;
            }

            if (!hasJumped)
            {
                velocity.Y = 0f;
            }
            position += velocity;

            CollisionRect = new Rectangle((int)this.Positition.X,(int) this.Positition.Y, idleAnimation.CurrentFrame.SourceRect.Width, idleAnimation.CurrentFrame.SourceRect.Height);
            _collisionRectangle = CollisionRect;
            direction = Direction.Right;

            SetBulletData();
            Shoot(bullets);

            SetDirectionToPlayer(player, 50);
        }

        public void Update(GameTime gameTime, List<Bullet> bullets)
        {
            throw new NotImplementedException();
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
                playerFlipped = true;
            }
            else if(pPosX < sPosX)
            {
                playerFlipped = true;
                direction = Direction.Left;
                playerFlipped = false;
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

        protected void Gravity()
        {
            velocity.Y += 0.15f * 1.0f;
            position.Y += velocity.Y;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
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

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            _collisionRectangle.X = (int)position.X;
            _collisionRectangle.Y = (int)position.Y;

            if (_collisionRectangle.TouchRightOf(newRectangle, velocity))
            {
                Debug.WriteLine("right");
                position.X = newRectangle.X-32;
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

        private void SetBulletData()
        {
            this.bullet.isFlipped = playerFlipped;

            if (IsFlipped)
                this.bullet.position = new Vector2(this.position.X - 5, this.position.Y + 5 + walkingSoldierTexture.Bounds.Height / 2);
            else
                this.bullet.position = new Vector2(this.position.X + 35, this.position.Y + 5 + walkingSoldierTexture.Bounds.Height / 2);

            this.bullet._velocity = new Vector2(7f, 0f);
            this.bullet.LifeSpan = 3f;
        }

    }
}
