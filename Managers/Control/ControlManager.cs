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

    public class ControlManager : Manager {

        public IControllableEntity PlayerOneControllableEntity { get; set; }

        public IControllableEntity PlayerTwoControllableEntity { get; set; }

        public IControllableEntity PlayerThreeControllableEntity { get; set; }

        public IControllableEntity PlayerFourControllableEntity { get; set; }

        public ControlManager(Game game) : base(game) {
            this.PlayerOneControllableEntity = null;
            this.PlayerTwoControllableEntity = null;
            this.PlayerThreeControllableEntity = null;
            this.PlayerFourControllableEntity = null;
        }

        public void ControlEntities(GameTime gameTime) {
            if (PlayerOneControllableEntity != null) {
                PlayerOneControllableEntity.Control(gameTime, GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), Mouse.GetState());
            }
            if (PlayerTwoControllableEntity != null) {
				PlayerOneControllableEntity.Control(gameTime, GamePad.GetState(PlayerIndex.Two), Keyboard.GetState(), Mouse.GetState());
            }
            if (PlayerThreeControllableEntity != null) {
				PlayerOneControllableEntity.Control(gameTime, GamePad.GetState(PlayerIndex.Three), Keyboard.GetState(), Mouse.GetState());
            }
            if (PlayerFourControllableEntity != null) {
				PlayerOneControllableEntity.Control(gameTime, GamePad.GetState(PlayerIndex.Four), Keyboard.GetState(), Mouse.GetState());
            }
        }

    }

}

