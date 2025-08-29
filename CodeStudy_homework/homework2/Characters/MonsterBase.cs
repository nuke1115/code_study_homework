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
