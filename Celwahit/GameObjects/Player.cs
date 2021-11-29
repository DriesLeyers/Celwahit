using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Celwahit.AnimationGameObjects;
using Microsoft.Xna.Framework.Input;
using Celwahit.Collisions;
using Celwahit.InputReaders;

namespace Celwahit.GameObjects
{
    public class Player : IGameObject, ICollisionGameObject
    {
        Animation walkingAnimationBody;
        Animation walkingAnimationLegs;

        Animation idleAnimationBody;
        Animation idleAnimationLegs;

        Texture2D walkingPlayerLegs;
        Texture2D walkingPlayerBody;
        Texture2D idlePlayerBody;
        Texture2D idlePlayerLegs;

        //in da filmpje van collision heeft die en _collisionRect en CollisionRect
        public Rectangle CollisionRect { get; set; }

        Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;
        //To get the sprites properly aligned
        Vector2 bodyOffset;

        bool playerFlipped;
        bool hasJumped;

        Direction direction;


        public Player(Texture2D walkingPlayerBody, Texture2D walkingPlayerLegs, Texture2D idlePlayerBody, Texture2D idlePlayerLegs)
        {

            this.walkingPlayerBody = walkingPlayerBody;
            this.walkingPlayerLegs = walkingPlayerLegs;
            this.idlePlayerBody = idlePlayerBody;
            this.idlePlayerLegs = idlePlayerLegs;

            walkingAnimationBody = PlayerAnimationBuilder.WalkingAnimationBody(walkingPlayerBody);
            walkingAnimationLegs = PlayerAnimationBuilder.WalkingAnimationLegs(walkingPlayerLegs);

            idleAnimationBody = PlayerAnimationBuilder.IdleAnimationBody(idlePlayerBody);
            idleAnimationLegs = PlayerAnimationBuilder.IdleAnimationLegs(idlePlayerLegs);

            direction = Direction.Idle;

            hasJumped = true;

            position = new Vector2(150,150);
            velocity = new Vector2(1.5f,0);
            acceleration = new Vector2(0.1f, 0.1f);
            bodyOffset = new Vector2(0,10);

            CollisionRect = new Rectangle((int)position.X, (int)position.Y, 32, 80);
        }

        public void Update(GameTime gameTime)
        {
            Rectangle _collisionRect = CollisionRect;
            //8, 12 MN for making sprite move normally
            walkingAnimationBody.Update(gameTime, 8);
            walkingAnimationLegs.Update(gameTime, 12);
            idleAnimationBody.Update(gameTime, 8);
            idleAnimationLegs.Update(gameTime, 12);
            Move();

            //Jump();

            if (hasJumped)
            {
                float i = 1;
                velocity.Y += 0.15f * i;
            }

            //if(position.Y >= 300)
            //{
            //    hasJumped = false;
            //}

            if (!hasJumped)
            {
                velocity.Y = 0f;
            }

            position += velocity;
            _collisionRect.X = (int)position.X;
            _collisionRect.Y = (int)position.Y;

            CollisionRect = _collisionRect;
        }

        public void StopJump()
        {
            hasJumped = false;
            velocity.Y = 0;
        }

        private void Jump()
        {
            if (!hasJumped)
            {
                position.Y -= 10f;
                velocity.Y = -5f;
            }
            hasJumped = true;
            position += velocity;
        }


        private void Move()
        {
            Keys[] pressedKeys = KeyboardReader.GetKeys();

            if (!(pressedKeys.Length == 0))
            {
                switch (pressedKeys[pressedKeys.Length - 1])
                {
                    //TODO: RECHTS + SPRING GAAT NIET LINKS + SPRING WEL FIX

                    case Keys.Right:
                        direction = Direction.Right;
                        //velocity.X *= -1;
                        acceleration.X = 0.25f;
                        playerFlipped = false;
                        Accelerate();
                        break;
                    case Keys.Left:
                        direction = Direction.Left;
                        //velocity.X *= -1;
                        //Check tutorial 
                        acceleration.X = -0.25f;
                        playerFlipped = true;
                        Accelerate();
                        break;
                    case Keys.Up:
                        direction = Direction.Jumping;
                        if (!hasJumped)
                            Jump();
                        Accelerate();
                        break;
                    case Keys.Down:
                        direction = Direction.Crouching;
                        break;
                    default:
                        velocity = new Vector2(0, 0);
                        acceleration = new Vector2(0, 0);
                        break;
                }
                position += velocity;
            }
            else
            {
                direction = Direction.Idle;
                Accelerate();
            }
        }
        //TODO: Movement.cs maken fzoeit
        private void Accelerate()
        {
            velocity += acceleration;
            velocity = Limit(velocity, 1.5f);
            
        }

        /// <summary>
        /// Limits the max length of a given Vector2
        /// </summary>
        /// <param name="vector">The vector thats needs to limited</param>
        /// <param name="maxVectorLength">max length of vector</param>
        /// <returns></returns>
        private Vector2 Limit(Vector2 vector, float maxVectorLength)
        {
            if (vector.Length() > maxVectorLength)
            {
                var ratio = maxVectorLength / vector.Length();
                vector.X *= ratio;
                vector.Y *= ratio;
            }

            return vector;
        }

        //TODO: drawFactory mss?
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (direction == Direction.Left)
            {
                spriteBatch.Draw(walkingPlayerLegs,position + bodyOffset, walkingAnimationLegs.CurrentFrame.SourceRect, Color.White,0f,new Vector2(0,0),1, SpriteEffects.FlipHorizontally,1f);
                spriteBatch.Draw(walkingPlayerBody, position, walkingAnimationBody.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
            else if(direction == Direction.Right)
            {
                spriteBatch.Draw(walkingPlayerLegs, position + bodyOffset, walkingAnimationLegs.CurrentFrame.SourceRect, Color.White);
                spriteBatch.Draw(walkingPlayerBody, position, walkingAnimationBody.CurrentFrame.SourceRect, Color.White);
            }
            else 
            {
                if (playerFlipped)
                {
                    spriteBatch.Draw(idlePlayerLegs, position + bodyOffset, idleAnimationLegs.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
                    spriteBatch.Draw(idlePlayerBody, position, idleAnimationBody.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
                }
                else
                {
                    spriteBatch.Draw(idlePlayerLegs, position + bodyOffset, idleAnimationLegs.CurrentFrame.SourceRect, Color.White);
                    spriteBatch.Draw(idlePlayerBody, position, idleAnimationBody.CurrentFrame.SourceRect, Color.White);
                }
            }

        }
    }
}
