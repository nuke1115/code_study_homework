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

#if true
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
            
            if(stratagy == eInputStratagies.SPACE_MODE)
            {
                _player.SetLogState(true);
                _monster.SetLogState(true);
            }
            else if(stratagy == eInputStratagies.A_MODE)
            {
                _player.SetLogState(false);
                _monster.SetLogState(false);
            }
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
            Console.WriteLine($"-----총 {_context.GetElapsedTurns}턴 ------");
            foreach (UnitBase unit in _player.GetCharacterList())
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


저거 TakeDamage함수만 호출 되면, 일단 데미지는 들어가잖아
그리고, 거기서 출력기능은 부가적인거고, 호출쪽에서 처리해도 문제가 없다?

-----------


일단, 방어 있고 없고 로직 빼기, 행동 선택 로직 뺴기,공격당한대상 다시 표시

 
선택메시지 켜고 끄기:

 

Thread.Sleep으로 메인 스레드를 멈춰도, 입력자체는 그 기간동안 입력된건 들어가네?
왜지?
입력은 메인스레드하고 별계로 처리되나? <= stream클래스에서 답을 볼 수 있을거같은데


-----
Thread.Sleep(5000);
Console.ReadLine();
이런 코드가 있을 때,
c#에서 현제 스레드를 잠시 정지하는 Thread.Sleep으로 현제 스레드를 잠깐 정지하는 동안 입력한 키보드 입력이 정지가 풀린 후 입력이 됩니다.
Console클래스는 표준 입력 스트림에서 데이터를 읽어온다고 하는데, 저 스트림에 데이터를 넣는 동작은 현제 스레드의 정지 여부에 상관 없이 동작하나

아니면, 정지가 끝난 후, 스트림이 한번에 읽어오나?
근데 ,그러면 어디에 쌓아둘건데?
딱히 어딘가에 쌓아둘만한것도 없을거같은데(키보드 누르면 인터럽트로 데이터 읽게한다는걸 듣긴 함)
표준입력스트림에 넣는건 메인스레드에 관계 없이 작동한다는게 가장 타당할거같은데
 */