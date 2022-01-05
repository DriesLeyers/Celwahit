using Celwahit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Celwahit.Scenes
{
    public class MenuState : SceneState, IGameState
    {
        StartScreen startScreen;

        GameSettings gameSettings;

        MouseState mouseState;
        MouseState previousMouseState;

        private Texture2D startButton;

        public MenuState(Game1 game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch) : base(game, graphics, spriteBatch)
        {
            LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            startScreen.DrawVectorStartButton(startButton, _spriteBatch);
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            startButton = Content.Load<Texture2D>("startscherm");

            gameSettings = new GameSettings(_graphics);

            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            startScreen = new StartScreen(gameSettings);
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            if (startScreen.CheckIfWantToPlay(previousMouseState))
            {
                Game1.ChangeSceneState(new LvlTwoState(Game1, _graphics, _spriteBatch));
            }

            previousMouseState = mouseState;
        }
    }
}
