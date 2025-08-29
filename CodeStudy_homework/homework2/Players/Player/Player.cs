using CodeStudy_homework_1.Characters.Units;
using HomeworkGame.Characters;
using HomeworkGame.Characters.Units;
using HomeworkGame.GameStatus;

namespace HomeworkGame.Players.Player
{
    public class Player : PlayerBase<UnitBase>
    {
        public Player(GameContext context) : base(context) { }

        public override void ResetCharacters()
        {
            _characters.Clear();

            for (int i = 0; i < _context.GetMaxUnitNum; i++)
            {
                int spawnTarget = _random.Next(0, 3);

                switch (spawnTarget)
                {
                    case 0:
                        _characters.Add(new Knight("흙(수저)기사", _random.Next(30, 41), _random.Next(20, 31)));
                        break;
                    case 1:
                        _characters.Add(new Archer("대한민국의 양궁 선수", _random.Next(30, 38), _random.Next(15, 26)));
                        break;
                    case 2:
                        _characters.Add(new Wizard("흑마법사", _random.Next(20, 26), _random.Next(10, 25)));
                        break;
                    default:
                        _characters.Add(new Knight("흙(수저)기사", _random.Next(30, 41), _random.Next(20, 31)));
                        break;
                }
            }

            _selectedUnit = _characters[0];
        }

        public override bool SelectCharacter(int index)
        {
            if(index >= _characters.Count || _characters[index].IsDead)
            {
                return false;
            }

            _selectedUnit = _characters[index];
            return true;
        }

        public override UnitBase GetSelectedCharacter()
        {
            return _selectedUnit;
        }

        public int GetKillCountSum()
        {
            int sum = 0;
            foreach (UnitBase unit in _characters)
            {
                sum += unit.GetKillCount;
            }

            return sum;
        }

        public int GetAttackCountSum()
        {
            int sum = 0;
            foreach (UnitBase unit in _characters)
            {
                sum += unit.GetAttackCount;
            }

            return sum;
        }

        public override bool IsAllDead()
        {
            foreach(UnitBase unit in _characters)
            {
                if(unit.IsDead == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override void PrintCharactersStatus()
        {
            for(int i = 0; i < _characters.Count; i++)
            {
                if (_characters[i].IsDead)
                {
                    Console.WriteLine($"{i+1} : 이 유닛은 더이상 세상에 존제하지 않습니다.");
                    continue;
                }
                Console.WriteLine($"{i+1} : 이름 : {_characters[i].GetName}, 직업 : {_characters[i].GetUnitType.ToString()}, 체력 : {_characters[i].GetHp}, 공격력 : {_characters[i].GetPower}");
            }
        }


        public override bool IsDead(int index)
        {
            return index < 0 || index >= _characters.Count || _characters[index].IsDead;
        }

        public override bool TryGetCharacter(int index, out UnitBase? target)
        {
            if (index < 0 || index >= _characters.Count)
            {
                target = null;
                return false;
            }

            target = _characters[index];
            return true;
        }
    }
}
