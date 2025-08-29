using HomeworkGame.Players.Player;

namespace HomeworkGame.Characters.Monsters
{
    public class Skeleton : MonsterBase
    {
        public Skeleton(string name, int hp, int power) : base(name, hp, power)
        {
            _type = eMonsterTypes.SKELETON;
        }
    }
}
