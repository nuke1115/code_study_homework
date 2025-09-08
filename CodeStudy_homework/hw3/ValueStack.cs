
namespace hw3
{
    public class ValueStack<T> where T : struct
    {
        private T[] _arr;
        private int _maxSize;
        private int _index = -1;
        private int _count = 0;
        private EqualityComparer<T> _comparer;

        public ValueStack(int capacity)
        {
            if(capacity < 0)
            {
                capacity = 0;
            }
            _maxSize = capacity;
            _arr = new T[_maxSize];
            _comparer = EqualityComparer<T>.Default;
        }

        public bool Resize(int newCapacity)
        {
            if (newCapacity < 0)
            {
                return false;
            }

            if(ArrayUtils.ResizeArray(ref _arr, newCapacity) == false)
            {
                return false;
            }

            if(GetCount() > newCapacity)
            {
                _index = newCapacity - 1;
                _count = newCapacity;
            }

            _maxSize = newCapacity;

            return true;
        }

        public void Push(T item)
        {
            if(IsFull())
            {
                throw new IndexOutOfRangeException("가득 찬 스텍에서 Push 시도");
            }

            _index++;
            _count++;
            _arr[_index] = item;
        }

        public void Pop()
        {
            if(IsEmpty())
            {
                throw new IndexOutOfRangeException("빈 스텍에서 Pop 시도");
            }

            _count--;
            _index--;
        }
        
        public T Peek()
        {
            if(IsEmpty())
            {
                throw new IndexOutOfRangeException("빈 스텍에서 Peek 시도");
            }

            return _arr[_index];
        }

        public int GetCount()
        {
            return _count;
        }

        public void Clear()
        {
            _index = -1;
            _count = 0;
        }

        public bool Contains(T item)
        {
            for(int i =0; i < _index + 1; i++)
            {
                if (_comparer.Equals(_arr[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        public bool IsFull()
        {
            return _count == _maxSize;
        }
    }
}
