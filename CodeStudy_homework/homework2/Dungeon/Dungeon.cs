using HomeworkGame.Players.Player;
using HomeworkGame.GameStatus;
using HomeworkGame.Players.Monster;

namespace HomeworkGame.DungeonPlace
{
    public class Dungeon : DungeonBase
    {
        public Dungeon(GameContext context) : base(context) { }

        public override bool DoGameLogic(Player player, Monster monster)
        {

            _context.MoveNextTurn();
            PrintGameStatus(player,monster);

            if (SelectUnit(player) == false)
            {
                return false;
            }
            player.GetSelectedCharacter().Act(monster);

            if (SelectMonster(monster) == false)
            {
                return false;
            }
            monster.GetSelectedCharacter().Act(player);
            
            return true;
        }

        private bool SelectUnit(Player player)
        {
            if (player.IsAllDead())
            {
                return false;
            }

            while (player.SelectCharacter(_random.Next(0, _context.GetMaxUnitNum)) == false) ;

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
            Console.WriteLine($"-----{_context.GetElapsedTurns}턴-----");
            Console.WriteLine("몬스터 정보 : ");
            monster.PrintCharactersStatus();
            Console.WriteLine("유닛 정보 : ");
            player.PrintCharactersStatus();
        }
    }
}
