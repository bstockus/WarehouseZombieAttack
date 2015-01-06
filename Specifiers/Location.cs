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

    /// <summary>
    /// The specification for the location of an object
    /// </summary>
    public class Location {

        #region Fields

        Vector2 position;
        Single scale;
        Single rotation;
        Matrix transformMatrix;
        Boolean transformMatrixDirty;

        #endregion

        #region Properties

        public Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
                transformMatrixDirty = true;
            }
        }

        public Single Scale {
            get {
                return scale;
            }
            set {
                scale = value;
                transformMatrixDirty = true;
            }
        }

        public Single Rotation {
            get {
                return rotation;
            }
            set {
                rotation = value;
                transformMatrixDirty = true;
            }
        }

        public Matrix TransformMatrix {
            get {
                if (transformMatrixDirty) {
                    transformMatrix = Location.CalculateTransformMatrix(position, scale, rotation);
                    transformMatrixDirty = false;
                }
                return transformMatrix;
            }
        }

        #endregion

        #region Methods

        public Location(Vector2 position, Single scale, Single rotation) {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.transformMatrix = new Matrix();
            this.transformMatrixDirty = true;
        }

        public Location() : this(Vector2.Zero, 0.0f, 0.0f) {

        }

        public Matrix TransformMatrixForOffset(Vector2 offset) {
            return Matrix.CreateTranslation(-offset.X, -offset.Y, 0.0f) * this.TransformMatrix;
        }

        public void TranslateBy(Single dx, Single dy) {
            this.Position += new Vector2(dx, dy);
        }

        public void TranslateBy(Vector2 dv) {
            this.Position += dv;
        }

        public void RotateBy(Single dr) {
            this.Rotation += dr;
        }

        public void ScaleBy(Single ds) {
            this.Scale += ds;
        }

        private static Matrix CalculateTransformMatrix(Vector2 position, Single scale, Single rotation) {
            Matrix matrix = Matrix.CreateRotationZ(rotation);
            matrix = matrix * Matrix.CreateScale(scale);
            matrix = matrix * Matrix.CreateTranslation(position.X, position.Y, 0.0f);
            return matrix;
        }

        #endregion

    }

}

