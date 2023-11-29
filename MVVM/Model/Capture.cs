using Audio_Interface.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio_Interface.MVVM.Model
{
    public class Capture : BaseViewModel, IPlayback
    {
        #region Fields
        private string _name;
        private double _volume;
        private uint _pid;
        #endregion
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public double Volume
        {
            get => _volume;
            set
            {
                float vol = (float)value / 100;
                Set(ref _volume, value);
                ChangeVolume(vol);
            }
        }
        public uint PID
        {
            get => _pid;
            set => Set(ref _pid, value);
        }

        public void ChangeVolume(float newVolume)
        {
            throw new NotImplementedException();
        }
    }
}
