using System.Net;
using System.Net.Sockets;

namespace FBUdpGenerator
{
    public class UdpReciver : UdpBase
    {
        public event EventHandler<byte[]> UdpRecived;

        public bool IsReciving => ReciveTask.Status < TaskStatus.RanToCompletion;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private Task ReciveTask;

        public UdpReciver() { }

        public async void StartRecive(int port)
        {
            this.EndPoint = new IPEndPoint(IPAddress.Any, port);
            this.UDPClient = new UdpClient(this.EndPoint);
            this.UDPClient.Client.ReceiveTimeout = 1000;

            CancellationToken cancellationToken = cts.Token;
            try
            {
                this.ReciveTask = Task.Run(() => 
                { 
                    while(cancellationToken.IsCancellationRequested ==  false)
                    {
                        byte[] bytes = this.UDPClient.Receive(ref this.EndPoint);
                        UdpRecived?.Invoke(this, bytes);
                    }
                },
                cancellationToken);
               
            }
            catch (SocketException er)
            {
                Console.WriteLine(er);
            }
        }

        public void StopRecive()
        {
            cts.Cancel();
        }
    }
}
