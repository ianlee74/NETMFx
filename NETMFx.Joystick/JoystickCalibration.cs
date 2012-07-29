namespace NETMFx.Joystick
{
    /// <summary>
    /// JoystickCalibration class is part of the NETMFx .NET Micro Framework Extensions by Ian Lee.
    /// http://netmfx.codeplex.com
    ///
    /// This class is used to store calibration data for one axis of a joystick.  It is used to 
    /// software calibrate joystick data.
    /// </summary>
    public class JoystickCalibration
    {
        public struct HighLowRange
        {
            public int Low;
            public int High;            
        }

        /// <summary>
        /// The high & low end calibration offset values.
        /// </summary>
        public HighLowRange EndPoints;

        /// <summary>
        /// The high & low origin values.  Used to help define when the joystick is at rest.
        /// </summary>
        public HighLowRange Origin;
    }
}
