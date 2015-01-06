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

    public static class FloatMathHelper {

        public static Single Cos(Single angle) {
            return (Single)Math.Cos((Double)angle);
        }

        public static Single Sin(Single angle) {
            return (Single)Math.Sin((Double)angle);
        }

        public static Single Atan2(Single y, Single x) {
            return (Single)Math.Atan2((Double)y, (Double)x);
        }

        /// <summary>
        /// Calculate the angular distance from r to R in a counter-clockwise direction
        /// </summary>
        /// <param name="r">First angle in Radians</param>
        /// <param name="R">First angle in Radians</param>
        /// <returns>Distance in Radians</returns>
        public static Single CounterClockwiseAngularDistance(Single r, Single R) {
            r = MathHelper.WrapAngle(r);
            R = MathHelper.WrapAngle(R);
            Single ar = Math.Abs(r), aR = Math.Abs(R);

            if (r < 0 && R < 0 && R >= r)
                return aR + MathHelper.TwoPi - ar;
            if (r < 0 && R < 0 && R < r)
                return aR - ar;
            if (r >= 0 && R >= 0 && R < r)
                return ar - aR;
            if (r >= 0 && R >= 0 && R >= r)
                return ar + MathHelper.TwoPi - aR;
            if (R >= 0 && r <= 0)
                return MathHelper.TwoPi - ar - aR;
            if (R < 0 && r > 0)
                return aR + ar;
            /*else*/
            return 0.0f;
        }

        /// <summary>
        /// Calculate the angular distance from r to R in a clockwise direction
        /// </summary>
        /// <param name="r">First angle in Radians</param>
        /// <param name="R">First angle in Radians</param>
        /// <returns>Distance in Radians</returns>
        public static Single ClockwiseAngularDistance(Single r, Single R) {
            return MathHelper.TwoPi - FloatMathHelper.CounterClockwiseAngularDistance(r, R);
        }

		public static Single Min(Single a, Single b) {
			if (a < b) {
				return a;
			} else {
				return b;
			}
		}

		public static Single Max(Single a, Single b) {
			if (a > b) {
				return a;
			} else {
				return b;
			}
		}

		public static Double Min(Double a, Double b) {
			if (a < b) {
				return a;
			} else {
				return b;
			}
		}

		public static Double Max(Double a, Double b) {
			if (a > b) {
				return a;
			} else {
				return b;
			}
		}

    }

}
