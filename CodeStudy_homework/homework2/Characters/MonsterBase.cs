using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters
{
    public abstract class MonsterBase : CharacterBase<Player,UnitBase>
    {
        protected eMonsterTypes _type;
        public MonsterBase(string name, int hp, int power) : base(name, hp, power){ }

        public eMonsterTypes GetMonsterType
        {
            get { return _type; }
        }

        public override void Act(Player type)
        {
            Attack(SelectTarget(type));
        }


        protected override void Attack(IDamage? target)
        {
            if (target is null || target.IsDead)
            {
                return;
            }

            target.TakeDamage(_power, _name);

        }


        protected override IDamage? SelectTarget(Player player)
        {
            if (player.IsAllDead())
            {
                return null;
            }

            UnitBase? target;

            while (player.TryGetCharacter(_random.Next(0, player.GetCharacterCount()), out target) == false) ;

            return target;
        }
    }
}
