using hw4.Engine.Component;
using hw4.Game.DataStructure;

namespace hw4.Game.Characters.CharacterManager
{
    public abstract class CharacterManagerComponentBase : ComponentBehaviour
    {
        protected CharacterArrayList _characterList = new CharacterArrayList(16);

        protected CharacterManagerComponentBase _opponentManager;
        
        protected Random _random = new Random();

        private int _lastPickedCharacterIndex = -1;
        public GameContext.GameContext GameContext { get; set; }
        public override void Awake()
        {
            ComponentOwner.UpdateEvent += Update;
        }

        protected void AddCharacter(int strength,int hp, string className, string name)
        {
            var Character = ComponentOwner.GameObjectRequester.Instantiate<GameObject>();
            CharacterComponentBundle bundle = new CharacterComponentBundle();
            bundle.attackerComponent = Character.AddComponent<AttackerComponent>();
            bundle.HPComponent = Character.AddComponent<HPComponent>();
            bundle.classComponent = Character.AddComponent<ClassComponent>();

            bundle.classComponent.Name = name;
            bundle.classComponent.Class = className;

            bundle.attackerComponent.Strength = strength;
            bundle.attackerComponent.GameContext = GameContext;

            bundle.HPComponent.SetHP(hp);

            _characterList.AddAtLastAliveCharacter(bundle);
        }

        public bool IsAllDead()
        {
            return _characterList.GetAliveCount() < 1;
        }

        public CharacterComponentBundle GetCharacter()
        {
            _lastPickedCharacterIndex = _random.Next(0, _characterList.GetAliveCount());
            return _characterList[_lastPickedCharacterIndex];
        }
        
        public void OnCharacterDeath()
        {
            _characterList.CondenseAliveCharacter(_lastPickedCharacterIndex);
        }

        public abstract void FillCharacters(int characterCnt);

        public void ClearCharacters()
        {
            _characterList.Clear();
        }

        public override void OnDestroy()
        {
            ComponentOwner.UpdateEvent -= Update;
            _characterList.Clear();
        }

        public void PrintCharacterStatus()
        {
            foreach (var character in _characterList)
            {
                Console.Write($"{character.classComponent.Name}({character.classComponent.Class}) : 체력({character.HPComponent.GetHP()}/");
                Console.Write(character.HPComponent.IsDead() ? "사망" : "생존");
                Console.WriteLine($" 처치 횟수 : ({character.attackerComponent.KillCnt})({character.attackerComponent.AttackCnt})");
            }
        }
    }
}
