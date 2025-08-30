using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Slime : MonsterBase
    {
        public Slime(string name, int hp, int power,bool printLog) : base(name, hp, power, printLog)
        {
            _type = eMonsterTypes.SLIME;
        }


        public override void Act(Player type)
        {
            foreach (IDamage? target in type.GetCharacterList())
            {
                Attack(target);
            }
        }
    }
}
