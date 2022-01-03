using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Interfaces
{
    interface IGameState
    {
        public void Update(GameTime gameTime);

        public void Draw(GameTime gameTime);

        public void Initialize();

        public void LoadContent();
    }
}
