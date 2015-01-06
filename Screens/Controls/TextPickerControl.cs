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

    public class TextPickerControl {

        #region Static Fields

        static Texture2D chevronsTexture;

        #endregion

        #region Instance Fields

        float[] widths;
        float titleWidth;
        GamePadState oldGamePadState;
		KeyboardState oldKeyboardState;

        #endregion

        #region Properties

        public string Title {
            get;
            private set;
        }

        public Vector2 Origin {
            get;
            private set;
        }

        public string[] Values {
            get;
            private set;
        }

        public int CurrentItem {
            get;
            set;
        }

        public Boolean Focus {
            get;
            set;
        }

        #endregion

        #region Methods

        public TextPickerControl(string title, string[] values, int defaultValue, Vector2 origin) {
            this.Title = title;
            this.Values = values;
            this.CurrentItem = defaultValue;
            this.Origin = origin;
            this.Focus = false;
            this.titleWidth = Fonts.GetFont("TextPickerControlSpriteFont").MeasureString(title).X;
            this.widths = new float[values.Length];
            for (int index = 0; index < values.Length; index++) {
				widths[index] = Fonts.GetFont("TextPickerControlSpriteFont").MeasureString(values[index]).X;
            }
        }

        public static void LoadContent(ContentManager contentManager) {
            chevronsTexture = contentManager.Load<Texture2D>(@"Images/GUI/Chevron");
        }

        public void Update(GameTime gameTime) {
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
			KeyboardState newKeyboardState = Keyboard.GetState();

            if (Focus) {
				if ((newGamePadState.DPad.Left != oldGamePadState.DPad.Left && newGamePadState.DPad.Left == ButtonState.Pressed)
					|| (newKeyboardState.IsKeyDown(Keys.Left) && oldKeyboardState.IsKeyUp(Keys.Left))
					&& CurrentItem > 0)
					CurrentItem--;

				if (((newGamePadState.DPad.Right != oldGamePadState.DPad.Right && newGamePadState.DPad.Right == ButtonState.Pressed)
				    || (newKeyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right)))
					&& CurrentItem < (Values.Length - 1))
					CurrentItem++;
            }

            oldGamePadState = newGamePadState;
			oldKeyboardState = newKeyboardState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			SpriteFont spriteFont = Fonts.GetFont("TextPickerControlSpriteFont");
            if (Focus) {
                spriteBatch.DrawString(spriteFont, Title, new Vector2(Origin.X - titleWidth - 10.0f, Origin.Y - 15.0f), Color.White);
            } else {
                spriteBatch.DrawString(spriteFont, Title, new Vector2(Origin.X - titleWidth - 10.0f, Origin.Y - 15.0f), Color.Gray);
            }
            if (CurrentItem > 0) {
                if (Focus) {
					spriteBatch.Draw(chevronsTexture, new Rectangle((int)Origin.X, (int)Origin.Y, 30, 30), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1.0f);
                } else {
					spriteBatch.Draw(chevronsTexture, new Rectangle((int)Origin.X, (int)Origin.Y, 30, 30), null, Color.Gray, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1.0f);
                }
            }
            if (Focus) {
                spriteBatch.DrawString(spriteFont, Values[CurrentItem], new Vector2(Origin.X + 40.0f, Origin.Y - 15.0f), Color.White);
            } else {
				spriteBatch.DrawString(spriteFont, Values[CurrentItem], new Vector2(Origin.X + 40.0f, Origin.Y - 15.0f), Color.Gray);
            }
            if (CurrentItem < (Values.Length - 1)) {
                if (Focus) {
					spriteBatch.Draw(chevronsTexture, new Rectangle((int)(Origin.X + 50.0f + widths[CurrentItem]), (int)Origin.Y, 30, 30), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
                } else {
					spriteBatch.Draw(chevronsTexture, new Rectangle((int)(Origin.X + 50.0f + widths[CurrentItem]), (int)Origin.Y, 30, 30), null, Color.Gray, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
                }
            }

        }

        #endregion

    }

}
