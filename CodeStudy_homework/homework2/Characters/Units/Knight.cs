

namespace HomeworkGame.Characters.Units
{
    public class Knight : UnitBase
    {
        public Knight(string name, int hp, int power) : base(name, hp, power)
        {
            _attackCount = 0;
            _killCount = 0;
            _type = eUnitTypes.KNIGHT;
        }

        protected override void Attack(IDamage? target)
        {

            if (target is null || target.IsDead)
            {
                return;
            }

            _attackCount++;

            target.TakeDamage(_power, _name);

            if (target.IsDead)
            {
                _killCount++;
            }
        }
    }
}
