using HomeworkGame.GameStatus;

namespace HomeworkGame.Input.InputStrategy
{
    public class AKey : IInput
    {

        public bool GetInput(GameContext context, out eInputStratagies nowInputStrategy)
        {
            nowInputStrategy = eInputStratagies.A_MODE;

            Console.WriteLine("5초 후 자동으로 재시작. 스페이스를 눌러 스페이스 모드로 전환.");
            Thread.Sleep(5000);//이거로 정지시켜도 입력 스트림에 넣는 동작은 살아있나

            if (!Console.KeyAvailable)
            {
                context.SetGameStatus(eGameStatus.GAME_RUNNING);
                return false;
            }

            
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Spacebar)
            {
                nowInputStrategy = eInputStratagies.SPACE_MODE;
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
