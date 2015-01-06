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

    /// <summary>
    /// The interface for an entity that can be collided with
    /// </summary>
    public interface ICollidableEntity {

        Rectangle CollisionBounds {
            get;
        }

        Boolean[,] CollisionBooleans {
            get;
        }

        Point[] CollisionPixels {
            get;
        }

        Matrix CollisionTransformMatrix {
            get;
        }

    }

}

