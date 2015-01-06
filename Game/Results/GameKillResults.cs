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
	public struct KillsStatsSaveStruct {
		public int FistPunchKills;
		public int CrowbarHitKills;
		public int ChainsawKills;
		public int PistolShotKills;
		public int ShotgunBlastKills;
		public int DoubleKills;
		public int TripleKills;
	}

	public class GameKillResults {

		public int FistPunchKills {
			get;
			set;
		}

		public int CrowbarHitKills {
			get;
			set;
		}

		public int ChainsawKills {
			get;
			set;
		}

		public int PistolShotKills {
			get;
			set;
		}

		public int ShotgunBlastKills {
			get;
			set;
		}

		public int DoubleKills {
			get;
			set;
		}

		public int TripleKills {
			get;
			set;
		}

		public KillsStatsSaveStruct KillsStatsSaveStruct {
			get {
				return new KillsStatsSaveStruct() {
					FistPunchKills = this.FistPunchKills,
					CrowbarHitKills = this.CrowbarHitKills,
					ChainsawKills = this.ChainsawKills,
					PistolShotKills = this.PistolShotKills,
					ShotgunBlastKills = this.ShotgunBlastKills,
					DoubleKills = this.DoubleKills,
					TripleKills = this.TripleKills
				};
			}
		}

		public GameKillResults(int fistPunchKills, int crowbarHitKills, int chainsawKills, int pistolShotKills, int shotgunBlastKills, int doubleKills, int tripleKills) {
			this.FistPunchKills = fistPunchKills;
			this.CrowbarHitKills = crowbarHitKills;
			this.ChainsawKills = chainsawKills;
			this.PistolShotKills = pistolShotKills;
			this.ShotgunBlastKills = shotgunBlastKills;
			this.DoubleKills = doubleKills;
			this.TripleKills = tripleKills;
		}

		public GameKillResults() : this(0, 0, 0, 0, 0, 0, 0) {

		}

		public GameKillResults(KillsStatsSaveStruct saveStruct) : this(saveStruct.FistPunchKills, saveStruct.CrowbarHitKills, saveStruct.ChainsawKills, saveStruct.PistolShotKills, 
		                                                               saveStruct.ShotgunBlastKills, saveStruct.DoubleKills, saveStruct.TripleKills) {

		}

	}

}

