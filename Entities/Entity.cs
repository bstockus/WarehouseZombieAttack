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

    public abstract class Entity {

        #region Fields

        Game game;

        #endregion

        #region Properties

        public Game Game {
            get {
                return game;
            }
        }

        #endregion

        #region Methods

        public Entity(Game game) {
            this.game = game;
        }

        #endregion

    }

}

