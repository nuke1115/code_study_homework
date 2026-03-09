using System;
using System.Collections.Generic;
using System.Text;

namespace hw7.Server.Packet
{
    public struct SetPacket
    {
        public SetPacket(int inX, int inY, char inCharacter, int inPlayerID)
        {
            x = inX;
            y = inY;
            playerID = inPlayerID;
            protocol = Protocol.MOVE;
            character = inCharacter;

        }
        public Protocol protocol;
        public int x;
        public int y;
        public int playerID;
        public char character;
    }
}
