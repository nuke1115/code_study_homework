using HomeworkGame.DungeonPlace;
using HomeworkGame.GameStatus;
using HomeworkGame.Players.Monster;
using HomeworkGame.Players.Player;

namespace HomeworkGame.DungeonPlace
{
    public class SkipDungeon : DungeonBase
    {
        public SkipDungeon(GameContext context) : base(context) { }

        public override bool DoGameLogic(Player player, Monster monster)
        {
            _context.MoveNextTurn();
            PrintGameStatus(player, monster);

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

        protected override bool SelectUnit(Player player)
        {
            if (player.IsAllDead())
            {
                return false;
            }

            while (player.SelectCharacter(_random.Next(0, _context.GetMaxUnitNum)) == false) ;

            return true;
        }

        protected override bool SelectMonster(Monster monster)
        {
            if (monster.IsAllDead())
            {
                return false;
            }

            while (monster.SelectCharacter(_random.Next(0, _context.GetMaxMonsterNum)) == false) ;

            return true;
        }

        protected override void PrintGameStatus(Player player, Monster monster)
        {
            Console.WriteLine($"-----{_context.GetElapsedTurns}턴-----");
        }
    }
}
