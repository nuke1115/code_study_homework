

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

        public override void ShowSelectMessage()
        {
            Console.WriteLine("궁수 : 대상의 방어를 뚫고 공격 가능하다.");
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
                Console.WriteLine($"{_type.ToString()} 인 {_name} 가 {selectedMonster.GetName}의 견고한 방어를 뚫었다.");
                selectedMonster.SetDefense(false);
            }

            selectedMonster.TakeDamage(_power);

            if (selectedMonster.IsDead)
            {
                _killCount++;
            }
        }
    }
}
