using System.Net;

namespace FBUdpGenerator
{
    public class UdpSender : UdpBase
    {
        public UdpSender() { }

        private void Send(byte[] session)
        {
            for (int j = 0; j < session.Length; j += 1500)
            {
                byte[] packet = session.Skip(j * 1500).Take(1500).ToArray();
                SendBytes(packet);
            }
        }

        private void SendBytes(byte[] bytes)
        {
            if (bytes.Length > 0 && this.EndPoint != null)
            {
                this.UDPClient.Send(bytes, bytes.Length, this.EndPoint);
            }
        }
    }

    public enum SendSpeed
    {

    }
}
