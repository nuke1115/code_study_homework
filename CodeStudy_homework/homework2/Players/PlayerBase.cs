

using HomeworkGame.Characters;
using HomeworkGame.GameStatus;

namespace HomeworkGame.Players
{
    public abstract class PlayerBase<T>
    {
        protected List<T> _characters;
        protected Random _random;
        protected T _selectedUnit;
        protected GameContext _context;
        protected bool _printLog;
        public PlayerBase(GameContext context,bool printLog)
        {
            _context = context;
            _characters = new List<T>();
            _random = new Random();
            SetLogState(printLog);
            ResetCharacters();
        }

        public void SetLogState(bool printLog)
        {
            _printLog = printLog;
        }

        public List<T> GetCharacterList()
        {
            return _characters;
        }


        public abstract void ResetCharacters();

        public abstract bool IsAllDead();

        public abstract void PrintCharactersStatus();

        public abstract bool SelectCharacter(int index);

        public abstract T GetSelectedCharacter();

        public abstract bool IsDead(int index);

        public abstract bool TryGetCharacter(int index, out T? target);

        public int GetCharacterCount()
        {
            return _characters.Count;
        }
    }
}
//타입제약을 걸어둬야, 베이스로 올려서 더 깔끔하게 짤 수 있겠다