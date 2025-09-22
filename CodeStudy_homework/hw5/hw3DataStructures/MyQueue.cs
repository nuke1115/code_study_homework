
namespace hw3
{
    public class MyQueue<T>
    {
        private T[] _arr;
        private int _maxCount;
        private int _count=0;
        private int _head = 0;
        private int _tail = 0;
        private EqualityComparer<T> _comparer;

        public MyQueue(int capacity)
        {
            if(capacity < 1)
            {
                capacity = 1;
            }
            _arr = new T[capacity];
            _maxCount = capacity;
            _comparer = EqualityComparer<T>.Default;
        }

        public void Enqueue(T item)
        {
            if(IsFull())
            {
                throw new IndexOutOfRangeException("꽉 찬 큐에 삽입 시도");
            }

            _count++;

            _arr[_head] = item;
            _head = MoveToNextIndex(_head);
        }

        public void Dequeue()
        {
            if(IsEmpty())
            {
                throw new IndexOutOfRangeException("빈 큐에 삭제 시도");
            }
            _count--;
            _tail = MoveToNextIndex(_tail);
        }

        public T Peek()
        {
            if (IsEmpty())
            {
                throw new IndexOutOfRangeException("빈 큐에 값 확인 시도");
            }
            return _arr[_tail];
        }

        public bool Resize(int newCapacity)
        {
            if(newCapacity < 1)
            {
                return false;
            }
            else if (_maxCount == newCapacity)
            {
                return true;
            }

            T[] newArray = new T[newCapacity];

            _count = _count < newCapacity ? _count : newCapacity;


            for(_head = 0; _head < _count; _head++)
            {
                newArray[_head] = _arr[_tail];
                _tail = MoveToNextIndex(_tail);
            }

            _maxCount = newCapacity;
            _arr = newArray;
            _head = MoveToNextIndex(_head - 1);
            _tail = 0;
            return true;
        }

        public bool Contains(T item)
        {
            for(int ptr = _tail, i = 0; i < _count ; i++)
            {
                if (_comparer.Equals(_arr[ptr] , item))
                {
                    return true;
                }
                ptr = MoveToNextIndex(ptr);
            }

            return false;
        }

        public int GetCount()
        {
            return _count;
        }

        public void Clear()
        {
            _head = _tail = 0;
            _count = 0;
            Array.Clear(_arr);
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        public bool IsFull()
        {
            return _count == _maxCount;
        }

        private int MoveToNextIndex(int index)
        {
            return (index + 1) % _maxCount;
        }
    }
}
