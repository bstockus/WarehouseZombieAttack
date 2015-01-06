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

    public class TextureSheet {

        #region Fields

        Texture2D texture;
        int cellCount;
        Rectangle[] cellSourceRectangles;
        Boolean[][,] cellCollisionBooleans;
        Point[][] cellCollisionPixels;
        Vector2[] cellOffsets;

        #endregion

        #region Properties

        public Texture2D Texture {
            get {
                return texture;
            }
        }
        
        public int CellCount {
            get {
                return cellCount;
            }
        }
        
        public Rectangle[] CellSourceRectangles {
            get {
                return cellSourceRectangles;
            }
        }
        
        public Boolean[][,] CellCollisionBooleans {
            get {
                return cellCollisionBooleans;
            }
        }

        public Point[][] CellCollisionPixels {
            get {
                return cellCollisionPixels;
            }
        }
        
        public Vector2[] CellOffsets {
            get {
                return cellOffsets;
            }
        }

        #endregion

        #region Methods

        public TextureSheet(Texture2D texture, Point cells, Vector2 cellOffset) {
            Vector2[] offsets = new Vector2[cells.X * cells.Y];
            for (int index = 0; index < cells.X * cells.Y; index++) {
                offsets[index] = cellOffset;
            }
            this.Constructor(texture, cells, offsets);
        }

        public TextureSheet(Texture2D texture, Point cells, Vector2[] cellOffsets) {
            this.Constructor(texture, cells, cellOffsets);
        }

        private void Constructor(Texture2D texture, Point cells, Vector2[] cellOffsets) {
            if (cells.X <= 0 || cells.Y <= 0 || cellOffsets.Length != (cells.X * cells.Y)) {
                throw new ArgumentOutOfRangeException();
            } else {
                this.texture = texture;
                this.cellCount = (cells.X * cells.Y);
                this.cellOffsets = cellOffsets;
                this.cellSourceRectangles = TextureHelper.CalculateSourceRectanglesForTextureSheet(texture, cells);
                this.cellCollisionBooleans = TextureHelper.CalculateCollisionBooleansForTextureSheet(texture, cells);
                this.cellCollisionPixels = TextureHelper.CalculateCollisionPixelsForTextureSheet(this.cellCollisionBooleans);
            }
        }

        public void DrawCellAtIndex(Camera camera, Vector2 position, int cellIndex) {
            DrawCellAtIndex(camera, new Location(position, 1.0f, 0.0f), cellIndex);
        }

        public void DrawCellAtIndex(Camera camera, Location location, int cellIndex) {
            DrawCellAtIndex(camera, location, cellIndex, SpriteEffects.None);
        }

        public void DrawCellAtIndex(Camera camera, Location location, int cellIndex, SpriteEffects spriteEffects) {
            camera.SpriteBatch.Draw(this.Texture, location.Position, this.CellSourceRectangles[cellIndex], Color.White, location.Rotation, this.CellOffsets[cellIndex], location.Scale, spriteEffects, 1.0f);
        }

        #endregion

    }

}

