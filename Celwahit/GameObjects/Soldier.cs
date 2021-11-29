using Celwahit.AnimationGameObjects;
using Celwahit.Collisions;
using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.GameObjects
{
    class Soldier : ICollisionGameObject, IGameObject
    {
        public Rectangle CollisionRect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Texture2D idleSoldier;
        Texture2D walkingSoldier;

        Animation walkingAnimation;
        Animation idleAnimation;

        Direction direction;


        Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;

        bool playerFlipped = false;

        public Soldier(Texture2D idleSoldier, Texture2D walkingSoldier)
        {
            this.idleSoldier = idleSoldier;
            this.walkingSoldier = walkingSoldier;

            walkingAnimation = AnimationBuilder.WalkingAnimationSoldier(walkingSoldier);
            idleAnimation = AnimationBuilder.IdleAnimationSoldier(idleSoldier);

            direction = Direction.Idle;


            position = new Vector2(150, 150);
            velocity = new Vector2(1.5f, 0);
            acceleration = new Vector2(0.1f, 0.1f);
        }

        public void Update(GameTime gameTime)
        {
            idleAnimation.Update(gameTime, 7);
            walkingAnimation.Update(gameTime, 12);

            direction = Direction.Left;
            position += velocity;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (direction == Direction.Right)
            {
                spriteBatch.Draw(walkingSoldier, position ,walkingAnimation.CurrentFrame.SourceRect, Color.White);
            }
            else if(direction == Direction.Left)
            {
                spriteBatch.Draw(walkingSoldier, position , walkingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
            else if (direction == Direction.Idle && !playerFlipped)
            {
                spriteBatch.Draw(idleSoldier, position, idleAnimation.CurrentFrame.SourceRect, Color.White);
            }
            else if (direction == Direction.Idle && playerFlipped)
            {
                spriteBatch.Draw(idleSoldier, position, idleAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
        }
    }
}
