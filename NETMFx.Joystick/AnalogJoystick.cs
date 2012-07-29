using System;
using Microsoft.SPOT.Hardware;
using NETMFx.Math;

#if GHI
using GHIElectronics.NETMF.Hardware;
#endif
#if NETDUINO
using SecretLabs.NETMF.Hardware;
#endif
namespace NETMFx.Joystick
{
    /// <summary>
    /// AnalogJoystick class is part of the NETMFx .NET Micro Framework Extensions by Ian Lee.
    /// http://netmfx.codeplex.com
    /// 
    /// This class is based from the netduino Helpers project (http://netduinohelpers.codeplex.com/) by Fabien Royer.
    /// 
    /// This class interfaces with an analog x, y joystick such as a the ones found in game controllers (PS/2, Xbox 360, etc.)
    /// It is compatible and tested with both GHI and netduino microprocessor boards.
    /// For GHI, use the conditional compile symbol "GHI"
    /// For netduino, use the conditional compile symbol "NETDUINO"
    /// 
    /// Tested with a thumb joystick by Adafruit: https://www.adafruit.com/products/245
    /// Datasheet: http://www.parallax.com/Portals/0/Downloads/docs/prod/sens/27800-2-AxisJoyStick-v1.0.pdf
    /// </summary>
    public class AnalogJoystick : IDisposable
    {
#if GHI
        protected AnalogIn XInput;
        protected AnalogIn YInput;
#endif
#if NETDUINO
        protected AnalogInput XInput;
        protected AnalogInput YInput;
#endif
        /// <summary>
        /// Calibration adders for the X axis values.
        /// </summary>
        public JoystickCalibration XCalibration = new JoystickCalibration();

        /// <summary>
        /// Calibrations adders for the Y axis values.
        /// </summary>
        public JoystickCalibration YCalibration = new JoystickCalibration();

        /// <summary>
        /// Angle of translation used to adjust the vector direction due to the mounting direction of the joystick.
        /// </summary>
        public Angle AngularCalibration;

        public AnalogJoystick()
        {
        }

        private int _rawX;
        /// <summary>
        /// Raw data for the X axis.
        /// </summary>
        public int RawX
        {
            get
            {
                _rawX = XInput.Read();
                return _rawX;
            }
        }

        private int _rawY;
        /// <summary>
        /// Raw data for the Y axis.
        /// </summary>
        public int RawY
        {
            get 
            {
                _rawY = YInput.Read();
                return _rawY;
            }
        }
        
        /// <summary>
        /// User-defined value range for the X and Y axis
        /// </summary>
        public LinearRange Range = new LinearRange();

        /// <summary>
        /// Vector data representation of the direction & magnitude of the current calibrated joystick direction.
        /// </summary>
        public EuclideanVector Vector
        {
            get
            {
                // Apply linear calibration.
                var endPoint = new Point(ApplyCalibration(RawX, XCalibration), ApplyCalibration(RawY, YCalibration));
                // Apply angular calibration.
                if (AngularCalibration != null && MathEx.Abs(AngularCalibration.Radians - 0) > 1E-10)
                {
                    endPoint.Rotate2D(AngularCalibration.Radians);
                }
                return new EuclideanVector(new Point(Range.Midpoint, Range.Midpoint), endPoint);
            }
        }

        /// <summary>
        /// Adds the given calibration values to a given raw piece of data.
        /// </summary>
        /// <param name="raw">Raw joystick data for one axis.</param>
        /// <param name="calibration">Calibration data to be applied to the raw data.</param>
        /// <returns>Calibration adjusted data to be used in place of the raw data.</returns>
        protected int ApplyCalibration(int raw, JoystickCalibration calibration)
        {
            var originHigh = calibration.Origin.High;
            var originLow = calibration.Origin.Low;

            // See if resting at the origin.
            if (raw >= originLow && raw <= originHigh) return 0;
            // See if in the positive direction.
            if (raw > originHigh) return raw + calibration.EndPoints.High;
            // Otherwise, we must be in the negative direction.
            return raw + calibration.EndPoints.Low;
        }

        /// <summary>
        /// Automatically determines the range of values defining the center for the X and Y axis.
        /// Assumes that the joystick is at the center position on the X & Y axis.
        /// Do not touch the joystick during auto-calibration :)
        /// </summary>
        /// <param name="centerDeadZoneRadius">A user-defined radius used to eliminate spurious readings around the center</param>
        public void AutoCalibrateCenter(int centerDeadZoneRadius)
        {
            var xMin = RawX;
            var xMax = xMin;
            var yMin = RawY;
            var yMax = yMin;

            for (var I = 0; I < 100; I++)
            {
                var tempX = RawX;
                var tempY = RawY;

                if (tempX < xMin)
                {
                    xMin = tempX;
                }

                if (tempX > xMax)
                {
                    xMax = tempX;
                }

                if (tempY < yMin)
                {
                    yMin = tempY;
                }

                if (tempY > yMax)
                {
                    yMax = tempY;
                }
            }
            XCalibration.Origin.Low = xMin - centerDeadZoneRadius;
            XCalibration.Origin.High = xMax + centerDeadZoneRadius;
            YCalibration.Origin.Low = yMin - centerDeadZoneRadius;
            YCalibration.Origin.High = yMax + centerDeadZoneRadius;
        }

        /// <summary>
        /// Expects two analog pins connected to the x & y axis of the joystick.
        /// </summary>
        /// <param name="xAxisPin">Analog pin for the x axis</param>
        /// <param name="yAxisPin">Analog pin for the y axis</param>
        /// <param name="minRange">The lowest value that can ever be returned from an axis.</param>
        /// <param name="maxRange">The highest value that can ever be returned from an axis.</param>
        /// <param name="centerDeadZoneRadius">The radius of the "dead zone" around the center.  Used to define when the stick is at rest.
        /// any values within this radius will be given a calibrated value equal to the midpoint of the range.
        /// </param>
        public AnalogJoystick(Cpu.Pin xAxisPin, Cpu.Pin yAxisPin, int minRange = 0, int maxRange = 1023, int centerDeadZoneRadius = 10)
        {
            Range.Min = minRange;
            Range.Max = maxRange;

#if GHI
            XInput = new AnalogIn((AnalogIn.Pin)xAxisPin);
            XInput.SetLinearScale(minRange, maxRange);
            YInput = new AnalogIn((AnalogIn.Pin)yAxisPin);
            YInput.SetLinearScale(minRange, maxRange);
#endif
#if NETDUINO
            XInput = new AnalogInput(xAxisPin);
            XInput.SetRange(minRange, maxRange);
            YInput = new AnalogInput(yAxisPin);
            YInput.SetRange(minRange, maxRange);
#endif
            AutoCalibrateCenter(centerDeadZoneRadius);
        }

        /// <summary>
        /// Frees the resources allocated for reading values from the analog joystick
        /// </summary>
        public void Dispose()
        {
            XInput.Dispose();
            XInput = null;

            YInput.Dispose();
            YInput = null;
        }
    }
}
