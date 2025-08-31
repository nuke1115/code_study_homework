using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Oak : MonsterBase
    {
        public Oak(string name, int hp, int power, bool printLog) : base(name, hp, power, printLog)
        {
            _type = eMonsterTypes.OAK;
        }
    }
}
