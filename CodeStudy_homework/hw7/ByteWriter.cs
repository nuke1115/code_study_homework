using System;
using System.Collections.Generic;
using System.Text;

namespace hw7_server
{
    public static class ByteWriter
    {
        public static void WriteInt(Span<byte> buffer, int data, ref int offset)
        {
            if (buffer.Length != 4)
            {
                return;
            }

            buffer[3] = (byte)(data >> 24);
            buffer[2] = (byte)(data >> 16);
            buffer[1] = (byte)(data >> 8);
            buffer[0] = (byte)(data);

            offset += 4;
        }
        public static void WriteUShort(Span<byte> buffer, ushort data, ref int offset)
        {
            if (buffer.Length != 2)
            {
                return;
            }
            buffer[1] = (byte)(data >> 8);
            buffer[0] = (byte)(data);
            offset += 2;
        }
    }
}
