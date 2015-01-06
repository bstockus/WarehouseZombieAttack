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

    public class ZombiesSubsystem : Subsystem {

        #region Constants

        static readonly int[] NUMBER_OF_ZOMBIES = new int[3] { 4, 6, 8 };
        static readonly int[] NUMBER_OF_ZOMBIES_LEVEL_INCREMENT = new int[3] { 1, 2, 3 };
        static readonly int ZOMBIES_PER_LEVEL_INCREMENT = 40;
        static readonly int MAXIMUM_ZOMBIES = 30;
        static readonly double MAXIMUM_TIME_BETWEEN_WAVES = 4.0;

        #endregion

		#region Fields

		double waveWaitInterval;

		#endregion

        #region Properties

        public List<ZombieSprite> ZombieSprites {
            get;
            private set;
        }

        public UpdateManager ZombiesUpdateManger {
            get;
            private set;
        }

        public CollisionManager ZombiesCollisionManager {
            get;
            private set;
        }

        public AttackManager ZombiesAttackManager {
            get;
            private set;
        }

        public double WaveWaitTimer {
            get;
            private set;
        }

		public double WaveTimer {
			get;
			set;
		}

        #endregion

        #region Public Methods

        public ZombiesSubsystem(Game game)
            : base(game) {
                this.ZombieSprites = new List<ZombieSprite>();
                this.ZombiesUpdateManger = new UpdateManager(game);
                this.ZombiesCollisionManager = new CollisionManager(game);
                this.ZombiesAttackManager = new AttackManager(game);
                this.WaveWaitTimer = 0.0;
        }

        public static void LoadContent(ContentManager contentManager) {
            ZombieSprite.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime) {
            ZombiesUpdateManger.UpdateEntities(gameTime);
            RemoveDeadZombies();
            SpawnNewZombies(gameTime);
        }

        public ZombieSpriteSaveStruct[] ZombieSaveStructs() {
            ZombieSpriteSaveStruct[] zombieSaveStructs = new ZombieSpriteSaveStruct[ZombieSprites.Count];
            for (int index = 0; index < ZombieSprites.Count; index++) {
                zombieSaveStructs[index] = ZombieSprites[index].SaveStruct;
            }
            return zombieSaveStructs;
        }

        #endregion

        #region Private Methods

        private void SpawnNewZombies(GameTime gameTime) {
            // Spawn new zombies to replace dead zombies
            int levelIncrement = Game.Results.NumberOfZombieKills / ZOMBIES_PER_LEVEL_INCREMENT;
            int numberOfZombies = NUMBER_OF_ZOMBIES[(int)Game.Options.GameDifficulty] + (levelIncrement * NUMBER_OF_ZOMBIES_LEVEL_INCREMENT[(int)Game.Options.GameDifficulty]);
            if (numberOfZombies > MAXIMUM_ZOMBIES) {
                numberOfZombies = MAXIMUM_ZOMBIES;
            }

			if (ZombieSprites.Count == 0 && WaveTimer > 0.0) {
				Game.HUD.AddMessage("Zombie Wave eliminated in " + WaveTimer.ToString("F1") + " seconds.", Color.Red, false);
				WaveTimer = 0.0;
				waveWaitInterval = RandomHelper.NextRandomDouble() * MAXIMUM_TIME_BETWEEN_WAVES;
				WaveWaitTimer = 0.0;
			}

            if (ZombieSprites.Count == 0) {
                WaveWaitTimer += gameTime.ElapsedGameTime.TotalSeconds;
            }

			if (ZombieSprites.Count == 0 && WaveWaitTimer >= waveWaitInterval) {
                WaveWaitTimer = 0.0;
				WaveTimer = 0.0;
                Game.Results.NumberOfZombieWaves++;
                int waveNumberOfZombies = RandomHelper.NextRadomInteger(numberOfZombies);
                while (waveNumberOfZombies < numberOfZombies / 2) {
                    waveNumberOfZombies = RandomHelper.NextRadomInteger(numberOfZombies);
                }
                Game.HUD.AddMessage("Zombie Wave with " + waveNumberOfZombies.ToString("F0") + " zombies has spawned!", Color.Red, false);
                for (int count = 0; count < waveNumberOfZombies; count++) {
                    this.CreateNewZombieSprite();
                }
            }

			if (ZombieSprites.Count > 0) {
				WaveTimer += gameTime.ElapsedGameTime.TotalSeconds;
			}

        }

        private void RemoveDeadZombies() {
            // Remove dead zombies from managers
            foreach (ZombieSprite zombieSprite in ZombieSprites) {
                if (!zombieSprite.Alive) {
                    ZombiesUpdateManger.UpdatableEntities.Remove(zombieSprite);
                    ZombiesAttackManager.AttackableEntities.Remove(zombieSprite);
                    ZombiesCollisionManager.CollidableEntities.Remove(zombieSprite);
                }
            }

            // Remove dead zombies from list
            for (int index = 0; index < ZombieSprites.Count; index++) {
                if (!ZombieSprites[index].Alive) {
                    ZombieSprites.Remove(ZombieSprites[index]);
                }
            }
        }

        private Location RandomZombieSpawnLocation() {
            int side = RandomHelper.NextRadomInteger(4);
            float rnd = (float)RandomHelper.Random.NextDouble();
            float width = Game.Window.ClientBounds.Width;
            float height = Game.Window.ClientBounds.Height;
            switch (side) {
                case 0: // Top Side
                    return new Location(new Vector2(width * rnd, -50.0f), 1.0f, 0.0f);
                case 1: // Bottom Side
                    return new Location(new Vector2(width * rnd, height + 50.0f), 1.0f, 0.0f);
                case 2: // Left Side
                    return new Location(new Vector2(-50.0f, height * rnd), 1.0f, 0.0f);
                case 3: // Right Side
                    return new Location(new Vector2(width + 50.0f, height * rnd), 1.0f, 0.0f);
                default:
                    return null;
            }
        }

        private ZombieSprite CreateNewZombieSprite() {
            ZombieSprite zombieSprite = new ZombieSprite(this.Game, RandomZombieSpawnLocation(), 0, RandomHelper.NextRandomSingle());
            this.AddZombieSprite(zombieSprite);
            return zombieSprite;
        }

        // TODO: Make private
        public void AddZombieSprite(ZombieSprite zombieSprite) {
            Game.SpritesDrawingManager.DrawableEntities.Add(zombieSprite);
            ZombiesUpdateManger.UpdatableEntities.Add(zombieSprite);
            ZombiesCollisionManager.CollidableEntities.Add(zombieSprite);
            ZombiesAttackManager.AttackableEntities.Add(zombieSprite);
            ZombieSprites.Add(zombieSprite);
        }

        #endregion

    }

}
