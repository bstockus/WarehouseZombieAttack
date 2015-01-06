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

    public class DrawingManager : Manager {

        List<IDrawableEntity> drawableEntities;
        Camera camera;

        public List<IDrawableEntity> DrawableEntities {
            get {
                return drawableEntities;
            }
        }

        public Camera Camera {
            get {
                return camera;
            }
        }

        public DrawingManager(Game game, GraphicsDevice graphicsDevice) : base(game) {
            drawableEntities = new List<IDrawableEntity>();
            camera = new Camera(graphicsDevice);
        }

        public void DrawEntities(GameTime gameTime) {
            camera.Draw(gameTime, drawableEntities.ToArray());
        }

    }

}

