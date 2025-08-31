using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Skeleton : MonsterBase
    {
        public Skeleton(string name, int hp, int power, bool printLog) : base(name, hp, power, printLog)
        {
            _type = eMonsterTypes.SKELETON;
        }
    }
}
