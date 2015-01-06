using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WarehouseZombieAttack {

    public enum GameStartingScreen {
        SplashScreen,
        SetupScreen,
        InstructionsScreen,
        GameOverScreen
    }

    public partial class Game : Microsoft.Xna.Framework.Game {

        #region Fields

        GameStartingScreen gameStartingScreen;

        #endregion

        #region Properties

        public GameStartingScreen GameStartingScreen {
            get {
                return gameStartingScreen;
            }
            set {
                gameStartingScreen = value;
            }
        }

        public SplashScreen SplashScreen {
            get;
            private set;
        }

        public SetupScreen SetupScreen {
            get;
            private set;
        }

        public InstructionsScreen InstructionsScreen {
            get;
            private set;
        }

        public GameOverScreen GameOverScreen {
            get;
            private set;
        }

        #endregion

        #region Methods

        private void LoadScreens() {
            SplashScreen = new SplashScreen(this);
            SetupScreen = new SetupScreen(this);
            InstructionsScreen = new InstructionsScreen(this);
            GameOverScreen = new GameOverScreen(this);
        }

        public void ResetToStarting(Boolean showSplash) {
            GameState = GameState.Starting;
            if (showSplash) {
                gameStartingScreen = GameStartingScreen.SplashScreen;
            } else {
                gameStartingScreen = GameStartingScreen.SetupScreen;
            }
        }

        private void ResetToGameOver(GameResults gameResults, GameOptions gameOptions) {
            GameState = GameState.Starting;
            GameOverScreen.GameResults = gameResults;
            GameOverScreen.GameOptions = gameOptions;
            gameStartingScreen = GameStartingScreen.GameOverScreen;
        }

        private void UpdateStarting(GameTime gameTime) {
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
			KeyboardState newKeyboardState = Keyboard.GetState();

			if ((newGamePadState.Buttons.Back != oldGamePadState.Buttons.Back && newGamePadState.Buttons.Back == ButtonState.Pressed) 
			    || (Keyboard.GetState().IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))) {
				this.gameWillQuit = true;
            }

            switch (gameStartingScreen) {
                case GameStartingScreen.SplashScreen: SplashScreen.Update(gameTime, newGamePadState, oldGamePadState, newKeyboardState, oldKeyboardState); break;
				case GameStartingScreen.SetupScreen: SetupScreen.Update(gameTime, newGamePadState, oldGamePadState, newKeyboardState, oldKeyboardState); break;
				case GameStartingScreen.InstructionsScreen: InstructionsScreen.Update(gameTime, newGamePadState, oldGamePadState, newKeyboardState, oldKeyboardState); break;
				case GameStartingScreen.GameOverScreen: GameOverScreen.Update(gameTime, newGamePadState, oldGamePadState, newKeyboardState, oldKeyboardState); break;
            }

            oldGamePadState = newGamePadState;
			oldKeyboardState = newKeyboardState;

        }

        private void DrawStarting(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(94, 99, 92));
            startingSpriteBatch.Begin();

            switch (gameStartingScreen) {
                case GameStartingScreen.SplashScreen: SplashScreen.Draw(gameTime, startingSpriteBatch); ; break;
                case GameStartingScreen.SetupScreen: SetupScreen.Draw(gameTime, startingSpriteBatch); break;
                case GameStartingScreen.InstructionsScreen: InstructionsScreen.Draw(gameTime, startingSpriteBatch); break;
                case GameStartingScreen.GameOverScreen: GameOverScreen.Draw(gameTime, startingSpriteBatch); break;
            }

            startingSpriteBatch.End();
        }

        public void BeginPlayingNewGame() {
            this.BeginPlayingNewGame(SetupScreen.GameDifficulty, SetupScreen.GameSurvivor, SetupScreen.AutoReload, SetupScreen.WeaponsUnlocked);
        }

        public void BeginPlayingNewGame(GameDifficulty gameDifficulty, GameSurvivor gameSurvivor, Boolean autoReload, Boolean weaponsUnlocked) {
            this.GameState = GameState.Playing;
            this.StartPlayingNewGame(gameDifficulty, gameSurvivor, autoReload, weaponsUnlocked);
        }

        #endregion

    }

}
