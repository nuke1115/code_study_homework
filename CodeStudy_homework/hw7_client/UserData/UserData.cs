using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace hw7_server.UserData
{
    public class UserData
    {
        private Vector2Int _pos;
        private int _id = -1;
        private char _character;

        public UserData(int id, char character)
        {
            _id = id;
            _pos = new Vector2Int(5, 5);
            _character = character;
        }


        public Vector2Int GetPos()
        {
            return _pos;
        }

        public char GetCharacter()
        {
            return _character;
        }

        public int GetID()
        {
            return _id;
        }

        public void SetPos(int x, int y)
        {
            _pos.X=x;
            _pos.Y=y;
        }
    }
}
