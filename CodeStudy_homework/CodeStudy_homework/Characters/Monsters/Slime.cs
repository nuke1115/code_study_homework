using HomeworkGame.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomeworkGame.Characters.Monsters
{
    public class Slime : MonsterBase
    {
        public Slime(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.SLIME;
        }

        public override void Attack(List<CharacterBase> targets)
        {
            foreach (CharacterBase target in targets)
            {
                if (target.IsDefense)
                {
                    Console.WriteLine($"{_type.ToString()} 인 {_name} 가 {target.GetType()} 인 {target.GetName} 를 공격하려 했으나 방어에 막혔다.");
                    return;
                }

                target.TakeDamage(_power);
            }
        }
    }
}
