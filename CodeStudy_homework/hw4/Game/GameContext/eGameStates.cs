using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Game.GameContext
{
    public enum eGameStates
    {
        GAME_INITIAL_SCREEN = 1,
        UNIT_TURN = 2,
        MONSTER_TURN=4,
        GAME_END=8
    }
}
