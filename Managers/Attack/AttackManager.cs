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

    public enum AttackResults {
        None,
        Damage,
        Kill
    }

	public struct MultiAttackResults {
		public int Damages;
		public int Kills;
	}

    public class AttackManager : Manager {

        List<IAttackableEntity> attackableEntities;

        public List<IAttackableEntity> AttackableEntities {
            get {
                return attackableEntities;
            }
        }

        public AttackManager(Game game) : base(game) {
            this.attackableEntities = new List<IAttackableEntity>();
        }

        public AttackResults AttackWithMelee(IAttackerEntity attackerEntity, float damage) {
            Rectangle attackerBounds = attackerEntity.AttackerBounds;
            Point[] attackerPixels = attackerEntity.AttackerPixels;
            Matrix attackerTransformMatrix = attackerEntity.AttackerTransformMatrix;
            Point[] attackerTransformedPixels = CollisionHelper.ConvertTexturePixelsToScreenPixels(attackerPixels, attackerTransformMatrix);
            foreach (IAttackableEntity attackableEntity in attackableEntities) {
                if (attackableEntity.AttackBounds.Intersects(attackerBounds)) {
                    Boolean[,] attackableBooleans = attackableEntity.AttackBooleans;
                    foreach (Point attackerPixel in attackerTransformedPixels) {
                        Vector2 attackablePixelVector = CollisionHelper.ConvertScreenPixelToTexturePixel(
                            new Vector2((float)attackerPixel.X, (float)attackerPixel.Y),
                            attackableEntity.AttackTransformMatrix);
                        Point attackablePoint = new Point((int)attackablePixelVector.X, (int)attackablePixelVector.Y);
                        if (attackablePoint.X >= 0 && attackablePoint.Y >= 0
                            && attackablePoint.X < attackableBooleans.GetLength(0)
                            && attackablePoint.Y < attackableBooleans.GetLength(1)) {
                                if (attackableBooleans[attackablePoint.X, attackablePoint.Y]) {
                                    if (attackableEntity.AttackWithDamage(DamageType.Melee, damage)) {
                                        return AttackResults.Kill;
                                    } else {
                                        return AttackResults.Damage;
                                    }
                                }
                        }
                    }
                }
            }

            return AttackResults.None;

        }

        public MultiAttackResults AttackWithProjectile(IAttackerEntity attackerEntity, float damage) {
            IAttackableEntity[] attackableEntitiesWithinAttackersBounds = AttackHelper.AttackableEntitiesWithinAttackersBounds(attackerEntity.AttackerBounds, this.AttackableEntities.ToArray());
            float[] damageValuesArray = new float[attackableEntitiesWithinAttackersBounds.Length];
			MultiAttackResults multiAttackResults = new MultiAttackResults();
            foreach (Point attackersPixel in attackerEntity.AttackerPixels) {
                Point attackersPixelInScreenCoordinates = CollisionHelper.ConvertTexturePixelToScreenPixel(attackersPixel, attackerEntity.AttackerTransformMatrix);
                for (int index = 0; index < attackableEntitiesWithinAttackersBounds.Length; index++) {
                    IAttackableEntity attackableEntity = attackableEntitiesWithinAttackersBounds[index];
                    Point attackersPixelInAttackableEntitysTextureCoordinates = CollisionHelper.ConvertScreenPixelToTexturePixel(attackersPixelInScreenCoordinates,
                        attackableEntity.AttackTransformMatrix);
                    if (attackersPixelInAttackableEntitysTextureCoordinates.X >= 0 && attackersPixelInAttackableEntitysTextureCoordinates.Y >= 0) {
                        if (attackersPixelInAttackableEntitysTextureCoordinates.X < attackableEntity.AttackBooleans.GetLength(0) &&
                            attackersPixelInAttackableEntitysTextureCoordinates.Y < attackableEntity.AttackBooleans.GetLength(1)) {
                                if (attackableEntity.AttackBooleans[attackersPixelInAttackableEntitysTextureCoordinates.X,attackersPixelInAttackableEntitysTextureCoordinates.Y]) {
                                    damageValuesArray[index] += damage;
                                }
                        }
                    }
                }
            }
            for (int index = 0; index < attackableEntitiesWithinAttackersBounds.Length; index++) {
                if (damageValuesArray[index] > 0.0f) {
					if (attackableEntitiesWithinAttackersBounds[index].AttackWithDamage(DamageType.Projectile, damageValuesArray[index])) {
						multiAttackResults.Kills++;
					} else {
						multiAttackResults.Damages++;
					}
                }
            }
			return multiAttackResults;
        }

		public AttackResults AttackWithProjectileSingleTarget(IAttackerEntity attackerEntity, float damage) {
			IAttackableEntity[] attackableEntitiesWithinAttackersBounds = AttackHelper.AttackableEntitiesWithinAttackersBounds(attackerEntity.AttackerBounds, this.AttackableEntities.ToArray());
			foreach (Point attackersPixel in attackerEntity.AttackerPixels) {
				Point attackersPixelInScreenCoordinates = CollisionHelper.ConvertTexturePixelToScreenPixel(attackersPixel, attackerEntity.AttackerTransformMatrix);
				for (int index = 0; index < attackableEntitiesWithinAttackersBounds.Length; index++) {
					IAttackableEntity attackableEntity = attackableEntitiesWithinAttackersBounds[index];
					Point attackersPixelInAttackableEntitysTextureCoordinates = CollisionHelper.ConvertScreenPixelToTexturePixel(attackersPixelInScreenCoordinates, attackableEntity.AttackTransformMatrix);
					if (attackersPixelInAttackableEntitysTextureCoordinates.X >= 0 && attackersPixelInAttackableEntitysTextureCoordinates.Y >= 0) {
						if (attackersPixelInAttackableEntitysTextureCoordinates.X < attackableEntity.AttackBooleans.GetLength(0) &&
						    attackersPixelInAttackableEntitysTextureCoordinates.Y < attackableEntity.AttackBooleans.GetLength(1)) {
							if (attackableEntity.AttackBooleans[attackersPixelInAttackableEntitysTextureCoordinates.X,attackersPixelInAttackableEntitysTextureCoordinates.Y]) {
								if (attackableEntity.AttackWithDamage(DamageType.Projectile, damage)) {
									return AttackResults.Kill;
								} else {
									return AttackResults.Damage;
								}
							}
						}
					}
				}
			}
			return AttackResults.None;
		}

        public void AttackWithProjectileVariableDamage(Rectangle attackerBounds, Matrix attackerTransformMatrix, Tuple<Point, float>[] attackerPointValueTuples, float damage) {
            IAttackableEntity[] attackableEntitiesWithinAttackersBounds = AttackHelper.AttackableEntitiesWithinAttackersBounds(attackerBounds,
                this.AttackableEntities.ToArray());
            foreach (Tuple<Point, float> attackerPointValueTuple in attackerPointValueTuples) {
                Point attackerPointOnScreen = CollisionHelper.ConvertTexturePixelToScreenPixel(attackerPointValueTuple.Item1, attackerTransformMatrix);
                float attackerValue = damage * attackerPointValueTuple.Item2;
                foreach (IAttackableEntity attackableEntity in attackableEntitiesWithinAttackersBounds) {
                    Point attackersPixelInAttackableEntitysTextureCoordinates = CollisionHelper.ConvertScreenPixelToTexturePixel(attackerPointOnScreen,
                        attackableEntity.AttackTransformMatrix);
                    if (attackersPixelInAttackableEntitysTextureCoordinates.X >= 0 && attackersPixelInAttackableEntitysTextureCoordinates.Y >= 0) {
                        if (attackersPixelInAttackableEntitysTextureCoordinates.X < attackableEntity.AttackBooleans.GetLength(0) &&
                            attackersPixelInAttackableEntitysTextureCoordinates.Y < attackableEntity.AttackBooleans.GetLength(1)) {
                            if (attackableEntity.AttackBooleans[attackersPixelInAttackableEntitysTextureCoordinates.X, attackersPixelInAttackableEntitysTextureCoordinates.Y]) {
                                attackableEntity.AttackWithDamage(DamageType.Projectile, attackerValue);
                            }
                        }
                    }
                }
            }

        }

    }

}

