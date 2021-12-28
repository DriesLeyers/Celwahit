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
    class Boss : EnemyObject
    {
        Animation gettingReadyAnimation;
        Animation shootingAnimation;
  
        //Direction direction;

        public Boss(Texture2D idleBoss, Texture2D walkingBoss, Texture2D gettingReadyBoss, Texture2D shootingBoss, Texture2D bullet, int startPlaceX, int startPlaceY)
        {
            idleAnimation = BossAnimationBuilder.IdleAnimation(idleBoss);
            walkingAnimation= BossAnimationBuilder.WalkingAnimation(walkingBoss);
            gettingReadyAnimation = BossAnimationBuilder.GettingReadyAnimation(gettingReadyBoss);
            shootingAnimation = BossAnimationBuilder.ShootingAnimation(shootingBoss);

            this.blueprintBullet = new Bullet(bullet);

            direction = Direction.Idle;

            _collisionRectangle = new Rectangle((int)position.X, (int)position.Y, shootingBoss.Bounds.Width, shootingBoss.Bounds.Height);
            hasJumped = true;

            position = new Vector2(startPlaceX, startPlaceY);
            velocity = new Vector2(0, 0);
            Health = 1000;
            velocity.Y += 3f;
        }

        public override void Update(GameTime gameTime, Player player, List<Bullet> bullets, bool playerDead)
        {
            base.Update(gameTime, player, bullets, playerDead);

            SetBulletData(30,30);

            if (gameTime.TotalGameTime.Milliseconds % 200 == 0 && !isShooting)
            {
                isShooting = true;
                if (!playerDead)
                    Shoot(bullets);
            }

            if (gameTime.TotalGameTime.Milliseconds % 200 != 0 && isShooting)
                isShooting = false;

            SetDirectionToPlayer(player, 1750);
        }


        //public new void Update(GameTime gameTime, Player player)
        //{
        //    idleAnimation.Update(gameTime, 7);
        //    walkingAnimation.Update(gameTime, 12);
        //    shootingAnimation.Update(gameTime, 12);

        //    if (this.hasJumped)
        //    {
        //        Debug.WriteLine(this.velocity + "BOSS" + this.position);
        //        base.Gravity();
        //        Debug.WriteLine(this.velocity + "BOSS 2" + this.position);
        //        this.velocity.Y += 0.15f * 1.0f;
        //        Debug.WriteLine(this.velocity + "BOSS 3" + this.position);
        //    }

        //    if (!this.hasJumped)
        //    {
        //        this.velocity.Y = 0f;
        //    }

        //    base.Gravity();
        //    this.position += this.velocity;

        //    CollisionRect = new Rectangle((int)this.Positition.X, (int)this.Positition.Y, idleAnimation.CurrentFrame.SourceRect.Width, idleAnimation.CurrentFrame.SourceRect.Height);
        //    _collisionRectangle = CollisionRect;
        //    direction = Direction.Right;

        //    SetDirectionToPlayer(player, 100);

        //}

        //public override void SetDirectionToPlayer(Player player, int distance)
        //{
        //    float sPosX = this.position.X;
        //    float pPosX = player.Positition.X;

        //    float sPosY = this.position.Y;
        //    float pPosY = player.Positition.Y;

        //    if (pPosX > sPosX)
        //    {
        //        direction = Direction.Right;
        //        playerFlipped = true;
        //    }
        //    else if (pPosX < sPosX)
        //    {
        //        direction = Direction.Left;
        //        playerFlipped = false;
        //    }

        //    //TODO check op Y-as verschill da em onder u komt te staan

        //    if (Math.Abs(pPosX - sPosX) >= distance)
        //    {
        //        if (direction == Direction.Right)
        //            velocity.X = 1.5f;
        //        if (direction == Direction.Left)
        //            velocity.X = -1.5f;
        //    }
        //    else
        //        velocity.X = 0;
        //}



        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Debug.WriteLine(position + " " + CollisionRect);

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
