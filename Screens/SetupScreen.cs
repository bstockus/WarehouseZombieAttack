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

namespace WarehouseZombieAttack {

    public class SetupScreen : Screen {

        static Texture2D setupScreenBackgroundTexture;

        TextPickerControl difficultyTextPickerControl;
        static readonly Vector2 DIFFICULTY_TEXT_PICKER_ORIGIN = new Vector2(550.0f, 100.0f);
        static readonly string[] DIFFICULTY_TEXT_STRINGS = new string[3] { "Easy", "Medium", "Hard" };

        TextPickerControl survivorTextPickerControl;
        static readonly Vector2 SURVIVOR_TEXT_PICKER_ORIGIN = new Vector2(550.0f, 150.0f);
        static readonly string[] SURVIVOR_TEXT_STRINGS = new string[2] { "Luke", "Bryan" };

        TextPickerControl autoReloadTextPickerControl;
        static readonly Vector2 AUTO_RELOAD_TEXT_PICKER_CONTROL_ORIGIN = new Vector2(550.0f, 200.0f);
        static readonly string[] AUTO_RELOAD_TEXT_STRINGS = new string[2] { "On", "Off" };

		TextPickerControl weaponsUnlockedTextPickerControl;
		static readonly Vector2 WEAPONS_UNLOCKED_TEXT_PICKER_CONTROL_ORIGIN = new Vector2(550.0f, 250.0f);
		static readonly string[] WEAPONS_UNLOCKED_TEXT_STRINGS = new string[2] { "No", "Yes" };

		TextPickerControl[] textPickerControls;

        int currentControlIndex = 0;

        public GameDifficulty GameDifficulty {
            get {
                return (GameDifficulty)difficultyTextPickerControl.CurrentItem;
            }
        }

        public GameSurvivor GameSurvivor {
            get {
                return (GameSurvivor)survivorTextPickerControl.CurrentItem;
            }
        }

        public Boolean AutoReload {
            get {
                if (autoReloadTextPickerControl.CurrentItem == 0) {
                    return true;
                } else {
                    return false;
                }
            }
        }

		public Boolean WeaponsUnlocked {
			get {
				if (weaponsUnlockedTextPickerControl.CurrentItem == 0) {
					return false;
				} else {
					return true;
				}
			}
		}

        public SetupScreen(Game game)
            : base(game) {
            difficultyTextPickerControl = new TextPickerControl("Difficulty:", DIFFICULTY_TEXT_STRINGS, 0, DIFFICULTY_TEXT_PICKER_ORIGIN + DrawOffsetVector);
			survivorTextPickerControl = new TextPickerControl("Survivor:", SURVIVOR_TEXT_STRINGS, 0, SURVIVOR_TEXT_PICKER_ORIGIN + DrawOffsetVector);
			autoReloadTextPickerControl = new TextPickerControl("Auto Reload:", AUTO_RELOAD_TEXT_STRINGS, 0, AUTO_RELOAD_TEXT_PICKER_CONTROL_ORIGIN + DrawOffsetVector);
			weaponsUnlockedTextPickerControl = new TextPickerControl("Weapons Unlocked:", WEAPONS_UNLOCKED_TEXT_STRINGS, 0, WEAPONS_UNLOCKED_TEXT_PICKER_CONTROL_ORIGIN + DrawOffsetVector);
			textPickerControls = new TextPickerControl[4] {
				difficultyTextPickerControl,
				survivorTextPickerControl,
				autoReloadTextPickerControl,
				weaponsUnlockedTextPickerControl
			};
            difficultyTextPickerControl.Focus = true;
        }

        public static void LoadContent(ContentManager contentManager) {
            setupScreenBackgroundTexture = contentManager.Load<Texture2D>(@"Images/GUI/SetupScreen");
            
        }

		public override void Update(GameTime gameTime, GamePadState newGamePadState, GamePadState oldGamePadState, KeyboardState newKeyboardState, KeyboardState oldKeyboardState) {

            if ((newGamePadState.Buttons.Start != oldGamePadState.Buttons.Start && newGamePadState.Buttons.Start == ButtonState.Pressed)
			    || (newKeyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))) {
                Game.BeginPlayingNewGame();
            }

            if ((newGamePadState.Buttons.A != oldGamePadState.Buttons.A && newGamePadState.Buttons.A == ButtonState.Pressed)
			    || (newKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
			    || (newKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A))) {
                Game.GameStartingScreen = GameStartingScreen.InstructionsScreen;
            }

            if ((newGamePadState.DPad.Up != oldGamePadState.DPad.Up && newGamePadState.DPad.Up == ButtonState.Pressed && currentControlIndex > 0)
			    || (newKeyboardState.IsKeyDown(Keys.Up) && oldKeyboardState.IsKeyUp(Keys.Up))) 
				currentControlIndex--;
            if ((newGamePadState.DPad.Down != oldGamePadState.DPad.Down && newGamePadState.DPad.Down == ButtonState.Pressed && currentControlIndex < (textPickerControls.Length - 1))
			    || (newKeyboardState.IsKeyDown(Keys.Down) && oldKeyboardState.IsKeyUp(Keys.Down))) 
				currentControlIndex++;

			if (currentControlIndex < 0)
				currentControlIndex = textPickerControls.Length - 1;

			if (currentControlIndex >= textPickerControls.Length)
				currentControlIndex = 0;

			for (int index = 0; index < textPickerControls.Length; index ++) {
				if (index == currentControlIndex) {
					textPickerControls[index].Focus = true;
				} else {
					textPickerControls[index].Focus = false;
				}
				textPickerControls[index].Update(gameTime);
			}
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(setupScreenBackgroundTexture, DrawOffsetVector, Color.White);
			foreach (TextPickerControl textPickerControl in textPickerControls) {
				textPickerControl.Draw(gameTime, spriteBatch);
			}
        }

    }

}
