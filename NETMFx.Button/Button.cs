using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace NETMFx.Button
{
    public delegate void ButtonPressedEventHandler(object sender, EventArgs e);

    public class Button
    {
        private readonly InterruptPort _button;

        /// <summary>
        /// Debounce (glitch) filter time period in Ticks.
        /// </summary>
        public int DebouncePeriod = 200;

        public Button(InterruptPort buttonPort)
        {
            _button = buttonPort;
            _button.OnInterrupt += OnButtonPressed;
        }

        private DateTime _lastButtonPress = DateTime.Now;
        private void OnButtonPressed(uint data1, uint data2, DateTime time)
        {
            if ((time - _lastButtonPress).Milliseconds <= DebouncePeriod) return;
            _lastButtonPress = DateTime.Now;
            OnButtonPressed(EventArgs.Empty);
        }

        public event ButtonPressedEventHandler ButtonPressed;
        protected virtual void OnButtonPressed(EventArgs e)
        {
            if (ButtonPressed != null)
                ButtonPressed(this, e);
        }

    }

}
