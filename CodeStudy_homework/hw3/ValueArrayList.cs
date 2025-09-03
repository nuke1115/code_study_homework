
using System.Security.Cryptography;

namespace hw3
{
    public class ValueArrayList<T> where T : struct
    {
        private T[] _arr;
        private int _maxSize;
        private int _count = -1;
        private EqualityComparer<T> _comparer;

        public ValueArrayList(int capacity)
        {
            if(capacity < 0)
            {
                capacity = 0;
            }
            _maxSize = capacity;
            _arr = new T[capacity];
            _comparer = EqualityComparer<T>.Default;
        }

        public T this[int index]
        {
            get
            {
                if(index < 0 || index > _count)
                {
                    throw new IndexOutOfRangeException("잘못된 인덱스에 접근");
                }

                return _arr[index];
            }
            set
            {
                if (index < 0 || index > _count)
                {
                    throw new IndexOutOfRangeException("잘못된 인덱스에 접근");
                }
                _arr[index] = value;
            }
        }

        public void Add(T value)
        {
            if(IsFull())
            {
                throw new IndexOutOfRangeException("가득 찬 리스트에 원소 삽입 시도");
            }
            
            _count++;
            _arr[_count] = value;
        }

        public void Insert(int index, T value)
        {
            if (index < 0 || index > _count)
            {
                throw new IndexOutOfRangeException("잘못된 인덱스에 접근");
            }

            if (IsFull())
            {
                throw new IndexOutOfRangeException("가득 찬 리스트에 원소 삽입 시도");
            }

            for(int i = _count; i >= index; i--)
            {
                _arr[i + 1] = _arr[i];
            }

            _count++;

            _arr[index] = value;
        }

        public void Remove(int index)
        {
            if (index < 0 || index > _count)
            {
                throw new IndexOutOfRangeException("잘못된 인덱스에 접근");
            }

            if (IsEmpty())
            {
                throw new IndexOutOfRangeException("빈 리스트에 삭제 시도");
            }

            for (int i = index; i < _count; i++)
            {
                _arr[i] = _arr[i + 1];
            }
            
            _count--;
        }

        public void Clear()
        {
            _count = -1;
        }

        public bool Resize(int newCapacity)
        {
            if (newCapacity < 0)
            {
                return false;
            }

            if (ArrayUtils.ResizeArray(ref _arr, newCapacity) == false)
            {
                return false;
            }

            if (_maxSize > newCapacity)
            {
                _count = newCapacity - 1;
            }

            _maxSize = newCapacity;

            return true;
        }

        public bool Constains(T item)
        {
            foreach (T item2 in _arr)
            {
                if(_comparer.Equals(item2, item))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCount()
        {
            return _count + 1;
        }

        public bool IsFull()
        {
            return _count >= _maxSize - 1;
        }

        public bool IsEmpty()
        {
            return _count < 0;
        }
    }
}
