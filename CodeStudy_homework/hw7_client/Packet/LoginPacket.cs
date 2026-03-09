using hw7.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace hw7_server.Server.Packet
{
    public struct LoginPacket
    {
        public LoginPacket(int inPlayerID, char character)
        {
            protocol = Protocol.LOGIN;
            PlayerID= inPlayerID;
            Character= character;
        }

        public Protocol protocol;
        public int PlayerID;
        public char Character;
    }
}
