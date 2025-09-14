using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Engine.GameObject
{
    public interface IGameObjectRequestable
    {
        public TGameObjectType Instantiate<TGameObjectType>() where TGameObjectType : GameObjectBehaviour, new();
        public TGameObjectType GetGameObject<TGameObjectType>(int objectNumber) where TGameObjectType : GameObjectBehaviour;
        public TGameObjectType[] GetGameObjects<TGameObjectType>(int objectNumber) where TGameObjectType : GameObjectBehaviour;
        public void DestroyDestroyedGameObjects();
    }
}
