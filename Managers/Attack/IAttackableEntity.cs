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

    public enum DamageType {
        Melee,
        Projectile,
        Blast,
        Flame
    }

    public interface IAttackableEntity {

        Rectangle AttackBounds { get; }

        Boolean[,] AttackBooleans { get; }

        Point[] AttackPixels { get; }

        Matrix AttackTransformMatrix { get; }

        Boolean AttackWithDamage(DamageType damageType, Single damage);
        
    }

}

