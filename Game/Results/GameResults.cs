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
    public struct StatsSaveStruct {
        public int NumberOfZombieKills;
        public int NumberOfZombieWaves;
        public double SurvivalTime;
        public CollectablesStatsSaveStruct CollectablesStats;
		public KillsStatsSaveStruct KillsStats;
		public WeaponsStatsSaveStruct FistWeaponsStats;
		public WeaponsStatsSaveStruct CrowbarWeaponsStats;
		public WeaponsStatsSaveStruct ChainsawWeaponsStats;
		public WeaponsStatsSaveStruct PistolWeaponsStats;
		public WeaponsStatsSaveStruct ShotgunWeaponsStats;
    }

    public class GameResults {

        static readonly int POINTS_PER_ZOMBIE_KILLED = 50;
        static readonly int POINTS_PER_DOUBLE_KILL = 200;
		static readonly int POINTS_PER_TRIPLE_KILL = 500;

        public double SurvivalTime {
            get;
            set;
        }

        public int NumberOfZombieKills {
            get;
            set;
        }

        public int NumberOfZombieWaves {
            get;
            set;
        }

        public GameCollectableResults CollectablesResults {
            get;
            private set;
        }

		public GameKillResults KillResults {
			get;
			private set;
		}

		public GameWeaponResults FistWeaponResults {
			get;
			private set;
		}

		public GameWeaponResults CrowbarWeaponResults {
			get;
			private set;
		}

		public GameWeaponResults ChainsawWeaponResults {
			get;
			private set;
		}

		public GameWeaponResults PistolWeaponResults {
			get;
			private set;
		}

		public GameWeaponResults ShotgunWeaponResults {
			get;
			private set;
		}

        public int Points {
            get {
				return + (NumberOfZombieKills * POINTS_PER_ZOMBIE_KILLED)
					+ (KillResults.DoubleKills * POINTS_PER_DOUBLE_KILL)
					+ (KillResults.TripleKills * POINTS_PER_TRIPLE_KILL);
            }
        }

        public StatsSaveStruct StatsSaveStruct {
            get {
                return new StatsSaveStruct() {
                    SurvivalTime = this.SurvivalTime,
                    NumberOfZombieKills = this.NumberOfZombieKills,
                    NumberOfZombieWaves = this.NumberOfZombieWaves,
                    CollectablesStats = CollectablesResults.CollectablesStatsSaveStruct,
					KillsStats = KillResults.KillsStatsSaveStruct,
					FistWeaponsStats = FistWeaponResults.WeaponsStatsSaveStruct,
					CrowbarWeaponsStats = CrowbarWeaponResults.WeaponsStatsSaveStruct,
					ChainsawWeaponsStats = ChainsawWeaponResults.WeaponsStatsSaveStruct,
					PistolWeaponsStats = PistolWeaponResults.WeaponsStatsSaveStruct,
					ShotgunWeaponsStats = ShotgunWeaponResults.WeaponsStatsSaveStruct
                };
            }
        }

        public GameResults() {
            this.SurvivalTime = 0.0;
            this.NumberOfZombieKills = 0;
            this.NumberOfZombieWaves = 0;
            this.CollectablesResults = new GameCollectableResults();
			this.KillResults = new GameKillResults();
			this.FistWeaponResults = new GameWeaponResults();
			this.CrowbarWeaponResults = new GameWeaponResults();
			this.ChainsawWeaponResults = new GameWeaponResults();
			this.PistolWeaponResults = new GameWeaponResults();
			this.ShotgunWeaponResults = new GameWeaponResults();
        }

        public GameResults(StatsSaveStruct statsSave) {
            this.SurvivalTime = statsSave.SurvivalTime;
            this.NumberOfZombieKills = statsSave.NumberOfZombieKills;
            this.NumberOfZombieWaves = statsSave.NumberOfZombieWaves;
            this.CollectablesResults = new GameCollectableResults(statsSave.CollectablesStats);
			this.KillResults = new GameKillResults(statsSave.KillsStats);
			this.FistWeaponResults = new GameWeaponResults(statsSave.FistWeaponsStats);
			this.CrowbarWeaponResults = new GameWeaponResults(statsSave.CrowbarWeaponsStats);
			this.ChainsawWeaponResults = new GameWeaponResults(statsSave.ChainsawWeaponsStats);
			this.PistolWeaponResults = new GameWeaponResults(statsSave.PistolWeaponsStats);
			this.ShotgunWeaponResults = new GameWeaponResults(statsSave.ShotgunWeaponsStats);
        }

    }
    
}
