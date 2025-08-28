using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Input
{
    public class StratagyManager
    {
        private Dictionary<string, IInput> _inputStrategies = new Dictionary<string, IInput>();
        public StratagyManager()
        {

        }

        public void RegisterStrategy(in string key, IInput strategy)
        {
            _inputStrategies.Add(key, strategy);
        }

        public IInput GetInputStrategy(string key)
        {
            return _inputStrategies[key];
        }
    }
}
