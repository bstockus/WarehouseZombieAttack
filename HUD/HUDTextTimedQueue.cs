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

    public class HUDTextTimedQueue {

        #region Constants

        static readonly Double QUEUE_LIFETIME = 20.0;
        static readonly Int32 QUEUE_SIZE = 10;

        #endregion

        #region Fields

        TimedQueue<Tuple<String, Color, Boolean>> stringsTimedQueue;
        Vector2 basePoint;

        static Texture2D pixelTexture;

        #endregion

        #region Properties

        #endregion

        #region Methods

        public HUDTextTimedQueue(Vector2 basePoint) {
            this.basePoint = basePoint;
            this.stringsTimedQueue = new TimedQueue<Tuple<String, Color, Boolean>>(QUEUE_SIZE, QUEUE_LIFETIME);
        }

        public static void LoadContent(ContentManager contentManager) {
            pixelTexture = contentManager.Load<Texture2D>(@"Images/HUD/Pixel");
        }

        public void Update(GameTime gameTime) {
            stringsTimedQueue.Update(gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            Vector2 currentBasePoint = basePoint;
            foreach (Tuple<String, Color, Boolean> value in stringsTimedQueue.CurrentValues) {
                SpriteFont spriteFont = Fonts.GetFont("MessageQueueSpriteFont");
                if (value.Item3) spriteFont = Fonts.GetFont("BoldMessageQueueSpriteFont");
                Vector2 stringSize = spriteFont.MeasureString(value.Item1);
                spriteBatch.Draw(pixelTexture, new Rectangle((Int32)currentBasePoint.X - 2, (Int32)currentBasePoint.Y, (Int32)stringSize.X + 4, (Int32)stringSize.Y), Color.Black);
                spriteBatch.DrawString(spriteFont, value.Item1, currentBasePoint, value.Item2);
                currentBasePoint.Y += stringSize.Y; ;
            }
        }

        public void AddMessage(String message, Color color, Boolean bold) {
            stringsTimedQueue.Add(new Tuple<String, Color, Boolean>(message, color, bold));
        }

        #endregion

    }

}
