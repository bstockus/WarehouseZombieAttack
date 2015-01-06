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

    public abstract class Screen {

        public Game Game {
            get;
            private set;
        }

		public Vector2 DrawOffsetVector {
			get {
				float width = (float)Game.Window.ClientBounds.Width;
				float height = (float)Game.Window.ClientBounds.Height;
				return new Vector2((width - 1100.0f) / 2.0f, (height - 900.0f) / 2.0f);
			}
		}

        public Screen(Game game) {
            this.Game = game;
        }

        public abstract void Update(GameTime gameTime, GamePadState newGamePadState, GamePadState oldGamePadState, KeyboardState newKeyboardState, KeyboardState oldKeyboardState);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }

}
