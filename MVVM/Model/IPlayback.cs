using Audio_Interface.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio_Interface.MVVM.Model
{
    interface IPlayback
    {
        #region Properties
        string Name { get; set; }

        double Volume { get; set; }

        uint PID { get; set; }
        #endregion

        #region Methods
        public abstract void ChangeVolume(float newVolume);
        #endregion
    }
}
