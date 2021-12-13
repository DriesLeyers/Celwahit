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
using System.Diagnostics;

namespace Celwahit.GameObjects
{
    public class Player :CharacterObject //: IGameObject //, ICollisionGameObject
    {
        Animation walkingAnimationBody;
        Animation walkingAnimationLegs;

        Animation idleAnimationBody;
        Animation idleAnimationLegs;

        Texture2D walkingPlayerLegs;
        Texture2D walkingPlayerBody;
        Texture2D idlePlayerBody;
        Texture2D idlePlayerLegs;

        KeyboardState _previousKey;
        KeyboardState _currentKey;

        //in da filmpje van collision heeft die en _collisionRect en CollisionRect


               



        //Vector2 position;
        
        Vector2 acceleration;
        //To get the sprites properly aligned
        Vector2 bodyOffset;

        Direction direction;

        bool hasJumped;

        

        Bullet bullet;


        public Player(Texture2D walkingPlayerBody, Texture2D walkingPlayerLegs, Texture2D idlePlayerBody, Texture2D idlePlayerLegs, Texture2D bullet)
        {
            this.bullet = new Bullet(bullet);
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

            position = new Vector2(0, 0);
            velocity = new Vector2(1.5f, 0);
            acceleration = new Vector2(0.0f, 0.0f);
            bodyOffset = new Vector2(0, 10);

            CollisionRect = new Rectangle((int)position.X, (int)position.Y, 32, 80);

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, 32, 38);

            if (velocity.Y < 10)
                velocity.Y += 0.4f;
        }

        public void Update(GameTime gameTime, List<Bullet> bullets)
        {
            Rectangle _collisionRect = CollisionRect;
            //8, 12 MN for making sprite move normally
            walkingAnimationBody.Update(gameTime, 8);
            walkingAnimationLegs.Update(gameTime, 12);
            idleAnimationBody.Update(gameTime, 8);
            idleAnimationLegs.Update(gameTime, 12);
            Move();
            SetBulletData();
            Shoot(bullets);

            if (hasJumped)
            {
                velocity.Y += 0.15f * 1.0f;
            }

            if (!hasJumped)
            {
                velocity.Y = 0f;
            }

            position += velocity;
            _collisionRect.X = (int)position.X;
            _collisionRect.Y = (int)position.Y;

            CollisionRect = _collisionRect;
        }

        private void Shoot(List<Bullet> bullets)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            if(_currentKey.IsKeyDown(Keys.LeftControl) && _previousKey.IsKeyUp(Keys.LeftControl))
            {
                bullets.Add(AddBullet());
            }
        }

        private void SetBulletData()
        {
            this.bullet.isFlipped = IsFlipped;
            this.bullet.position = new Vector2(this.position.X+35, this.position.Y+walkingPlayerBody.Bounds.Height/2);
            this.bullet._velocity = new Vector2(7f,0f);
            this.bullet.LifeSpan = 3f;
        }

        public void StopJump()
        {
            hasJumped = false;
            velocity.Y = 0;
        }

        private void Jump()
        {
            velocity.X = 0;
            acceleration = new Vector2(0, 0);

            Debug.WriteLine("Jump");
            if (!hasJumped)
            {
                position.Y -= 10f;
                velocity.Y = -2.5f;
            }
            hasJumped = true;

            position += velocity;
        }

        private void Move()
        {
            Keys[] pressedKeys = KeyboardReader.GetKeys();

            if (!(pressedKeys.Length == 0))
            {
                for (int i = 0; i < pressedKeys.Length; i++)
                {
                    switch (pressedKeys[i])
                    {
                        case Keys.Right:
                            direction = Direction.Right;
                            velocity.X = 1.5f;
                            //acceleration.X = 0.25f;
                            IsFlipped = false;
                            //Accelerate();
                            break;
                        case Keys.Left:
                            direction = Direction.Left;
                            velocity.X = -1.5f;
                            //Check tutorial 
                            //acceleration.X = -0.25f;
                            IsFlipped = true;
                            //Accelerate();
                            break;
                        case Keys.Up:
                            direction = Direction.Jumping;
                            if (!hasJumped)
                                Jump();
                            //Accelerate();
                            break;
                        case Keys.Down:
                            velocity.X = 0;
                            acceleration = new Vector2(0, 0);
                            direction = Direction.Crouching;
                            break;

                    }

                    position += velocity;
                }
            }
            else
            {
                direction = Direction.Idle;
                velocity.X = 0;
                acceleration = new Vector2(0, 0);
                //Accelerate();
                position += velocity;
            }
        }

        public Bullet AddBullet()
        {
            
            var newBullet = this.bullet.Clone() as Bullet;
            return newBullet;
            
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
                spriteBatch.Draw(walkingPlayerLegs, position + bodyOffset, walkingAnimationLegs.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
                spriteBatch.Draw(walkingPlayerBody, position, walkingAnimationBody.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
            else if (direction == Direction.Right)
            {
                spriteBatch.Draw(walkingPlayerLegs, position + bodyOffset, walkingAnimationLegs.CurrentFrame.SourceRect, Color.White);
                spriteBatch.Draw(walkingPlayerBody, position, walkingAnimationBody.CurrentFrame.SourceRect, Color.White);
            }
            else
            {
                if (IsFlipped)
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

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            _collisionRectangle.X = (int)position.X;
            _collisionRectangle.Y = (int)position.Y;

            if (_collisionRectangle.TouchRightOf(newRectangle, velocity))
            {
                Debug.WriteLine("right");

                position.X = newRectangle.X - _collisionRectangle.Width;
            }
            else if (_collisionRectangle.TouchLeftOf(newRectangle, velocity) && velocity.X < 0)
            {
                Debug.WriteLine("left");
                position.X = newRectangle.X + newRectangle.Width;

            } else
            if (_collisionRectangle.TouchBottomOf(newRectangle))
            {
                Debug.WriteLine("bottom");

                position.Y = newRectangle.Y - 38;
                velocity.Y = 0f;
                hasJumped = false;
            }
            else if(_collisionRectangle.TouchTopOf(newRectangle))
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
