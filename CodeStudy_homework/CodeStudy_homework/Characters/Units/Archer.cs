using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void Attack(List<CharacterBase> targets)
        {
            _attackCount++;
            if (targets[0].IsDefense)
            {
                Console.WriteLine($"{_type.ToString()} 인 {_name} 가 {targets[0].GetType()} 인 {targets[0].GetName}의 견고한 방어를 뚫었다.");
            }

            targets[0].TakeDamage(_power);

            if (targets[0].IsDead)
            {
                _killCount++;
            }
        }
    }
}
