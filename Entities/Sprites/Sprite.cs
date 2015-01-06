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

    public abstract class Sprite : Entity, ILocatableEntity, IDrawableEntity, IUpdateableEntity {

        #region Fields

        Location location;

        #endregion

        #region Properties

        public Location Location {
            get {
                return location;
            }
        }
        
        #endregion

        #region Methods

        public Sprite(Game game, Location location) : base(game) {
            this.location = location;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, Camera camera);

        #endregion

    }

}

