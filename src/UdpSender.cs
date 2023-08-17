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

        /// <summary>
        /// Sending byte array to reciver
        /// </summary>
        /// <param name="session">byte for sending</param>
        /// <param name="reciver"></param>
        /// <param name="trafficLoad">uploading speed</param>
        /// <returns></returns>
        public void Send(byte[] session, IPEndPoint reciver, TrafficLoadMBits trafficLoad)
        {
            int bytePerSecond = (int)trafficLoad * 125000;
            double microsecondbyte = 1000000.0 / bytePerSecond;

            CancellationToken cancellationToken = cts.Token;

            this.SendTask = Task.Run(() =>
            {
                for (int j = 0; j < session.Length && cancellationToken.IsCancellationRequested == false; j += 1500)
                {
                    byte[] packet = session.Skip(j).Take(1500).ToArray();
                    long wait = (long)Math.Round(packet.Length * microsecondbyte);
                    SendBytes(packet, reciver);
                    udelay(wait);
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

        private static void udelay(long us)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            long v = (us * System.Diagnostics.Stopwatch.Frequency) / 1000000;
            while (sw.ElapsedTicks < v) ; ;
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
