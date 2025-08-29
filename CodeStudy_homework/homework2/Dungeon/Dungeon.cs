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

            Console.WriteLine("-------플레이어 턴-------");
            if (SelectUnit(player) == false)
            {
                return false;
            }
            player.GetSelectedCharacter().Act(monster);

            Console.WriteLine("-------몬스터 턴-------");
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
            if(monster.IsAllDead())
            {
                return false;
            }

            while (monster.SelectCharacter(_random.Next(0, _context.GetMaxMonsterNum)) == false);

            return true;
        }
    }
}
