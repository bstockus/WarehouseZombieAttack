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

    public class SplashScreen : Screen {
        
		static Texture2D splashScreenBackgroundTexture;

        public SplashScreen(Game game) : base(game) {

        }

        public static void LoadContent(ContentManager contentManager) {
            splashScreenBackgroundTexture = contentManager.Load<Texture2D>(@"Images/GUI/SplashScreen");
        }

		public override void Update(GameTime gameTime, GamePadState newGamePadState, GamePadState oldGamePadState, KeyboardState newKeyboardState, KeyboardState oldKeyboardState) {
			if ((newGamePadState.Buttons.A != oldGamePadState.Buttons.A && newGamePadState.Buttons.A == ButtonState.Pressed)
			    || (newKeyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))) {
				Game.GameStartingScreen = GameStartingScreen.SetupScreen;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			spriteBatch.Draw(splashScreenBackgroundTexture, DrawOffsetVector, Color.White);
        }

    }

}
