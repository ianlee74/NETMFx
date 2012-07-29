namespace NETMFx.Math
{
    /// <summary>
    /// LinearRange class is part of the NETMFx .NET Micro Framework Extensions by Ian Lee.
    /// http://netmfx.codeplex.com
    /// 
    /// Class that represents a point in Cartesian 3D space.
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Point() {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">X location.</param>
        /// <param name="y">Y location.</param>
        /// <param name="z">Z location.  Defaults to zero.</param>
        public Point(float x, float y, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// X value.
        /// </summary>
        public float X;

        /// <summary>
        /// Y value.
        /// </summary>
        public float Y;

        /// <summary>
        /// Z value.
        /// </summary>
        public float Z;

        /// <summary>
        /// The Cartesian quadrant where the point exists.
        /// </summary>
        public byte Quadrant
        {
            get
            {
                if (MathEx.Abs(X - 0) < 1E-10 || MathEx.Abs(Y - 0) < 1E-10) return 0;
                if (X > 0)
                {
                    return (byte) (Y > 0 ? 1 : 4);
                }
                return (byte) (Y > 0 ? 2 : 3);
            }
        }

        /// <summary>
        /// Rotates the X & Y about the Z axis a given angle.
        /// </summary>
        /// <param name="radians">The angle of rotation in radians.</param>
        public void Rotate2D(float radians)
        {
            var newX = X*MathEx.Cos(radians) - Y*MathEx.Sin(radians);
            var newY = X*MathEx.Sin(radians) + Y*MathEx.Cos(radians);
            X = newX;
            Y = newY;
        }
    }
}
