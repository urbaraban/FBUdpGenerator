using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace FBUdpGenerator
{
    public class UdpReciver : UdpBase
    {
        public event EventHandler<byte[]> UdpRecived;

        public bool IsReciving => ReciveTask != null && ReciveTask.Status < TaskStatus.RanToCompletion;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private Task ReciveTask;

        public UdpReciver() { }

        public async void StartRecive(int port)
        {
            this.EndPoint = new IPEndPoint(IPAddress.Any, port);
            this.UDPClient = new UdpClient(EndPoint);
            this.UDPClient.Client.ReceiveTimeout = 1000;

            CancellationToken cancellationToken = cts.Token;

            var from = new IPEndPoint(IPAddress.Any, 0);
            this.ReciveTask = Task.Run(() =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        byte[] bytes = this.UDPClient.Receive(ref from);
                        Debug.WriteLine($"incoming {bytes.Length}");
                    }
                    catch (SocketException er)
                    {
                        Console.WriteLine(er);
                    }
                }
                this.UDPClient.Close();
            },
            cancellationToken);
        }

        public void StopRecive()
        {
            cts.Cancel();
        }
    }
}
