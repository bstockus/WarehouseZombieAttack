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

    public class CollisionManager : Manager {

        List<ICollidableEntity> collidableEntities;

        public List<ICollidableEntity> CollidableEntities {
            get {
                return collidableEntities;
            }
        }
        
        public CollisionManager(Game game) : base(game) {
            collidableEntities = new List<ICollidableEntity>();
        }

        public Boolean CheckForCollisionsWith(ICollidableEntity collidableEntity) {
            Rectangle collidableEntityCollisionBounds = collidableEntity.CollisionBounds;
            Point[] collidableEntityCollisionPixels = collidableEntity.CollisionPixels;
            Matrix collidableEntityCollisionTransformMatrix = collidableEntity.CollisionTransformMatrix;
            Point[] collisionPixels = CollisionHelper.ConvertTexturePixelsToScreenPixels(collidableEntityCollisionPixels, collidableEntityCollisionTransformMatrix);
            foreach (ICollidableEntity _collidableEntity in collidableEntities) {
                if (_collidableEntity.CollisionBounds.Intersects(collidableEntityCollisionBounds) && collidableEntity != _collidableEntity) {
                    Boolean[,] collisionBooleans = _collidableEntity.CollisionBooleans;
                    foreach (Point collisionPixel in collisionPixels) {
                        Vector2 _collisionPixelVector = CollisionHelper.ConvertScreenPixelToTexturePixel(new Vector2((float)collisionPixel.X, (float)collisionPixel.Y), _collidableEntity.CollisionTransformMatrix);
                        Point _collisionPixel = new Point((int)_collisionPixelVector.X, (int)_collisionPixelVector.Y);
                        if (_collisionPixel.X >= 0 && _collisionPixel.Y >= 0 && _collisionPixel.X < collisionBooleans.GetLength(0) && _collisionPixel.Y < collisionBooleans.GetLength(1)) {
                            if (collisionBooleans[_collisionPixel.X, _collisionPixel.Y]) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

    }

}

