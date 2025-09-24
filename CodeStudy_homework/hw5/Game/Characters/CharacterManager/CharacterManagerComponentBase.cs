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
            _characterList.Clear();
        }

        public int GetCharacterCnt()
        {
            return _characterList.GetCount();
        }
        public void PrintCharacterStatus()
        {
            for(int i = 0, characterCnt = _characterList.GetCount(); i < characterCnt; i++)
            {
                Console.Write($"{_characterList[i].classComponent.Name}({_characterList[i].classComponent.Class}) : 체력({_characterList[i].HPComponent.GetHP()})");
                Console.Write("생존 여부({0})", _characterList[i].HPComponent.IsDead() ? "사망" : "생존");
                Console.WriteLine($" 처치 횟수({_characterList[i].attackerComponent.KillCnt}) 공격횟수({_characterList[i].attackerComponent.AttackCnt})");
            }
        }
    }
}
