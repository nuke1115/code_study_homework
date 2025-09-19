using hw4.Engine.Component;
using hw4.Game.DataStructure;

namespace hw4.Game.Characters.CharacterManager
{
    public abstract class CharacterManagerComponentBase : ComponentBehaviour
    {
        private CharacterArrayList _characterList = new CharacterArrayList(16);
        Random Random = new Random();
        public override void Awake()
        {
            ComponentOwner.UpdateEvent += Update;
        }

        public void Init(int characterCnt)
        {
            _characterList.Clear();
        }

        public override void OnDestroy()
        {
            ComponentOwner.UpdateEvent -= Update;
            _characterList.Clear();
        }
    }
}
