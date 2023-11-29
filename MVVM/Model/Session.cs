using Audio_Interface.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;

namespace Audio_Interface.MVVM.Model
{
    public class Session : BaseViewModel, IPlayback
    {
        #region Fields
        private string _Name;
        private double _volume;
        private uint _pid;
        #endregion

        #region Porperties
        public string Name
        {
            get => _Name;
            set
            {
                Set(ref _Name, value);
            }
        }

        public double Volume
        {
            get => _volume;
            set
            {
                float vol = (float)value;
                Set(ref _volume, value * 100);
                ChangeVolume(vol);
            }
        }

        public uint PID
        {
            get => _pid;
            set
            {
                Set(ref _pid, value);
            }
        }
        #endregion

        #region Constructor
        public Session(string name, double volume, uint pid)
        {
            Name = name;
            Volume = volume;
            PID = pid;
        }
        #endregion

        #region Methods
        public void ChangeVolume(float newVolume)
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            SessionCollection sessions = device.AudioSessionManager.Sessions;

            for (int i = 0; i < sessions.Count; i++)
            {
                if (sessions[i].GetProcessID == PID)
                {
                    sessions[i].SimpleAudioVolume.Volume = newVolume;
                }
            }
        }
        #endregion
    }
}