using hw7.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace hw7_server.Server.Packet
{
    public struct LogoutPacket
    {
        public LogoutPacket(int inPlayerID)
        {
            protocol = Protocol.LOGIN;
            PlayerID = inPlayerID;
        }
        public Protocol protocol;
        public int PlayerID;
    }
}
