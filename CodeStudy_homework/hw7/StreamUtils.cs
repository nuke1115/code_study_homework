using hw7_server.Server.Packet;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace hw7_server
{
    public static class StreamUtils
    {
        public static async Task<bool> ReadBufferWithProtocol(NetworkStream stream, byte[] buffer, int payloadSize, PacketPayloadSize packetSize, CancellationToken token)
        {
            if (payloadSize != (int)packetSize)
            {
                await DisposePacket(payloadSize, stream, buffer,token);
                return false;
            }

            if (await ReadBufferAsync(stream, buffer, payloadSize,token) == false)
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> ReadBufferAsync(NetworkStream stream, byte[] buffer, int payloadSize, CancellationToken token)
        {
            int siz = 0;

            try
            {
                while (siz < payloadSize && stream.CanRead)
                {
                    int read = await stream.ReadAsync(buffer, siz, payloadSize - siz, token);
                    if (read == 0)
                    {
                        return false;
                    }
                    siz += read;
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
                Console.WriteLine($"오류 : {e.Message}");
                return false;
            }

            return true;
        }

        public static async Task DisposePacket(int payloadSize, NetworkStream reader, byte[] buffer, CancellationToken token)
        {

            // 남은 쓰레기 데이터를 읽어서 버림 (스트림 싱크 유지)
            try
            {
                while (payloadSize > 0 && reader.CanRead)
                {
                    int read = await reader.ReadAsync(buffer, 0, Math.Min(512, payloadSize), token);

                    if (read == 0)
                    {
                        return;
                    }

                    payloadSize -= read;
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine($"       -> {payloadSize} bytes의 알 수 없는 데이터를 안전하게 무시했습니다.");

        }
    }
}
