using System.Net;
using System.Net.Sockets;

namespace FBUdpGenerator
{
    public class UdpReciver : UdpBase
    {
        public event EventHandler<(string, byte[])> UdpRecived;

        public bool IsReciving => ReciveTask != null && ReciveTask.Status < TaskStatus.RanToCompletion;

        private Task ReciveTask;

        public UdpReciver() { }

        public void StartRecive(int port, int buffersize = 8192)
        {
            this.EndPoint = new IPEndPoint(IPAddress.Any, port);
            this.UDPClient = new UdpClient(EndPoint);
            this.UDPClient.Client.ReceiveBufferSize = buffersize;

            CancellationToken cancellationToken = cts.Token;

            var from = new IPEndPoint(IPAddress.Any, 0);
            this.ReciveTask = Task.Run(() =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    byte[] bytes = this.UDPClient.Receive(ref from);
                    UdpRecived?.Invoke(this, (from.Address.ToString(), bytes));
                }
                this.UDPClient.Close();
                
            },
            cancellationToken);
        }

        public void StopRecive()
        {
            cts.Cancel();
            Thread.Sleep(300);
            if (this.UDPClient != null)
            {
                this.UDPClient.Close();
                this.UDPClient.Dispose();
            }
            cts = new CancellationTokenSource();
        }
    }
}
