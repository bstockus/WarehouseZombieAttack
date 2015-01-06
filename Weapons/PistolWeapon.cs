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

    public class PistolWeapon : Weapon, IAttackerEntity {

        static readonly Single TRIGGER_DEADBAND = 0.4f;
        static readonly Double DELAY_BETWEEN_SHOTS = 0.1;
        static readonly Double DELAY_BETWEEN_RELOAD_AND_SHOTS = 1.0;

        //static SoundEffect pistolShotSoundEffect;
        //static SoundEffect pistolEmptySoundEffect;
        //static SoundEffect pistolReloadSoundEffect;
        public static TextureSheet pistolAttackTextureSheet;

        Boolean fired = false;
        Double delayTimer;

        public Rectangle AttackerBounds {
            get;
            private set;
        }

        public Boolean[,] AttackerBooleans {
            get;
            private set;
        }

        public Point[] AttackerPixels {
            get;
            private set;
        }

        public Matrix AttackerTransformMatrix {
            get;
            private set;
        }

        public override DamageType DamageType {
            get {
                return DamageType.Projectile;
            }
        }

        public override Single DamageValue {
            get {
                return 60.0f;
            }
        }

        public override HUDWeaponInfo HUDInfo {
            get {
                return new HUDWeaponInfo(2, "Pistol", HUDWeaponType.Pistol, 20.0f);
            }
        }

        public override Int32 CellIndex {
            get {
                return 2;
            }
        }

        public PistolWeapon()
            : base() {
                delayTimer = 0.0;
        }

        public static void LoadContent(ContentManager contentManager) {
            //pistolShotSoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Pistol/pistolShot");
			//pistolEmptySoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Pistol/pistolEmpty");
			//pistolReloadSoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Pistol/pistolReload");
            pistolAttackTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Weapons/PistolAttack"), new Point(1, 1), new Vector2(-40.0f, 0.0f));
        }

        public override void Deploy() {
            delayTimer = 0.0;
        }

        public override void Stow() {

        }

		public override void Update(GameTime gameTime, Boolean leftFireValue, Boolean rightFireValue, Boolean reload, SurvivorSprite survivorSprite) {
            delayTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (!fired && rightFireValue && delayTimer <= 0.0) {
                if (survivorSprite.Ammunition.PistolRoundsInCurrentClip > 0) {
                    survivorSprite.Ammunition.PistolRoundsInCurrentClip--;
                    //pistolShotSoundEffect.Play();
					Sounds.GetSound("Weapons.Pistol.Shot").Play();
                    this.Attack(survivorSprite);
                } else if (!survivorSprite.Game.Options.AutoReload || survivorSprite.Ammunition.PistolRoundsTotal == 0) {
                    //pistolEmptySoundEffect.Play();
					Sounds.GetSound("Weapons.Pistol.Empty").Play();
                }
                fired = true;
                delayTimer = DELAY_BETWEEN_SHOTS;
            } else if (!rightFireValue) {
                fired = false;
                if (reload || (survivorSprite.Ammunition.PistolRoundsInCurrentClip == 0 && survivorSprite.Game.Options.AutoReload) && delayTimer <= 0.0) {
                    if (survivorSprite.Ammunition.ReloadPistolClip()) {
                        //pistolReloadSoundEffect.Play();
						Sounds.GetSound("Weapons.Pistol.Reload").Play();
                        delayTimer = DELAY_BETWEEN_RELOAD_AND_SHOTS;
                    }
                }
            }
        }

        private void Attack(SurvivorSprite survivorSprite) {
            this.AttackerBooleans = pistolAttackTextureSheet.CellCollisionBooleans[0];
            this.AttackerPixels = pistolAttackTextureSheet.CellCollisionPixels[0];
            this.AttackerBounds = CollisionHelper.CalculateCollisionRectangle(
                survivorSprite.Location.TransformMatrixForOffset(pistolAttackTextureSheet.CellOffsets[0]),
                                                                   new Point(pistolAttackTextureSheet.CellSourceRectangles[0].Width,
                                                                             pistolAttackTextureSheet.CellSourceRectangles[0].Height));
            this.AttackerTransformMatrix = survivorSprite.Location.TransformMatrixForOffset(pistolAttackTextureSheet.CellOffsets[0]);
			AttackResults attackResults = survivorSprite.Game.ZombiesSubsystem.ZombiesAttackManager.AttackWithProjectileSingleTarget(this, DamageValue);
			if (attackResults == AttackResults.Kill) {
				survivorSprite.Game.Results.PistolWeaponResults.UsageResultingInKills++;
				survivorSprite.Game.Results.KillResults.PistolShotKills++;
			} else if (attackResults == AttackResults.Damage) {
				survivorSprite.Game.Results.PistolWeaponResults.UsageResultingInDamage++;
			} else {
				survivorSprite.Game.Results.PistolWeaponResults.UsageResultingInNoDamage++;
			}
        }

    }

}
