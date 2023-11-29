using Audio_Interface.MVVM.Model;
using Audio_Interface.MVVM.View;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO.Ports;
using System.Management;
using System.Windows;
using System.Xml.Linq;
using NAudio.CoreAudioApi;
using System.IO;
using System.Threading;
using NAudio.Extras;
using System.Windows.Threading;

namespace Audio_Interface.MVVM.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties
        public EqualizerViewModel EQVM { get; set; } = new EqualizerViewModel();

        public InterfaceViewModel IFVM { get; set; } = new InterfaceViewModel();
        #endregion
    }
}