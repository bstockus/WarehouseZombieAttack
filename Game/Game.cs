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
using SimpleGameFramework.Content;

namespace WarehouseZombieAttack {

    public enum GameState {
        Playing,
        Starting
    }

    public partial class Game : Microsoft.Xna.Framework.Game {

        #region Fields

        GraphicsDeviceManager graphics;
        GamePadState oldGamePadState;
		KeyboardState oldKeyboardState;
        SpriteBatch startingSpriteBatch;
		Boolean gameWillQuit = false;
		Boolean drawWillQuit = false;

        #endregion

        #region Properties

        public GameState GameState {
            get;
            private set;
        }

        #endregion

        #region Methods

        public Game() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferWidth = 1920;
			graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize() {
//			graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
//			graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
			//graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
			Console.WriteLine("Screen Size = " + graphics.PreferredBackBufferWidth.ToString() + "," + graphics.PreferredBackBufferHeight.ToString());
			base.Initialize();
        }

        protected override void LoadContent() {
            
			Fonts.LoadFonts(Content, "Fonts.xml");
			Sounds.LoadSounds(Content, "Sounds.xml");

			// Load Game Content
            startingSpriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);

			// Get Screen Size
			int screenWidth = this.Window.ClientBounds.Width;
			int screenHeight = this.Window.ClientBounds.Height;

            // Load Control Content
            TextPickerControl.LoadContent(Content);

            // Load Screen Content
            SplashScreen.LoadContent(Content);
            SetupScreen.LoadContent(Content);
            InstructionsScreen.LoadContent(Content);
            GameOverScreen.LoadContent(Content);
            LoadScreens();

            // Load Subsystem Contents
            SurvivorSubsystem.LoadContent(Content);
            ZombiesSubsystem.LoadContent(Content);
            CollectablesSubsystem.LoadContent(Content);
            AchievmentsSubsystem.LoadContent(Content);

            // Load Entity Contents
            HUD.LoadContent(Content, screenWidth, screenHeight);
            
			String saveFilePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "save.sav");
			if (File.Exists(saveFilePath)) {
				FileStream fileStream = File.OpenRead(saveFilePath);
                XmlSerializer fileSerializer = new XmlSerializer(typeof(GameSaveStruct));
                GameSaveStruct gameSaveStruct = (GameSaveStruct)fileSerializer.Deserialize(fileStream);
                fileStream.Close();
				File.Delete(saveFilePath);
                GameState = GameState.Playing;
                this.StartPlayingSavedGame(gameSaveStruct);
            } else {
                ResetToStarting(true);
            }

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
			if (gameWillQuit) {
				if (drawWillQuit) this.Exit();
			} else {
				if (GameState == GameState.Playing) {
					UpdatePlaying(gameTime);
				} else if (GameState == GameState.Starting) {
					UpdateStarting(gameTime);
				}
				oldGamePadState = GamePad.GetState(PlayerIndex.One);
				oldKeyboardState = Keyboard.GetState();
				base.Update(gameTime);
			}
        }

        protected override void Draw(GameTime gameTime) {
			if (gameWillQuit) {
				this.drawWillQuit = true;
			} else {
				if (GameState == GameState.Playing) DrawPlaying(gameTime);
				if (GameState == GameState.Starting) DrawStarting(gameTime);
				base.Draw(gameTime);
			}
        }

        #endregion

    }

}
