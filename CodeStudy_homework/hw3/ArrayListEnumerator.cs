using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw3
{
    public class ArrayListEnumerator<T> : IEnumerator<T> where T : struct
    {

        private T[] _target;
        private int _index = -1;
        private int _count = 0;

        public ArrayListEnumerator(T[] target, int itemCount)
        {
            _target = target;
            _count = itemCount;
        }

        public T Current => _target[_index];

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            _index++;
            if(_index < _count )
            {
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
