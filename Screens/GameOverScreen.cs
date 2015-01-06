using System;
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

    public class GameOverScreen : Screen {

        static Texture2D gameOverScreenTexture;

        public string GameResultsString {
            get {
				string results = "Survival Time: " + new TimeSpan(0, 0, 0, (int)GameResults.SurvivalTime, 0).ToString(@"hh\:mm\:ss") +
					" (Points: " + GameResults.Points.ToString("F0") +
					", Waves Survived: " + GameResults.NumberOfZombieWaves.ToString("F0") + ")" + Environment.NewLine;

				results += "Total Zombie Kills: " + GameResults.NumberOfZombieKills.ToString("F0") + 
					" (Double Kills: " + GameResults.KillResults.DoubleKills.ToString("F0") +
					", Triple Kills: " + GameResults.KillResults.TripleKills.ToString("F0") + ")" + Environment.NewLine;

				results += "Fist Kills: " + GameResults.KillResults.FistPunchKills.ToString("F0") +
					" (Kill Rate: " + (GameResults.FistWeaponResults.KillsPerUsage * 100.0f).ToString("F2") + "%" +
					", Damage Rate: " + (GameResults.FistWeaponResults.DamagesPerUsage * 100.0f).ToString("F2") + "%" +
					", Usages: " + GameResults.FistWeaponResults.TotalUsages.ToString("F0") + ")" + Environment.NewLine;

				results += "Crowbar Kills: " + GameResults.KillResults.CrowbarHitKills.ToString("F0") +
					" (Kill Rate: " + (GameResults.CrowbarWeaponResults.KillsPerUsage * 100.0f).ToString("F2") + "%" +
					", Damage Rate: " + (GameResults.CrowbarWeaponResults.DamagesPerUsage * 100.0f).ToString("F2") + "%" +
					", Usages: " + GameResults.CrowbarWeaponResults.TotalUsages.ToString("F0") + ")" + Environment.NewLine;

				results += "Chainsaw Kills: " + GameResults.KillResults.ChainsawKills.ToString("F0") +
					" (Kill Rate: " + (GameResults.ChainsawWeaponResults.KillsPerUsage * 100.0f).ToString("F2") + "%" +
					", Damage Rate: " + (GameResults.ChainsawWeaponResults.DamagesPerUsage * 100.0f).ToString("F2") + "%" +
					", Usages: " + GameResults.ChainsawWeaponResults.TotalUsages.ToString("F0") + ")" + Environment.NewLine;

				results += "Pistol Kills: " + GameResults.KillResults.PistolShotKills.ToString("F0") +
					" (Kill Rate: " + (GameResults.PistolWeaponResults.KillsPerUsage * 100.0f).ToString("F2") + "%" +
					", Damage Rate: " + (GameResults.PistolWeaponResults.DamagesPerUsage * 100.0f).ToString("F2") + "%" +
					", Usages: " + GameResults.PistolWeaponResults.TotalUsages.ToString("F0") + ")" + Environment.NewLine;

				results += "Shotgun Kills: " + GameResults.KillResults.ShotgunBlastKills.ToString("F0") +
					" (Kill Rate: " + (GameResults.ShotgunWeaponResults.KillsPerUsage * 100.0f).ToString("F2") + "%" +
					", Damage Rate: " + (GameResults.ShotgunWeaponResults.DamagesPerUsage * 100.0f).ToString("F2") + "%" +
					", Usages: " + GameResults.ShotgunWeaponResults.TotalUsages.ToString("F0") + ")" + Environment.NewLine;

				results += "Health Collected: " + GameResults.CollectablesResults.HealthCollectablesCollected.ToString("F0") + Environment.NewLine;
				results += "Gasoline Collected: " + GameResults.CollectablesResults.GasolineCollectablesCollected.ToString("F0") + Environment.NewLine;
				results += "Energy Collected: " + GameResults.CollectablesResults.EnergyCollectablesCollected.ToString("F0") + Environment.NewLine;
				results += "Pistol Ammo Collected: " + GameResults.CollectablesResults.PistolAmmoCollectablesCollected.ToString("F0") + Environment.NewLine;
				results += "Shotgun Ammo Collected: " + GameResults.CollectablesResults.ShotgunAmmoCollectablesCollected.ToString("F0") + Environment.NewLine;
                return results;
            }
        }

        public GameResults GameResults {
            get;
            set;
        }

        public GameOptions GameOptions {
            get;
            set;
        }

        public GameOverScreen(Game game) : base(game) {

        }

        public static void LoadContent(ContentManager contentManager) {
            gameOverScreenTexture = contentManager.Load<Texture2D>(@"Images/GUI/GameOverScreen");
        }

		public override void Update(GameTime gameTime, GamePadState newGamePadState, GamePadState oldGamePadState, KeyboardState newKeyboardState, KeyboardState oldKeyboardState) {
            if ((newGamePadState.Buttons.B != oldGamePadState.Buttons.B && newGamePadState.Buttons.B == ButtonState.Pressed)
			    || (newKeyboardState.IsKeyDown(Keys.Delete) && oldKeyboardState.IsKeyUp(Keys.Delete))
			    || (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))) {
                Game.ResetToStarting(false);
            }

            if ((newGamePadState.Buttons.A != oldGamePadState.Buttons.A && newGamePadState.Buttons.A == ButtonState.Pressed)
			    || (newKeyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
			    || (newKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A))) {
                Game.BeginPlayingNewGame(GameOptions.GameDifficulty, GameOptions.GameSurvivor, GameOptions.AutoReload, GameOptions.WeaponsUnlocked);
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(gameOverScreenTexture, DrawOffsetVector, Color.White);
			Fonts.DrawTextTopLeftAligned("LargeSpriteFont", GameResultsString, spriteBatch, new Vector2(50.0f, 150.0f) + DrawOffsetVector, Color.White);
        }

    }

}
