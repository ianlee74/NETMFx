namespace NETMFx.Math
{
    /// <summary>
    /// Angle class is part of the NETMFx .NET Micro Framework Extensions by Ian Lee.
    /// http://netmfx.codeplex.com
    /// 
    /// A class that represents a geometric angle.
    /// </summary>
    public class Angle
    {
        public Angle() {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="radians">The angle value in radians.</param>
        public Angle(float radians)
        {
            Radians = radians;
        }

        /// <summary>
        /// The angle value in radians.
        /// </summary>
        public float Radians;

        /// <summary>
        /// The angle value in Degrees.  This value is mathematically computed based on the Radians
        /// angle when called.  So, the Radians value should be used when speed is important.
        /// </summary>
        public float Degrees
        {
            get { return ConvertRadiansToDegrees(Radians); }
            set { Radians = ConvertDegreesToRadians(value); }
        }

        /// <summary>
        /// Converts a radians angle value to degrees.
        /// </summary>
        /// <param name="radians">Angle value in radians.</param>
        /// <returns>Angle value in degrees.</returns>
        public static float ConvertRadiansToDegrees(float radians)
        {
            return radians*180F/MathEx.Pi;
        }

        /// <summary>
        /// Converts degrees angle value to radians.
        /// </summary>
        /// <param name="degrees">Angle value in degrees.</param>
        /// <returns>Angle value in radians.</returns>
        public static float ConvertDegreesToRadians(float degrees)
        {
            return degrees*MathEx.Pi/180F;
        }
    }
}
