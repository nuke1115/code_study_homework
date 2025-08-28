using HomeworkGame.Players.Player;
using HomeworkGame.Characters;
using HomeworkGame.DungeonPlace;
using HomeworkGame.GameStatus;
using System;
using HomeworkGame.Players.Monster;
using HomeworkGame.Input;
using HomeworkGame.Input.InputStrategy;

namespace HomeworkGame
{

    ///test





    ///test


    public class MainProgram
    {
        GameContext _context;
        Player _player;
        Monster _monster;
        Dungeon _dungeon;
        Random _random;
        StratagyManager _mgr = new StratagyManager();
        IInput _nowInputStrategy;


        public MainProgram()
        {
            _context = new GameContext(3);
            _player = new Player();
            _monster = new Monster(_context);
            _dungeon = new Dungeon(_context);
            _random = new Random();


            _mgr.RegisterStrategy("AKey", new AKey(_mgr));
            _mgr.RegisterStrategy("SpaceKey", new SpaceKey(_mgr));

            _nowInputStrategy = _mgr.GetInputStrategy("SpaceKey");
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
                    if(_nowInputStrategy.GetInput(_context, ref _nowInputStrategy))
                    {
                        //모든 객체들의 입력 함수 업데이트 로직 짜기
                        UpdateObjectInpputStrategy();
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

        private void UpdateObjectInpputStrategy()
        {
            Console.WriteLine("업데이트");
        }

        private void InitGameState()
        {
            _context.ResetContext(_random.Next(1, 5));
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