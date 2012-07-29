namespace NETMFx.Math
{
    /// <summary>
    /// LinearRange class is part of the NETMFx .NET Micro Framework Extensions by Ian Lee.
    /// http://netmfx.codeplex.com
    /// 
    /// Represents the minimum & maximum values of a linear range.
    /// </summary>
    public class LinearRange
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LinearRange() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="min">Minimum value of the range.</param>
        /// <param name="max">Maximum value fo the range.</param>
        public LinearRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        private int _min;
        /// <summary>
        /// Minimum value of the range.
        /// </summary>
        public int Min
        {
            get { return _min; }
            set
            {
                _min = value;
                SetMidpoint();
            }
        }

        private int _max;
        /// <summary>
        /// Maximum value of the range.
        /// </summary>
        public int Max
        {
            get { return _max; }
            set
            {
                _max = value;
                SetMidpoint();
            }
        }

        /// <summary>
        /// Midpoint of the range.
        /// </summary>
        public int Midpoint { get; private set; }

        /// <summary>
        /// Calculates the midpoint value.
        /// </summary>
        private void SetMidpoint()
        {
            Midpoint = _min + (_max - _min)/2;
        }
    }
}