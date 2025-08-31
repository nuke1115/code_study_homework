

namespace HomeworkGame.Characters.Units
{
    public class Knight : UnitBase
    {
        public Knight(string name, int hp, int power,bool printLog) : base(name, hp, power, printLog)
        {
            _attackCount = 0;
            _killCount = 0;
            _type = eUnitTypes.KNIGHT;
        }
    }
}
