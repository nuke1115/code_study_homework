using HomeworkGame.Characters;
using HomeworkGame.Characters.Monsters;
using HomeworkGame.GameStatus;
using System;

namespace HomeworkGame.Players.Monster
{
    public class Monster : PlayerBase<MonsterBase>
    {

        private GameContext _context;
        public Monster(GameContext context) : base()
        {
            _context = context;
            ResetCharacters();
            
            
        }

        public override MonsterBase GetSelectedCharacter()
        {
            return _selectedUnit;
        }

        public override bool SelectCharacter(int index)
        {
            if (index >= _characters.Count || _characters[index].IsDead)
            {
                return false;
            }

            _selectedUnit = _characters[index];
            return true;
        }

        public override bool IsAllDead()
        {
            foreach (MonsterBase monster in _characters)
            {
                if (monster.IsDead == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override void PrintCharactersStatus()
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                if (_characters[i].IsDead)
                {
                    Console.WriteLine($"{i+1} : 사망");
                    continue;
                }
                Console.WriteLine($"{i + 1} : 이름 : {_characters[i].GetName}, 종족 : {_characters[i].GetMonsterType.ToString()}, 체력 : {_characters[i].GetHp}, 공격력 : {_characters[i].GetPower}, 방어 : {_characters[i].IsDefense}");
            }
        }

        public override void ResetCharacters()
        {
            _characters.Clear();

            for (int i = 0; i < _context.GetMaxMonsterNum; i++)
            {
                int spawnTarget = _random.Next(0, 3);
                MonsterBase spawnedMonster;
                switch (spawnTarget)
                {
                    case 0:
                        spawnedMonster = new Oak($"지나가던 오크 {i + 1}", _random.Next(30, 41), _random.Next(20, 31));
                        break;
                    case 1:
                        spawnedMonster = new Skeleton($"지나가던 스켈레톤 {i + 1}", _random.Next(30, 38), _random.Next(15, 26));
                        break;
                    case 2:
                        spawnedMonster = new Slime($"지나가던 슬라임 {i + 1}", _random.Next(20, 26), _random.Next(10, 25));
                        break;
                    default:
                        spawnedMonster = new Slime($"던전에 상주하던 슬라임 {i + 1}", _random.Next(20, 26), _random.Next(10, 25));
                        break;
                }
                _characters.Add(spawnedMonster);
            }

            _selectedUnit = _characters[0];
        }

        public override void ResetCharacterStatus()
        {
            foreach (MonsterBase unit in _characters)
            {
                unit.SetDefense(false);
            }
        }

        public override bool IsDead(int index)
        {
            return index < 0 || index >= _characters.Count || _characters[index].IsDead;
        }

        public override bool TryGetCharacter(int index, out MonsterBase? target)
        {
            if(index < 0 || index >= _characters.Count)
            {
                target = null;
                return false;
            }

            target = _characters[index];
            return true;
        }
    }
}
