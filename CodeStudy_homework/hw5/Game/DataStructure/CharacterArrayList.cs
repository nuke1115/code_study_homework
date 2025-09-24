using hw3;
using hw4.Game.Characters;
using System.Collections;

namespace hw4.Game.DataStructure
{
    public class CharacterArrayList : IEnumerable<CharacterComponentBundle>
    {
        private MyArrayList<CharacterComponentBundle> _list;
        private int _lastAliveIndex = -1;

        public CharacterArrayList(int capacity)
        {
            _list = new MyArrayList<CharacterComponentBundle>(capacity);
        }

        public CharacterComponentBundle this[int index]
        {
            get
            {
                return _list[index];
            }
        }


        public int GetAliveCount()
        {
            return _lastAliveIndex + 1;
        }

        public int GetCount()
        {
            return _list.GetCount();
        }

        public void AddAtLastAliveCharacter(CharacterComponentBundle character)
        {
            if(_list.IsFull() && _list.Resize(_list.GetCount() + 16) == false)
            {
                throw new OutOfMemoryException("배열 크기 초과");
            }

            _list.Insert(GetAliveCount(),character);
            _lastAliveIndex++;
        }

        public void CondenseAliveCharacter(int deadCharacterIndex)
        {
            if(GetAliveCount() < 1 || deadCharacterIndex < 0 || deadCharacterIndex >= GetAliveCount() || deadCharacterIndex >= _list.GetCount())
            {
                return;
            }

            var tmp = _list[deadCharacterIndex];
            _list[deadCharacterIndex] = _list[_lastAliveIndex];
            _list[_lastAliveIndex] = tmp;
            _lastAliveIndex--;
        }

        public void ClearWithoutDestroy()
        {
            _lastAliveIndex = -1;
            _list.Clear();
        }
        public IEnumerator<CharacterComponentBundle> GetEnumerator()
        {
            return  _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
