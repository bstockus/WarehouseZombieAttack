using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
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
    public enum GameDifficulty {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }

    [Serializable]
    public enum GameSurvivor {
        Luke = 0,
        Bryan = 1
    }

    [Serializable]
    public struct OptionsSaveStruct {
        public GameDifficulty GameDifficulty;
        public GameSurvivor GameSurvivor;
        public Boolean AutoReload;
		public Boolean WeaponsUnlocked;
    }

    public class GameOptions {

        public GameDifficulty GameDifficulty {
            get;
            set;
        }

        public GameSurvivor GameSurvivor {
            get;
            set;
        }

        public Boolean AutoReload {
            get;
            set;
        }

		public Boolean WeaponsUnlocked {
			get;
			set;
		}

        public OptionsSaveStruct OptionsSaveStruct {
            get {
                return new OptionsSaveStruct() {
                    GameDifficulty = this.GameDifficulty,
                    GameSurvivor = this.GameSurvivor,
                    AutoReload = this.AutoReload,
					WeaponsUnlocked = this.WeaponsUnlocked
                };
            }
        }

        public GameOptions(GameDifficulty gameDifficulty, GameSurvivor gameSurvivor, Boolean autoReload, Boolean weaponsUnlocked) {
            this.GameDifficulty = gameDifficulty;
            this.GameSurvivor = gameSurvivor;
            this.AutoReload = autoReload;
			this.WeaponsUnlocked = weaponsUnlocked;
        }

        public GameOptions(OptionsSaveStruct saveStruct)
            : this(saveStruct.GameDifficulty, saveStruct.GameSurvivor, saveStruct.AutoReload, saveStruct.WeaponsUnlocked) {

        }

    }

}
