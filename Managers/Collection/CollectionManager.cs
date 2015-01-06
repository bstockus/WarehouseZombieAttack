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

    public class CollectionManager : Manager {

        List<ICollectableEntity> collectableEntities;

        public List<ICollectableEntity> CollectableEntities {
            get {
                return collectableEntities;
            }
        }

        public CollectionManager(Game game) : base(game) {
            this.collectableEntities = new List<ICollectableEntity>();
        }

        public List<CollectableEntityType> Collect(ICollidableEntity collidableEntity) {
            List<CollectableEntityType> collectedEntityTypes = new List<CollectableEntityType>();
            Rectangle collidableBounds = collidableEntity.CollisionBounds;
            Point[] collidablePixels = collidableEntity.CollisionPixels;
            Matrix collidableTransformMatrix = collidableEntity.CollisionTransformMatrix;
            Point[] collidableTransformedPixels = CollisionHelper.ConvertTexturePixelsToScreenPixels(collidablePixels, collidableTransformMatrix);
            foreach (ICollectableEntity collectableEntity in collectableEntities) {
                if (collectableEntity.CollectionBounds.Intersects(collidableBounds)) {
                    Boolean[,] collectableBooleans = collectableEntity.CollectionBooleans;
                    foreach (Point collectionPixel in collidableTransformedPixels) {
                        Vector2 collectablePixelVector = CollisionHelper.ConvertScreenPixelToTexturePixel(
                            new Vector2((float)collectionPixel.X, (float)collectionPixel.Y),
                            collectableEntity.CollectionTransformMatrix);
                        Point collectablePoint = new Point((int)collectablePixelVector.X, (int)collectablePixelVector.Y);
                        if (collectablePoint.X >= 0 && collectablePoint.Y >= 0
                            && collectablePoint.X < collectableBooleans.GetLength(0)
                            && collectablePoint.Y < collectableBooleans.GetLength(1)) {
                                collectedEntityTypes.Add(collectableEntity.CollectionEntityType);
                                collectableEntity.Collected();
                                break;
                        }
                    }
                }
            }
            return collectedEntityTypes;
        }

    }

}
