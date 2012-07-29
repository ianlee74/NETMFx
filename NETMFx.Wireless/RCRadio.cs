using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using NETMFx.Collections;

namespace NETMFx.Wireless
{
    public delegate void RadioDataReceivedEventHandler(object sender, RadioDataReceivedEventArgs e);
    public class RadioDataReceivedEventArgs : EventArgs
    {
        public string[] Data;

        public RadioDataReceivedEventArgs() {}

        public RadioDataReceivedEventArgs(string[] data)
        {
            Data = data;
        }
    }

    public class RCRadio : IDisposable
    {
// ReSharper disable InconsistentNaming
        private const char COMMAND_PREFIX = '[';
        private const char COMMAND_SUFFIX = ']';
        private const char COMMAND_DELIMITER = '|';
// ReSharper restore InconsistentNaming

        public SerialPort Port;
        protected Thread SenderThread;
        public event RadioDataReceivedEventHandler DataReceived;

        public RCRadio(string id, string port = "COM1", int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            Port = new SerialPort(port, baudRate, parity, dataBits, stopBits);
            AutoPing = true;
            PingPeriod = 3000;
            DataQueue = new StringFifoQueue();
        }

        public void Activate(bool autoPing = false)
        {
            Port.Open();
            Port.DataReceived += OnDataReceived;
            if (autoPing) DoAutoPing();

            SenderThread = new Thread(SendData);
            SenderThread.Start();
        }

        public string Id;
        public string PartnerId;

        public bool AutoPing;
        public int PingPeriod;

        protected void DoAutoPing()
        {
            var pingThread = new Thread(() => {
                                                   while (AutoPing)
                                                   {
                                                       Send("PNG|" + DateTime.Now);
                                                       Thread.Sleep(PingPeriod);
                                                   }
                                               });
            pingThread.Start();
        }

        public void Send(string message)
        {
            DataQueue.Add(message);
        }

        public delegate void DataReceiver(string[] data);

        private static string _lastCommand;
        public DataReceiver DataProcessor;
        private static string _commandBuffer;
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(20);
            var received = _commandBuffer;
            // Read all data and buffer to a string.
            var bytesReceived = ((SerialPort) sender).BytesToRead;
            var bytes = new byte[bytesReceived];
            ((SerialPort) sender).Read(bytes, 0, bytesReceived);
            received += new string(Encoding.UTF8.GetChars(bytes));
            // Make sure there is a command prefix.
            var prefixNdx = received.IndexOf(COMMAND_PREFIX);
            if (prefixNdx < 0) return;      // No valid command prefix exists.
            // If there is not also a suffix then carry the received data over to the next packet.
            var suffixNdx = received.IndexOf(COMMAND_SUFFIX);
            if (suffixNdx < 0)
            {
                _commandBuffer = received;
                return;
            }
            var command = "";
            for(var ndx = prefixNdx; ndx < received.Length; ndx++)
            {
                var c = received[ndx];
                if (c == COMMAND_SUFFIX)
                {
                    if(command != "" && command != _lastCommand) ProcessCommand(command);
                    command = "";
                }
                else if (c != COMMAND_PREFIX) command += c;
            }
            _commandBuffer = command;
        }

        private void ProcessCommand(string command)
        {
            _lastCommand = command;
#if DEBUG
            Debug.Print(command);
#endif
            var data = command.Split(COMMAND_DELIMITER);
            // If data came from the partner radio then raise event, otherwise discard.
            if (data == null || data.Length <= 1 || data[0] != PartnerId) return;
            // Remove partner Id & terminator from data.
            var d2 = new string[data.Length - 1];
            for (byte ndx = 1; ndx < data.Length; ndx++)
            {
                d2[ndx - 1] = data[ndx];
            }
            if (DataReceived != null) DataReceived(this, new RadioDataReceivedEventArgs(d2));
        }

        protected StringFifoQueue DataQueue;
        public int SendFrequency = 100;
        protected void SendData()
        {
            while (true)
            {
                var data = DataQueue.GetNext();
                if (data != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(COMMAND_PREFIX + Id + COMMAND_DELIMITER + data + COMMAND_SUFFIX);
                    Port.Write(bytes, 0, bytes.Length);
                    Port.Flush();       // TODO:  Really necessary?
                }
                Thread.Sleep(SendFrequency);
            }
        }

        public int QueueSize
        {
            get { return DataQueue.Count; }
        }

        public void Dispose()
        {
            SenderThread.Abort();
            Port.Dispose();
        }
    }
}
