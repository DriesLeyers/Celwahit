using Celwahit.GameObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Celwahit.Interfaces
{
    interface IEnemyObject
    {
        public void Update(GameTime gameTime, Player player, List<Bullet> bullets, bool playerDead);

    }
}