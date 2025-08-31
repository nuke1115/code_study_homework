using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Input
{
    public class StratagyManager
    {
        private Dictionary<eInputStratagies, IInput> _inputStrategies = new Dictionary<eInputStratagies, IInput>();
        public StratagyManager()
        {

        }

        public void RegisterStrategy(eInputStratagies key, IInput strategy)
        {
            _inputStrategies.Add(key, strategy);
        }

        public IInput GetInputStrategy(eInputStratagies key)
        {
            return _inputStrategies[key];
        }
    }
}
