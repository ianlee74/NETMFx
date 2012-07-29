using System.Collections;

namespace NETMFx.Collections
{
    public class StringFifoQueue
    {
        protected ArrayList Queue;
        public StringFifoQueue()
        {
            Queue = new ArrayList();
        }

        public int Count
        {
            get { return Queue.Count; }
        }

        public void Add(string data)
        {
            Queue.Add(data);
        }

        public string GetNext()
        {
            // TODO:  Lock thread.
            var data = PeekNext();
            if (data == null) return null;
            Queue.RemoveAt(0);
            return data;
        }

        public string PeekNext()
        {
            return Queue.Count > 0 ? Queue[0].ToString() : null;
        }
    }
}
