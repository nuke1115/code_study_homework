using System;
using System.Collections.Generic;
using System.Text;

namespace hw7_server.UserData
{
    public struct Vector2Int
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void AddVec2Int(in Vector2Int v)
        {
            X+= v.X;
            Y+= v.Y;
        }

        public void ScalaMul(int val)
        {
            X*= val;
            Y*= val;
        }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
