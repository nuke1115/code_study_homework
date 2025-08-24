using HomeworkGame.Characters;
using HomeworkGame.Players.Monster;

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
            Console.WriteLine("마법사 : 여러 대상을 한번에 공격 가능하다");
            Console.WriteLine("행동 : (1) : 범위공격, (2) : 방어, (정의되지 않은 입력) : 턴 넘기기");
        }

        protected override void Attack(List<MonsterBase>? targets)
        {

            if(targets is null)
            {
                return;
            }

            foreach(MonsterBase selectedMonster in targets)
            {
                if(selectedMonster.IsDead)
                {
                    continue;
                }
                _attackCount++;

                if (selectedMonster.IsDefense)
                {
                    Console.WriteLine($"{_type.ToString()} 인 {_name} 가  {selectedMonster.GetName} 를 공격하려 했으나 방어에 막혔다.");
                    selectedMonster.SetDefense(false);
                    continue;
                }

                selectedMonster.TakeDamage(_power);

                if (selectedMonster.IsDead)
                {
                    _killCount++;
                }
            }
        }

        protected override List<MonsterBase>? SelectTarget(Monster monster)
        {
            return monster.GetCharacterList();

        }
    }
}
