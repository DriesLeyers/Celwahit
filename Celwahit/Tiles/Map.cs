using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit
{
    class Map
    {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        public List<CollisionTiles> CollisionTiles 
        {
            get { return collisionTiles; }
        }

        private int width, height;
        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Map()
        {

        }

        public void Generate(int[,] map, int size)
        {
            for(int i = 0; i < map.GetLength(1); i++)
                for(int  j = 0; j < map.GetLength(0); j++)
                {
                    int number = map[j, i];

                    if(number > 0)
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(i * size, j * size, size, size)));

                    width = (i + 1) * size;
                    height = (j + 1) * size;
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
        }
    }
}
