
namespace HomeworkGame.GameStatus
{
    public class GameContext
    {
        private eGameStatus _gameStatus = eGameStatus.INITIAL_SCREEN;
        private int[] _selectedTargets;
        private int _turns = 0;


        public GameContext(int maxTargetNums)
        {
            _selectedTargets = new int[maxTargetNums];
            _turns = 0;
        }

        public void ResetContext(int maxTargetNums)
        {
            _selectedTargets = new int[maxTargetNums];
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

        public int[] GetSelectedTargets()
        {
            return _selectedTargets;
        }

        public void SetSelectedTargets(int[] targets)
        {
            _selectedTargets = targets;

            //for (int i = 0; i < targets.Length; i++)//_selectedTargets.len >= targets.len임이 보장됨
            //{
            //    _selectedTargets[i] = targets[i];
            //}
            //근데, 그냥 대입으로 해도 되지 않나?
            //어차피 참조를 받아오는거고, 보내는쪽은 참조가 함수 파괴되면서 사라질텐데
        }
    }
}
