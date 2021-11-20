using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Celwahit.AnimationGameObjects;

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
        Vector2 bodyOffset;


        public Player(Texture2D playerBody, Texture2D playerLegs)
        {
            this.playerBody = playerBody;
            this.playerLegs = playerLegs;

            animationBody = new Animation();
            animationLegs = new Animation();

            position = new Vector2(10,10);
            velocity = new Vector2(1.5f,0);
            bodyOffset = new Vector2(0,18);

            setFrames();
        }

        public void setFrames()
        {
            //Body frames
            int moveRectangleBody_X = 0;
            for(int i = 0; i < 4; i++)
            {
                animationBody.AddFrame(new AnimationFrame(new Rectangle(moveRectangleBody_X, 0, 32, 29)));
                moveRectangleBody_X += 32;
            }

            //Legs frames
            int moveRectangleLegs_X = 0;
            for(int i = 0; i < 4; i++)
            {
                animationLegs.AddFrame(new AnimationFrame(new Rectangle(moveRectangleLegs_X, 23, 32, 28)));
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
            position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(playerLegs, position+bodyOffset, animationLegs.CurrentFrame.SourceRect, Color.White);
            spriteBatch.Draw(playerBody, position, animationBody.CurrentFrame.SourceRect, Color.White);
        }
    }
}
