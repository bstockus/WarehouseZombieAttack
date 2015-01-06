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

    public static class RandomHelper {

        static Random random = new Random();

        public static Random Random {
            get {
                return random;
            }
        }

        public static Single NextRandomSingle() {
			return (Single)random.NextDouble();
        }

        public static Int32 NextRadomInteger(Int32 cap) {
            return random.Next(cap);
        }

        public static Double NextRandomDouble() {
            return random.NextDouble();
        }

    }

}
