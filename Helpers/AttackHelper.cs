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

    public static class AttackHelper {

        public static IAttackableEntity[] AttackableEntitiesWithinAttackersBounds(Rectangle attackersBounds, IAttackableEntity[] attackableEntites) {
            List<IAttackableEntity> attackableEntitieWithinBounds = new List<IAttackableEntity>();
            foreach (IAttackableEntity attackableEntity in attackableEntites) {
                if (attackersBounds.Intersects(attackableEntity.AttackBounds)) {
                    attackableEntitieWithinBounds.Add(attackableEntity);
                }
            }
            return attackableEntitieWithinBounds.ToArray();
        }

        public static Single[,] CalculateAttackValuesForTexture(Texture2D texture) {

            Int32 textureWidth = texture.Width, textureHeight = texture.Height;
            Color[] textureColors = new Color[textureHeight * textureWidth];
            texture.GetData(textureColors);
            Single[,] attackValues = new Single[textureWidth, textureHeight];

            for (Int32 x = 0; x < textureWidth; x++) {
                for (Int32 y = 0; y < textureHeight; y++) {
                    attackValues[x, y] = (Single)(textureColors[(x * textureWidth) + y].R / 256);
                }
            }

            return attackValues;

        }

        public static Tuple<Point, Single>[] CalculateAttackPointValueTuplesForTexture(Texture2D texture) {
            Int32 textureWidth = texture.Width, textureHeight = texture.Height;
            Color[] textureColors = new Color[textureHeight * textureWidth];
            texture.GetData(textureColors);

            List<Tuple<Point, Single>> attackPointValueTuples = new List<Tuple<Point, Single>>();

            for (Int32 x = 0; x < textureWidth; x++) {
                for (Int32 y = 0; y < textureHeight; y++) {
                    Color color = textureColors[(x * textureWidth) + y];
                    if (color.A != 0) {
                        attackPointValueTuples.Add(new Tuple<Point, Single>(new Point(x, y), (Single)(color.R) / 256.0f));
                    }
                }
            }

            return attackPointValueTuples.ToArray();

        }

    }

}
