

namespace HomeworkGame.Players
{
    public abstract class PlayerBase<T>
    {
        protected List<T> _characters;
        protected Random _random;
        protected T _selectedUnit;

        public PlayerBase()
        {
            _characters = new List<T>();
            _random = new Random();
        }

        public List<T> GetCharacterList()
        {
            return _characters;
        }


        public abstract void ResetCharacters();

        public abstract bool IsAllDead();

        public abstract void PrintCharactersStatus();

        public abstract void ResetCharacterStatus();

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
