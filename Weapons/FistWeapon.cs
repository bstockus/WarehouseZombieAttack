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

    public class FistWeapon : Weapon {

        static readonly Int32[] ANIMATED_CELL_INDICIES = new Int32[4] { 0, 4, 8, 12 };
        static readonly Single TRIGGER_DEADBAND = 0.4f;
        static readonly Int32 ATTACK_CELL_INDEX = 3;
        static readonly Single STAMINA_LOSS_VALUE = 2.0f;

        //static SoundEffect survivorFistPunchSoundEffect;

        Int32 currentAnimationIndex;

        public override DamageType DamageType {
            get {
                return DamageType.Melee;
            }
        }

        public override Single DamageValue {
            get {
                return 20.0f;
            }
        }

        public override HUDWeaponInfo HUDInfo {
            get {
                return new HUDWeaponInfo(0, "Fists", HUDWeaponType.None, 20.0f);
            }
        }

        public override Int32 CellIndex {
            get {
                return ANIMATED_CELL_INDICIES[currentAnimationIndex];
            }
        }

        public FistWeapon()
            : base() {

        }

        public static void LoadContent(ContentManager contentManager) {
            //survivorFistPunchSoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Fist/fistPunch");
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
					Sounds.GetSound("Weapons.Fist.Punch").Play();
					survivorSprite.Game.Results.FistWeaponResults.UsageResultingInDamage++;
				} else if (attackResults == AttackResults.Kill) {
					Sounds.GetSound("Weapons.Fist.Punch").Play();
					survivorSprite.Game.Results.FistWeaponResults.UsageResultingInKills++;
					survivorSprite.Game.Results.KillResults.FistPunchKills++;
				} else {
					survivorSprite.Game.Results.FistWeaponResults.UsageResultingInNoDamage++;
				}
                survivorSprite.CurrentStamina -= STAMINA_LOSS_VALUE;
            }
        }

    }

}
