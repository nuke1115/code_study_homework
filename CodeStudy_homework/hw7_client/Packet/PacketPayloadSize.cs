using System;
using System.Collections.Generic;
using System.Text;

namespace hw7_server.Server.Packet
{
    public enum PacketPayloadSize
    {
        LOGIN = 6,
        MOVE = 12,
        LOGOUT=4,
        SET = 14

    }
}
