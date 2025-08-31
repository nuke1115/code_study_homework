
namespace HomeworkGame.GameStatus
{
    public class GameContext
    {
        private eGameStatus _gameStatus = eGameStatus.INITIAL_SCREEN;
        private int _maxMonsterNum;
        private int _maxUnitNum;
        private int _turns = 0;
        private int _totalElapsedGames = 0;

        public GameContext(int maxTargetNums, int maxUnitNums)
        {
            _maxUnitNum = maxUnitNums;
            _maxMonsterNum = maxTargetNums;
            _turns = 0;
        }

        public void ResetContext(int maxTargetNums, int maxUnitNums)
        {
            _maxUnitNum = maxUnitNums;
            _maxMonsterNum = maxTargetNums;
            _turns = 0;
        }

        public void ResetGame()
        {
            ResetContext(3,3);
            _totalElapsedGames = 0;
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

        public int GetMaxUnitNum
        {
            get { return _maxUnitNum; }
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


        public int GetTotalElapsedGames
        {
            get { return _totalElapsedGames; }
        }
        
        public void MoveNextGame()
        {
            _totalElapsedGames++;
        }
    }
}
