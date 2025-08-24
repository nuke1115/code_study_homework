using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Slime : MonsterBase
    {
        public Slime(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.SLIME;
        }

        protected override void Attack(List<UnitBase>? targets)
        {
            if(targets is null)
            {
                return;
            }
            
            foreach(UnitBase selectedTarget in targets)
            {
                if(selectedTarget.IsDead)
                {
                    continue;
                }

                if (selectedTarget.IsDefense)
                {
                    Console.WriteLine($"{_type.ToString()} 인 {_name} 가  {selectedTarget.GetName} 를 공격하려 했으나 방어에 막혔다.");
                    selectedTarget.SetDefense(false);
                    continue;
                }

                selectedTarget.TakeDamage(_power);
            }

        }

        protected override List<UnitBase>? SelectTarget(Player player)
        {
            return player.GetCharacterList();
        }
    }
}
