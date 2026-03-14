using hw7.Server.Packet;
using hw7_server;
using hw7_server.Server.Packet;
using hw7_server.UserData;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleTcpServer
{

    class Program
    {

        private bool [,]_map = new bool[50, 31];
        public ConcurrentQueue<(int id, Protocol protocol)> _requestQueue = new ConcurrentQueue<(int id, Protocol protocol)>();
        //public event Action<ReadOnlySpan<byte>> WriteBroadcastEvent;
        private ConcurrentDictionary<int, UserData> _userData = new ConcurrentDictionary<int, UserData>();
        private TcpListener? _server;
        private Vector2Int _borderMax = new Vector2Int(50,31); 
        private Vector2Int _borderMin = new Vector2Int(0,0);

        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private ushort _port;
        
        Program()
        {

            while(true)
            {
                Console.Write("포트 입력>>");
                if(ushort.TryParse(Console.ReadLine(), out _port))
                {
                    break;
                }
            }

            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        static void Main(string[] args)
        {

            var p =new Program();
            
            var t = Task.Run(p.StartServer);
            p.StartKey();
            t.Wait();

            Console.WriteLine("서버 종료");
            Console.ReadKey(true);
        }

        public void StartKey()
        {
            byte[] buffer = new byte[512];
            while (_token.IsCancellationRequested == false)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    //await Task.Delay(1);
                    Vector2Int pos = new Vector2Int(0, 0);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            _cts.Cancel();
                            foreach (var u in _userData.Values)
                            {
                                u.Release();
                            }

                            if(_server is not null)
                            {
                                _server.Stop();
                            }
                            return;
                        default:
                            continue;
                    }
                }
            }
        }

        public async Task StartServer()
        {
            _server = new TcpListener(IPAddress.Any, _port);
            _server.Start();
            Thread t = new Thread(ExecuteQueue);
            t.Start();
            Console.WriteLine($"[서버] {_port}포트에서 클라이언트 접속 대기중...");



            try
            {
                while (_token.IsCancellationRequested == false)
                {
                    TcpClient client = await _server.AcceptTcpClientAsync(_token);
                    _ = HandleAsync(client);
                }
            }
            catch(Exception)
            {

            }
            finally
            {
                t.Join();
            }

        }

        public void ExecuteQueue()
        {
            UserData? data;
            Span<byte> buffer = stackalloc byte[512];
            int offset;
            while(_token.IsCancellationRequested == false)
            {
                offset = 0;
                if(_requestQueue.TryDequeue(out var req) == false)
                {
                    continue;
                }

                switch(req.protocol)
                {
                    case Protocol.LOGIN:

                        if(_userData.TryGetValue(req.id, out data) == false)
                        {
                            continue;
                        }

                        int packetOffset=0;
                        Span<byte> loginPackage = buffer.Slice(packetOffset, (int)PacketPayloadSize.LOGIN + 4);
                        packetOffset += loginPackage.Length;
                        Span<byte> loginPackageToNew = buffer.Slice(packetOffset, (int)PacketPayloadSize.SET + 4);

                        ByteWriter.WriteUShort(loginPackage.Slice(offset,2),(ushort)loginPackage.Length, ref offset);
                        ByteWriter.WriteUShort(loginPackage.Slice(offset, 2),(ushort)Protocol.LOGIN, ref offset);
                        ByteWriter.WriteInt(loginPackage.Slice(offset, 4),data.GetID(), ref offset);
                        ByteWriter.WriteUShort(loginPackage.Slice(offset,2),data.GetCharacter(), ref offset);


                        foreach(var user in _userData.Values)
                        {
                            offset = 0;
                            ByteWriter.WriteUShort(loginPackageToNew.Slice(offset, 2), (ushort)loginPackageToNew.Length, ref offset);
                            ByteWriter.WriteUShort(loginPackageToNew.Slice(offset, 2), (ushort)Protocol.SET, ref offset);
                            ByteWriter.WriteInt(loginPackageToNew.Slice(offset, 4), user.GetPos().X, ref offset);
                            ByteWriter.WriteInt(loginPackageToNew.Slice(offset, 4), user.GetPos().Y, ref offset);
                            ByteWriter.WriteInt(loginPackageToNew.Slice(offset, 4), user.GetID(), ref offset);
                            ByteWriter.WriteUShort(loginPackageToNew.Slice(offset, 2), user.GetCharacter(), ref offset);
                            if(user.GetID() != req.id)
                            {
                                user.WriteMessage(loginPackage);
                                data.WriteMessage(loginPackageToNew);
                            }
                        }
                        break;
                    case Protocol.LOGOUT:
                        if (_userData.TryRemove(req.id, out data) == false)
                        {
                            continue;
                        }

                        Span<byte> logoutPackage = buffer.Slice(0, (int)PacketPayloadSize.LOGOUT + 4);

                        ByteWriter.WriteUShort(logoutPackage.Slice(offset, 2), (ushort)logoutPackage.Length, ref offset);
                        ByteWriter.WriteUShort(logoutPackage.Slice(offset, 2), (ushort)Protocol.LOGOUT, ref offset);
                        ByteWriter.WriteInt(logoutPackage.Slice(offset, 4), data.GetID(), ref offset);

                        foreach (var user in _userData.Values)
                        {
                            user.WriteMessage(logoutPackage);
                        }
                        data.Release();



                        break;
                    case Protocol.MOVE:
                        if (_userData.TryGetValue(req.id, out data) == false)
                        {
                            continue;
                        }

                        Span<byte> movePackage = buffer.Slice(0, (int)PacketPayloadSize.MOVE + 4);
                        ByteWriter.WriteUShort(movePackage.Slice(offset, 2), (ushort)movePackage.Length, ref offset);
                        ByteWriter.WriteUShort(movePackage.Slice(offset, 2), (ushort)Protocol.MOVE, ref offset);
                        ByteWriter.WriteInt(movePackage.Slice(offset, 4), data.GetPos().X, ref offset);
                        ByteWriter.WriteInt(movePackage.Slice(offset, 4), data.GetPos().Y, ref offset);
                        ByteWriter.WriteInt(movePackage.Slice(offset, 4), data.GetID(), ref offset);

                        foreach (var user in _userData.Values)
                        {
                            user.WriteMessage(movePackage);
                        }

                        break;
                }
            }
        }

        public async Task HandleAsync(TcpClient client)
        {
            UserData? myData = null;
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[512];
            bool isRunning = false;
            int id = -1;
            try
            {
                ushort packetSize = 0;
                Protocol protocol = 0;
                int payloadSize = 0;
                if (await StreamUtils.ReadBufferAsync(stream, buffer, 4,_token))
                {
                    packetSize = BitConverter.ToUInt16(buffer, 0);
                    protocol = (Protocol)BitConverter.ToUInt16(buffer, 2);
                    payloadSize = packetSize - 4;
                    Console.WriteLine($"{packetSize} {protocol} {payloadSize}");
                }

                if((payloadSize == (int)PacketPayloadSize.LOGIN && protocol == Protocol.LOGIN) &&
                    await StreamUtils.ReadBufferWithProtocol(stream, buffer, payloadSize, PacketPayloadSize.LOGIN,_token) == false)
                {
                    return;
                }

                myData = new UserData(BitConverter.ToInt32(buffer, 0), BitConverter.ToChar(buffer, 4), stream);

                if (myData is null)
                {
                    return;
                }

                isRunning = true;
                _userData[myData.GetID()] = myData;
                _requestQueue.Enqueue((myData.GetID(), Protocol.LOGIN));
                Console.WriteLine($"[접속] : {myData.GetID()}");
                id = myData.GetID();




                //int cnt = 0;
                //int banCnt = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();



                while (isRunning && _token.IsCancellationRequested == false)
                {
                    if(await StreamUtils.ReadBufferAsync(stream, buffer, 4, _token) == false)
                    {
                        Console.WriteLine("asdfasdf");
                        break;
                    }

                    double time = sw.Elapsed.TotalMilliseconds;
                    sw.Restart();
                    

                    // 1. 패킷 전체 크기 읽기 (2바이트)
                    packetSize = BitConverter.ToUInt16(buffer, 0);

                    // 2. 프로토콜 ID 읽기 (2바이트)
                    protocol = (Protocol)BitConverter.ToUInt16(buffer, 2);

                    // 헤더 크기(Size 2바이트 + ID 2바이트 = 4바이트)를 뺀 나머지가 실제 데이터(Payload) 크기
                    payloadSize = packetSize - 4;

                    switch (protocol)
                    {
                        case Protocol.MOVE:
                            if(await StreamUtils.ReadBufferWithProtocol(stream, buffer, payloadSize, PacketPayloadSize.MOVE, _token) == false)
                            {
                                Console.WriteLine("서버와의 연결이 원활하지 않습니다.");
                                isRunning = false;
                                break;
                            }

                            MovePacket packet = new MovePacket(
                                BitConverter.ToInt32(buffer,0),
                                BitConverter.ToInt32(buffer,4),
                                BitConverter.ToInt32(buffer,8)
                                );
                            myData.MoveTo(new Vector2Int(packet.x, packet.y),in _borderMin, in _borderMax);

                            _requestQueue.Enqueue((myData.GetID(), Protocol.MOVE));

                            break;
                        case Protocol.SHOOT:
                            break;
                        default:
                            await StreamUtils.DisposePacket(payloadSize, stream,buffer,_token);
                            isRunning = false;
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            { }
            catch (Exception ex)
            {
                Console.WriteLine($"[오류] : {ex.Message}");
                //Console.WriteLine($"[서버] 종료됨: {ex.Message}");
            }
            finally
            {
                if(myData is not null)
                {
                    
                    _requestQueue.Enqueue((myData.GetID(),Protocol.LOGOUT));
                    //WriteBroadcastEvent -= myData.WriteMessage;
                }
                
            }

            Console.WriteLine($"{id} : 연결종료");
        }
    }
}