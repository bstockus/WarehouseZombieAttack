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

    class HUDBar {

        static Texture2D barBackgroundTexture;
        static Texture2D barBarTexture;

        Rectangle frame;
        String title;
        Vector2 titlePosition;
        Vector2 valuePosition;
        Color backgroundColor;
        Color barColor;

        public Single Value {
            get;
            set;
        }

        public HUDBar(Color backgroundColor, Color barColor, Rectangle frame, String title) {
            this.backgroundColor = backgroundColor;
            this.barColor = barColor;
            this.frame = frame;
            this.title = title;
            this.titlePosition = new Vector2((float)frame.X - Fonts.GetFont("HUDBarSpriteFont").MeasureString(title).X, (float)frame.Y - 2.0f);
            this.valuePosition = new Vector2((float)(frame.X + frame.Width), (float)frame.Y - 2.0f);
        }

        public static void LoadContent(ContentManager contentManager) {
            barBackgroundTexture = contentManager.Load<Texture2D>(@"Images/HUD/BarBackground");
            barBarTexture = contentManager.Load<Texture2D>(@"Images/HUD/BarBar");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            Rectangle barRectangle = new Rectangle(frame.X + 1, frame.Y + 1, (int)((frame.Width - 2) * Value), (frame.Height - 2));
            spriteBatch.Draw(barBackgroundTexture, frame, backgroundColor);
            spriteBatch.Draw(barBarTexture, barRectangle, barColor);
			Fonts.DrawTextTopLeftAligned("HUDBarSpriteFont", title, spriteBatch, titlePosition, Color.White);
			Fonts.DrawTextTopLeftAligned("HUDBarSpriteFont", (Value * 100.0f).ToString("F2") + "%", spriteBatch, valuePosition, Color.White);
        }

    }

}
