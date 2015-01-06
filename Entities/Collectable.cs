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
    public struct CollectableSaveStruct {
        public Vector2 Position;
        public Single Scale;
        public Single Rotation;
        public Double LifeTime;
        public CollectableEntityType CollectableEntityType;
    }

    public class Collectable : Entity, ILocatableEntity, ICollectableEntity, IDrawableEntity, IUpdateableEntity {

        #region Fields

        static TextureSheet gasolineCollectableTextureSheet;
        static TextureSheet healthCollectableTextureSheet;
        static TextureSheet pistolAmmoCollectableTextureSheet;
        static TextureSheet shotgunAmmoCollectableTextureSheet;
        static TextureSheet energyCollectableTextureSheet;

        CollectableEntityType collectableEntityType;

        #endregion

        #region Properties

        public Location Location {
            get;
            private set;
        }

        public Boolean IsCollected {
            get;
            set;
        }

        public Double LifeTime {
            get;
            set;
        }

        public Rectangle CollectionBounds {
            get {
                return CollisionHelper.CalculateCollisionRectangle(Location.TransformMatrixForOffset(gasolineCollectableTextureSheet.CellOffsets[0]),
                                                                   new Point(gasolineCollectableTextureSheet.CellSourceRectangles[0].Width,
                                                                             gasolineCollectableTextureSheet.CellSourceRectangles[0].Height));
            }
        }

        public Boolean[,] CollectionBooleans {
            get {
                return gasolineCollectableTextureSheet.CellCollisionBooleans[0];
            }
        }

        public Point[] CollectionPixels {
            get {
                return gasolineCollectableTextureSheet.CellCollisionPixels[0];
            }
        }

        public Matrix CollectionTransformMatrix {
            get {
                return Location.TransformMatrixForOffset(gasolineCollectableTextureSheet.CellOffsets[0]);
            }
        }

        public CollectableEntityType CollectionEntityType {
            get {
                return collectableEntityType;
            }
        }

        public CollectableSaveStruct SaveStruct {
            get {
                CollectableSaveStruct saveStruct = new CollectableSaveStruct() {
                    Position = this.Location.Position,
                    Rotation = this.Location.Rotation,
                    Scale = this.Location.Scale,
                    LifeTime = this.LifeTime,
                    CollectableEntityType = this.collectableEntityType
                };
                return saveStruct;
            }
        }

        #endregion

        #region Methods

        public static void LoadContent(ContentManager contentManager) {
            gasolineCollectableTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Collectables/gasolineCollectable"), new Point(1, 1), new Vector2(15.0f, 15.0f));
            healthCollectableTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Collectables/healthCollectable"), new Point(1, 1), new Vector2(15.0f, 15.0f));
            pistolAmmoCollectableTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Collectables/pistolAmmoCollectable"), new Point(1, 1), new Vector2(15.0f, 15.0f));
            shotgunAmmoCollectableTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Collectables/shotgunAmmoCollectable"), new Point(1, 1), new Vector2(15.0f, 15.0f));
            energyCollectableTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Collectables/energyCollectable"), new Point(1, 1), new Vector2(15.0f, 15.0f));
        }

        public Collectable(Game game, CollectableSaveStruct saveStruct)
            : this(game, new Location(saveStruct.Position, saveStruct.Scale, saveStruct.Rotation), saveStruct.LifeTime, saveStruct.CollectableEntityType) {

        }

        public Collectable(Game game, Location location, Double lifeTime, CollectableEntityType collectableEntityType)
            : base(game) {
            this.collectableEntityType = collectableEntityType;
            this.Location = location;
            this.IsCollected = false;
            this.LifeTime = lifeTime;

			Console.WriteLine("Collectable Spawned: " + lifeTime.ToString("F2"));

        }

        public void Collected() {
            System.Diagnostics.Debug.WriteLine("Collected");
            IsCollected = true;
        }

        public void Update(GameTime gameTime) {
            LifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (LifeTime <= 0.0) IsCollected = true;
        }

        public void Draw(GameTime gameTime, Camera camera) {
            switch (collectableEntityType) {
                case CollectableEntityType.Gasoline: gasolineCollectableTextureSheet.DrawCellAtIndex(camera, Location, 0); break;
                case CollectableEntityType.Health: healthCollectableTextureSheet.DrawCellAtIndex(camera, Location, 0); break;
                case CollectableEntityType.PistolAmmo: pistolAmmoCollectableTextureSheet.DrawCellAtIndex(camera, Location, 0); break;
                case CollectableEntityType.ShotgunAmmo: shotgunAmmoCollectableTextureSheet.DrawCellAtIndex(camera, Location, 0); break;
                case CollectableEntityType.Energy: energyCollectableTextureSheet.DrawCellAtIndex(camera, Location, 0); break;
                default: break;
            }
        }

        #endregion

    }

}
