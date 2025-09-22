using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Engine.Component
{
    public interface IComponentable
    {
        public TComponentType AddComponent<TComponentType>() where TComponentType : ComponentBehaviour, new();
        public TComponentType GetComponent<TComponentType>() where TComponentType : ComponentBehaviour;
        public TComponentType[] GetComponents<TComponentType>() where TComponentType : ComponentBehaviour;
        public void RemoveDestroyedComponents();
    }
}
