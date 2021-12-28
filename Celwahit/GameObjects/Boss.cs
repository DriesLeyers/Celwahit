using Celwahit.AnimationGameObjects;
using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Celwahit.GameObjects
{
    class Boss : Soldier
    {

        Animation idleAnimation;
        Animation walkingAnimation;
        Animation gettingReadyAnimation;
        Animation shootingAnimation;

        
        //Direction direction;
        bool playerFlipped = false;
        bool gettingReady = false;

        public Boss(Texture2D idleBoss, Texture2D walkingBoss, Texture2D gettingReadyBoss, Texture2D shootingBoss, int startPlaceX, int startPlaceY)
        {
            idleAnimation = BossAnimationBuilder.IdleAnimation(idleBoss);
            walkingAnimation= BossAnimationBuilder.WalkingAnimation(walkingBoss);
            gettingReadyAnimation = BossAnimationBuilder.GettingReadyAnimation(gettingReadyBoss);
            shootingAnimation = BossAnimationBuilder.ShootingAnimation(shootingBoss);

            direction = Direction.Idle;

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, shootingBoss.Bounds.Width, shootingBoss.Bounds.Height);
            hasJumped = true;

            position = new Vector2(startPlaceX, startPlaceY);
            velocity = new Vector2(0, 0);
            Health = 1000;
            velocity.Y += 3f;
        }

      
        public new void Update(GameTime gameTime, Player player)
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

            CollisionRect = new Rectangle((int)this.Positition.X, (int)this.Positition.Y, idleAnimation.CurrentFrame.SourceRect.Width, idleAnimation.CurrentFrame.SourceRect.Height);
            _collisionRectangle = CollisionRect;
            direction = Direction.Right;

            Debug.WriteLine("BOSS MAN: " + this.direction);
            Debug.WriteLine("BOSS MAN: " + this.playerFlipped);

            SetDirectionToPlayer(player, 100);
            
        }

        public override void SetDirectionToPlayer(Player player, int distance)
        {
            float sPosX = this.position.X;
            float pPosX = player.Positition.X;

            float sPosY = this.position.Y;
            float pPosY = player.Positition.Y;

            if (pPosX > sPosX)
            {
                direction = Direction.Right;
                playerFlipped = true;
            }
            else if (pPosX < sPosX)
            {
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



        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (direction == Direction.Left)
            {
                spriteBatch.Draw(walkingAnimation.Texture, position, walkingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 1f);
            }
            else if (direction == Direction.Right)
            {
                spriteBatch.Draw(walkingAnimation.Texture, position, walkingAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }
            else if (direction == Direction.Idle && !playerFlipped)
            {
                spriteBatch.Draw(idleAnimation.Texture, position, idleAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 1f);
            }
            else if (direction == Direction.Idle && playerFlipped)
            {
                spriteBatch.Draw(idleAnimation.Texture, position, idleAnimation.CurrentFrame.SourceRect, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 1f);
            }

            //draw de rest
        }
    }
}
