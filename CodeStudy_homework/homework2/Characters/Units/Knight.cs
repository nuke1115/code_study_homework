

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
    }
}
