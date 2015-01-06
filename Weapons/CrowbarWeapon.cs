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
using SimpleGameFramework.Content;

namespace WarehouseZombieAttack {

    public class CrowbarWeapon : Weapon {

        static readonly Int32[] ANIMATED_CELL_INDICIES = new Int32[4] { 1, 5, 9, 13 };
        static readonly Single TRIGGER_DEADBAND = 0.4f;
        static readonly Int32 ATTACK_CELL_INDEX = 3;
        static readonly Single STAMINA_LOSS_VALUE = 3.0f;

        //static SoundEffect survivorCrowbarHitSoundEffect;

        Int32 currentAnimationIndex;

        public override DamageType DamageType {
            get {
                return DamageType.Melee;
            }
        }

        public override Single DamageValue {
            get {
                return 50.0f;
            }
        }

        public override HUDWeaponInfo HUDInfo {
            get {
                return new HUDWeaponInfo(4, "Crowbar", HUDWeaponType.None, 50.0f);
            }
        }

        public override Int32 CellIndex {
            get {
                return ANIMATED_CELL_INDICIES[currentAnimationIndex];
            }
        }

        public static void LoadContent(ContentManager contentManager) {
            //survivorCrowbarHitSoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Crowbar/crowbarHit");
        }

        public CrowbarWeapon()
            : base() {

        }

        public override void Deploy() {

        }

        public override void Stow() {

        }

		public override void Update(GameTime gameTime, Boolean leftFireValue, Boolean rightFireValue, Boolean reload, SurvivorSprite survivorSprite) {
            Int32 oldFistIndex = currentAnimationIndex;
            if (rightFireValue && currentAnimationIndex < (ANIMATED_CELL_INDICIES.Length - 1)) {
                currentAnimationIndex++;
            } else if (!rightFireValue && currentAnimationIndex > 0) {
                currentAnimationIndex--;
            }
            if (oldFistIndex != currentAnimationIndex && currentAnimationIndex == ATTACK_CELL_INDEX) {
                AttackResults attackResults = survivorSprite.PerformMeleeAttack(DamageValue);
				if (attackResults == AttackResults.Damage) {
					Sounds.GetSound("Weapons.Crowbar.Hit").Play();
//					survivorCrowbarHitSoundEffect.Play();
					survivorSprite.Game.Results.CrowbarWeaponResults.UsageResultingInDamage++;
				} else if (attackResults == AttackResults.Kill) {
					Sounds.GetSound("Weapons.Crowbar.Hit").Play();
//					survivorCrowbarHitSoundEffect.Play();
					survivorSprite.Game.Results.CrowbarWeaponResults.UsageResultingInKills++;
					survivorSprite.Game.Results.KillResults.CrowbarHitKills++;
				} else {
					survivorSprite.Game.Results.CrowbarWeaponResults.UsageResultingInNoDamage++;
				}
                survivorSprite.CurrentStamina -= STAMINA_LOSS_VALUE;
            }
        }

    }

}
