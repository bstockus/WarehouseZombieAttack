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
	public struct WeaponsStatsSaveStruct {
		public int UsageResultingInNoDamage;
		public int UsageResultingInDamage;
		public int UsageResultingInKills;
	}

	public class GameWeaponResults {

		public int UsageResultingInNoDamage {
			get;
			set;
		}

		public int UsageResultingInDamage {
			get;
			set;
		}

		public int UsageResultingInKills {
			get;
			set;
		}

		public int TotalUsages {
			get {
				return UsageResultingInKills + UsageResultingInDamage + UsageResultingInNoDamage;
			}
		}

		public int TotalUsagesWithKillsOrDamage {
			get {
				return UsageResultingInKills + UsageResultingInDamage;
			}
		}

		public float KillsPerUsage {
			get {
				if (TotalUsages > 0) {
					return (float)UsageResultingInKills / (float)TotalUsages;
				} else {
					return 0.0f;
				}
			}
		}

		public float DamagesPerUsage {
			get {
				if (TotalUsages > 0) {
					return (float)UsageResultingInDamage / (float)TotalUsages;
				} else {
					return 0.0f;
				}
			}
		}

		public float KillsOrDamagePerUsage {
			get {
				if (TotalUsages > 0) {
					return (float)TotalUsagesWithKillsOrDamage / (float)TotalUsages;
				} else {
					return 0.0f;
				}
			}
		}

		public WeaponsStatsSaveStruct WeaponsStatsSaveStruct {
			get {
				return new WeaponsStatsSaveStruct() {
					UsageResultingInNoDamage = this.UsageResultingInNoDamage,
					UsageResultingInDamage = this.UsageResultingInDamage,
					UsageResultingInKills = this.UsageResultingInKills
				};
			}
		}

		public GameWeaponResults(int noDamage, int damage, int kills) {
			this.UsageResultingInNoDamage = noDamage;
			this.UsageResultingInDamage = damage;
			this.UsageResultingInKills = kills;
		}

		public GameWeaponResults() : this(0, 0, 0) {

		}

		public GameWeaponResults(WeaponsStatsSaveStruct saveStruct) : this(saveStruct.UsageResultingInNoDamage, saveStruct.UsageResultingInDamage, saveStruct.UsageResultingInKills) {

		}

	}

}

