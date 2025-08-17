using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Characters.Monsters
{
    public class Skeleton : MonsterBase
    {
        public Skeleton(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.SKELETON;
        }

        public override void Attack(List<CharacterBase> targets)
        {
            if (targets[0].IsDefense)
            {
                Console.WriteLine($"{_type.ToString()} 인 {_name} 가 {targets[0].GetType()} 인 {targets[0].GetName}의 견고한 방어를 뚫었다.");
            }

            targets[0].TakeDamage(_power);
        }
    }
}
