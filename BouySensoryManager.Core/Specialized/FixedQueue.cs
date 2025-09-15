namespace BuoySensorManager.Core.Specialized
{
    public class FixedQueue<T>
    {
        private readonly int _maxSize;
        private readonly Queue<T> _queue;

        public FixedQueue(int maxSize)
        {
            _maxSize = maxSize;
            _queue = new Queue<T>();
        }

        public void Add(T item)
        {
            if (_queue.Count == _maxSize)
                _queue.Dequeue();

            _queue.Enqueue(item);
        }

        public IEnumerable<T> Items => _queue;
    }
}
