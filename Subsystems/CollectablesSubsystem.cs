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

    public class CollectablesSubsystem : Subsystem {

        #region Constants

        static readonly Double HEALTH_COLLECTABLE_SPAWN_PROBABILITY = 0.005;
        static readonly Double GASOLINE_COLLECTABLE_SPAWN_PROBABILITY = 0.001;
        static readonly Double PISTOL_AMMO_COLLECTABLE_SPAWN_PROBABILITY = 0.001;
        static readonly Double SHOTGUN_AMMO_COLLECTABLE_SPAWN_PROBABILITY = 0.001;
        static readonly Double ENERGY_COLLECTABLE_SPAWN_PROBABILITY = 0.001;

        static readonly int MAXIMUM_HEALTH_COLLECTABLES_SPAWNABLE = 2;
        static readonly int MAXIMUM_GASOLINE_COLLECTABLES_SPWANABLE = 1;
        static readonly int MAXIMUM_PISTOL_AMMO_COLLECTABLES_SPWANABLE = 1;
        static readonly int MAXIMUM_SHOTGUN_AMMO_COLLECTABLES_SPWANABLE = 1;
        static readonly int MAXIMUM_ENERGY_COLLECTABLES_SPWANABLE = 1;

		static readonly double MAXIMUM_HEALTH_COLLECTABLE_LIFETIME = 20.0;
		static readonly double MAXIMUM_GASOLINE_COLLECTABLE_LIFETIME = 20.0;
		static readonly double MAXIMUM_PISTOL_AMMO_COLLECTABLE_LIFETIME = 20.0;
		static readonly double MAXIMUM_SHOTGUN_AMMO_COLLECTABLE_LIFETIME = 20.0;
		static readonly double MAXIMUM_ENERGY_COLLECTABLE_LIFETIME = 20.0;

		static readonly double MINIMUM_HEALTH_COLLECTABLE_LIFETIME = 5.0;
		static readonly double MINIMUM_GASOLINE_COLLECTABLE_LIFETIME = 5.0;
		static readonly double MINIMUM_PISTOL_AMMO_COLLECTABLE_LIFETIME = 5.0;
		static readonly double MINIMUM_SHOTGUN_AMMO_COLLECTABLE_LIFETIME = 5.0;
		static readonly double MINIMUM_ENERGY_COLLECTABLE_LIFETIME = 5.0;

        #endregion

        #region Fields

        #endregion

        #region Properties
        
        public List<Collectable> Collectables {
            get;
            private set;
        }

        public UpdateManager CollectablesUpdateManger {
            get;
            private set;
        }

        public CollectionManager CollectablesCollectionManger {
            get;
            private set;
        }

        public int HealthCollectablesSpawned {
            get;
            set;
        }

        public int GasolineCollectablesSpawned {
            get;
            set;
        }

        public int PistolAmmoCollectablesSpawned {
            get;
            set;
        }

        public int ShotgunAmmoCollectablesSpawned {
            get;
            set;
        }

        public int EnergyCollectablesSpawned {
            get;
            set;
        }

        #endregion

        #region Public Methods

        public CollectablesSubsystem(Game game)
            : base(game) {
                this.Collectables = new List<Collectable>();
                this.CollectablesUpdateManger = new UpdateManager(game);
                this.CollectablesCollectionManger = new CollectionManager(game);
                this.HealthCollectablesSpawned = 0;
                this.GasolineCollectablesSpawned = 0;
                this.PistolAmmoCollectablesSpawned = 0;
                this.ShotgunAmmoCollectablesSpawned = 0;
                this.EnergyCollectablesSpawned = 0;
        }

        public static void LoadContent(ContentManager contentManager) {
            Collectable.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime) {
            CollectablesUpdateManger.UpdateEntities(gameTime);
            RemoveCollectedCollectables();
            SpawnNewCollectables();
        }

        public void AddCollectable(Collectable genericCollectable) {
            CollectablesCollectionManger.CollectableEntities.Add(genericCollectable);
            Game.CollectablesDrawingManager.DrawableEntities.Add(genericCollectable);
            CollectablesUpdateManger.UpdatableEntities.Add(genericCollectable);
            Collectables.Add(genericCollectable);
        }

        public CollectableSaveStruct[] CollectableSaveStructs() {
            List<CollectableSaveStruct> collectableSaveStructs = new List<CollectableSaveStruct>();
            foreach (Collectable collectable in Collectables) {
                collectableSaveStructs.Add(collectable.SaveStruct);
            }
            return collectableSaveStructs.ToArray();
        }

        #endregion

        #region Private Methods

        private void SpawnNewCollectables() {
            // Add New Random Collectables
            if (RandomHelper.NextRandomDouble() < HEALTH_COLLECTABLE_SPAWN_PROBABILITY 
                && HealthCollectablesSpawned < MAXIMUM_HEALTH_COLLECTABLES_SPAWNABLE 
                && Game.SurvivorSubsystem.PlayerOneSurvivorSprite.CurrentHealth < 80.0f) {
                    CreateNewHealthCollectable();
                    HealthCollectablesSpawned++;
            }
            if (RandomHelper.NextRandomDouble() < GASOLINE_COLLECTABLE_SPAWN_PROBABILITY 
                && GasolineCollectablesSpawned < MAXIMUM_GASOLINE_COLLECTABLES_SPWANABLE
                && Game.AchievmentsSubsystem.ChainsawWeaponUnlockedAchievment) {
                    CreateNewGasolineCollectable();
                    GasolineCollectablesSpawned++;
            }
            if (RandomHelper.NextRandomDouble() < PISTOL_AMMO_COLLECTABLE_SPAWN_PROBABILITY
                && PistolAmmoCollectablesSpawned < MAXIMUM_PISTOL_AMMO_COLLECTABLES_SPWANABLE
                && Game.AchievmentsSubsystem.PistolWeaponUnlockedAchievment) {
                    CreateNewPistolAmmoCollectable();
                    PistolAmmoCollectablesSpawned++;
            }
            if (RandomHelper.NextRandomDouble() < SHOTGUN_AMMO_COLLECTABLE_SPAWN_PROBABILITY
                && ShotgunAmmoCollectablesSpawned < MAXIMUM_SHOTGUN_AMMO_COLLECTABLES_SPWANABLE
                && Game.AchievmentsSubsystem.ShotgunWeaponUnlockedAchievment) {
                    CreateNewShotgunAmmoCollectable();
                    ShotgunAmmoCollectablesSpawned++;
            }
            if (RandomHelper.NextRandomDouble() < ENERGY_COLLECTABLE_SPAWN_PROBABILITY
                && EnergyCollectablesSpawned < MAXIMUM_ENERGY_COLLECTABLES_SPWANABLE) {
                    CreateNewEnergyCollectable();
                    EnergyCollectablesSpawned++;
            }
        }

        private void RemoveCollectedCollectables() {
            // Remove Collected Collectables
            for (Int32 index = 0; index < Collectables.Count; index++) {
                if (Collectables[index].IsCollected) {
                    CollectablesCollectionManger.CollectableEntities.Remove(Collectables[index]);
                    Game.CollectablesDrawingManager.DrawableEntities.Remove(Collectables[index]);
                    CollectablesUpdateManger.UpdatableEntities.Remove(Collectables[index]);
                    switch (Collectables[index].CollectionEntityType) {
                        case CollectableEntityType.Health: HealthCollectablesSpawned--; break;
                        case CollectableEntityType.Gasoline: GasolineCollectablesSpawned--; break;
                        case CollectableEntityType.PistolAmmo: PistolAmmoCollectablesSpawned--; break;
                        case CollectableEntityType.ShotgunAmmo: ShotgunAmmoCollectablesSpawned--; break;
                        case CollectableEntityType.Energy: EnergyCollectablesSpawned--; break;
                        default: break;
                    }
                    Collectables.Remove(Collectables[index]);
                }
            }


        }

        public Collectable CreateNewHealthCollectable() {
            Collectable healthCollectable = new Collectable(Game, 
			                                                RandomCollectableSpawnLocation(), 
			                                                FloatMathHelper.Max(MINIMUM_HEALTH_COLLECTABLE_LIFETIME, RandomHelper.NextRandomDouble() * MAXIMUM_HEALTH_COLLECTABLE_LIFETIME), 
			                                                CollectableEntityType.Health);
            this.AddCollectable(healthCollectable);
            return healthCollectable;
        }

        public Collectable CreateNewGasolineCollectable() {
            Collectable gasolineCollectable = new Collectable(Game, 
			                                                  RandomCollectableSpawnLocation(), 
			                                                  FloatMathHelper.Max(MINIMUM_GASOLINE_COLLECTABLE_LIFETIME, RandomHelper.NextRandomDouble() * MAXIMUM_GASOLINE_COLLECTABLE_LIFETIME), 
			                                                  CollectableEntityType.Gasoline);
            this.AddCollectable(gasolineCollectable);
            return gasolineCollectable;
        }

        public Collectable CreateNewPistolAmmoCollectable() {
            Collectable pistolAmmoCollectable = new Collectable(Game, 
			                                                    RandomCollectableSpawnLocation(), 
			                                                    FloatMathHelper.Max(MINIMUM_PISTOL_AMMO_COLLECTABLE_LIFETIME, RandomHelper.NextRandomDouble() * MAXIMUM_PISTOL_AMMO_COLLECTABLE_LIFETIME), 
			                                                    CollectableEntityType.PistolAmmo);
            this.AddCollectable(pistolAmmoCollectable);
            return pistolAmmoCollectable;
        }

        public Collectable CreateNewShotgunAmmoCollectable() {
            Collectable shotgunAmmoCollectable = new Collectable(Game, 
			                                                     RandomCollectableSpawnLocation(), 
			                                                     FloatMathHelper.Max(MINIMUM_SHOTGUN_AMMO_COLLECTABLE_LIFETIME, RandomHelper.NextRandomDouble() * MAXIMUM_SHOTGUN_AMMO_COLLECTABLE_LIFETIME), 
			                                                     CollectableEntityType.ShotgunAmmo);
            this.AddCollectable(shotgunAmmoCollectable);
            return shotgunAmmoCollectable;
        }

        public Collectable CreateNewEnergyCollectable() {
            Collectable energyCollectable = new Collectable(Game, 
			                                                RandomCollectableSpawnLocation(), 
			                                                FloatMathHelper.Max(MINIMUM_ENERGY_COLLECTABLE_LIFETIME, RandomHelper.NextRandomDouble() * MAXIMUM_ENERGY_COLLECTABLE_LIFETIME), 
			                                                CollectableEntityType.Energy);
            this.AddCollectable(energyCollectable);
            return energyCollectable;
        }

        private Location RandomCollectableSpawnLocation() {
            float rndX = RandomHelper.NextRandomSingle();
            float rndY = RandomHelper.NextRandomSingle();
            float width = Game.Window.ClientBounds.Width - 30.0f;
            float height = Game.Window.ClientBounds.Height - 30.0f;
            return new Location(new Vector2((rndX * width) + 15.0f, (rndY * height) + 15.0f), 1.0f, 0.0f);
        }

        #endregion

    }

}
