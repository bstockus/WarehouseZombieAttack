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

    public enum SurvivorSpriteWeapon {
        Fists,
        Crowbar,
        Chainsaw,
        Pistol
    }

    public struct MeleeWeaponSpecifier {
        public Int32[] TextureCellIndexes;
        public Int32 AttackCellIndex;
        public Single DamageValue;
    }

    [Serializable]
    public struct SurvivorSpriteSaveStruct {
        public Single Health;
        public Boolean Alive;
        public Vector2 Position;
        public Single Scale;
        public Single Rotation;
        public Single Stamina;
        public Boolean[] WeaponsEnabled;
        public AmmunitionSaveStruct Ammo;
		public Int32 CurrentWeaponIndex;
    }

    public class SurvivorSprite : Sprite, IControllableEntity, ICollidableEntity, IAttackerEntity, IAttackableEntity {

        #region Constants

        static readonly Single TOP_FORWARD_SPEED = 3.0f;
        static readonly Single TOP_BACKWARD_SPEED = 1.5f;
        static readonly Single TOP_STRAFE_SPEED = 0.5f;
        static readonly Single TOP_TURN_SPEED = 0.1f;
        static readonly Single Y_AXIS_DEADBAND = 0.4f;
        static readonly Single X_AXIS_DEADBAND = 0.5f;
        static readonly Single TURN_AXIS_DEADBAND = 0.6f;
        static readonly Single MAXIMUM_HEALTH_VALUE = 100.0f;
        static readonly Single HEALTH_COLLECTABLE_HEALTH_VALUES = 20.0f;
        static readonly Single MAXIMUM_STAMINA_VALUE = 100.0f;
        static readonly Single STAMINA_LOSS_RATE = 0.5f;
        static readonly Single STAMINA_GAIN_RATE = 0.3f;
        static readonly Double DAMAGE_VIBRATION_TIME = 0.2;
        

        #endregion

        #region Weapon Constants

        public static readonly Int32 FIST_WEAPON_INDEX = 0;
        public static readonly Int32 CROWBAR_WEAPON_INDEX = 1;
        public static readonly Int32 CHAINSAW_WEAPON_INDEX = 2;
        public static readonly Int32 PISTOL_WEAPON_INDEX = 3;
        public static readonly Int32 SHOTGUN_WEAPON_INDEX = 4;

        static readonly Weapon[] WEAPON_SPECIFIERS = new Weapon[5] {
            new FistWeapon(),
            new CrowbarWeapon(),
            new ChainsawWeapon(),
            new PistolWeapon(),
            new ShotgunWeapon()
        };

        #endregion

        #region Instance Fields

        GamePadState oldGamePadState = new GamePadState();
		KeyboardState oldKeyboardState = new KeyboardState();
		MouseState oldMouseState = new MouseState();
        Int32 currentWeaponSpecifierIndex;
        Boolean[] weaponsEnabled;
        Double damageVibrationTimer = 0.0;
        Boolean damageVibrationActive = false;

        #endregion

        #region Static Fields

        static TextureSheet survivorDrawTextureSheet;
        static TextureSheet survivorAttackTextureSheet;
        static TextureSheet survivorCollisionTextureSheet;
        static TextureSheet survivorDefenseTextureSheet;

        #endregion

        #region Collision Properties

        public Rectangle CollisionBounds {
            get {
                return CollisionHelper.CalculateCollisionRectangle(Location.TransformMatrixForOffset(survivorCollisionTextureSheet.CellOffsets[CurrentCellIndex]),
                                                                   new Point(survivorCollisionTextureSheet.CellSourceRectangles[CurrentCellIndex].Width,
                                                                             survivorCollisionTextureSheet.CellSourceRectangles[CurrentCellIndex].Height));
            }
        }

        public Boolean[,] CollisionBooleans {
            get {
                return survivorCollisionTextureSheet.CellCollisionBooleans[CurrentCellIndex];
            }
        }

        public Point[] CollisionPixels {
            get {
                return survivorCollisionTextureSheet.CellCollisionPixels[CurrentCellIndex];
            }
        }

        public Matrix CollisionTransformMatrix {
            get {
                return Location.TransformMatrixForOffset(survivorCollisionTextureSheet.CellOffsets[CurrentCellIndex]);
            }
        }

        #endregion

        #region Attacker Properties

        public Rectangle AttackerBounds {
            get {
                return CollisionHelper.CalculateCollisionRectangle(Location.TransformMatrixForOffset(survivorAttackTextureSheet.CellOffsets[CurrentCellIndex]),
                                                                   new Point(survivorAttackTextureSheet.CellSourceRectangles[CurrentCellIndex].Width,
                                                                             survivorAttackTextureSheet.CellSourceRectangles[CurrentCellIndex].Height));
            }
        }

        public Boolean[,] AttackerBooleans {
            get {
                return survivorAttackTextureSheet.CellCollisionBooleans[CurrentCellIndex];
            }
        }

        public Point[] AttackerPixels {
            get {
                return survivorAttackTextureSheet.CellCollisionPixels[CurrentCellIndex];
            }
        }

        public Matrix AttackerTransformMatrix {
            get {
                return this.CollisionTransformMatrix;
            }
        }

        #endregion

        #region Attack Properties

        public Rectangle AttackBounds {
            get {
                return CollisionHelper.CalculateCollisionRectangle(Location.TransformMatrixForOffset(survivorDefenseTextureSheet.CellOffsets[CurrentCellIndex]),
                                                                   new Point(survivorDefenseTextureSheet.CellSourceRectangles[CurrentCellIndex].Width,
                                                                             survivorDefenseTextureSheet.CellSourceRectangles[CurrentCellIndex].Height));
            }
        }

        public Boolean[,] AttackBooleans {
            get {
                return survivorDefenseTextureSheet.CellCollisionBooleans[CurrentCellIndex];
            }
        }

        public Point[] AttackPixels {
            get {
                return survivorDefenseTextureSheet.CellCollisionPixels[CurrentCellIndex];
            }
        }

        public Matrix AttackTransformMatrix {
            get {
                return this.CollisionTransformMatrix;
            }
        }

        #endregion

        #region Other Properties

        public Single CurrentHealth {
            get;
            private set;
        }

        public Boolean Alive {
            get;
            private set;
        }

        public Weapon[] Weapons {
            get {
                return WEAPON_SPECIFIERS;
            }
        }

        public Boolean[] WeaponsEnabled {
            get {
                return weaponsEnabled;
            }
        }

        public Weapon CurrentWeapon {
            get {
                return WEAPON_SPECIFIERS[currentWeaponSpecifierIndex];
            }
        }

        public SurvivorSpriteSaveStruct SaveStruct {
            get {
                SurvivorSpriteSaveStruct saveStruct = new SurvivorSpriteSaveStruct() {
                    Health = this.CurrentHealth,
                    Alive = this.Alive,
                    Position = this.Location.Position,
                    Scale = this.Location.Scale,
                    Rotation = this.Location.Rotation,
                    Stamina = this.CurrentStamina,
                    WeaponsEnabled = this.weaponsEnabled,
                    Ammo = this.Ammunition.AmmunitionSaveStruct,
					CurrentWeaponIndex = this.currentWeaponSpecifierIndex
                };
                return saveStruct;
            }
        }

        public Single CurrentStamina {
            get;
            set;
        }

        public Int32 CurrentCellIndex {
            get {
                return CurrentWeapon.CellIndex;
            }
        }

        public Ammunition Ammunition {
            get;
            set;
        }

        #endregion

        #region Public Methods

        public static void LoadContent(ContentManager contentManager) {
            Vector2[] offsets = new Vector2[5 * 4];
            for (Int32 index = 0; index < offsets.Length; index++) {
                offsets[index] = new Vector2(10, 21);
            }

            // Load Weapons Content
            FistWeapon.LoadContent(contentManager);
            CrowbarWeapon.LoadContent(contentManager);
            ChainsawWeapon.LoadContent(contentManager);
            PistolWeapon.LoadContent(contentManager);
            ShotgunWeapon.LoadContent(contentManager);

            survivorDrawTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/SurvivorSprite/survivor"), new Point(5, 4), offsets);
            survivorAttackTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/SurvivorSprite/survivorAttack"), new Point(5, 4), offsets);
            survivorCollisionTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/SurvivorSprite/survivorCollision"), new Point(5, 4), offsets);
            survivorDefenseTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/SurvivorSprite/survivorDefense"), new Point(5, 4), offsets);
        }

        public SurvivorSprite(Game game, Location location, Single currentHealth, Boolean alive, Single currentStamina, bool[] weaponsEnabled, Ammunition ammo, Int32 currentWeaponIndex)
            : base(game, location) {
                this.currentWeaponSpecifierIndex = currentWeaponIndex;
                this.weaponsEnabled = weaponsEnabled;
                this.Alive = alive;
                this.CurrentHealth = currentHealth;
                this.CurrentStamina = currentStamina;
                this.Ammunition = ammo;
                CurrentWeapon.Deploy();
        }

        public SurvivorSprite(Game game, SurvivorSpriteSaveStruct saveStruct)
            : this(game, new Location(saveStruct.Position, saveStruct.Scale, saveStruct.Rotation)
            ,saveStruct.Health, saveStruct.Alive, saveStruct.Stamina, saveStruct.WeaponsEnabled, new Ammunition(saveStruct.Ammo), saveStruct.CurrentWeaponIndex) {

        }

        public SurvivorSprite(Game game, Location location)
            : this(game, location, MAXIMUM_HEALTH_VALUE, true, MAXIMUM_STAMINA_VALUE, new bool[5] { true, false, false, false, false }, new Ammunition(), 0) {

        }

        public override void Update(GameTime gameTime) {
            //System.Diagnostics.Debug.WriteLine(this.ToString());
        }

        public void Control(GameTime gameTime, GamePadState gamePadState, KeyboardState keyboardState, MouseState mouseState) {
            // Get Position and Rotation
            Vector2 oldPosition = Location.Position;
            Vector2 position = oldPosition;
            Single oldRotation = Location.Rotation;
            Single rotation = oldRotation;

            // Get Game Pad State
            GameControl gameControl = new GameControl(gamePadState, oldGamePadState, keyboardState, oldKeyboardState, mouseState, oldMouseState);

            // Update Weapon Status
            CalculateWeaponStatus(gameTime, gameControl);

            // Calculate Velocity, Strafe and Rotation
            Single velocity = CalculateVelocity(gameControl.MovementControlY, gameControl.MoveForward, gameControl.MoveBackwards);
			Single strafe = CalculateStrafe(gameControl.MovementControlX, gameControl.StrafeLeft, gameControl.StrafeRight);
            rotation = CalculateRotation(rotation, gameControl.OrientationControlX, gameControl.OrientationControlY, gameControl.turnLeft, gameControl.turnRight);

            // Calculate Movement Velocity Vector
            Vector2 velocityVector = new Vector2(FloatMathHelper.Cos(rotation), FloatMathHelper.Sin(rotation));
            Single strafeRotation = MathHelper.WrapAngle(rotation + MathHelper.PiOver2);
            Vector2 strafeVector = new Vector2(FloatMathHelper.Cos(strafeRotation), FloatMathHelper.Sin(strafeRotation));
            
            // Check for running condition
            if (gameControl.RunningControl) {
                Single multiplicationFactor = 0.0f;
                if (CurrentStamina > (0.2 * MAXIMUM_STAMINA_VALUE)) {
                    //multiplicationFactor = 1.0f * (CurrentStamina / MAXIMUM_STAMINA_VALUE);
                    multiplicationFactor = 1.0f;
                    CurrentStamina -= STAMINA_LOSS_RATE;
                }
                velocity *= (1.0f + multiplicationFactor);
            } else {
                if (CurrentHealth > (0.2 * MAXIMUM_HEALTH_VALUE)) {
                    CurrentStamina += (STAMINA_GAIN_RATE * (CurrentHealth / MAXIMUM_HEALTH_VALUE));
                }
            }
            if (CurrentStamina > MAXIMUM_STAMINA_VALUE) CurrentStamina = MAXIMUM_STAMINA_VALUE;

            // Normalize Vectors and Update Position
            velocityVector.Normalize();
            strafeVector.Normalize();
            position = position + (velocityVector * velocity) + (strafeVector * strafe);
            
            // Update SpriteSpecifier
            Location.Position = position;
            Location.Rotation = rotation;

            // Check for Collisions
            if (Game.ZombiesSubsystem.ZombiesCollisionManager.CheckForCollisionsWith(this)) {
                Location.Position = oldPosition;
                Location.Rotation = oldRotation;
            }

            // Check for Collections
            List<CollectableEntityType> collectedEntities = Game.CollectablesSubsystem.CollectablesCollectionManger.Collect(this);
            foreach (CollectableEntityType collectedEntity in collectedEntities) {
                switch (collectedEntity) {
                    case CollectableEntityType.Health:
                        CurrentHealth += HEALTH_COLLECTABLE_HEALTH_VALUES;
                        Game.HUD.AddMessage("Collected Health.", Color.Blue, false);
                        Game.Results.CollectablesResults.HealthCollectablesCollected++;
                        break;
                    case CollectableEntityType.Gasoline:
                        Ammunition.CollectGasolineCan();
                        Game.HUD.AddMessage("Collected Gasoline Can.", Color.Blue, false);
                        Game.Results.CollectablesResults.GasolineCollectablesCollected++;
                        break;
                    case CollectableEntityType.Energy:
                        //TODO: Implement Enrgy Collectable Action
                        Game.HUD.AddMessage("Collected Energy.", Color.Blue, false);
                        Game.Results.CollectablesResults.EnergyCollectablesCollected++;
                        break;
                    case CollectableEntityType.PistolAmmo:
                        Ammunition.CollectPistolClip();
                        Game.HUD.AddMessage("Collected Pistol Ammo.", Color.Blue, false);
                        Game.Results.CollectablesResults.PistolAmmoCollectablesCollected++;
                        break;
                    case CollectableEntityType.ShotgunAmmo:
                        Ammunition.CollectShotgunShells(6);
                        Game.HUD.AddMessage("Collected Shotgun Ammo.", Color.Blue, false);
                        Game.Results.CollectablesResults.ShotgunAmmoCollectablesCollected++;
                        break;
                    default: break;
                }
            }

            // Normalize State Values
            if (CurrentHealth > MAXIMUM_HEALTH_VALUE) {
                CurrentHealth = MAXIMUM_HEALTH_VALUE;
            } else if (CurrentHealth < 0.0f) {
                CurrentHealth = 0.0f;
            }
            if (CurrentStamina > MAXIMUM_STAMINA_VALUE) {
                CurrentStamina = MAXIMUM_STAMINA_VALUE;
            } else if (CurrentStamina < 0.0f) {
                CurrentStamina = 0.0f;
            }

            // Update Damage Vibration
            if (damageVibrationActive) {
                damageVibrationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (damageVibrationTimer <= 0.0) {
                    damageVibrationActive = false;
                    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                } else {
                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 0.3f);
                }
            } else {
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            }

            // Update Game Pad State Info
            oldGamePadState = gamePadState;
			oldKeyboardState = keyboardState;
			oldMouseState = mouseState;
        }

        public override void Draw(GameTime gameTime, Camera camera) {
            survivorDrawTextureSheet.DrawCellAtIndex(camera, Location, CurrentCellIndex);
            //ShotgunWeapon.shotgunAttackTextureSheet.DrawCellAtIndex(camera, Location, 0);
        }

        public Boolean AttackWithDamage(DamageType damageType, Single damage) {
            CurrentHealth -= damage;
            if (damage > 0.0f) {
                damageVibrationActive = true;
                damageVibrationTimer = DAMAGE_VIBRATION_TIME;
            }
            if (CurrentHealth <= 0.0f) {
                Alive = false;
                return true;
            } else {
                return false;
            }
        }

        public override String ToString() {
			return "{" + this.Location.Position.ToString() + "," 
				+ this.Location.Rotation.ToString() + "," 
				+ this.Location.Scale.ToString() + "," 
				+ this.CurrentHealth.ToString() + "," 
				+ this.Alive.ToString() + "}";
        }

        public void GameOver() {
            CurrentWeapon.Stow();
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

        public AttackResults PerformMeleeAttack(Single damage) {
            return this.Game.ZombiesSubsystem.ZombiesAttackManager.AttackWithMelee(this, damage);
        }

        public void EnableWeapon(Int32 weaponIndex) {
            weaponsEnabled[weaponIndex] = true;
        }

        #endregion

        #region Private Methods

        private static Single CalculateVelocity(Single gamePadRightY, Boolean moveForward, Boolean moveBackwards) {
            Single velocity = 0;
            if (gamePadRightY >= Y_AXIS_DEADBAND) {
                velocity = gamePadRightY * TOP_FORWARD_SPEED;
            } else if (gamePadRightY <= -Y_AXIS_DEADBAND) {
                velocity = gamePadRightY * TOP_BACKWARD_SPEED;
            }
			if (moveForward) {
				velocity = TOP_FORWARD_SPEED;
			} else if (moveBackwards) {
				velocity = -1 * TOP_BACKWARD_SPEED;
			}
            return velocity;
        }

        private static Single CalculateStrafe(Single gamePadRightX, Boolean strafeLeft, Boolean strafeRight) {
            Single strafe = 0;
            if (gamePadRightX >= X_AXIS_DEADBAND || gamePadRightX <= -X_AXIS_DEADBAND) {
                strafe = gamePadRightX * TOP_STRAFE_SPEED;
            }
			if (strafeLeft) {
				strafe = -1 * TOP_STRAFE_SPEED;
			} else if (strafeRight) {
				strafe = TOP_STRAFE_SPEED;
			}
            return strafe;
        }

        private static Single CalculateRotation(Single rotation, Single gamePadLeftX, Single gamePadLeftY, Boolean turnLeft, Boolean turnRight) {
            if (gamePadLeftX >= TURN_AXIS_DEADBAND || gamePadLeftX <= -TURN_AXIS_DEADBAND || gamePadLeftY >= TURN_AXIS_DEADBAND || gamePadLeftY <= -TURN_AXIS_DEADBAND) {
                Single desiredRotation = FloatMathHelper.Atan2(gamePadLeftY, gamePadLeftX);
                if (Math.Abs(rotation - desiredRotation) < TOP_TURN_SPEED) {
                    rotation = desiredRotation;
                } else {
                    if (FloatMathHelper.CounterClockwiseAngularDistance(rotation, desiredRotation) < FloatMathHelper.ClockwiseAngularDistance(rotation, desiredRotation)) {
                        rotation -= TOP_TURN_SPEED;
                    } else {
                        rotation += TOP_TURN_SPEED;
                    }
                }
            } else {
				if (turnLeft) {
					rotation -= TOP_TURN_SPEED * 0.5f;
				} else if (turnRight) {
					rotation += TOP_TURN_SPEED * 0.5f;
				}
			}
            return MathHelper.WrapAngle(rotation);
        }

        private void CalculateWeaponStatus(GameTime gameTime, GameControl gameControl) {
            // Cycle through Weapons
            if (gameControl.WeaponNextCycleControl) {
                Weapon oldWeapon = WEAPON_SPECIFIERS[currentWeaponSpecifierIndex];
                currentWeaponSpecifierIndex++;
                if (currentWeaponSpecifierIndex >= WEAPON_SPECIFIERS.Length) currentWeaponSpecifierIndex = 0;
                while (!weaponsEnabled[currentWeaponSpecifierIndex]) {
                    currentWeaponSpecifierIndex++;
                    if (currentWeaponSpecifierIndex >= WEAPON_SPECIFIERS.Length) currentWeaponSpecifierIndex = 0;
                }
                oldWeapon.Stow();
                CurrentWeapon.Deploy();
            } else if (gameControl.WeaponPreviousCycleControl) {
                Weapon oldWeapon = WEAPON_SPECIFIERS[currentWeaponSpecifierIndex];
                currentWeaponSpecifierIndex--;
                if (currentWeaponSpecifierIndex < 0) currentWeaponSpecifierIndex = WEAPON_SPECIFIERS.Length - 1;
                while (!weaponsEnabled[currentWeaponSpecifierIndex]) {
                    currentWeaponSpecifierIndex--;
                    if (currentWeaponSpecifierIndex < 0) currentWeaponSpecifierIndex = WEAPON_SPECIFIERS.Length - 1;
                }
                oldWeapon.Stow();
                CurrentWeapon.Deploy();
            }

            // Update current weapon
            CurrentWeapon.Update(gameTime, gameControl.WeaponLeftFireControl, gameControl.WeaponRightFireControl, gameControl.WeaponReloadControl, this);
            
        }
        
        #endregion

    }

}

