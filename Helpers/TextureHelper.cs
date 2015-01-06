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

    public static class TextureHelper {

        public static Rectangle[] CalculateSourceRectanglesForTextureSheet(Texture2D texture, Point cells) {
            //Color[] colors1D = new Color[texture.Width * texture.Height];
            //texture.GetData(colors1D);
            Single cellWidth = texture.Width / (Single)cells.X;
            Single cellHeight = texture.Height / (Single)cells.Y;
            System.Diagnostics.Debug.WriteLine(cellWidth.ToString() + "," + cellHeight.ToString());
            Rectangle[] sourceRectangles = new Rectangle[cells.X * cells.Y];
            Int32 index = 0;
            for (Int32 column = 0; column < cells.X; column++) {
                for (Int32 row = 0; row < cells.Y; row++) {
                    sourceRectangles[index++] = new Rectangle((Int32)(column * cellWidth), (Int32)(row * cellHeight), (Int32)cellWidth, (Int32)cellHeight);
                }
            }
            return sourceRectangles;
        }
        
        public static Boolean[][,] CalculateCollisionBooleansForTextureSheet(Texture2D texture, Point cells) {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            Single cellWidth = texture.Width / (Single)cells.X;
            Single cellHeight = texture.Height / (Single)cells.Y;
            Boolean[][,] collisionBooleans = new Boolean[cells.X * cells.Y][,];
            Int32 index = 0;
            for (Int32 column = 0; column < cells.X; column++) {
                for (Int32 row = 0; row < cells.Y; row++) {
                    collisionBooleans[index++] = TextureHelper.CalculateCollisionBooleansForTextureSheetCell(colors1D, (Int32)texture.Width, (Int32)cellWidth, (Int32)cellHeight, column, row);
                }
            }
            return collisionBooleans;
        }

        public static Point[][] CalculateCollisionPixelsForTextureSheet(Boolean[][,] collisionBooleans) {
            Point[][] collisionPixels = new Point[collisionBooleans.Length][];
            for (Int32 index = 0; index < collisionBooleans.Length; index++) {
                collisionPixels[index] = TextureHelper.CalculateCollisionPixeslForTextureSheetCell(collisionBooleans[index]);
            }
            return collisionPixels;
        }

        public static Boolean[,] CalculateCollisionBooleansForTextureSheetCell(Color[] colors1D, Int32 textureWidth, Int32 cellWidth, Int32 cellHeight, Int32 cellColumn, Int32 cellRow) {
            Boolean[,] collisionBooleans = new Boolean[cellWidth, cellHeight];
            for (Int32 x = 0; x < cellWidth; x++) {
                for (Int32 y = 0; y < cellHeight; y++) {
                    Int32 tx = x + (cellColumn * cellWidth);
                    Int32 ty = y + (cellRow * cellHeight);
                    if (colors1D[tx + ty * textureWidth].A > 0.0f) {
                        collisionBooleans[x, y] = true;
                    } else {
                        collisionBooleans[x, y] = false;
                    }
                }
            }
            return collisionBooleans;
        }

        public static Point[] CalculateCollisionPixeslForTextureSheetCell(Boolean[,] collisionBooleans) {
            List<Point> collisionPointsList = new List<Point>();
            for (Int32 x = 0; x < collisionBooleans.GetLength(0); x++) {
                for (Int32 y = 0; y < collisionBooleans.GetLength(1); y++) {
                    if (collisionBooleans[x, y]) {
                        collisionPointsList.Add(new Point(x, y));
                    }
                }
            }
            return collisionPointsList.ToArray();
        }

    }

}