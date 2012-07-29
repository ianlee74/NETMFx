using System;
using Gadgeteer;
using Gadgeteer.Interfaces;
using GT = Gadgeteer;

namespace NETMFx.GroveHeartRateSensor
{
    public delegate void HeartBeatEventHandler(object sender, DateTime time);

    public class GroveHeartRateSensor
    {
        private static InterruptInput _pin;
        private static CircularDateQueue _beatQueue;

        /// <summary>
        /// Event that is raised every time a heart beat is detected.
        /// </summary>
        public event HeartBeatEventHandler HeartBeat;

        protected virtual void OnHeartBeat(DateTime time)
        {
            if (HeartBeat != null) HeartBeat(this, time);
        }

        public GroveHeartRateSensor(GT.Socket socket, int sampleSize)
        {
            _beatQueue = new CircularDateQueue(sampleSize);
            _pin = new InterruptInput(socket, Socket.Pin.Three, GlitchFilterMode.Off, ResistorMode.Disabled, InterruptMode.RisingEdge, null);
            _pin.Interrupt += OnInterrupt;
        }

        private void OnInterrupt(InterruptInput sender, bool value)
        {
            var time = DateTime.Now;
            _beatQueue.Add(time);
            OnHeartBeat(time);
        }

        /// <summary>
        /// The calculated heart beats per minute (BPM).
        /// </summary>
        public int BeatsPerMinute
        {
            get
            {
                float secs = _beatQueue.TimeSpan.Ticks/10000000;
                return (byte)(_beatQueue.Count / secs * 60.0);
            }
        }
    }
}
