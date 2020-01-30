using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace UDP_GoogleProtoBuff
{
    class Program
    {
        private const int listenPort = 2500;
        static void Main(string[] args)
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.10"), listenPort);
            IPEndPoint clientEP = new IPEndPoint(IPAddress.Parse("127.0.0.20"), listenPort);

            UdpClient listener = new UdpClient(clientEP);
            Data objData = new Data();
            byte[] tempByte = listener.Receive(ref serverEP);
            Console.WriteLine($"Received message :");
            Data Recive = (Data)GPBDeserialization(tempByte);
            //Console.WriteLine($"{Encoding.ASCII.GetString(tempByte, 0, tempByte.Length)}");
            Console.WriteLine(Recive.label);
            Console.WriteLine(Recive.value);

        }

        public static byte[] GPBSerialization(object objtoSerialize)
        {
            if (objtoSerialize == null)
                return null;
            try
            {
                using (MemoryStream Stream = new MemoryStream())
                {

                    Serializer.Serialize(Stream, objtoSerialize);
                    return Stream.ToArray();

                }
            }
            catch
            {

                throw;
            }
        }
        public static object GPBDeserialization(byte[] data)
        {
            if (data == null)
                return null;
            try
            {
                using (MemoryStream Stream = new MemoryStream(data))
                {
                    return Serializer.Deserialize(typeof(Data), Stream);
                }
            }
            catch
            {

                throw;
            }
        }
    }

    [ProtoContract]
    public class Data
    {
        [ProtoMember(1)]
        public string label { get; set; }
        [ProtoMember(2)]
        public float value { get; set; }

    }
}

