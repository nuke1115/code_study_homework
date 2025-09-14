using hw3;
using hw4.Engine.Component;
using hw4.Engine.Core;
using hw4.Engine.Core.KeyEvent;

namespace hw4.Engine.GameObject
{
    public abstract class GameObjectBehaviour : IComponentable, ILifeCyclable
    {
        private MyArrayList<ComponentBehaviour> _components = new MyArrayList<ComponentBehaviour>(16);
        private bool _componentRemoveFlag = false;
        public bool IsDestroyed { get; set; }
        public int GameObjectNumber { get; set; } = 0;
        public event LifeCycleAction UpdateEvent;
        public event LifeCycleAction FixedUpdateEvent;
        public event LifeCycleAction LateUpdateEvent;
        
        public IKeyEventPublisher KeyEventPublisher { get; set; }
        public IGameObjectRequestable GameObjectRequester { get; set; }
        public ITerminatable Terminator { get; set; }
        
        
        public TComponentType AddComponent<TComponentType>() where TComponentType : ComponentBehaviour, new()
        {
            if(_components.IsFull())
            {
                return default(TComponentType);
            }
            TComponentType component = new TComponentType();
            component.ComponentOwner = this;
            component.Awake();
            component.Start();
            _components.Add(component);
            return component;
        }

        public TComponentType GetComponent<TComponentType>() where TComponentType : ComponentBehaviour
        {
            foreach (var component in _components)
            {
                if(!component.IsDestroyed && component is TComponentType)
                {
                    return (TComponentType)component;
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
        public virtual void Awake() { }
        public virtual void Start() { }
        public void Update()
        {
            UpdateEvent?.Invoke();
        }
        public void FixedUpdate()
        {
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
            foreach(var component in _components)
            {
                component.OnDestroy();
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
