using hw3;
using hw4.Engine.Component;
using hw4.Engine.Core;
using hw4.Engine.Core.KeyEvent;
using System.ComponentModel;

namespace hw4.Engine.GameObject
{
    public abstract class GameObjectBehaviour : IComponentable, ILifeCyclable
    {
        private MyArrayList<ComponentBehaviour> _components = new MyArrayList<ComponentBehaviour>(32);
        private MyArrayList<ComponentBehaviour> _instantiatedComponents = new MyArrayList<ComponentBehaviour>(32);
        private bool _componentRemoveFlag = false;
        public bool IsDestroyed { get; set; }
        public int GameObjectNumber { get; set; } = 0;
        private event LifeCycleAction UpdateEvent;
        private event LifeCycleAction FixedUpdateEvent;
        private event LifeCycleAction LateUpdateEvent;

        
        public IKeyEventPublisher KeyEventPublisher { get; set; }
        public IGameObjectRequestable GameObjectRequester { get; set; }
        public ITerminatable Terminator { get; set; }

        private void RegisterInstantiatedObjects()
        {
            if (!_instantiatedComponents.IsEmpty())
            {
                for(int i = 0, objCnt = _instantiatedComponents.GetCount(); i < objCnt; i++)
                {
                    _instantiatedComponents[i].Start();
                    UpdateEvent += _instantiatedComponents[i].Update;
                    FixedUpdateEvent += _instantiatedComponents[i].FixedUpdate;
                    LateUpdateEvent += _instantiatedComponents[i].LateUpdate;
                }
                _instantiatedComponents.Clear();
            }
        }
        
        public TComponentType AddComponent<TComponentType>() where TComponentType : ComponentBehaviour, new()
        {
            if (_components.IsFull() && _components.Resize(_components.GetCount() * 2))
            {
                throw new OutOfMemoryException("컴포넌트 최대 양 초과");
            }

            if (_instantiatedComponents.IsFull() && !_instantiatedComponents.Resize(_instantiatedComponents.GetCount() * 2))
            {
                throw new OutOfMemoryException("컴포넌트 최대 양 초과");
            }

            TComponentType component = new TComponentType();
            component.ComponentOwner = this;
            component.Awake();

            _instantiatedComponents.Add(component);

            _components.Add(component);
            return component;
        }

        public TComponentType GetComponent<TComponentType>() where TComponentType : ComponentBehaviour
        {
            for(int i = 0, objCnt = _components.GetCount(); i < objCnt; i++)
            {
                if (!_components[i].IsDestroyed && _components[i] is TComponentType)
                {
                    return (TComponentType)_components[i];
                }
            }
            return default(TComponentType);
        }

        [Obsolete("살릴지 말지 고민중입니다")]
        public TComponentType[] GetComponents<TComponentType>() where TComponentType : ComponentBehaviour
        {
            throw new NotImplementedException("살릴지 말지 고민중입니다");
        }

        public void RemoveDestroyedComponents()
        {
            _componentRemoveFlag = true;
        }
        //여기서 오브젝트의 컴포넌트 등록하기
        public void Awake() { }
        public virtual void Start() { }
        public void Update()
        {
            RegisterInstantiatedObjects();

            UpdateEvent?.Invoke();
        }
        public void FixedUpdate()
        {
            RegisterInstantiatedObjects();

            FixedUpdateEvent?.Invoke();
        }
        public void LateUpdate()
        {
            LateUpdateEvent?.Invoke();
            
            if(_componentRemoveFlag)
            {
                _componentRemoveFlag = false;
                DestroyMarkedComponent();
            }
        }
        public void OnDestroy()
        {
            for(int i = 0, objCnt = _components.GetCount(); i < objCnt; i++)
            {
                _components[i].Destroy();
            }
            _components.Clear();
        }

        public void Destroy()
        {
            IsDestroyed = true;
            GameObjectRequester.DestroyDestroyedGameObjects();
        }

        private void DestroyMarkedComponent()
        {
            for(int i = _components.GetCount() - 1; i >= 0; i--)
            {
                if(_components[i].IsDestroyed)
                {
                    _components[i].OnDestroy();
                    _components.Remove(i);
                }
            }
        }
    }
}
