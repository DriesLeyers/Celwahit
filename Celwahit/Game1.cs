using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Celwahit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D playerLegs;
        private Texture2D playerBody;

        private Rectangle partRectangleLegs;
        private int moveRectangleLegs_X = 32;

        private Rectangle partRectangleBody;
        private int moveRectangleBody_X = 36;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            partRectangleLegs = new Rectangle(moveRectangleLegs_X, 23, 32, 23);
            partRectangleBody = new Rectangle(moveRectangleBody_X, 0, 36, 30);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerLegs = Content.Load<Texture2D>("PlayerLegsSheet");
            playerBody = Content.Load<Texture2D>("PlayerBodySheet");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(playerLegs, new Vector2(0, 18), partRectangleLegs, Color.White);
            _spriteBatch.Draw(playerBody, new Vector2(0, 0), partRectangleBody, Color.White);

            _spriteBatch.End();

            moveRectangleBody_X += 36;
            if (moveRectangleBody_X > 159)
                moveRectangleBody_X = 2;

            partRectangleBody.X = moveRectangleBody_X;

            moveRectangleLegs_X += 32;
            if (moveRectangleLegs_X > 384)
                moveRectangleLegs_X = 0;

            partRectangleLegs.X = moveRectangleLegs_X;

            base.Draw(gameTime);
        }
    }
}
