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

    public class UpdateManager : Manager {

        List<IUpdateableEntity> updatableEntities;

        public List<IUpdateableEntity> UpdatableEntities {
            get {
                return updatableEntities;
            }
        }

        public UpdateManager(Game game) : base(game) {
            updatableEntities = new List<IUpdateableEntity>();
        }

        public void UpdateEntities(GameTime gameTime) {
            foreach (IUpdateableEntity updatableEntity in updatableEntities) {
                updatableEntity.Update(gameTime);
            }
        }

    }

}

