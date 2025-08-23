using HomeworkGame.Players.Player;
using HomeworkGame.GameStatus;
using HomeworkGame.Players.Monster;

namespace HomeworkGame.DungeonPlace
{
    public class Dungeon
    {
        private GameContext _context;
        private Random _random;
        public Dungeon(GameContext context)
        {
            _random = new Random();
            _context = context;
        }

        public bool DoGameLogic(Player player, Monster monster)
        {

            _context.MoveNextTurn();
            PrintGameStatus(player,monster);
            if(SelectUnit(player) == false)
            {
                return false;
            }
            player.GetSelectedCharacter().Act(monster);
            if(SelectMonster(monster) == false)
            {
                return false;
            }
            monster.GetSelectedCharacter().Act(player);
            return true;
        }

        private bool SelectUnit(Player player)
        {
            if(player.IsAllDead())
            {
                return false;
            }

            Console.WriteLine("유닛을 번호로 선택해주세요");

            while(int.TryParse(Console.ReadLine(), out var selectedUnit) == false || player.SelectCharacter(selectedUnit - 1) == false);

            return true;
        }

        private bool SelectMonster(Monster monster)
        {
            if(monster.IsAllDead())
            {
                return false;
            }

            while (monster.SelectCharacter(_random.Next(0, _context.GetMaxMonsterNum)) == false);

            return true;
        }

        private void PrintGameStatus(Player player, Monster monster)
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
