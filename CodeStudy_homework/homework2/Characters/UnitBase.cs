using HomeworkGame.Players.Monster;

namespace HomeworkGame.Characters
{
    public abstract class UnitBase : CharacterBase<Monster,MonsterBase>
    {

        protected int _attackCount;
        protected int _killCount;
        protected eUnitTypes _type;

        public UnitBase(string name, int hp, int power) : base(name, hp, power)
        {
            _attackCount = 0;
            _killCount = 0;
        }

        public int GetAttackCount
        {
            get { return _attackCount; }
        }

        public int GetKillCount
        {
            get { return _killCount; }
        }

        public eUnitTypes GetUnitType
        {
            get { return _type; }
        }

        public override void Act(Monster type)
        {
            Attack(SelectTarget(type));
        }

        protected override IDamage? SelectTarget(Monster monster)
        {

            if(monster.IsAllDead())
            {
                return null;
            }

            MonsterBase? target;

            while (monster.TryGetCharacter(_random.Next(0,monster.GetCharacterCount()), out target) == false) ;

            return target;
        }
    }
}
