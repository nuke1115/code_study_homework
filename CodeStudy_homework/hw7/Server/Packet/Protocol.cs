using System;
using System.Collections.Generic;
using System.Text;

namespace hw7.Server.Packet
{
    public enum Protocol : ushort
    {
        MOVE=1,
        LOGIN=3,
        LOGOUT=7,
        SET=8,
        SHOOT=9
    }
}
