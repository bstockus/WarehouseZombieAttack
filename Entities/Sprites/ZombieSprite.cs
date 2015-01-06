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

    [Serializable]
    public struct ZombieSpriteSaveStruct {
        public Single HitPoints;
        public Boolean Alive;
        public Vector2 Position;
        public Single Scale;
        public Single Rotation;
        public Single Random;
    }

    public class ZombieSprite : Sprite, ICollidableEntity, IAttackableEntity, IAttackerEntity {

        #region Constants

        static readonly Single[] TOP_TURNING_SPEED = new Single[3] { 0.01f, 0.05f, 0.10f };
        static readonly Single[] BASE_TURNING_SPEED = new Single[3] { 0.009f, 0.04f, 0.08f };
        static readonly Single[] TOP_MOVEMENT_SPEED = new Single[3] { 1.0f, 1.50f, 2.00f };
        static readonly Single[] BASE_MOVEMENT_SPEED = new Single[3] { 0.8f, 1.0f, 1.25f };
        static readonly Single TURNING_MOVEMENT_LIMIT = 0.02f;
        static readonly Int32 DEFENSE_TEXTURE_CELL_INDEX = 0;
        static readonly Single ZOMBIE_ATTACK_VALUE = 0.5f;
        static readonly Double ZOMBIE_MAXIMUM_STATIONARY_TIME = 5.0;
        static readonly Double[] ZOMBIE_ATTACK_DELAY_TIME = new Double[3] { 1.0, 0.75, 0.5 };
        static readonly Int32[] ANIMATION_CELL_INDICIES = new Int32[4] { 0, 2, 4, 6 };
        static readonly Double ANIMATION_DELAY = 0.1;

        #endregion

        #region Instance Fields

        Int32 animationIndex;
        Boolean animating;
        Int32 currentCellIndex;
        Single hitPoints;
        //SoundEffectInstance zombieDeathSoundEffectInstance;
        Double zombieStationaryTime;
        Double zombieAttackTime;
        Double zombieAnimationTime;

        Single random;

        #endregion

        #region Static Fields

        static TextureSheet zombieDrawTextureSheet;
        static TextureSheet zombieDefenseTextureSheet;
        static TextureSheet zombieCollisionTextureSheet;
        static TextureSheet zombieAttackTextureSheet;

        #endregion

        #region Collision Properties

        public Rectangle CollisionBounds {
            get {
                return CollisionHelper.CalculateCollisionRectangle(Location.TransformMatrixForOffset(zombieCollisionTextureSheet.CellOffsets[currentCellIndex]),
                                                                   new Point(zombieCollisionTextureSheet.CellSourceRectangles[currentCellIndex].Width,
                                                                             zombieCollisionTextureSheet.CellSourceRectangles[currentCellIndex].Height));
            }
        }
        
        public Boolean[,] CollisionBooleans {
            get {
                return zombieCollisionTextureSheet.CellCollisionBooleans[currentCellIndex];
            }
        }
        
        public Point[] CollisionPixels {
            get {
                return zombieCollisionTextureSheet.CellCollisionPixels[currentCellIndex];
            }
        }
        
        public Matrix CollisionTransformMatrix {
            get {
                return Location.TransformMatrixForOffset(zombieCollisionTextureSheet.CellOffsets[currentCellIndex]);
            }
        }

        #endregion

        #region Attack Properties

        public Rectangle AttackBounds {
            get {
                return CollisionHelper.CalculateCollisionRectangle(Location.TransformMatrixForOffset(zombieDefenseTextureSheet.CellOffsets[DEFENSE_TEXTURE_CELL_INDEX]),
                                                                   new Point(zombieDefenseTextureSheet.CellSourceRectangles[DEFENSE_TEXTURE_CELL_INDEX].Width,
                                                                             zombieDefenseTextureSheet.CellSourceRectangles[DEFENSE_TEXTURE_CELL_INDEX].Height));
            }
        }

        public Boolean[,] AttackBooleans {
            get {
                return zombieDefenseTextureSheet.CellCollisionBooleans[DEFENSE_TEXTURE_CELL_INDEX];
            }
        }

        public Point[] AttackPixels {
            get {
                return zombieDefenseTextureSheet.CellCollisionPixels[DEFENSE_TEXTURE_CELL_INDEX];
            }
        }

        public Matrix AttackTransformMatrix {
            get {
                return this.CollisionTransformMatrix;
            }
        }

        #endregion

        #region Attacker Properties

        public Rectangle AttackerBounds {
            get {
                return CollisionHelper.CalculateCollisionRectangle(Location.TransformMatrixForOffset(zombieAttackTextureSheet.CellOffsets[currentCellIndex]),
                                                                   new Point(zombieAttackTextureSheet.CellSourceRectangles[currentCellIndex].Width,
                                                                             zombieAttackTextureSheet.CellSourceRectangles[currentCellIndex].Height));
            }
        }

        public Boolean[,] AttackerBooleans {
            get {
                return zombieAttackTextureSheet.CellCollisionBooleans[currentCellIndex];
            }
        }

        public Point[] AttackerPixels {
            get {
                return zombieAttackTextureSheet.CellCollisionPixels[currentCellIndex];
            }
        }

        public Matrix AttackerTransformMatrix {
            get {
                return this.CollisionTransformMatrix;
            }
        }

        #endregion

        #region Other Properties

        public Boolean Alive { get; set; }

        public ZombieSpriteSaveStruct SaveStruct {
            get {
                ZombieSpriteSaveStruct saveStrct = new ZombieSpriteSaveStruct() {
                    HitPoints = this.hitPoints,
                    Alive = this.Alive,
                    Position = this.Location.Position,
                    Scale = this.Location.Scale,
                    Rotation = this.Location.Rotation,
                    Random = this.random
                };
                return saveStrct;
            }
        }

        #endregion

        #region Public Methods

        public static void LoadContent(ContentManager contentManager) {
            Vector2[] offsets = new Vector2[5 * 2];
            for (Int32 index = 0; index < offsets.Length; index++) {
                offsets[index] = new Vector2(10, 21);
            }
            zombieDrawTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/ZombieSprite/zombies"), new Point(5, 2), offsets);
            zombieDefenseTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/ZombieSprite/zombiesDefense"), new Point(5, 2), offsets);
            zombieCollisionTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/ZombieSprite/zombiesCollision"), new Point(5, 2), offsets);
            zombieAttackTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/ZombieSprite/zombiesAttack"), new Point(5, 2), offsets);
        }

        public ZombieSprite(Game game, Location location, Int32 cellIndex, Single hitPoints, Boolean alive, Single random)
            : base(game, location) {
                this.currentCellIndex = cellIndex;
                this.hitPoints = hitPoints;
                this.Alive = alive;
                //this.zombieDeathSoundEffectInstance = zombieDeathSoundEffect.CreateInstance();
                this.zombieStationaryTime = 0.0;
                this.zombieAttackTime = 0.0;
                this.random = random;
        }

        public ZombieSprite(Game game, ZombieSpriteSaveStruct saveStruct)
            : this(game, new Location(saveStruct.Position, saveStruct.Scale, saveStruct.Rotation), 0, saveStruct.HitPoints, saveStruct.Alive, saveStruct.Random) {

        }

        public ZombieSprite(Game game, Location location, Single random)
            : this(game, location, 0, random) {
            
        }

        public ZombieSprite(Game game, Location location, Int32 cellIndex, Single random)
            : this(game, location, 0, 100.0f, true, random) {

        }

        public override void Update(GameTime gameTime) {
            if (this.Alive) {
                Vector2 homingPosition = this.Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Location.Position;
                Vector2 oldPosition = Location.Position;
                Single oldRotation = Location.Rotation;

                Single currentRotation = Location.Rotation;
                Vector2 currentPosition = Location.Position;
                Vector2 deltaPosition = homingPosition - currentPosition;
                Single rotation = CalculateRotation(currentRotation, deltaPosition, TopTurningSpeed());
                Vector2 velocityVector = new Vector2(FloatMathHelper.Cos(rotation), FloatMathHelper.Sin(rotation));
                if (Math.Abs(deltaPosition.Length()) > TopMovementSpeed()) {
                    Single speed = 0.0f;
                    Single cwAngularDistance = FloatMathHelper.ClockwiseAngularDistance(currentRotation, rotation);
                    Single ccwAngularDistance = FloatMathHelper.CounterClockwiseAngularDistance(currentRotation, rotation);
                    Single deltaRotation = MathHelper.Min(cwAngularDistance, ccwAngularDistance);
                    if (deltaRotation <= TURNING_MOVEMENT_LIMIT) {
                        speed = TopMovementSpeed() * ((TURNING_MOVEMENT_LIMIT - deltaRotation) / TURNING_MOVEMENT_LIMIT);
                    }
                    Location.Position += (velocityVector * speed);
                    Location.Rotation = rotation;
                    if (Game.SurvivorSubsystem.SurvivorsCollisionManager.CheckForCollisionsWith(this) || Game.ZombiesSubsystem.ZombiesCollisionManager.CheckForCollisionsWith(this)) {
                        Location.Position = oldPosition;
                        Location.Rotation = oldRotation;
                        zombieStationaryTime += gameTime.ElapsedGameTime.TotalSeconds;
                    } else {
                        zombieStationaryTime = 0.0;
                    }
                }

                if (zombieStationaryTime >= ZOMBIE_MAXIMUM_STATIONARY_TIME) {
                    Alive = false;
                }

                Single zombieAttackValue = 0.0f;

                if (zombieAttackTime >= ZOMBIE_ATTACK_DELAY_TIME[(int)Game.Options.GameDifficulty]) {
                    zombieAttackValue = ZOMBIE_ATTACK_VALUE;
                }

                if (Game.SurvivorSubsystem.SurvivorsAttackManager.AttackWithMelee(this, zombieAttackValue) == AttackResults.Damage) {
                    zombieAttackTime += gameTime.ElapsedGameTime.TotalSeconds;
                } else {
                    zombieAttackTime = 0.0;
                }

                if (animating) {
                    zombieAnimationTime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (zombieAnimationTime >= ANIMATION_DELAY) {
                        animationIndex++;
                        if (animationIndex < ANIMATION_CELL_INDICIES.Length) {
                            currentCellIndex = ANIMATION_CELL_INDICIES[animationIndex];
                            zombieAnimationTime = 0.0;
                        } else {
                            currentCellIndex = 0;
                            animating = false;
                        }
                    }
                }

            }
        }

        public override void Draw(GameTime gameTime, Camera camera) {
            if (this.Alive) {
                zombieDrawTextureSheet.DrawCellAtIndex(camera, Location, currentCellIndex);
                //zombieDefenseTextureSheet.DrawCellAtIndex(camera, Location, currentCellIndex);
            }
        }

        public Boolean AttackWithDamage(DamageType damageType, Single damage) {
            if (this.Alive) {
                hitPoints -= damage;
                animating = true;
                zombieAnimationTime = 0.0;
                animationIndex = 0;
                //System.Diagnostics.Debug.WriteLine("Zombie Hit!");
                if (hitPoints <= 0.0f) {
                    //zombieDeathSoundEffectInstance.Play();
					//zombieDeathSoundEffect.Play();
					Sounds.GetSound("Sprites.Zombie.Death").Play();
                    this.Alive = false;
                    Game.Results.NumberOfZombieKills++;
                    //Game.HUD.AddMessage("Zombie #" + Game.GameResults.NumberOfZombieKills.ToString("F0") + " killed!", Color.White, false);
                    return true;
                } else {
					//zombieDeathSoundEffectInstance.Play();
					//zombieDeathSoundEffect.Play();
					//Sounds.GetSound("Sprites.Zombie.Death").Play();
                    return false;
                }
            } else {
                return false;
            }
        }

        public override string ToString() {
            return "{" + this.Location.Position.ToString() + "," + this.Location.Rotation.ToString() + "," + this.Location.Scale.ToString() + "," + this.hitPoints.ToString() + "," + this.Alive.ToString() + "}";
        }

        #endregion

        #region Private Methods

        private static Single CalculateRotation(Single currentRotation, Vector2 rotationVector, Single topTurningSpeed) {
            rotationVector.Normalize();
            Single desiredRotation = FloatMathHelper.Atan2(rotationVector.Y, rotationVector.X);
            if (Math.Abs(currentRotation - desiredRotation) < topTurningSpeed) {
                currentRotation = desiredRotation;
            } else {
                if (FloatMathHelper.CounterClockwiseAngularDistance(currentRotation, desiredRotation) < FloatMathHelper.ClockwiseAngularDistance(currentRotation, desiredRotation)) {
                    currentRotation -= topTurningSpeed;
                } else {
                    currentRotation += topTurningSpeed;
                }
            }
            return MathHelper.WrapAngle(currentRotation);
        }

        private Single TopTurningSpeed() {
            return MathHelper.Max(TOP_TURNING_SPEED[(Int32)Game.Options.GameDifficulty] * random, BASE_TURNING_SPEED[(Int32)Game.Options.GameDifficulty]);
        }

        private Single TopMovementSpeed() {
            return MathHelper.Max(TOP_MOVEMENT_SPEED[(Int32)Game.Options.GameDifficulty] * random, BASE_MOVEMENT_SPEED[(Int32)Game.Options.GameDifficulty]);
        }

        #endregion

    }

}

