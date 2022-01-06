using Microsoft.Xna.Framework;

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
