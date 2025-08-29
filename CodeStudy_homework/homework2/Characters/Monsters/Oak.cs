using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Oak : MonsterBase
    {
        public Oak(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.OAK;
        }
    }
}
