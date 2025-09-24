using hw3;
using hw4.Engine.GameObject;
using hw4.Game;
using hw4.Game.Characters;

namespace hw5.Game.DataStructure
{
    public class CharacterObjectPool
    {
        private MyQueue<CharacterComponentBundle> _pool;
        private IGameObjectRequestable _objRequester;
        private bool _autoResize;
        public CharacterObjectPool(int capacity,IGameObjectRequestable objCreator, bool autoResize = false)
        {
            _pool = new MyQueue<CharacterComponentBundle>(capacity);
            _objRequester = objCreator;
            _autoResize = autoResize;

            for(int i = 0; i < capacity; i++)
            {
                _pool.Enqueue(CreateObject());
            }

        }

        public void ClearPool()
        {
            while(!_pool.IsEmpty())
            {
                _pool.Peek().classComponent.ComponentOwner.Destroy();
                _pool.Dequeue();
            }
        }

        private CharacterComponentBundle CreateObject()
        {
            CharacterComponentBundle bundle = new CharacterComponentBundle();
            var go = _objRequester.Instantiate<GameObject>();
            bundle.HPComponent = go.AddComponent<HPComponent>();
            bundle.attackerComponent = go.AddComponent<AttackerComponent>();
            bundle.classComponent = go.AddComponent<ClassComponent>();
            return bundle;
        }

        public CharacterComponentBundle GetFromPool()
        {
            CharacterComponentBundle bundle;
            if (_pool.IsEmpty())
            {
                bundle = CreateObject();
            }
            else
            {
                bundle = _pool.Peek();
                _pool.Dequeue();
            }
            return bundle;
        }

        public void ReturnToPool(CharacterComponentBundle bundle)
        {
            if(_pool.IsFull() && (!_autoResize || !_pool.Resize(_pool.GetCapacity() + 16)))
            {
                bundle.HPComponent.ComponentOwner.Destroy();
                return;
            }

            _pool.Enqueue(bundle);
        }

        public bool IncreasePool(int newCapacity)
        {
            if(newCapacity <= _pool.GetCapacity())
            {
                return false;
            }
            return _pool.Resize(newCapacity);
        }

        public bool IsFull()
        {
            return _pool.IsFull();
        }

        public bool IsEmpty()
        {
            return _pool.IsEmpty();
        }

        public int GetCount()
        {
            return _pool.GetCount();
        }

        public int GetCapacity()
        {
            return _pool.GetCapacity();
        }
        
    }
}
