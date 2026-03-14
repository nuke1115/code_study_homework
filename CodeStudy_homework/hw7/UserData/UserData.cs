using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace hw7_server.UserData
{
    public class UserData
    {
        private Vector2Int _pos;
        private NetworkStream _stream;
        private int _id = -1;
        private char _character;

        public UserData(int id, char character, NetworkStream stream)
        {
            _id = id;
            _pos = new Vector2Int(5, 5);
            _stream = stream;
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

        public void MoveTo(in Vector2Int delta, in Vector2Int borderMin, in Vector2Int borderMax)
        {
            if(_pos.X + delta.X < borderMin.X || _pos.Y + delta.Y < borderMin.Y)
            {
                return;
            }

            if (_pos.X + delta.X >= borderMax.X || _pos.Y + delta.Y >= borderMax.Y)
            {
                return;
            }

            _pos.AddVec2Int(delta);
        }

        public void Release()
        {
            _stream.Close();
        }

        public void WriteMessage(ReadOnlySpan<byte> span)
        {
            if(_stream.CanWrite)
            {
                _stream.Write(span);
            }
        }
    }
}
