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

    public abstract class Weapon {

        public abstract DamageType DamageType {
            get;
        }

        public abstract Single DamageValue {
            get;
        }

        public abstract HUDWeaponInfo HUDInfo {
            get;
        }

        public abstract Int32 CellIndex {
            get;
        }

        public Weapon() {
        }

        public abstract void Deploy();

        public abstract void Stow();

        public abstract void Update(GameTime gameTime, Boolean leftFireValue, Boolean rightFireValue, Boolean reload, SurvivorSprite survivorSprite);

    }

}
