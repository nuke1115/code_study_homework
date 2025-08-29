

namespace HomeworkGame.Characters.Units
{
    public class Archer : UnitBase
    {
        public Archer(string name, int hp, int power) : base(name, hp, power)
        {
            _attackCount = 0;
            _killCount = 0;
            _type = eUnitTypes.ARCHER;
        }

        protected override void Attack(IDamage? target)
        {
            if (target is null || target.IsDead)
            {
                return;
            }

            _attackCount++;

            target.TakeDamage(_power);

            if (target.IsDead)
            {
                _killCount++;
            }
        }
    }
}
