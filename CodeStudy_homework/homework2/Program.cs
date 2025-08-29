using HomeworkGame.Players.Player;
using HomeworkGame.Characters;
using HomeworkGame.DungeonPlace;
using HomeworkGame.GameStatus;
using HomeworkGame.Players.Monster;
using HomeworkGame.Input;
using HomeworkGame.Input.InputStrategy;

namespace HomeworkGame
{

    public class MainProgram
    {
        GameContext _context;
        Player _player;
        Monster _monster;
        DungeonBase _dungeon;
        Random _random;
        StratagyManager _mgr = new StratagyManager();
        DungeonManager _dungeonManager = new DungeonManager();
        IInput _nowInputStrategy;


        public MainProgram()
        {
            _context = new GameContext(3,3);
            _player = new Player(_context);
            _monster = new Monster(_context);
            _dungeon = new Dungeon(_context);
            _random = new Random();

            _mgr.RegisterStrategy(eInputStratagies.A_MODE, new AKey());
            _mgr.RegisterStrategy(eInputStratagies.SPACE_MODE, new SpaceKey());

            _dungeonManager.RegisterStrategy(eInputStratagies.A_MODE, new SkipDungeon(_context));
            _dungeonManager.RegisterStrategy(eInputStratagies.SPACE_MODE, new Dungeon(_context));

            UpdateStrategy(eInputStratagies.SPACE_MODE);
        }
        

        public static void Main()
        {

            MainProgram program = new MainProgram();

            if (program.RunGame() == false)
            {
                Console.WriteLine("게임이 비정상적으로 종료되었습니다");
                return;
            }

            Console.WriteLine("게임 종료");
        }

        private bool RunGame()
        {
            while(_context.GetGameStatus != eGameStatus.GAME_TERMINATION)
            {
                if (_context.GetGameStatus == eGameStatus.INITIAL_SCREEN)
                {
                    
                    if(_nowInputStrategy.GetInput(_context, out eInputStratagies  stratagy))
                    {
                        UpdateStrategy(stratagy);
                    }

                    Console.Clear();
                    InitGameState();

                    if (_context.GetGameStatus == eGameStatus.GAME_TERMINATION)
                    {
                        break;
                    }
                }

#if false
                ProcessGameLogics();
#else
                _context.SetGameStatus(eGameStatus.INITIAL_SCREEN);
#endif


                if (CheckGameEndCondition())
                {
                    EndGame();
                }
            }

            return true;
        }

        private void UpdateStrategy(eInputStratagies stratagy)
        {
            _nowInputStrategy = _mgr.GetInputStrategy(stratagy);
            _dungeon = _dungeonManager.GetStrategy(stratagy);
        }

        private void InitGameState()
        {
            _context.ResetContext(_random.Next(1, 10), _random.Next(1, 10));
            _player.ResetCharacters();
            _monster.ResetCharacters();
        }

        private bool ProcessGameLogics()
        {
            bool ret = _dungeon.DoGameLogic(_player, _monster);
            Console.WriteLine("--------------------");
            return ret;
        }

        private bool CheckGameEndCondition()
        {
            return _player.IsAllDead() || _monster.IsAllDead();
        }

        private void EndGame()
        {

            Console.WriteLine("========== 게임 종료 ==========");
            foreach(UnitBase unit in _player.GetCharacterList())
            {
                Console.WriteLine("--------------------");
                Console.WriteLine($"{unit.GetName} ({unit.GetUnitType.ToString()})");
                Console.WriteLine($"공격 횟수 : {unit.GetAttackCount} / 처치 횟수 {unit.GetKillCount}");
                Console.Write($"체력 : {unit.GetHp} / 상태 : ");
                Console.WriteLine(unit.IsDead ? "사망" : "생존");
            }
            Console.WriteLine("----------------------");
            Console.Write($"결과 : ");
            Console.WriteLine(_player.IsAllDead() ? "패배" : "승리");
            Console.WriteLine("===========================");
            _context.SetGameStatus(eGameStatus.INITIAL_SCREEN);
        }
    }
}
/*

일단, 방어 있고 없고 로직 빼기, 행동 선택 로직 뺴기

 
선택메시지 켜고 끄기:


공격대상 선택 기능 켜고 끄기:



공격당한 대상 쓰기
 
 */