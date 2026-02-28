namespace hw6
{
    public class Ringbuffer<T> where T:struct
    {
        private bool _isOverwritingOn = false;
        private int _head = 0;
        private int _tail = 0;
        private int _cnt = 0;
        private int _filter = 0;
        private T[] _buffer;

        /// <summary>
        /// size = 1<<shlValue
        /// </summary>
        /// <param name="shlValue"></param>
        /// <param name="isOverwritingOn"></param>
        public Ringbuffer(byte shlValue, bool isOverwritingOn = false)
        {
            if(shlValue < 0)
            {
                shlValue = 0;
            }
            else if(shlValue > 31)
            {
                shlValue = 31;
            }

            _filter = (1<<shlValue)-1;
            _buffer = new T[1<<shlValue];
            _isOverwritingOn = isOverwritingOn;
        }

        public bool Resize(int newShlSize)
        {

            if (newShlSize < 0 || newShlSize > 31)
            {
                return false;
            }

            T[] buf = new T[1<<newShlSize];
            int idx = 0;

            while(!IsEmpty()&&idx<buf.Length)
            {
                buf[idx] = Dequeue();
                idx++;
            }

            _cnt = idx;
            _tail = 0;
            _head = idx;
            _buffer = buf;
            _filter = (1 << newShlSize) - 1;

            return true;
        }

        public void Clear()
        {
            _head = 0;
            _tail = 0;
            _cnt = 0;
        }

        public bool Enqueue(T item)
        {

            if(IsFull() == false)
            {
                _buffer[_head] = item;
                _head = GetNextIdx(_head);
                _cnt++;
                return true;
            }
            else if(_isOverwritingOn)
            {
                _buffer[_head] = item;
                _head = GetNextIdx(_head);
                _tail = GetNextIdx(_tail);
                return true;
            }

            return false;
        }

        public T Dequeue()
        {
            T tmp = _buffer[_tail];
            _tail = GetNextIdx(_tail);
            _cnt--;
            return tmp;
        }

        public bool TryDequeue(out T item)
        {
            item = default(T);
            if(IsEmpty())
            {
                return false;
            }
            item = Dequeue();
            return true;
        }

        public bool IsEmpty()
        {
            return _cnt <= 0;
        }

        public bool IsFull()
        {
            return _cnt >= _buffer.Length;//이것도 바꿀 수 있을듯
        }

        public int GetNextIdx(int oldIdx)
        {
            return (oldIdx+1) & _filter;
        }
    }
}