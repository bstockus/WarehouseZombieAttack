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
    public enum CollectableEntityType {
        Health,
        Gasoline,
        PistolAmmo,
        ShotgunAmmo,
        Energy
    }

    /// <summary>
    /// The interface for an entity that is collectable
    /// </summary>
    public interface ICollectableEntity {

        Rectangle CollectionBounds {
            get;
        }

        Boolean[,] CollectionBooleans {
            get;
        }

        Point[] CollectionPixels {
            get;
        }

        Matrix CollectionTransformMatrix {
            get;
        }

        CollectableEntityType CollectionEntityType {
            get;
        }

        void Collected();

    }

}

