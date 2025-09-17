
namespace hw4.Game.GameContext
{
    public class GameContext
    {
        private int _cnt = 0;
        private int _elapsedGames = 0;
        public bool SkipInfo {  get; set; }
        public int GameLoopCnt { get { return _cnt; }}
        public int ElapsedGameCnt { get { return _elapsedGames; }}
        public ConsoleKeyInfo PressedKeyInfo { get; set; }

        public eGameStates GameState { get; set; }
        public void ResetLoopCnt()
        {
            _cnt = 0;
        }
        public void MoveToNextTurn()
        {
            _cnt++;
        }

        public void ResetGameCnt()
        {
            _elapsedGames = 0;
        }
        public void MoveToNextGame()
        {
            _elapsedGames++;
        }
    }
}
