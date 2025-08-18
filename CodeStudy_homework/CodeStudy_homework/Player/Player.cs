using CodeStudy_homework_1.Characters.Units;
using HomeworkGame.Characters;
using HomeworkGame.Characters.Units;

namespace HomeworkGame.PlayerUnit
{
    public class Player
    {
        private List<UnitBase> _units;
        private UnitBase _selectedUnit;
        private Random _random;
        public Player()
        {
            _units = new List<UnitBase>();
            _random = new Random();
            ResetUnits();
            _selectedUnit = _units[0];
        }

        public void ResetUnits()
        {
            _units.Clear();
            _units.Add(new Knight("흙(수저)기사", _random.Next(30, 40), _random.Next(20, 30)));
            _units.Add(new Archer("대한민국의 양궁 선수", _random.Next(30, 37), _random.Next(15, 25)));
            _units.Add(new Wizard("흑마법사", _random.Next(20, 25), _random.Next(10, 24)));
        }

        public bool SelectUnit(int index)
        {
            if(index >= _units.Count)
            {
                Console.WriteLine("그 유닛은 이 세상에 존제하지 않습니다.");
                return false;
            }

            _selectedUnit = _units[index];
            _selectedUnit.ShowSelectMessage();
            return true;
        }

        public UnitBase GetSelectedUnit()
        {
            return _selectedUnit;
        }

        public List<UnitBase> GetUnits()
        {
            return _units;
        }

        public int GetKillCountSum()
        {
            int sum = 0;
            foreach (UnitBase unit in _units)
            {
                sum += unit.GetKillCount;
            }

            return sum;
        }

        public int GetAttackCountSum()
        {
            int sum = 0;
            foreach (UnitBase unit in _units)
            {
                sum += unit.GetAttackCount;
            }

            return sum;
        }

        public bool IsAllDead()
        {
            foreach(UnitBase unit in _units)
            {
                if(unit.IsDead)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
