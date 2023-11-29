using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio_Interface.MVVM.Model
{
    public class SessionCollection
    {
        #region Fields
        private double volume;
        private ObservableCollection<Session> sessions = new ObservableCollection<Session>();
        #endregion

        #region Properties
        public string? Name { get; set; }

        public double Volume
        {
            get => volume;
            set
            {
                float vol = (float)value;
                volume = value * 100;
                SetVolume(vol);
            }
        }

        public ObservableCollection<Session> Sessions
        {
            get => sessions;
            set
            {
                sessions = value;
                if(Name == null && Sessions.Count == 1)
                {
                    Name = Sessions[0].Name;
                }
            }
        }
        #endregion

        #region Methods
        public void SetVolume(float volume)
        {
            for (int i = 0; i < Sessions.Count; i++)
            {
                Sessions[i].Volume = volume;
            }
        }
        #endregion
    }
}
