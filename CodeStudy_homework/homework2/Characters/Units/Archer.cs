

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
    }
}
