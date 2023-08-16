using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace FBUdpGenerator
{
    public class UdpSender : UdpBase
    {
        public bool IsSending => SendTask != null && SendTask.Status < TaskStatus.RanToCompletion;

        private Task SendTask;

        public UdpSender() 
        {
            this.UDPClient = new UdpClient();
        }

        public void Send(byte[] session, IPEndPoint reciver, TrafficLoadMBits trafficLoad)
        {
            int bitsPerSecond = (int)trafficLoad * 125000;
            double millisecondbybit = 1000.0 / bitsPerSecond;

            CancellationToken cancellationToken = cts.Token;

            this.SendTask = Task.Run(() =>
            {
                for (int j = 0; j < session.Length && cancellationToken.IsCancellationRequested == false; j += 1500)
                {
                    byte[] packet = session.Skip(j).Take(1500).ToArray();
                    int wait = (int)Math.Round(packet.Length * millisecondbybit);
                    SendBytes(packet, reciver);
                    Thread.Sleep(wait);
                }
            }, cancellationToken);
        }

        public void Cancel()
        {
            this.cts.Cancel();
        }

        private void SendBytes(byte[] bytes, IPEndPoint reciver)
        {
            if (bytes.Length > 0 && reciver != null)
            {
                this.UDPClient.Send(bytes, bytes.Length, reciver);
            }
        }
    }

    public enum TrafficLoadMBits : int
    {
        [Description("1Mbit")]
        OneMbit = 1,
        [Description("100Mbit")]
        HundredMBit = 100,
        [Description("1Gbit")]
        OneGbit = 1000
    }
}
