using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit
{
    class StartScreen
    {
        public Vector2 startButtonPosition;

        GameState gameState;

        MouseState mouseState;
        MouseState previousMouseState;

        enum GameState
        {
            StartMenu,
            Playing,
            Paused
        }

        public StartScreen(GameSettings gameSettings)
        {
            gameSettings.Graphics.PreferredBackBufferWidth = 1280;
            gameSettings.Graphics.PreferredBackBufferHeight = 720;
            gameSettings.Graphics.ApplyChanges();

            startButtonPosition = new Vector2(450, 554);
        }

        public bool CheckIfWantToPlay(MouseState previousMouseState)
        {
            bool wantToPlay = false;

            this.previousMouseState = previousMouseState; 

            mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                wantToPlay = true;
            }
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                bool clicked = MouseClicked(mouseState.X, mouseState.Y);
                if (clicked)
                    wantToPlay = true;
            }


            return wantToPlay;
        }

        private bool MouseClicked(int x, int y)
        {
            bool clicked = false;
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 375, 100);

                if (mouseClickRect.Intersects(startButtonRect))
                {
                    clicked = true;
                }
            }

            return clicked;
        }

        public void DrawVectorStartButton(Texture2D startButton, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(startButton, new Vector2(0, 0), Color.White);
        }
    }
}
