using Celwahit.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit
{
    public enum TypeTiles
    {
        Tile,
        Spike
    }
    class Tiles
    {
        public TypeTiles typeTile;


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
        public CollisionTiles(int i, Rectangle newRectangle, string level)
        {
            if (level.Equals("level1"))
            {
                if (i == 1)
                {
                    texture = Content.Load<Texture2D>("floorTile");
                    typeTile = TypeTiles.Tile;
                }
            }
            else if (level.Equals("level2"))
            {
                if (i == 1)
                {
                    texture = Content.Load<Texture2D>("floorTileLvl2");
                    typeTile = TypeTiles.Tile;
                }
                else if (i == 2)
                {
                    texture = Content.Load<Texture2D>("spikes");
                    typeTile = TypeTiles.Spike;
                }
            }



            this.Rectangle = newRectangle;
        }
    }
}
