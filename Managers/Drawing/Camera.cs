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

    /// <summary>
    /// A class for a camera object
    /// </summary>
    public class Camera {

        SpriteBatch spriteBatch;

        public SpriteBatch SpriteBatch {
            get {
                return spriteBatch;
            }
        }

        public Camera(GraphicsDevice graphicsDevice) {
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void Draw(GameTime gameTime, IDrawableEntity[] drawables) {
            spriteBatch.Begin();
            foreach (IDrawableEntity drawable in drawables)
                drawable.Draw(gameTime, this);
            spriteBatch.End();
        }

    }

}

