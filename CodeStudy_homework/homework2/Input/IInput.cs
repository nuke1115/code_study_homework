using HomeworkGame.GameStatus;

namespace HomeworkGame.Input
{
    public interface IInput
    {
        public bool GetInput(GameContext context, out eInputStratagies nowInputStrategy);
    }
}
