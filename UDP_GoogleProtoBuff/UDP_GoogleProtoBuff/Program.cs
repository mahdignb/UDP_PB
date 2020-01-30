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
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("192.168.14.50"), listenPort);
            IPEndPoint clientEP = new IPEndPoint(IPAddress.Parse("192.168.14.117"), listenPort);

            UdpClient listener = new UdpClient(serverEP);

            Data objData = new Data();
            objData.label = "This is Data";
            objData.value = 1.23f;
            byte[] tempByte = GPBSerialization(objData);
            listener.Send(tempByte, tempByte.Length, clientEP);
            // Data objReconstructe = (Data)GPBDeserialization(tempByte);


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

