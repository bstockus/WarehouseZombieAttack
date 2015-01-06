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

    public class InstructionsScreen : Screen {

        static Texture2D instructionsScreenBackgroundTexture;

        public InstructionsScreen(Game game)
            : base(game) {

        }

        public static void LoadContent(ContentManager contentManager) {
            instructionsScreenBackgroundTexture = contentManager.Load<Texture2D>(@"Images/GUI/InstructionsScreen");
        }

		public override void Update(GameTime gameTime, GamePadState newGamePadState, GamePadState oldGamePadState, KeyboardState newKeyboardState, KeyboardState oldKeyboardState) {
            if ((newGamePadState.Buttons.B != oldGamePadState.Buttons.B && newGamePadState.Buttons.B == ButtonState.Pressed)
			    || (newKeyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
			    || (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))) {
                Game.GameStartingScreen = GameStartingScreen.SetupScreen;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(instructionsScreenBackgroundTexture, DrawOffsetVector, Color.White);
        }

    }

}
