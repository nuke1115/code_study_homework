using hw4.Engine.Core;
using hw4.Engine.GameObject;

namespace hw4.Engine.Component
{
    public abstract class ComponentBehaviour : ILifeCyclable
    {
        public GameObjectBehaviour ComponentOwner { get; set; }

        public bool IsDestroyed { get; set; } = false;


        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }

        public virtual void OnDestroy() { }
        public void Destroy()
        {
            IsDestroyed = true;
            ComponentOwner.RemoveDestroyedComponents();
        }
    }
}
