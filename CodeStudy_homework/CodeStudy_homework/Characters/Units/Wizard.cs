using HomeworkGame.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStudy_homework_1.Characters.Units
{
    public class Wizard : UnitBase
    {
        public Wizard(string name, int hp, int power) : base(name, hp, power)
        {
            _attackCount = 0;
            _killCount = 0;
            _type = eUnitTypes.WIZARD;
        }

        public override void ShowSelectMessage()
        {
            Console.WriteLine("공격할 대상 또는 대상들의 번호를 입력하세요. : ");
        }

        public override void Attack(List<CharacterBase> targets)
        {
            foreach (CharacterBase target in targets)
            {
                _attackCount++;

                if (target.IsDefense)
                {
                    Console.WriteLine($"{_type.ToString()} 인 {_name} 가 {target.GetType()} 인 {target.GetName} 를 공격하려 했으나 방어에 막혔다.");
                    return;
                }

                target.TakeDamage(_power);

                if (target.IsDead)
                {
                    _killCount++;
                }
            }
        }
    }
}
