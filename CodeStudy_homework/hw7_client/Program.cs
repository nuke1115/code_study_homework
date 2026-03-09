using hw7.Server.Packet;

using hw7_server;
using hw7_server.Server.Packet;
using hw7_server.UserData;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleTcpClient
{
    public enum Names
    {
        Wall_1 = 1,
        Wall_2 = 2,
        Wall_3=3,
        Wall_4=4,
        Console_Cleaner
    }

    class Program
    {
        const int x = 50;
        const int y = 31;
        int id = 0;
        Dictionary<int,UserData> _users = new Dictionary<int,UserData>();
        NetworkStream _stream;
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        static void Main(string[] args)
        {
            Program p = new Program();
            Task.Run(p.StartClient);
            Task.Run(p.StartKey).Wait();
        }
        private static readonly string RectFrame = GenerateRectFrame(x, y);

        private static string GenerateRectFrame(int x, int y)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    // 첫 줄, 마지막 줄, 혹은 각 줄의 양 끝인 경우 '*'
                    if (i == 0 || i == y - 1 || j == 0 || j == x - 1)
                    {
                        sb.Append('*');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void DrawBox()
        {
            //Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write(RectFrame);
        }

        public void DrawPlayer()
        {
            foreach(var user in _users.Values)
            {
                Console.SetCursorPosition(user.GetPos().X, user.GetPos().Y);
                Console.Write(user.GetCharacter());
            }
        }

        public async Task StartKey()
        {
            byte[] buffer = new byte[512];
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    await Task.Delay(1);
                    Vector2Int pos = new Vector2Int(0, 0);
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            pos.X -= 1;
                            break;
                        case ConsoleKey.RightArrow:
                            pos.X += 1;
                            break;
                        case ConsoleKey.UpArrow:
                            pos.Y -= 1;
                            break;
                        case ConsoleKey.DownArrow:
                            pos.Y += 1;
                            break;
                        case ConsoleKey.Escape:
                            _stream.Close();
                            return;
                        default:
                            continue;
                    }
                    Span<byte> movePackage = new Span<byte>(buffer).Slice(0, (int)PacketPayloadSize.MOVE + 4);
                    ByteWriter.WriteUShort(movePackage.Slice(0, 2), (ushort)movePackage.Length);
                    ByteWriter.WriteUShort(movePackage.Slice(2, 2), (ushort)Protocol.MOVE);
                    ByteWriter.WriteInt(movePackage.Slice(4, 4), pos.X);
                    ByteWriter.WriteInt(movePackage.Slice(8, 4), pos.Y);
                    ByteWriter.WriteInt(movePackage.Slice(12, 4), id);
                    _stream.Write(movePackage);
                }
            }
        }

        public async Task StartClient()
        {
            Console.WriteLine("[클라이언트] 서버에 접속을 시도합니다...");
            using TcpClient client = new TcpClient("211.104.119.61", 7777);

            id = 0;
            char character = '\0';

            if (int.TryParse(Console.ReadLine(), out id) == false)
            {
                return;
            }

            character = Console.ReadKey().KeyChar;

            _stream = client.GetStream();
            _users[id] = new UserData(id, character);
            UserData myData = _users[id];

            try
            {

                byte[] buffer = new byte[512];
                Span<byte> loginPackage = new Span<byte>(buffer).Slice(0, (int)PacketPayloadSize.LOGIN + 4);

                ByteWriter.WriteUShort(loginPackage.Slice(0, 2), (ushort)loginPackage.Length);
                ByteWriter.WriteUShort(loginPackage.Slice(2, 2), (ushort)Protocol.LOGIN);
                ByteWriter.WriteInt(loginPackage.Slice(4, 4), _users[id].GetID());
                ByteWriter.WriteUShort(loginPackage.Slice(8, 2), _users[id].GetCharacter());
                _stream.Write(loginPackage);

                while (true)
                {

                    DrawBox();
                    DrawPlayer();

                    ushort packetSize = 0;
                    Protocol protocol = 0;
                    int payloadSize = 0;

                    if (await ReadBufferAsync(_stream, buffer, 4) == false)
                    {
                        break;
                    }

                    // 1. 패킷 전체 크기 읽기 (2바이트)
                    packetSize = BitConverter.ToUInt16(buffer, 0);

                    // 2. 프로토콜 ID 읽기 (2바이트)
                    protocol = (Protocol)BitConverter.ToUInt16(buffer, 2);

                    // 헤더 크기(Size 2바이트 + ID 2바이트 = 4바이트)를 뺀 나머지가 실제 데이터(Payload) 크기
                    payloadSize = packetSize - 4;

                    //Console.WriteLine(Enum.GetName(protocol));

                    switch (protocol)
                    {
                        case Protocol.LOGIN:
                            if (ReadBufferWithProtocol(_stream, buffer, payloadSize, PacketPayloadSize.LOGIN) == false)
                            {
                                continue;
                            }

                            int id = BitConverter.ToInt32(buffer, 0);
                            char charac = BitConverter.ToChar(buffer, 4);

                            UserData d = new UserData(
                                id,
                                charac
                                );
                            _users[id] = d;

                            break;
                        case Protocol.SET:
                            if(ReadBufferWithProtocol(_stream, buffer,payloadSize,PacketPayloadSize.SET) == false)
                            {
                                continue;
                            }

                            int newID = BitConverter.ToInt32(buffer, 8);
                            int x = BitConverter.ToInt32(buffer, 0);
                            int y = BitConverter.ToInt32(buffer, 4);
                            if(_users.ContainsKey(newID))
                            {
                                continue;
                            }
                            UserData newUData = new UserData(
                                newID,
                                BitConverter.ToChar(buffer,12)
                                );
                            _users[newUData.GetID()] = newUData;
                            _users[newID].SetPos(x, y);

                            break;
                        case Protocol.LOGOUT:
                            if (ReadBufferWithProtocol(_stream, buffer, payloadSize, PacketPayloadSize.LOGOUT) == false)
                            {
                                continue;
                            }

                            _users.Remove(BitConverter.ToInt32(buffer, 0));
                            break;
                        case Protocol.MOVE:
                            if (ReadBufferWithProtocol(_stream, buffer, payloadSize, PacketPayloadSize.MOVE) == false)
                            {
                                continue;
                            }

                            int tarID = BitConverter.ToInt32(buffer, 8);
                            int posX = BitConverter.ToInt32(buffer, 0);
                            int posY = BitConverter.ToInt32(buffer, 4);

                            if(_users.TryGetValue(tarID, out var user))
                            {
                                user.SetPos(posX, posY);
                            }


                            break;
                        default:
                            await DisposePacket(payloadSize, _stream, buffer);
                            break;
                    }

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool ReadBufferWithProtocol(NetworkStream stream, byte[] buffer, int payloadSize, PacketPayloadSize packetSize)
        {
            if (payloadSize != (int)packetSize)
            {
                DisposePacket(payloadSize, stream, buffer).Wait();
                return false;
            }

            if (ReadBufferAsync(stream, buffer, payloadSize).Result == false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ReadBufferAsync(NetworkStream stream, byte[] buffer, int payloadSize)
        {
            int siz = 0;

            try
            {
                while (siz < payloadSize)
                {
                    int read = await stream.ReadAsync(buffer, siz, payloadSize - siz);
                    if (read == 0)
                    {
                        return false;
                    }
                    siz += read;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"오류 : {e.Message}");
                return false;
            }

            return true;
        }

        public async Task DisposePacket(int payloadSize, NetworkStream reader, byte[] buffer)
        {

            // 남은 쓰레기 데이터를 읽어서 버림 (스트림 싱크 유지)
            while (payloadSize > 0)
            {
                int read = await reader.ReadAsync(buffer, 0, Math.Min(512, payloadSize));
                payloadSize -= read;
            }

            Console.WriteLine($"       -> {payloadSize} bytes의 알 수 없는 데이터를 안전하게 무시했습니다.");

        }
    }
}