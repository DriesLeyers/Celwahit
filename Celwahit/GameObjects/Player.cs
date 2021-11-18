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


        //private Rectangle partRectangleBody;
        //private int moveRectangleBody_X = 36;

        //private Rectangle partRectangleLegs;
        //private int moveRectangleLegs_X = 32;


        public Player(Texture2D playerBody, Texture2D playerLegs)
        {
            this.playerBody = playerBody;
            this.playerLegs = playerLegs;

            animationBody = new Animation();
            animationLegs = new Animation();
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
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(playerLegs, new Vector2(0, 18), animationLegs.CurrentFrame.SourceRect, Color.White);
            spriteBatch.Draw(playerBody, new Vector2(0, 0), animationBody.CurrentFrame.SourceRect, Color.White);
        }
    }
}
