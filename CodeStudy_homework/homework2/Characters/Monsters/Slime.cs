using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Slime : MonsterBase
    {
        public Slime(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.SLIME;
        }

        protected override void Attack(IDamage? target)
        {
            if (target is null || target.IsDead)
            {
                return;
            }

            target.TakeDamage(_power, _name);

        }


        public override void Act(Player type)
        {
            foreach (IDamage? target in type.GetCharacterList())
            {
                Attack(target);
            }
        }
    }
}
