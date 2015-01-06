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

    public class ChainsawWeapon : Weapon {

        static readonly Single TRIGGER_DEADBAND = 0.4f;
        static readonly Single GASOLINE_USAGE_RATE = 0.1f;

        //static SoundEffect survivorChainsawSoundEffect;

        SoundEffectInstance chainsawSoundEffectInstance;

        public override DamageType DamageType {
            get {
                return DamageType.Melee;
            }
        }

        public override Single DamageValue {
            get {
                return 8.0f;
            }
        }

        public override HUDWeaponInfo HUDInfo {
            get {
                return new HUDWeaponInfo(8, "Chainsaw", HUDWeaponType.Gasoline, 200.0f);
            }
        }

        public override Int32 CellIndex {
            get {
                return 10;
            }
        }

        public ChainsawWeapon()
            : base() {

        }

        public static void LoadContent(ContentManager contentManager) {
            //survivorChainsawSoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Chainsaw/chainsaw");
        }

        public override void Deploy() {
            if (chainsawSoundEffectInstance == null) {
                chainsawSoundEffectInstance = Sounds.GetSound("Weapons.Chainsaw.Running").CreateInstance();
            }
        }

        public override void Stow() {
            if (chainsawSoundEffectInstance.State == SoundState.Playing) {
                chainsawSoundEffectInstance.Stop();
            }
        }

		public override void Update(GameTime gameTime, Boolean leftFireValue, Boolean rightFireValue, Boolean reload, SurvivorSprite survivorSprite) {
            if (rightFireValue && survivorSprite.Ammunition.GasolineInCurrentCan > 0.0f) {
				AttackResults attackResults = survivorSprite.PerformMeleeAttack(DamageValue);
				if (attackResults == AttackResults.Kill) {
					survivorSprite.Game.Results.ChainsawWeaponResults.UsageResultingInKills++;
					survivorSprite.Game.Results.KillResults.ChainsawKills++;
				} else if (attackResults == AttackResults.Damage) {
					survivorSprite.Game.Results.ChainsawWeaponResults.UsageResultingInDamage++;
				} else {
					survivorSprite.Game.Results.ChainsawWeaponResults.UsageResultingInNoDamage++;
				}
                survivorSprite.Ammunition.GasolineInCurrentCan -= GASOLINE_USAGE_RATE;
            }
            if (rightFireValue && chainsawSoundEffectInstance.State != SoundState.Playing && survivorSprite.Ammunition.GasolineInCurrentCan > 0.0f) {
                chainsawSoundEffectInstance.Play();
            } else if (!rightFireValue) {
                chainsawSoundEffectInstance.Stop();
            }
            if (survivorSprite.Ammunition.GasolineInCurrentCan < 0.0f && chainsawSoundEffectInstance.State == SoundState.Playing) {
                chainsawSoundEffectInstance.Stop();
            }
            if (reload) survivorSprite.Ammunition.ReloadGasolineCan();
        }

    }

}
