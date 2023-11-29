using NAudio.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio_Interface.MVVM.Model.Equalizer
{
    public class BetterEqualizerBand : EqualizerBand
    {
        public BetterEqualizerBand() { }

        public EqualizerBand[] Bands
        {
            get;
            set;
        }

        public string Name { get; set; }

        public bool IsEditable { get; set; }
    }
}
