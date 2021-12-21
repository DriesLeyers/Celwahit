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
    class Soldier : CharacterObject, IGameObject//, ICollisionGameObject
    {
        //public Rectangle CollisionRect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Animation walkingAnimation;
        Animation idleAnimation;

        Direction direction;


        //Vector2 position;
        //Vector2 velocity;

        bool playerFlipped = false;

        public Soldier(Texture2D idleSoldier, Texture2D walkingSoldier)
        {

            walkingAnimation = SoldierAnimationBuilder.WalkingAnimation(walkingSoldier);
            idleAnimation = SoldierAnimationBuilder.IdleAnimation(idleSoldier);

            direction = Direction.Idle;

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, idleSoldier.Bounds.Width, idleSoldier.Bounds.Height);

            position = new Vector2(150, 150);
            velocity = new Vector2(0, 0);

            velocity.Y += 3f;

        }

        public void Update(GameTime gameTime)
        {
            idleAnimation.Update(gameTime, 7);
            walkingAnimation.Update(gameTime, 12);
            //Gravity();

            CollisionRect = new Rectangle((int)this.Positition.X,(int) this.Positition.Y, idleAnimation.CurrentFrame.SourceRect.Width, idleAnimation.CurrentFrame.SourceRect.Height);

            direction = Direction.Right;

            velocity.X = 1.5f;

            position += velocity;

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

                position.X = newRectangle.X - _collisionRectangle.Width;
            }
            else if (_collisionRectangle.TouchLeftOf(newRectangle, velocity) && velocity.X < 0)
            {
                Debug.WriteLine("left");
                position.X = newRectangle.X + newRectangle.Width;

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
