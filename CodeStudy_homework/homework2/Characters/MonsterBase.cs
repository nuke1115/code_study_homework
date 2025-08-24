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
            int act = _random.Next(1, 3);

            if (act == 1)
            {
                Attack(SelectTarget(type));
            }
            else if (act == 2)
            {
                SetDefense(true);
            }
        }

        protected override List<UnitBase>? SelectTarget(Player player)
        {
            if (player.IsAllDead())
            {
                return null;
            }

            UnitBase? target;

            while (player.TryGetCharacter(_random.Next(0, player.GetCharacterCount()), out target) == false) ;

            return new List<UnitBase>() { target };
        }
    }
}
