using HomeworkGame.GameStatus;
using HomeworkGame.Players.Monster;
using HomeworkGame.Players.Player;

namespace HomeworkGame.DungeonPlace
{
    public abstract class DungeonBase
    {
        protected GameContext _context;
        protected Random _random;

        public DungeonBase(GameContext context)
        {
            _context = context;
            _random = new Random();
        }

        public abstract bool DoGameLogic(Player player, Monster monster);


        protected abstract bool SelectUnit(Player player);

        protected abstract bool SelectMonster(Monster monster);

        protected virtual void PrintGameStatus(Player player, Monster monster)
        {
            Console.WriteLine("--------------------");
            Console.WriteLine($"턴 {_context.GetElapsedTurns} :");
            Console.WriteLine("몬스터 정보 : ");
            monster.PrintCharactersStatus();
            Console.WriteLine("유닛 정보 : ");
            player.PrintCharactersStatus();
        }
    }
}
