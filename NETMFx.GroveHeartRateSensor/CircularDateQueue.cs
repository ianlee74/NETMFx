using System;

namespace NETMFx.GroveHeartRateSensor
{
    internal class CircularDateQueue
    {
        private DateTime[] _queue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">The max number of items that the queue will contain.</param>
        public CircularDateQueue(int size)
        {
            Size = size;
        }

        /// <summary>
        /// The max number of items that the queue will contain.
        /// </summary>
        public int Size
        {
            get { return _size; }
            set 
            {
                _size = value;
                Clear();
            }
        }
        private int _size;

        /// <summary>
        /// Clears the queue.
        /// </summary>
        public void Clear()
        {
            _queue = new DateTime[Size];
            Top = 0;
            Bottom = 0;
            Count = 0;
        }

        /// <summary>
        /// Index of the top (newest) item.
        /// </summary>
        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }
        private int _top;

        /// <summary>
        /// Index of the bottom (oldest) item.
        /// </summary>
        public int Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }
        private int _bottom;

        /// <summary>
        /// Count of items currently in the queue.
        /// </summary>
        public int Count
        {
            get { return _count; }
            private set { _count = value; }
        }
        private int _count;

        /// <summary>
        /// Index of the queue position where the next item will go.
        /// </summary>
        private int Next(int index)
        {
            if (Count == 0) return 0;
            if (index < Size-1) return index + 1;
            return 0;
        }

        /// <summary>
        /// Add an item to the queue.
        /// </summary>
        /// <param name="time">The DateTime item to add.</param>
        public void Add(DateTime item)
        {
            // Add the item to the top of the queue (overwriting the oldest item if the queue is full).
            var nextNdx = Next(Top);
            _queue[nextNdx] = item;
            Top = nextNdx;

            // Reset the bottom pointer.
            Bottom = Count < Size-1 ? 0 : Next(Bottom);

            if(Count < Size-1) Count++;
        }

        /// <summary>
        /// Returns the value of the specified item.
        /// </summary>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>DateTime object.</returns>
        public DateTime Peek(int index)
        {
            return _queue[index];
        }

        /// <summary>
        /// The total timespan as measured as the difference between the top item and the bottom items of the queue.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get 
            { 
                var start = Peek(Bottom);
                var end = Peek(Top);
                return end.Subtract(start);
            }
        }
    }
}
