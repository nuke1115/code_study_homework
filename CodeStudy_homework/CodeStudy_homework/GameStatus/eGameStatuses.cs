using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.GameStatus
{
    public enum eGameStatus
    {
        INITIAL_SCREEN  = 1,
        PRINT_STATUS    = 2,
        SELECT_UNIT     = 4,
        SELECT_ACTION   = 8,
        SELECT_TARGET   = 16,
        MONSTER_TURN    = 32,
        GAME_END        = 64
    }
}
