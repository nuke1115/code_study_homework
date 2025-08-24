using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Oak : MonsterBase
    {
        public Oak(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.OAK;
        }



        protected override void Attack(List<UnitBase>? targets)
        {
            
            if(targets is null)
            {
                return;
            }

            UnitBase selectedMonster = targets[0];

            if (selectedMonster.IsDefense)
            {
                Console.WriteLine($"{_type.ToString()} 인 {_name} 가  {selectedMonster.GetName} 를 공격하려 했으나 방어에 막혔다.");
                selectedMonster.SetDefense(false);
                return;
            }

            selectedMonster.TakeDamage(_power);

        }
    }
}
