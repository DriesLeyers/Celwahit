﻿using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit
{
    class Skybox : IStaticGameObject
    {
        Texture2D texture;
        public int width;
        public int height;

        public Skybox(Texture2D texture)
        {
            this.texture = texture;
            this.width = 512;
            this.height = 224;
        }

        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(texture, new Vector2(i * width, 0), texture.Bounds, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            }
        }
    }
}
