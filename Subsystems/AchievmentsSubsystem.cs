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
    public struct AchievmentsSaveStruct {
        public Boolean CrowbarWeaponUnlockedAchievment;
        public Boolean ChainsawWeaponUnlockedAchievment;
        public Boolean PistolWeaponUnlockedAchievment;
        public Boolean ShotgunWeaponUnlockedAchievment;
    }

    public class AchievmentsSubsystem : Subsystem {

        #region Constants

        static readonly int CROWBAR_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK = 5;
        static readonly int CHAINSAW_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK = 10;
        static readonly int PISTOL_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK = 20;
        static readonly int SHOTGUN_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK = 30;

        #endregion

        #region Fields

        #endregion

        #region Properties

        public Boolean CrowbarWeaponUnlockedAchievment {
            get;
            set;
        }

        public Boolean ChainsawWeaponUnlockedAchievment {
            get;
            set;
        }

        public Boolean PistolWeaponUnlockedAchievment {
            get;
            set;
        }

        public Boolean ShotgunWeaponUnlockedAchievment {
            get;
            set;
        }

        public AchievmentsSaveStruct AchievmentsSaveStruct {
            get {
                return new AchievmentsSaveStruct() {
                    CrowbarWeaponUnlockedAchievment = this.CrowbarWeaponUnlockedAchievment,
                    ChainsawWeaponUnlockedAchievment = this.ChainsawWeaponUnlockedAchievment,
                    PistolWeaponUnlockedAchievment = this.PistolWeaponUnlockedAchievment,
                    ShotgunWeaponUnlockedAchievment = this.ShotgunWeaponUnlockedAchievment
                };
            }
        }

        #endregion

        #region Public Methods

        public AchievmentsSubsystem(Game game)
            : base(game) {
                this.ChainsawWeaponUnlockedAchievment = false;
                this.CrowbarWeaponUnlockedAchievment = false;
                this.PistolWeaponUnlockedAchievment = false;
                this.ShotgunWeaponUnlockedAchievment = false;
        }

        public static void LoadContent(ContentManager contentManager) {

        }

        public void Initialize(AchievmentsSaveStruct saveStruct) {
            this.CrowbarWeaponUnlockedAchievment = saveStruct.CrowbarWeaponUnlockedAchievment;
            this.ChainsawWeaponUnlockedAchievment = saveStruct.ChainsawWeaponUnlockedAchievment;
            this.PistolWeaponUnlockedAchievment = saveStruct.PistolWeaponUnlockedAchievment;
            this.ShotgunWeaponUnlockedAchievment = saveStruct.ShotgunWeaponUnlockedAchievment;
        }

        public override void Update(GameTime gameTime) {
            UnlockWeapons();
        }

        #endregion

        #region Private Methods

        private void UnlockWeapons() {
			if (!Game.Options.WeaponsUnlocked) {
				if (Game.Results.NumberOfZombieKills >= CROWBAR_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK && !CrowbarWeaponUnlockedAchievment) {
					Game.SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.CROWBAR_WEAPON_INDEX);
					Game.HUD.AddMessage("Crowbar Weapon Unlocked!", Color.White, true);
					CrowbarWeaponUnlockedAchievment = true;
				}
				if (Game.Results.NumberOfZombieKills >= CHAINSAW_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK && !ChainsawWeaponUnlockedAchievment) {
					Game.SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.CHAINSAW_WEAPON_INDEX);
					Game.HUD.AddMessage("Chainsaw Weapon Unlocked!", Color.White, true);
					ChainsawWeaponUnlockedAchievment = true;
				}
				if (Game.Results.NumberOfZombieKills >= PISTOL_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK && !PistolWeaponUnlockedAchievment) {
					Game.SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.PISTOL_WEAPON_INDEX);
					Game.HUD.AddMessage("Pistol Weapon Unlocked!", Color.White, true);
					PistolWeaponUnlockedAchievment = true;
				}
				if (Game.Results.NumberOfZombieKills >= SHOTGUN_NUMBER_OF_ZOMBIE_KILLS_FOR_UNLOCK && !ShotgunWeaponUnlockedAchievment) {
					Game.SurvivorSubsystem.PlayerOneSurvivorSprite.EnableWeapon(SurvivorSprite.SHOTGUN_WEAPON_INDEX);
					Game.HUD.AddMessage("Shotgun Weapon Unlocked!", Color.White, true);
					ShotgunWeaponUnlockedAchievment = true;
				}
			} else {
				ChainsawWeaponUnlockedAchievment = true;
				CrowbarWeaponUnlockedAchievment = true;
				PistolWeaponUnlockedAchievment = true;
				ShotgunWeaponUnlockedAchievment = true;
			}
        }

        #endregion

    }
}
