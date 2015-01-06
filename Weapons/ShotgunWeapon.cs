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

    public class ShotgunWeapon : Weapon, IAttackerEntity {

        static readonly Single TRIGGER_DEADBAND = 0.4f;
        static readonly Double DELAY_BETWEEN_SHOTS = 0.3;
        static readonly Double DELAY_BETWEEN_RELOADS = 0.6;

        //static SoundEffect shotgunBlastSoundEffect;
        //static SoundEffect shotgunEmptySoundEffect;
        //static SoundEffect shotgunReloadSoundEffect;
        public static TextureSheet shotgunAttackTextureSheet;

        Boolean fired = false;
        Boolean reloading = false;
        Int32 reloadCounter;
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
                return 1.5f;
            }
        }

        public override HUDWeaponInfo HUDInfo {
            get {
                return new HUDWeaponInfo(6, "Shotgun", HUDWeaponType.Shotgun, 20.0f);
            }
        }

        public override Int32 CellIndex {
            get {
                return 6;
            }
        }

        public ShotgunWeapon() : base() {
			delayTimer = 0.0;
		}

        public static void LoadContent(ContentManager contentManager) {
            //shotgunBlastSoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Shotgun/shotgunBlast");
			//shotgunEmptySoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Shotgun/shotgunEmpty");
			//shotgunReloadSoundEffect = contentManager.Load<SoundEffect>(@"Sounds/Weapons/Shotgun/shotgunReload");
            shotgunAttackTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Weapons/ShotgunAttack"), new Point(1, 1), new Vector2(-40.0f, 10.0f));
        }

        public override void Deploy() {
            delayTimer = 0.0;
            reloading = false;
        }

        public override void Stow() {

        }

		public override void Update(GameTime gameTime, Boolean leftFireValue, Boolean rightFireValue, Boolean reload, SurvivorSprite survivorSprite) {
            delayTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (reloading) {
                if (delayTimer <= 0.0) {
                    reloadCounter--;
                    if (reloadCounter < 0) {
                        reloading = false;
                    } else {
                        delayTimer = DELAY_BETWEEN_RELOADS;
                        //shotgunReloadSoundEffect.Play();
						Sounds.GetSound("Weapons.Shotgun.Reload").Play();
                    }
                }
            } else {
                if (!fired && rightFireValue && delayTimer <= 0.0) {
                    if (survivorSprite.Ammunition.ShotgunShellsInMagazine > 0) {
                        survivorSprite.Ammunition.ShotgunShellsInMagazine--;
                        //shotgunBlastSoundEffect.Play();
						Sounds.GetSound("Weapons.Shotgun.Blast").Play();
                        this.delayTimer = DELAY_BETWEEN_SHOTS;
                        this.Attack(survivorSprite);
                    } else if (!survivorSprite.Game.Options.AutoReload || survivorSprite.Ammunition.ShotgunShellsTotal == 0) {
                        //shotgunEmptySoundEffect.Play();
						Sounds.GetSound("Weapons.Shotgun.Empty").Play();
                        this.delayTimer = DELAY_BETWEEN_SHOTS;
                    }
                    fired = true;
                } else if (!rightFireValue) {
                    fired = false;
                }
                if (reload || (survivorSprite.Ammunition.ShotgunShellsInMagazine == 0 && survivorSprite.Game.Options.AutoReload)) {
                    Int32 reloadCount = survivorSprite.Ammunition.ReloadShotgunMagazine();
                    if (reloadCount > 0) {
                        reloading = true;
                        reloadCounter = reloadCount - 1;
                        delayTimer = DELAY_BETWEEN_RELOADS;
                        //shotgunReloadSoundEffect.Play();
						Sounds.GetSound("Weapons.Shotgun.Reload").Play();
                    }
                }
            }
        }

        private void Attack(SurvivorSprite survivorSprite) {
            this.AttackerBooleans = shotgunAttackTextureSheet.CellCollisionBooleans[0];
            this.AttackerPixels = shotgunAttackTextureSheet.CellCollisionPixels[0];
            this.AttackerBounds = CollisionHelper.CalculateCollisionRectangle(
                survivorSprite.Location.TransformMatrixForOffset(shotgunAttackTextureSheet.CellOffsets[0]),
                                                                   new Point(shotgunAttackTextureSheet.CellSourceRectangles[0].Width,
                                                                             shotgunAttackTextureSheet.CellSourceRectangles[0].Height));
            this.AttackerTransformMatrix = survivorSprite.Location.TransformMatrixForOffset(shotgunAttackTextureSheet.CellOffsets[0]);
            MultiAttackResults multiAttackResults = survivorSprite.Game.ZombiesSubsystem.ZombiesAttackManager.AttackWithProjectile(this, DamageValue);
			survivorSprite.Game.Results.KillResults.ShotgunBlastKills += multiAttackResults.Kills;
			if (multiAttackResults.Kills > 0) {
				survivorSprite.Game.Results.ShotgunWeaponResults.UsageResultingInKills++;
				if (multiAttackResults.Kills == 2) {
					survivorSprite.Game.Results.KillResults.DoubleKills++;
				} else if (multiAttackResults.Kills == 3) {
					survivorSprite.Game.Results.KillResults.TripleKills++;
				}
			} else if (multiAttackResults.Damages > 0) {
				survivorSprite.Game.Results.ShotgunWeaponResults.UsageResultingInDamage++;
			} else {
				survivorSprite.Game.Results.ShotgunWeaponResults.UsageResultingInNoDamage++;
			}
        }

    }

}
