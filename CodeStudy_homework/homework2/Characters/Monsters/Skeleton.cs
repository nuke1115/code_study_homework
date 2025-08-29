using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Skeleton : MonsterBase
    {
        public Skeleton(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.SKELETON;
        }
        protected override void Attack(IDamage? target)
        {
            if (target is null || target.IsDead)
            {
                return;
            }

            target.TakeDamage(_power);

        }
    }
}
