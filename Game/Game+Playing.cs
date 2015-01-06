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

    [Serializable]
    public struct GameSaveStruct {
        public SurvivorSpriteSaveStruct PlayerOneSurvivorSprite;
        public ZombieSpriteSaveStruct[] ZombieSprites;
        public CollectableSaveStruct[] Collectables;
        public OptionsSaveStruct Options;
        public StatsSaveStruct Stats;
        public AchievmentsSaveStruct Achievments;
		public Double WaveTimer;
    }

    public partial class Game : Microsoft.Xna.Framework.Game {

        #region Properties

        public ZombiesSubsystem ZombiesSubsystem {
            get;
            private set;
        }

        public SurvivorSubsystem SurvivorSubsystem {
            get;
            private set;
        }

        public CollectablesSubsystem CollectablesSubsystem {
            get;
            private set;
        }

        public AchievmentsSubsystem AchievmentsSubsystem {
            get;
            private set;
        }

        public HUD HUD {
            get;
            private set;
        }

        public DrawingManager SpritesDrawingManager {
            get;
            private set;
        }

        public DrawingManager CollectablesDrawingManager {
            get;
            private set;
        }

        public DrawingManager OverlayDrawingManager {
            get;
            private set;
        }

        public GameResults Results {
            get;
            set;
        }

        public GameOptions Options {
            get;
            set;
        }

        public Boolean GamePaused {
            get;
            set;
        }

        #endregion

        #region Methods

        private void StartPlayingGame() {
            
            // Create Subsystems
            this.ZombiesSubsystem = new ZombiesSubsystem(this);
            this.SurvivorSubsystem = new SurvivorSubsystem(this);
            this.CollectablesSubsystem = new CollectablesSubsystem(this);
            this.AchievmentsSubsystem = new AchievmentsSubsystem(this);

            // Create Managers
            this.SpritesDrawingManager = new DrawingManager(this, GraphicsDevice);
            this.CollectablesDrawingManager = new DrawingManager(this, GraphicsDevice);
            this.OverlayDrawingManager = new DrawingManager(this, GraphicsDevice);

            this.HUD = new HUD(this, this.Options);
            OverlayDrawingManager.DrawableEntities.Add(HUD);

        }

        private void StartPlayingNewGame(GameDifficulty gameDifficulty, GameSurvivor gameSurvivor, Boolean autoReload, Boolean weaponsUnlocked) {
            this.Options = new GameOptions(gameDifficulty, gameSurvivor, autoReload, weaponsUnlocked);
            this.StartPlayingGame();

            // Create Entities
            SurvivorSubsystem.AddPlayerOneSurvivorSprite(new SurvivorSprite(this, new Location(new Vector2(100.0f, 100.0f), 1.0f, 0.0f)));

			// Unlock Weapons if Option Selected
			if (Options.WeaponsUnlocked) {
				SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.CROWBAR_WEAPON_INDEX);
				SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.CHAINSAW_WEAPON_INDEX);
				SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.PISTOL_WEAPON_INDEX);
				SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.SHOTGUN_WEAPON_INDEX);
			}

            // Reset Stats
            this.Results = new GameResults();
            this.GamePaused = false;
        }

        private void StartPlayingSavedGame(GameSaveStruct saveStruct) {
            this.Options = new GameOptions(saveStruct.Options);
            this.StartPlayingGame();
            
            SurvivorSubsystem.AddPlayerOneSurvivorSprite(new SurvivorSprite(this, saveStruct.PlayerOneSurvivorSprite));
            foreach (ZombieSpriteSaveStruct zombieSpriteSaveStruct in saveStruct.ZombieSprites) {
                ZombieSprite zombieSprite = new ZombieSprite(this, zombieSpriteSaveStruct);
                ZombiesSubsystem.AddZombieSprite(zombieSprite);
            }

            foreach (CollectableSaveStruct collectableSaveStruct in saveStruct.Collectables) {
                CollectablesSubsystem.AddCollectable(new Collectable(this, collectableSaveStruct));
                switch (collectableSaveStruct.CollectableEntityType) {
                    case CollectableEntityType.Health: CollectablesSubsystem.HealthCollectablesSpawned++; break;
                    case CollectableEntityType.Gasoline: CollectablesSubsystem.GasolineCollectablesSpawned++; break;
                    case CollectableEntityType.PistolAmmo: CollectablesSubsystem.PistolAmmoCollectablesSpawned++; break;
                    case CollectableEntityType.ShotgunAmmo: CollectablesSubsystem.ShotgunAmmoCollectablesSpawned++; break;
                    case CollectableEntityType.Energy: CollectablesSubsystem.EnergyCollectablesSpawned++; break;
                    default: break;
                }
            }

			// Reset Zombie Subsystem
			ZombiesSubsystem.WaveTimer = saveStruct.WaveTimer;

            AchievmentsSubsystem.Initialize(saveStruct.Achievments);
            
            this.Results = new GameResults(saveStruct.Stats);

            this.GamePaused = true;
        }

        private void UpdatePlaying(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || oldKeyboardState.IsKeyDown(Keys.Escape)) {
                this.SaveGame();
				this.gameWillQuit = true;
            }

			if ((GamePad.GetState(PlayerIndex.One).Buttons.Start != oldGamePadState.Buttons.Start && GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed) || (Keyboard.GetState().IsKeyDown(Keys.Delete) && oldKeyboardState.IsKeyUp(Keys.Delete))) {
                SurvivorSubsystem.PlayerOneSurvivorSprite.GameOver();
                this.ResetToGameOver(Results, Options);
            }

            if (!GamePaused) {

				if ((GamePad.GetState(PlayerIndex.One).Buttons.Y != oldGamePadState.Buttons.Y && GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed) || (Keyboard.GetState().IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))) {
                    this.GamePaused = true;
                }

                Results.SurvivalTime += gameTime.ElapsedGameTime.TotalSeconds;

                SurvivorSubsystem.Update(gameTime);
                ZombiesSubsystem.Update(gameTime);
                CollectablesSubsystem.Update(gameTime);
                AchievmentsSubsystem.Update(gameTime);

                // Check for Game Over
                if (SurvivorSubsystem.PlayerOneSurvivorSprite.CurrentHealth <= 0.0f) {
                    SurvivorSubsystem.PlayerOneSurvivorSprite.GameOver();
                    this.ResetToGameOver(Results, Options);
                }

            } else if (GamePaused) {
				if ((GamePad.GetState(PlayerIndex.One).Buttons.Y != oldGamePadState.Buttons.Y && GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed) || (Keyboard.GetState().IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))) {
                    this.GamePaused = false;
                }
            }

            // Update HUD
            HUD.Update(gameTime, Results);
        }

        private void DrawPlaying(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Green);
            CollectablesDrawingManager.DrawEntities(gameTime);
            SpritesDrawingManager.DrawEntities(gameTime);
            OverlayDrawingManager.DrawEntities(gameTime);
            if (GamePaused) {
                startingSpriteBatch.Begin();
				Fonts.DrawTextMiddleCenterAligned("VeryLargeSpriteFont", "Paused", startingSpriteBatch, new Vector2(this.Window.ClientBounds.Width / 2.0f, this.Window.ClientBounds.Height / 2.0f), Color.White);
                startingSpriteBatch.End();
            }
        }

        private GameSaveStruct GetGameSaveStruct() {
            GameSaveStruct gameSaveStruct = new GameSaveStruct() {
                PlayerOneSurvivorSprite = SurvivorSubsystem.PlayerOneSurvivorSprite.SaveStruct,
                ZombieSprites = ZombiesSubsystem.ZombieSaveStructs(),
                Collectables = CollectablesSubsystem.CollectableSaveStructs(),
                Options = Options.OptionsSaveStruct,
                Stats = Results.StatsSaveStruct,
                Achievments = AchievmentsSubsystem.AchievmentsSaveStruct,
				WaveTimer = ZombiesSubsystem.WaveTimer
            };
            return gameSaveStruct;
        }

        private void SaveGame() {
			FileStream saveGameStream = File.Create(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "save.sav"));
            XmlSerializer serializer = new XmlSerializer(typeof(GameSaveStruct));
            serializer.Serialize(saveGameStream, this.GetGameSaveStruct());
            saveGameStream.Close();
        }

        #endregion

    }

}
