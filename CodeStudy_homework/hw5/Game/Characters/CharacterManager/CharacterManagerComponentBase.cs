using hw4.Engine.Component;
using hw4.Game.DataStructure;
using hw5.Game.DataStructure;

namespace hw4.Game.Characters.CharacterManager
{
    public abstract class CharacterManagerComponentBase : ComponentBehaviour
    {
        protected CharacterArrayList _characterList = new CharacterArrayList(16);

        protected CharacterManagerComponentBase _opponentManager;

        private CharacterObjectPool _characterPool;
        
        protected Random _random = new Random();

        private int _lastPickedCharacterIndex = -1;
        public GameContext.GameContext GameContext { get; set; }

        public override void Awake()
        {
            _characterPool = new CharacterObjectPool(64, ComponentOwner.GameObjectRequester, true);
        }

        protected void AddCharacter(int strength,int hp, string className, string name)
        {
            CharacterComponentBundle bundle = _characterPool.GetFromPool();
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
            for(int i = 0, objCnt = _characterList.GetCount(); i < objCnt; i++)
            {
                _characterPool.ReturnToPool(_characterList[i]);
            }
            _characterList.ClearWithoutDestroy();
        }

        public override void OnDestroy()
        {
            foreach(var character in _characterList)
            {
                _characterPool.ReturnToPool(character);
            }
            _characterPool.ClearPool();
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
