﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FBUdpGeneratorAPP.ViewModel
{
    public abstract class NotifyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
