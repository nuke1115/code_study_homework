
using System.Collections;

namespace hw3
{
    public class MyArrayList<T> : IEnumerable<T>
    {
        private T[] _arr;
        private int _maxSize;
        private int _index = -1;
        private int _count = 0;
        private EqualityComparer<T> _comparer;

        public MyArrayList(int capacity)
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
                if(index < 0 || index >= _count)
                {
                    throw new IndexOutOfRangeException("잘못된 인덱스에 접근");
                }

                return _arr[index];
            }
            set
            {
                if (index < 0 || index >= _count)
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
            
            _index++;
            _count++;
            _arr[_index] = value;
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

            for(int i = _index; i >= index; i--)
            {
                _arr[i + 1] = _arr[i];
            }

            _index++;
            _count++;
            _arr[index] = value;
        }

        public void Remove(int index)
        {
            if (index < 0 || index > _index)
            {
                throw new IndexOutOfRangeException("잘못된 인덱스에 접근");
            }

            if (IsEmpty())
            {
                throw new IndexOutOfRangeException("빈 리스트에 삭제 시도");
            }

            for (int i = index; i < _index; i++)
            {
                _arr[i] = _arr[i + 1];
            }

            _count--;
            _index--;
        }

        public void Clear()
        {
            _index = -1;
            _count = 0;
            Array.Clear(_arr);
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

            if (_index + 1 > newCapacity)
            {
                _index = newCapacity - 1;
                _count = newCapacity;
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
            return _count;
        }

        public bool IsFull()
        {
            return _count == _maxSize;
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayListEnumerator<T>(_arr, _count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
/*



*/