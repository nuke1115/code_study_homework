using HomeworkGame.GameStatus;

namespace HomeworkGame.Input.InputStrategy
{
    public class AKey : IInput
    {
        StratagyManager _mgr;
        public AKey(StratagyManager mgr)
        {
            _mgr = mgr;
        }

        public bool GetInput(GameContext context, ref IInput nowInputStrategy)
        {

            Console.WriteLine("5초 후 자동으로 재시작. 스페이스를 눌러 스페이스 모드로 전환.");
            Thread.Sleep(5000);

            if (!Console.KeyAvailable)
            {
                context.SetGameStatus(eGameStatus.GAME_RUNNING);
                return false;
            }

            
            ConsoleKey key = Console.ReadKey().Key;

            if (key == ConsoleKey.Spacebar)
            {
                nowInputStrategy = _mgr.GetInputStrategy("SpaceKey");
                context.SetGameStatus(eGameStatus.GAME_RUNNING);
                return true;
            }
            else
            {
                context.SetGameStatus(eGameStatus.GAME_TERMINATION);
                return false;
            }
        }
    }
}
