using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit
{
    class Tiles
    {
        protected Texture2D texture;

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle.Location.ToVector2(), texture.Bounds, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
        }
    }

    class CollisionTiles : Tiles
    {
        public CollisionTiles(int i, Rectangle newRectangle)
        {

            if (i == 1)

                texture = Content.Load<Texture2D>("floorTile");
            else
                texture = Content.Load<Texture2D>("blockTile");

            this.Rectangle = newRectangle;
        }
    }
}
