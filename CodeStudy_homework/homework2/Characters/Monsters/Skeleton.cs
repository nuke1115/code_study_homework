using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Skeleton : MonsterBase
    {
        public Skeleton(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.SKELETON;
        }
        protected override void Attack(List<UnitBase>? targets)
        {
            if(targets is null)
            {
                return;
            }

            UnitBase selectedUnit = targets[0];
            if (selectedUnit.IsDefense)
            {
                Console.WriteLine($"{_type.ToString()} 인 {_name} 가 {selectedUnit.GetName}의 견고한 방어를 뚫었다.");
                selectedUnit.SetDefense(false);
            }

            selectedUnit.TakeDamage(_power);

        }
    }
}
