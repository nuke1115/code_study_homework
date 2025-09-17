using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Game.GameContext
{
    public class GameContext
    {
        private int _cnt = 0;
        public bool SkipInfo {  get; set; }
        public int GameLoopCnt { get { return _cnt; }}
        public ConsoleKeyInfo PressedKeyInfo { get; set; }
        public void ResetLoopCnt()
        {
            _cnt = 0;
        }
        public void MoveToNextTurn()
        {
            _cnt++;
        }
    }
}
