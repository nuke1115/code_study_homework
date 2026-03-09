using System;
using System.Collections.Generic;
using System.Text;

namespace hw7.Server.Packet
{
    public struct MovePacket
    {
        public MovePacket(int inX, int inY, int inPlayerID)
        {
            x = inX;
            y = inY;
            playerID = inPlayerID;
            protocol = Protocol.MOVE;

        }
        public Protocol protocol;
        public int x;
        public int y;
        public int playerID;
    }
}
