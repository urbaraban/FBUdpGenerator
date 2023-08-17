using FBUdpGenerator;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Input;
using System.Windows.Threading;

namespace FBUdpGeneratorAPP.ViewModel
{
    public class Reciver : NotifyModel
    {
        public string Address { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 11000;
        public bool IsReciving => UdpReciver.IsReciving;

        public int ReciverBufferSize
        {
            get => buffersize;
            set
            {
                buffersize = value;
            }
        }
        private int buffersize = 8192;

        public int PacketsCommonCount
        {
            get => packetscommoncount;
            set
            {
                packetscommoncount = value;
                OnPropertyChanged(nameof(PacketsCommonCount));
            }
        }
        private int packetscommoncount;
        public int PacketsLossCount
        {
            get => packetslosscount;
            set
            {
                packetslosscount = value;
                OnPropertyChanged(nameof(PacketsLossCount));
            }
        }
        private int packetslosscount;
        public int BytesCount
        {
            get => bytescount;
            set
            {
                bytescount = value;
                OnPropertyChanged(nameof(BytesCount));
            }
        }
        private int bytescount;

        public ObservableCollection<string> Log { get; } = new ObservableCollection<string>();

        private UdpReciver UdpReciver { get; set; } = new UdpReciver();

        private Dispatcher Dispatcher { get; }

        public Reciver()
        {
            this.Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public ICommand StartReciveCommand => new ActionCommand(() =>
        {
            if (this.UdpReciver.IsReciving == false)
            {
                try
                {
                    Log.Add($"Слушатель запущен на {Address}:{Port}");
                    this.UdpReciver.StartRecive(this.Port, buffersize);
                }
                catch
                {
                    Log.Add($"Abort recive");
                }
                finally
                {
                    this.UdpReciver.UdpRecived += UdpReciver_UdpRecived;
                }
            }
            else
            {
                Log.Add($"Слушатель уже запущен");
            }
        });

        public ICommand StopReciveCommand => new ActionCommand(() =>
        {
            this.UdpReciver.StopRecive();
            this.UdpReciver.UdpRecived -= UdpReciver_UdpRecived;
            this.Log.Add($"Слушатель остановлен");
            
        });

        public ICommand ClearCommand => new ActionCommand(() =>
        {
            this.Log.Clear();
            this.BytesCount = 0;
            this.PacketsCommonCount = 0;
            this.PacketsLossCount = 0;
        });

        private void UdpReciver_UdpRecived(object? sender, (string, byte[]) e)
        {
            Dispatcher.Invoke(() =>
            {
                this.PacketsCommonCount += 1;
                if (e.Item1 == this.Address || string.IsNullOrEmpty(this.Address) == true)
                {
                    this.Log.Add($"{DateTime.Now.Millisecond} mls: Приняли {e.Item2.Length} байт");
                    this.BytesCount += e.Item2.Length;
                }
                else
                {
                    this.PacketsLossCount += 1;
                }
            });
        }
    }
}
