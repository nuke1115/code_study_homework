using HomeworkGame.GameStatus;

namespace HomeworkGame.Input.InputStrategy
{
    public class SpaceKey : IInput
    {
        private StratagyManager _mgr;
        public SpaceKey(StratagyManager stratagyManager)
        {
            _mgr = stratagyManager;
        }

        public bool GetInput(GameContext context, ref IInput nowInputStrategy)
        {

            Console.WriteLine("숙제-게임\n스페이스를 눌러 시작");

            ConsoleKey key = Console.ReadKey().Key;


            if (key == ConsoleKey.Spacebar)
            {
                context.SetGameStatus(eGameStatus.GAME_RUNNING);
                return false;
            }
            else if(key == ConsoleKey.A)
            {
                context.SetGameStatus(eGameStatus.GAME_RUNNING);
                nowInputStrategy = _mgr.GetInputStrategy("AKey");
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
//인터페이스의 명시적 구현?