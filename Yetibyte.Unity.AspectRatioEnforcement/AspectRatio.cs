using System;
using UnityEngine;

namespace Yetibyte.Unity.AspectRatioEnforcement {

    [Serializable]
    public struct AspectRatio : IEquatable<AspectRatio> {

        #region Static Props

        public static AspectRatio Ratio16To9 => new AspectRatio(16, 9);

        public static AspectRatio Ratio4To3 => new AspectRatio(4, 3);

        public static AspectRatio Ratio16To10 => new AspectRatio(16, 10);

        public static AspectRatio Ratio5To4 => new AspectRatio(5, 4);

        #endregion

        #region Fields

        [SerializeField]
        private int _horizontal;

        [SerializeField]
        private int _vertical;

        #endregion

        #region Props

        public int Horizontal => _horizontal;
        public int Vertical => _vertical;

        public float Value => (float)Horizontal / Vertical;

        #endregion

        #region Ctors
        public AspectRatio(int horizontal, int vertical) {
            _horizontal = horizontal;
            _vertical = vertical;
        }


        #endregion

        #region Static Methods

        public static AspectRatio Calculate(int width, int height) {

            int x = width;
            int y = height;

            while(y != 0) {

                int remainder = x % y;
                x = y;
                y = remainder;

            }

            x = Math.Abs(x);

            return new AspectRatio(width / x, height / x);

        }

        #endregion

        #region Methods

        public override string ToString() => $"{Horizontal}:{Vertical}";

        public bool Equals(AspectRatio other) => other.Horizontal == Horizontal && other.Vertical == Vertical;

        public override bool Equals(object obj) => obj is AspectRatio a && a.Equals(this);

        public override int GetHashCode() {

            int hashCode = 1238135884;
            hashCode = hashCode * -1521134295 + Horizontal.GetHashCode();
            hashCode = hashCode * -1521134295 + Vertical.GetHashCode();
            return hashCode;

        }


        #endregion

        #region Operators

        public static bool operator ==(AspectRatio a, AspectRatio b) => a.Equals(b);

        public static bool operator !=(AspectRatio a, AspectRatio b) => !a.Equals(b);

        public static explicit operator AspectRatio(Vector2 vector) => new AspectRatio((int)vector.x, (int)vector.y);

        public static implicit operator float(AspectRatio aspectRatio) => aspectRatio.Value;

        #endregion


    }


}
