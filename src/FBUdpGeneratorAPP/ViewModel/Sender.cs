using FBUdpGenerator;
using FBUdpGenerator.Services;
using Microsoft.Xaml.Behaviors.Core;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FBUdpGeneratorAPP.ViewModel
{
    public class Sender : NotifyModel
    {
        public string Address { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 11000;
        public int DummySize { get; set; } = 100;

        public byte[] Dummy 
        {
            get => _dummy;
            set
            {
                _dummy = value;
                OnPropertyChanged(nameof(Dummy));
            }
        }
        private byte[] _dummy = new byte[1];

        public ObservableCollection<string> Log { get; } = new ObservableCollection<string>();

        public TrafficLoadMBits[] TrafficLoadList { get; } = new TrafficLoadMBits[]
        {
            TrafficLoadMBits.OneMbit,
            TrafficLoadMBits.HundredMBit,
            TrafficLoadMBits.OneGbit
        };
        public TrafficLoadMBits SelectTrafficLoad { get; set; } = TrafficLoadMBits.OneMbit;

        private UdpSender udpSender;

        public ICommand StartSendCommand => new ActionCommand(() =>
        {
            try
            {
                this.udpSender = new UdpSender();
                var endpoint = new IPEndPoint(IPAddress.Parse(Address), Port);
                this.udpSender.Send(this.Dummy, endpoint, SelectTrafficLoad);
                this.Log.Add($"Отправили {this.Dummy.Length} байт на {Address}");
            }
            catch
            {
                Log.Add($"Отправка сломалась");
            }

        });

        public ICommand GenerateCommand => new ActionCommand(() =>
        {
            this.Dummy = ShuffleGenerator.GetByteArray(DummySize);
            Log.Add($"Размер сгенерированного пакета {this.Dummy.Length * 8} бит");
        });

        public ICommand StopSendCommand => new ActionCommand(() =>
        {
            this.udpSender.Cancel();
        });

    }
}
