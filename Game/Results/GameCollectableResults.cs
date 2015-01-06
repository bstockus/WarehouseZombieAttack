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

    [Serializable]
    public struct CollectablesStatsSaveStruct {
        public int HealthCollected;
        public int GasolineCollected;
        public int EnergyCollected;
        public int PistolAmmoCollected;
        public int ShotgunAmmoCollected;
    }

    public class GameCollectableResults {

        public int HealthCollectablesCollected {
            get;
            set;
        }

        public int GasolineCollectablesCollected {
            get;
            set;
        }

        public int EnergyCollectablesCollected {
            get;
            set;
        }

        public int PistolAmmoCollectablesCollected {
            get;
            set;
        }

        public int ShotgunAmmoCollectablesCollected {
            get;
            set;
        }

        public CollectablesStatsSaveStruct CollectablesStatsSaveStruct {
            get {
                return new CollectablesStatsSaveStruct() {
                    GasolineCollected = this.GasolineCollectablesCollected,
                    HealthCollected = this.HealthCollectablesCollected,
                    EnergyCollected = this.EnergyCollectablesCollected,
                    PistolAmmoCollected = this.PistolAmmoCollectablesCollected,
                    ShotgunAmmoCollected = this.ShotgunAmmoCollectablesCollected
                };
            }
        }

        public GameCollectableResults(int gasolineCollected, int healthCollected, int energyCollected, int pistolAmmoCollected, int shotgunAmmoCollected) {
            this.GasolineCollectablesCollected = gasolineCollected;
            this.HealthCollectablesCollected = healthCollected;
            this.EnergyCollectablesCollected = energyCollected;
            this.PistolAmmoCollectablesCollected = pistolAmmoCollected;
            this.ShotgunAmmoCollectablesCollected = ShotgunAmmoCollectablesCollected;
        }

        public GameCollectableResults(CollectablesStatsSaveStruct saveStruct)
            : this(saveStruct.GasolineCollected, saveStruct.HealthCollected, saveStruct.EnergyCollected, saveStruct.PistolAmmoCollected, saveStruct.ShotgunAmmoCollected) {

        }

        public GameCollectableResults()
            : this(0, 0, 0, 0, 0) {

        }

    }

}
