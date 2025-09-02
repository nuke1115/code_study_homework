
namespace hw3
{
    public class ValueStack<T> where T : struct
    {
        private T[] _arr;
        private int _maxSize;
        private int _index = -1;

        public ValueStack(int capacity)
        {
            if(capacity < 0)
            {
                capacity = 0;
            }
            _maxSize = capacity;
            _arr = new T[_maxSize];
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

            if(_maxSize > newCapacity)
            {
                _index = newCapacity - 1;
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
            _arr[_index] = item;
        }

        public void Pop()
        {
            if(IsEmpty())
            {
                throw new IndexOutOfRangeException("빈 스텍에서 Pop 시도");
            }

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

        public bool IsEmpty()
        {
            return _index < 0;
        }

        public bool IsFull()
        {
            return _index == _maxSize - 1;
        }
    }
}
