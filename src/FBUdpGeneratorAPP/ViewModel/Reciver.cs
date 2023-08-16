using FBUdpGenerator;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace FBUdpGeneratorAPP.ViewModel
{
    public class Reciver : NotifyModel
    {
        public string Address { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 11000;

        public bool IsReciving => UdpReciver.IsReciving;

        public ObservableCollection<string> Log { get; } = new ObservableCollection<string>();

        private UdpReciver UdpReciver { get; set; } = new UdpReciver();

        public ICommand StartReciveCommand => new ActionCommand(() => 
        {
            if (this.UdpReciver.IsReciving == false)
            {
                try
                {
                    Log.Add($"Start recive");
                    this.UdpReciver.StartRecive(this.Port, Address);
                    this.UdpReciver.UdpRecived += UdpReciver_UdpRecived;
                }
                catch
                {
                    Log.Add($"Abort recive");
                }
            }
            else
            {
                Log.Add($"Слушатель уже запущен");
            }


        });

        private Dispatcher Dispatcher { get; }

        public Reciver()
        {
            this.Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public ICommand StopReciveCommand => new ActionCommand(() =>
        {
            this.UdpReciver.StopRecive();
            UdpReciver.UdpRecived -= UdpReciver_UdpRecived;
            Log.Add($"Stop recive");
            
        });

        public ICommand ClearCommand => new ActionCommand(() =>
        {
            this.Log.Clear();
        });

        private void UdpReciver_UdpRecived(object? sender, byte[] e)
        {
            Dispatcher.Invoke(() =>
            {
                Log.Add($"{DateTime.Now.Millisecond} mls: packet {e.Length} length");
            });
        }
    }
}
