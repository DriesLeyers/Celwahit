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

            CollisionRect = new Rectangle((int)position.X, (int)position.Y, idleSoldier.Bounds.Width, idleSoldier.Bounds.Height);

            position = new Vector2(150, 150);
            velocity = new Vector2(0, 0);
        }

        public void Update(GameTime gameTime)
        {
            idleAnimation.Update(gameTime, 7);
            walkingAnimation.Update(gameTime, 12);
            //Gravity();

            direction = Direction.Right;
            velocity.Y += 0.15f * 1.0f;

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
    }
}
