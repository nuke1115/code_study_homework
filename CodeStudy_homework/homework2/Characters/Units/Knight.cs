

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

        public override void ShowSelectMessage()
        {
            Console.WriteLine("기사 : 그냥 단단하고, 강하다.");
            base.ShowSelectMessage();
        }



        protected override void Attack(List<MonsterBase>? targets)
        {

            if(targets is null)
            {
                return;
            }

            MonsterBase selectedMonster = targets[0];
            _attackCount++;

            if (selectedMonster.IsDefense)
            {
                Console.WriteLine($"{_type.ToString()} 인 {_name} 가  {selectedMonster.GetName} 를 공격하려 했으나 방어에 막혔다.");
                selectedMonster.SetDefense(false);
                return;
            }

            selectedMonster.TakeDamage(_power);

            if (selectedMonster.IsDead)
            {
                _killCount++;
            }
        }
    }
}
