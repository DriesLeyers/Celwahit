using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Celwahit.AnimationGameObjects;
using Microsoft.Xna.Framework.Input;

namespace Celwahit.GameObjects
{
    public class Player : IGameObject
    {
        Texture2D playerBody;
        Texture2D playerLegs;

        Animation animationBody;
        Animation animationLegs;

        Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;
        //To get the sprites properly aligned
        Vector2 bodyOffset;

        KeyboardState keyboardState;

        bool moveRight;

        public Player(Texture2D playerBody, Texture2D playerLegs)
        {
            this.playerBody = playerBody;
            this.playerLegs = playerLegs;

            animationBody = new Animation();
            animationLegs = new Animation();

            moveRight = true;

            position = new Vector2(10,10);
            velocity = new Vector2(1.5f,0);
            acceleration = new Vector2(0.1f, 0);
            bodyOffset = new Vector2(0,14);

            setFrames();
        }

        public void setFrames()
        {
            //TODO: Sprite afsnijden zodat geen random pixels :)
            //Body frames
            int moveRectangleBody_X = 0;
            for(int i = 0; i < 12; i++)
            {
                animationBody.AddFrame(new AnimationFrame(new Rectangle(moveRectangleBody_X, 0, 32, 29)));
                moveRectangleBody_X += 32;
            }

            //Legs frames
            int moveRectangleLegs_X = 0;
            for(int i = 0; i < 12; i++)
            {
                animationLegs.AddFrame(new AnimationFrame(new Rectangle(moveRectangleLegs_X, 0, 32, 28)));
                moveRectangleLegs_X += 32;
            }

        }

        public void Update(GameTime gameTime)
        {
            //8, 12 MN for making sprite move normally
            animationBody.Update(gameTime, 8);
            animationLegs.Update(gameTime, 12);
            Move();
        }

        private void Move()
        {
            keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            if (!(pressedKeys.Length == 0))
            {
                switch (pressedKeys[pressedKeys.Length - 1])
                {
                    case Keys.Right:
                        moveRight = true;
                        //velocity.X *= -1;
                        acceleration.X = 0.25f;
                        Accelerate();
                        break;
                    case Keys.Left:
                        moveRight = false;
                        //velocity.X *= -1;
                        acceleration.X = -0.25f;
                        Accelerate();
                        break;
                    default:
                        velocity = new Vector2(0, 0);
                        break;
                }
                position += velocity;
            }

            //if (position.x > 600 || position.x < 0)
            //{
            //    velocity.x *= -1;
            //    acceleration.x *= -1;

            //}
            //if (position.y > 400 || position.y < 0)
            //{
            //    velocity.y *= -1;
            //    acceleration.y *= -1;
            //}
        }

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
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!moveRight)
            {
                spriteBatch.Draw(playerLegs,position + bodyOffset, animationLegs.CurrentFrame.SourceRect, Color.White,0f,new Vector2(0,0),1, SpriteEffects.FlipHorizontally,1f);
                spriteBatch.Draw(playerBody, position, animationBody.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
            else
            {
                spriteBatch.Draw(playerLegs, position + bodyOffset, animationLegs.CurrentFrame.SourceRect, Color.White);
                spriteBatch.Draw(playerBody, position, animationBody.CurrentFrame.SourceRect, Color.White);
            }

        }
    }
}
