using hw7.Server.Packet;

using hw7_server;
using hw7_server.Server.Packet;
using hw7_server.UserData;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleTcpClient
{

    class Program
    {

        ushort _port;
        const int x = 50;

        const int y = 31;
        int id = 0;
        Dictionary<int,UserData> _users = new Dictionary<int,UserData>();
        NetworkStream? _stream;

        CancellationTokenSource _cts;
        CancellationToken _cancelToken;

        Program()
        {
            while (true)
            {
                Console.Write("포트 입력>>");
                if (ushort.TryParse(Console.ReadLine(), out _port))
                {
                    break;
                }
            }
            _cts = new CancellationTokenSource();
            _cancelToken = _cts.Token;
        }

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        static void Main(string[] args)
        {

            Program p = new Program();
            var t = Task.Run(p.StartClient);
            Task.Run(p.StartKey);//p.StartKey
            t.Wait();
            
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("접속 종료");
            Console.ReadKey();

        }
        private static readonly string RectFrame = GenerateRectFrame();

        private static string GenerateRectFrame()
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
            while (_cancelToken.IsCancellationRequested == false)
            {
                if(_stream is null)
                {
                    continue;
                }
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }

                    await Task.Delay(5);
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
                            _cts.Cancel();
                            return;
                        default:
                            continue;
                    }


                    Span<byte> movePackage = new Span<byte>(buffer).Slice(0, (int)PacketPayloadSize.MOVE + 4);

                    int offset = 0;
                    ByteWriter.WriteUShort(movePackage.Slice(offset, 2), (ushort)movePackage.Length, ref offset);
                    ByteWriter.WriteUShort(movePackage.Slice(offset, 2), (ushort)Protocol.MOVE, ref offset);
                    ByteWriter.WriteInt(movePackage.Slice(offset, 4), pos.X, ref offset);
                    ByteWriter.WriteInt(movePackage.Slice(offset, 4), pos.Y, ref offset);
                    ByteWriter.WriteInt(movePackage.Slice(offset, 4), id, ref offset);
                    _stream.Write(movePackage);
                }
            }
        }

        public async Task StartClient()
        {
            Console.WriteLine($"[클라이언트] {_port}포트로 열린 서버에 접속을 시도합니다...");
            try
            {
                using TcpClient client = new TcpClient("211.104.119.61", _port);

                id = 0;
                char character = '\0';
                Console.Write("원하는 아이디 정수 입력>>");

                if (int.TryParse(Console.ReadLine(), out id) == false)
                {
                    _cts.Cancel();
                    return;
                }

                Console.Write("캐릭터로 사용할 문자 키 입력");
                character = Console.ReadKey().KeyChar;

                _stream = client.GetStream();
                _users[id] = new UserData(id, character);
                UserData myData = _users[id];

            

                byte[] buffer = new byte[512];
                Span<byte> loginPackage = new Span<byte>(buffer).Slice(0, (int)PacketPayloadSize.LOGIN + 4);

                int offset = 0;
                ByteWriter.WriteUShort(loginPackage.Slice(offset, 2), (ushort)loginPackage.Length,ref offset);
                ByteWriter.WriteUShort(loginPackage.Slice(offset, 2), (ushort)Protocol.LOGIN, ref offset);
                ByteWriter.WriteInt(loginPackage.Slice(offset, 4), _users[id].GetID(), ref offset);
                ByteWriter.WriteUShort(loginPackage.Slice(offset, 2), _users[id].GetCharacter(), ref offset);
                _stream.Write(loginPackage);

                Console.Clear();



                while (_cancelToken.IsCancellationRequested == false)
                {

                    DrawBox();
                    DrawPlayer();


                    

                    ushort packetSize = 0;
                    Protocol protocol = 0;
                    int payloadSize = 0;

                    if (await StreamUtils.ReadBufferAsync(_stream, buffer, 4,_cancelToken) == false)
                    {
                        break;
                    }

                    // 1. 패킷 전체 크기 읽기 (2바이트)
                    packetSize = BitConverter.ToUInt16(buffer, 0);

                    // 2. 프로토콜 ID 읽기 (2바이트)
                    protocol = (Protocol)BitConverter.ToUInt16(buffer, 2);

                    // 헤더 크기(Size 2바이트 + ID 2바이트 = 4바이트)를 뺀 나머지가 실제 데이터(Payload) 크기
                    payloadSize = packetSize - 4;

                    switch (protocol)
                    {
                        case Protocol.LOGIN:
                            if (await StreamUtils.ReadBufferWithProtocol(_stream, buffer, payloadSize, PacketPayloadSize.LOGIN,_cancelToken) == false)
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
                            if(await StreamUtils.ReadBufferWithProtocol(_stream, buffer,payloadSize,PacketPayloadSize.SET, _cancelToken) == false)
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
                            if (await StreamUtils.ReadBufferWithProtocol(_stream, buffer, payloadSize, PacketPayloadSize.LOGOUT, _cancelToken) == false)
                            {
                                continue;
                            }

                            _users.Remove(BitConverter.ToInt32(buffer, 0));
                            break;
                        case Protocol.MOVE:
                            if (await StreamUtils.ReadBufferWithProtocol(_stream, buffer, payloadSize, PacketPayloadSize.MOVE, _cancelToken) == false)
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
                            await StreamUtils.DisposePacket(payloadSize, _stream, buffer,_cancelToken);
                            break;
                    }

                    
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _cts.Cancel();
                if(_stream is not null)
                {
                    _stream.Close();
                }
            }
        }
    }
}