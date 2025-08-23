
namespace HomeworkGame.GameStatus
{
    public class GameContext
    {
        private eGameStatus _gameStatus = eGameStatus.INITIAL_SCREEN;
        private int _maxMonsterNum;
        private int _turns = 0;


        public GameContext(int maxTargetNums)
        {
            _maxMonsterNum = maxTargetNums;
            _turns = 0;
        }

        public void ResetContext(int maxTargetNums)
        {
            _maxMonsterNum = maxTargetNums;
            _turns = 0;
            _gameStatus = eGameStatus.INITIAL_SCREEN;
        }
        
        public int GetElapsedTurns
        {
            get
            {
                return _turns;
            }
        }

        public int GetMaxMonsterNum
        {
            get { return _maxMonsterNum; }
        }
        public void MoveNextTurn()
        {
            _turns++;
        }

        public eGameStatus GetGameStatus
        {
            get
            {
                return _gameStatus;
            }
        }

        public void SetGameStatus(eGameStatus gameStatus)
        {
            _gameStatus = gameStatus;
        }
    }
}
