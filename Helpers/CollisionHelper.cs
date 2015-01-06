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
    
    public static class CollisionHelper {

        public static Boolean CheckForCollisions(SpriteSpecifier activeSpriteSpecifier, SpriteSpecifier[] passiveSpriteSpecifiers) {
            foreach (SpriteSpecifier passiveSpriteSpecifier in passiveSpriteSpecifiers) {
                if (passiveSpriteSpecifier.DestinationRectangle.Intersects(activeSpriteSpecifier.DestinationRectangle)) {
                    if (TexturesCollide(activeSpriteSpecifier.Colors, activeSpriteSpecifier.Matrix, passiveSpriteSpecifier.Colors, passiveSpriteSpecifier.Matrix)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Boolean TexturesCollide(Color[,] texture1, Matrix matrix1, Color[,] texture2, Matrix matrix2) {
            Matrix mat1to2 = matrix1 * Matrix.Invert(matrix2);
            Int32 width1 = texture1.GetLength(0);
            Int32 height1 = texture1.GetLength(1);
            Int32 width2 = texture2.GetLength(0);
            Int32 height2 = texture2.GetLength(1);

            for (Int32 x1 = 0; x1 < width1; x1++) {
                for (Int32 y1 = 0; y1 < height1; y1++) {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    Int32 x2 = (Int32)pos2.X;
                    Int32 y2 = (Int32)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2)) {
                        if ((y2 >= 0) && (y2 < height2)) {
                            if (texture1[x1, y1].A > 0) {
                                if (texture2[x2, y2].A > 0) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static Vector2 ConvertScreenPixelToTexturePixel(Vector2 screenPixel, Matrix transformMatrix) {
            Matrix inverseTransformMatrix = Matrix.Invert(transformMatrix);
            return Vector2.Transform(screenPixel, inverseTransformMatrix);
        }

        public static Point ConvertScreenPixelToTexturePixel(Point screenPixel, Matrix transformMatrix) {
            Matrix inverseTransformMatrix = Matrix.Invert(transformMatrix);
            Vector2 transformedVector = Vector2.Transform(new Vector2((Single)screenPixel.X, (Single)screenPixel.Y), inverseTransformMatrix);
            return new Point((Int32)transformedVector.X, (Int32)transformedVector.Y);
        }

        public static Point ConvertTexturePixelToScreenPixel(Point texturePixel, Matrix transformMatrix) {
            Vector2 transformedVector = Vector2.Transform(new Vector2((Single)texturePixel.X, (Single)texturePixel.Y), transformMatrix);
            return new Point((Int32)transformedVector.X, (Int32)transformedVector.Y);
        }

        public static Point[] ConvertTexturePixelsToScreenPixels(Point[] texturePixels, Matrix transformMatrix) {
            Point[] screenPixels = new Point[texturePixels.Length];
            for (Int32 index = 0; index < screenPixels.Length; index++) {
                Vector2 transformedLocation = Vector2.Transform(new Vector2((Single)texturePixels[index].X, (Single)texturePixels[index].Y), transformMatrix);
                screenPixels[index] = new Point((Int32)transformedLocation.X, (Int32)transformedLocation.Y);
            }
            return screenPixels;
        }

        public static Rectangle CalculateCollisionRectangle(Matrix transformMatrix, Point size) {
            Vector2 upperLeftPoint = Vector2.Transform(new Vector2(0.0f, 0.0f), transformMatrix);
            Vector2 upperRightPoint = Vector2.Transform(new Vector2((Single)size.X, 0.0f), transformMatrix);
            Vector2 lowerLeftPoint = Vector2.Transform(new Vector2(0.0f, (Single)size.Y), transformMatrix);
            Vector2 lowerRightPoint = Vector2.Transform(new Vector2((Single)size.X, (Single)size.Y), transformMatrix);
            
            Single maximumX = MathHelper.Max(MathHelper.Max(upperLeftPoint.X, upperRightPoint.X), MathHelper.Max(lowerLeftPoint.X, lowerRightPoint.X));
            Single minimumX = MathHelper.Min(MathHelper.Min(upperLeftPoint.X, upperRightPoint.X), MathHelper.Min(lowerLeftPoint.X, lowerRightPoint.X));
            Single maximumY = MathHelper.Max(MathHelper.Max(upperLeftPoint.Y, upperRightPoint.Y), MathHelper.Max(lowerLeftPoint.Y, lowerRightPoint.Y));
            Single minimumY = MathHelper.Min(MathHelper.Min(upperLeftPoint.Y, upperRightPoint.Y), MathHelper.Min(lowerLeftPoint.Y, lowerRightPoint.Y));

            Int32 x = (Int32)(minimumX);
            Int32 y = (Int32)(minimumY);
            Int32 width = (Int32)(maximumX - minimumX);
            Int32 height = (Int32)(maximumY - minimumY);
            
            return new Rectangle(x, y, width, height);
        }

    }

}
