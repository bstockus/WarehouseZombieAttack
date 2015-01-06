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

    public class SpriteSpecifier {

        #region Fields

        // Base Fields
        Vector2 _origin = new Vector2(0.0f, 0.0f);
        Vector2 _position = new Vector2(0.0f, 0.0f);
        Single _scale = 1.0f;
        Single _rotation = 0.0f;
        Texture2D _texture;
        Rectangle[] _sourceRectangle;
        int _columns;
        int _rows;
        int _cellWidth;
        int _cellHeight;
        int _cellIndex;

        // Dynamic, Cached and Calculated Fields        
        Color[][,] _colors;
        Rectangle _destinationRectangle;
        Boolean _destinationRectangleDirtyFlag;
        Matrix _matrix;
        Boolean _matrixDirtyFlag;

        #endregion Fields

        #region Properties

        public Vector2 Origin {
            get {
                return _origin;
            }
            set {
                _origin = value;
                _destinationRectangleDirtyFlag = true;
                _matrixDirtyFlag = true;
            }
        }

        public Vector2 Position {
            get {
                return _position;
            }
            set {
                _position = value;
                _destinationRectangleDirtyFlag = true;
                _matrixDirtyFlag = true;
            }
        }

        public Single Scale {
            get {
                return _scale;
            }
            set {
                _scale = value;
                _destinationRectangleDirtyFlag = true;
                _matrixDirtyFlag = true;
            }
        }

        public Single Rotation {
            get {
                return _rotation;
            }
            set {
                _rotation = value;
                _destinationRectangleDirtyFlag = true;
                _matrixDirtyFlag = true;
            }
        }

        public Texture2D Texture {
            get {
                return _texture;
            }            
        }

        public Rectangle SourceRectangle {
            get {
                return _sourceRectangle[_cellIndex];
            }
        }

        public Color[,] Colors {
            get {
                return _colors[_cellIndex];
            }
        }

        public int Columns {
            get {
                return _columns;
            }
        }

        public int Rows {
            get {
                return _rows;
            }
        }

        public int CellWidth {
            get {
                return _cellWidth;
            }
        }

        public int CellHeight {
            get {
                return _cellHeight;
            }
        }

        public int CellIndex {
            get {
                return _cellIndex;
            }
            set {
                if (value < (_rows * _columns) && value >= 0)
                    _cellIndex = value;                
            }
        }

        public Rectangle DestinationRectangle {
            get {
                RecalculateDestinationRectangle();
                return _destinationRectangle;
            }
        }

        public Matrix Matrix {
            get {
                RecalculateMatrix();
                return _matrix;
            }
        }

        #endregion Properties

        #region Methods

        public SpriteSpecifier(Texture2D texture, int columns, int rows) {
            _texture = texture;
            _columns = columns;
            _rows = rows;
            _cellWidth = (int)texture.Width / columns;
            _cellHeight = (int)texture.Height / rows;
            _cellIndex = 0;
            CalculateTextureColorsAndSourceRectangles();
            _matrixDirtyFlag = true;
            _destinationRectangleDirtyFlag = true;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, _position, _sourceRectangle[_cellIndex], Color.White, _rotation, _origin, _scale, SpriteEffects.None, 1.0f);
        }              

        private void CalculateTextureColorsAndSourceRectangles() {
            Color[] colors1D = new Color[_texture.Width * _texture.Height];
            _texture.GetData(colors1D);
            _colors = new Color[_columns * _rows][,];
            _sourceRectangle = new Rectangle[_columns * _rows];
            for (int column = 0; column < _columns; column++) {
                for (int row = 0; row < _rows; row++) {
                    _sourceRectangle[column + row * _rows] = new Rectangle(column * _cellWidth, row * _cellHeight, _cellWidth, _cellHeight);
                    _colors[column + row * _rows] = CalculateCellColors(colors1D, row, column);
                }
            }
        }

        private Color[,] CalculateCellColors(Color[] colors1D, int row, int column) {
            Color[,] colors2D = new Color[_cellWidth, _cellHeight];
            for (int x = 0; x < _cellWidth; x++) {
                for (int y = 0; y < _cellHeight; y++) {
                    int tx = x + (column * _cellWidth);
                    int ty = y + (row * _cellHeight);
                    colors2D[x, y] = colors1D[tx + ty * _texture.Width];
                }
            }
            return colors2D;
        }

        private void RecalculateDestinationRectangle() {
            if (_destinationRectangleDirtyFlag) {
                _destinationRectangleDirtyFlag = false;
                RecalculateMatrix();
                Vector2 upperLeftPoint = Vector2.Transform(new Vector2(0.0f, 0.0f), _matrix);
                Vector2 upperRightPoint = Vector2.Transform(new Vector2(_sourceRectangle[_cellIndex].Width, 0.0f), _matrix);
                Vector2 lowerLeftPoint = Vector2.Transform(new Vector2(0.0f, _sourceRectangle[_cellIndex].Height), _matrix);
                Vector2 lowerRightPoint = Vector2.Transform(new Vector2(_sourceRectangle[_cellIndex].Width, _sourceRectangle[_cellIndex].Height), _matrix);

                Single maximumX = MathHelper.Max(MathHelper.Max(upperLeftPoint.X, upperRightPoint.X), MathHelper.Max(lowerLeftPoint.X, lowerRightPoint.X));
                Single minimumX = MathHelper.Min(MathHelper.Min(upperLeftPoint.X, upperRightPoint.X), MathHelper.Min(lowerLeftPoint.X, lowerRightPoint.X));
                Single maximumY = MathHelper.Max(MathHelper.Max(upperLeftPoint.Y, upperRightPoint.Y), MathHelper.Max(lowerLeftPoint.Y, lowerRightPoint.Y));
                Single minimumY = MathHelper.Min(MathHelper.Min(upperLeftPoint.Y, upperRightPoint.Y), MathHelper.Min(lowerLeftPoint.Y, lowerRightPoint.Y));

                int x = (int)(minimumX);
                int y = (int)(minimumY);
                int width = (int)(maximumX - minimumX);
                int height = (int)(maximumY - minimumY);

                _destinationRectangle = new Rectangle(x, y, width, height);
            }
            
        }

        private void RecalculateMatrix() {
            if (_matrixDirtyFlag) {
                _matrixDirtyFlag = false;
                _matrix = Matrix.CreateTranslation(-_origin.X, -_origin.Y, 0.0f);
                _matrix *= Matrix.CreateRotationZ(_rotation);
                _matrix *= Matrix.CreateScale(_scale);
                _matrix *= Matrix.CreateTranslation(_position.X, _position.Y, 0.0f);
            }
        }

        #endregion Methods

    }

}
