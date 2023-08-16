using System.Net;
using System.Net.Sockets;

namespace FBUdpGenerator
{
    public class UdpBase
    {
        protected IPEndPoint EndPoint;
        protected UdpClient UDPClient;
    }
}