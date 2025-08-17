using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void Attack(List<CharacterBase> targets)
        {
            _attackCount++;

            if (targets[0].IsDefense)
            {
                Console.WriteLine($"{_type.ToString()} 인 {_name} 가 {targets[0].GetType()} 인 {targets[0].GetName} 를 공격하려 했으나 방어에 막혔다.");
                return;
            }

            targets[0].TakeDamage(_power);

            if (targets[0].IsDead)
            {
                _killCount++;
            }
        }
    }
}
