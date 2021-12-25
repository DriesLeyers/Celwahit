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
    class Soldier : CharacterObject//, //IGameObject//, ICollisionGameObject
    {
        //public Rectangle CollisionRect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Animation walkingAnimation;
        Animation idleAnimation;

        Direction direction;

        //Vector2 position;
        //Vector2 velocity;

        public int Health = 100;

        bool playerFlipped = false;

        public Soldier(Texture2D idleSoldier, Texture2D walkingSoldier, int startPlaceX, int startPlaceY)
        {
            walkingAnimation = SoldierAnimationBuilder.WalkingAnimation(walkingSoldier);
            idleAnimation = SoldierAnimationBuilder.IdleAnimation(idleSoldier);

            direction = Direction.Idle;

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, idleSoldier.Bounds.Width, idleSoldier.Bounds.Height);
            hasJumped = true;

            position = new Vector2(startPlaceX, startPlaceY);
            velocity = new Vector2(0, 0);

            velocity.Y += 3f;

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

        public void Update(GameTime gameTime, Player player)
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
            direction = Direction.Right;
            
            SetDirectionToPlayer(player);
        }

        private void SetDirectionToPlayer(Player player)
        {
            float sPosX = this.position.X;
            float pPosX = player.Positition.X;

            float sPosY = this.position.Y;
            float pPosY = player.Positition.Y;

            if (pPosX > sPosX)
            {
                direction = Direction.Right;
            }
            else if(pPosX < sPosX)
            {
                direction = Direction.Left;
            }

            if (Math.Abs(pPosX - sPosX) >= 150)
            {
                if (direction == Direction.Right)
                    velocity.X = 1.5f;
                if (direction == Direction.Left)
                    velocity.X = -1.5f;
            }
            else
                velocity.X = 0;
        }

        private void Gravity()
        {
            //throw new NotImplementedException();
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
            //Debug.WriteLine(velocity.Y);

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
                Debug.WriteLine("Soldier: bottom");

                this.position.Y = newRectangle.Y - 38;
                this.velocity.Y = 0f;
                this.hasJumped = false;
            }
            else if (_collisionRectangle.TouchTopOf(newRectangle))
            {
                Debug.WriteLine("top");

                velocity.Y = 1f;
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - _collisionRectangle.Width) position.X = xOffset - _collisionRectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - _collisionRectangle.Height) position.Y = yOffset - _collisionRectangle.Height;
        }

    }
}
