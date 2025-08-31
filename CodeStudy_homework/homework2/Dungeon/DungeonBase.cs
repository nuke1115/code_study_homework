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
        
    }
}
