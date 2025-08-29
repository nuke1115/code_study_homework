using HomeworkGame.Input;

namespace HomeworkGame.DungeonPlace
{
    public class DungeonManager
    {
        private Dictionary<eInputStratagies, DungeonBase> _stratagies = new Dictionary<eInputStratagies, DungeonBase>();
        public DungeonManager() { }

        public void RegisterStrategy(eInputStratagies key, DungeonBase strategy)
        {
            _stratagies.Add(key, strategy);
        }

        public DungeonBase GetStrategy(eInputStratagies key)
        {
            return _stratagies[key];
        }

    }
}
