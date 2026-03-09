using hw7.Server.Packet;
using hw7_server;
using hw7_server.Server.Packet;
using hw7_server.UserData;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace SimpleTcpServer
{

    class Program
    {
        public ConcurrentQueue<(int id, Protocol protocol)> _requestQueue = new ConcurrentQueue<(int id, Protocol protocol)>();
        public event Action<ReadOnlySpan<byte>> WriteBroadcastEvent;
        private ConcurrentDictionary<int, UserData> _userData = new ConcurrentDictionary<int, UserData>();
        private TcpListener _server;
        private Vector2Int _borderMax = new Vector2Int(50,31); 
        private Vector2Int _borderMin = new Vector2Int(0,0); 

        
        static void Main(string[] args)
        {
            Task.Run(()=>new Program().StartServer()).Wait();
        }

        public async Task StartServer()
        {
            _server = new TcpListener(IPAddress.Any, 7777);
            _server.Start();
            Thread t = new Thread(ExecuteQueue);
            t.Start();
            Console.WriteLine("[서버] 시작되었습니다. 클라이언트의 접속을 기다립니다...");



            while(true)
            {
                TcpClient client = await _server.AcceptTcpClientAsync();
                _ = HandleAsync(client);
            }
        }

        public void ExecuteQueue()
        {
            UserData data;
            Span<byte> buffer = stackalloc byte[512];
            while(true)
            {
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

                        int offset = 0;

                        Span<byte> loginPackage = buffer.Slice(offset,(int)PacketPayloadSize.LOGIN + 4);
                        offset += loginPackage.Length;
                        Span<byte> loginPackageToNew = buffer.Slice(offset, (int)PacketPayloadSize.SET + 4);

                        ByteWriter.WriteUShort(loginPackage.Slice(0,2),(ushort)loginPackage.Length);
                        ByteWriter.WriteUShort(loginPackage.Slice(2,2),(ushort)Protocol.LOGIN);
                        ByteWriter.WriteInt(loginPackage.Slice(4,4),data.GetID());
                        ByteWriter.WriteUShort(loginPackage.Slice(8,2),data.GetCharacter());

                        foreach(var user in _userData.Values)
                        {
                            
                            ByteWriter.WriteUShort(loginPackageToNew.Slice(0, 2), (ushort)loginPackageToNew.Length);
                            ByteWriter.WriteUShort(loginPackageToNew.Slice(2, 2), (ushort)Protocol.SET);
                            ByteWriter.WriteInt(loginPackageToNew.Slice(4, 4), user.GetPos().X);
                            ByteWriter.WriteInt(loginPackageToNew.Slice(8, 4), user.GetPos().Y);
                            ByteWriter.WriteInt(loginPackageToNew.Slice(12, 4), user.GetID());
                            ByteWriter.WriteUShort(loginPackageToNew.Slice(16, 2), user.GetCharacter());
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

                        ByteWriter.WriteUShort(logoutPackage.Slice(0, 2), (ushort)logoutPackage.Length);
                        ByteWriter.WriteUShort(logoutPackage.Slice(2, 2), (ushort)Protocol.LOGOUT);
                        ByteWriter.WriteInt(logoutPackage.Slice(4, 4), data.GetID());

                        foreach (var user in _userData.Values)
                        {
                            Console.WriteLine("지워짐");
                            user.WriteMessage(logoutPackage);
                        }
                        data.Release();

                        Console.WriteLine("func");


                        break;
                    case Protocol.MOVE:
                        if (_userData.TryGetValue(req.id, out data) == false)
                        {
                            continue;
                        }

                        Span<byte> movePackage = buffer.Slice(0, (int)PacketPayloadSize.MOVE + 4);
                        ByteWriter.WriteUShort(movePackage.Slice(0, 2), (ushort)movePackage.Length);
                        ByteWriter.WriteUShort(movePackage.Slice(2, 2), (ushort)Protocol.MOVE);
                        ByteWriter.WriteInt(movePackage.Slice(4, 4), data.GetPos().X);
                        ByteWriter.WriteInt(movePackage.Slice(8, 4), data.GetPos().Y);
                        ByteWriter.WriteInt(movePackage.Slice(12, 4), data.GetID());

                        foreach (var user in _userData.Values)
                        {
                            Console.WriteLine("발신됨");
                            user.WriteMessage(movePackage);
                        }

                        Console.WriteLine($"{_userData.Values.Count}수신됨");

                        break;
                }
            }
        }

        public async Task HandleAsync(TcpClient client)
        {
            UserData myData = null;
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[512];
            bool isRunning = false;
            try
            {
                ushort packetSize = 0;
                Protocol protocol = 0;
                int payloadSize = 0;
                if (await ReadBufferAsync(stream, buffer, 4))
                {
                    packetSize = BitConverter.ToUInt16(buffer, 0);
                    protocol = (Protocol)BitConverter.ToUInt16(buffer, 2);
                    payloadSize = packetSize - 4;
                    Console.WriteLine($"{packetSize} {protocol} {payloadSize}");
                }

                if(payloadSize == (int)PacketPayloadSize.LOGIN && protocol == Protocol.LOGIN)
                {

                    if(ReadBufferWithProtocol(stream, buffer, payloadSize, PacketPayloadSize.LOGIN))
                    {
                        myData = new UserData(
                            BitConverter.ToInt32(buffer,0),
                            BitConverter.ToChar(buffer,4),
                            stream
                            );
                        Console.WriteLine($"{myData.GetCharacter()} {myData.GetID()}");

                        if(myData is not null)
                        {
                            isRunning = true;
                            _userData[myData.GetID()] = myData;
                            _requestQueue.Enqueue((myData.GetID(), Protocol.LOGIN));
                            Console.WriteLine($"[접속] : {myData.GetID()}");
                        }
                    }
                }

                while (isRunning)
                {
                    if(await ReadBufferAsync(stream,buffer,4) == false)
                    {
                        Console.WriteLine("asdfasdf");
                        break;
                    }

                    Console.WriteLine(stream.CanRead.ToString());

                    // 1. 패킷 전체 크기 읽기 (2바이트)
                    packetSize = BitConverter.ToUInt16(buffer, 0);

                    // 2. 프로토콜 ID 읽기 (2바이트)
                    protocol = (Protocol)BitConverter.ToUInt16(buffer, 2);

                    // 헤더 크기(Size 2바이트 + ID 2바이트 = 4바이트)를 뺀 나머지가 실제 데이터(Payload) 크기
                    payloadSize = packetSize - 4;

                    switch (protocol)
                    {
                        case Protocol.MOVE:
                            if(ReadBufferWithProtocol(stream,buffer,payloadSize,PacketPayloadSize.MOVE) == false)
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
                        default:
                            await DisposePacket(payloadSize, stream,buffer);
                            isRunning = false;
                            break;
                    }
                }
            }
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
                    WriteBroadcastEvent -= myData.WriteMessage;
                }
                
            }

            Console.WriteLine("연결종료");
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
                while (siz < payloadSize && stream.CanRead )
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
            while (payloadSize > 0 && reader.CanRead)
            {
                int read = await reader.ReadAsync(buffer, 0, Math.Min(512, payloadSize));
                payloadSize -= read;
            }

            Console.WriteLine($"       -> {payloadSize} bytes의 알 수 없는 데이터를 안전하게 무시했습니다.");

        }
    }
}