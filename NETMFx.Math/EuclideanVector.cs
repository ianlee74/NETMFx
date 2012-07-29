namespace NETMFx.Math
{
    /// <summary>
    /// EuclideanVector class is part of the NETMFx .NET Micro Framework Extensions by Ian Lee.
    /// http://netmfx.codeplex.com
    /// 
    /// A class to represent a Euclidean vector and functions to help work with vectors.
    /// </summary>
    public class EuclideanVector
    {
        /// <summary>
        /// Constructor.  Starting point defaults to (0,0).
        /// </summary>
        /// <param name="endX">X distance of the endpoint.</param>
        /// <param name="endY">Y distance of the endpoint.</param>
        public EuclideanVector(float endX, float endY)
        {
            Start = new Point(0,0);
            End = new Point(endX, endY);
            Direction = new Angle();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="start">Start point.</param>
        /// <param name="end">End point.</param>
        public EuclideanVector(Point start, Point end)
        {
            Start = start;
            End = end;
            Direction = CalculateDirection(Start, End);
            Magnitude = CalculateMagnitude(Start, End);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="start">Start point.</param>
        /// <param name="direction">Angular direction of the vector</param>
        /// <param name="magnitude">Magnitude of the vector.</param>
        public EuclideanVector(Point start, Angle direction, float magnitude)
        {
            Start = start;
            Direction = direction;
            Magnitude = magnitude;
            End = CalculateEndPoint(start, direction, magnitude);
        }

        /// <summary>
        /// Constructor.  Starting point defaults to (0,0).
        /// </summary>
        /// <param name="direction">Angular direction of the vector.</param>
        /// <param name="magnitude">Magnitude of the vector.</param>
        public EuclideanVector(Angle direction, float magnitude)
        {
            Start = new Point(0, 0);
            Direction = direction;
            Magnitude = magnitude;
        }

        /// <summary>
        /// Starting point of the vector.
        /// </summary>
        public Point Start { get; private set; }

        /// <summary>
        /// End point of the vector.
        /// </summary>
        public Point End { get; private set; }

        /// <summary>
        /// Angular direction of the vector.
        /// </summary>
        public Angle Direction { get; private set; }

        /// <summary>
        /// Magnitude of the vector.
        /// </summary>
        public float Magnitude { get; private set; }

        /// <summary>
        /// The quadrant that the vector would exist in if we were to move the starting point to the origin.
        /// </summary>
        public byte RelativeQuadrant
        {
            get
            {
                var direction = Direction.Radians;
                if (direction > 0 && direction < MathEx.Pi / 2F) return 1;
                if (direction > MathEx.Pi / 2F && direction < MathEx.Pi) return 2;
                if (direction > -MathEx.Pi && direction < -MathEx.Pi / 2F) return 3;
                if (direction > -MathEx.Pi / 2F && direction < 0F) return 4;
                return 0;       // Indicates that we're on an axis.
            }
        }

        /// <summary>
        /// Calculates the angular direction given two points.
        /// </summary>
        /// <param name="start">Start point.</param>
        /// <param name="end">End point.</param>
        /// <returns>Angle object.</returns>
        public static Angle CalculateDirection(Point start, Point end)
        {
            return new Angle(MathEx.Atan2(end.Y - start.Y, end.X - start.X));
        }

        /// <summary>
        /// Calculates the magnitude (distance) between two points.
        /// </summary>
        /// <param name="start">Start point.</param>
        /// <param name="end">End point.</param>
        /// <returns></returns>
        public static float CalculateMagnitude(Point start, Point end)
        {
            return MathEx.Sqrt(MathEx.Pow(end.X - start.X, 2) + MathEx.Pow(end.Y - start.Y, 2));
        }

        /// <summary>
        /// Calculates the end point given the start poin, direction, & magnitude.
        /// </summary>
        /// <param name="start">Start point.</param>
        /// <param name="direction">Angular direction.</param>
        /// <param name="magnitude">Vector magnitude.</param>
        /// <returns></returns>
        public static Point CalculateEndPoint(Point start, Angle direction, float magnitude)
        {
            return new Point(start.X + MathEx.Cos(direction.Radians) / magnitude, start.Y + MathEx.Sin(direction.Radians) / magnitude);
        }
    }
}
