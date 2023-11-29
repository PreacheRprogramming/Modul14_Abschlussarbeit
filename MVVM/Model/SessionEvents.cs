using System.Threading;
using Audio_Interface.MVVM.ViewModel;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Threading;
using System.ComponentModel;

namespace Audio_Interface.MVVM.Model
{
    internal class SessionEvents : BaseViewModel, INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Session> _sessions;
        #endregion

        #region Properties
        public int ActiveSessions { get; private set; }

        public ObservableCollection<Session> Sessions
        {
            get => _sessions;
            set
            {
                Set(ref _sessions, value);
                ActiveSessions = value.Count;
            }
        }
        #endregion

        #region Constructor
        public SessionEvents(ObservableCollection<Session> sess)
        {
            Sessions = sess;
            Task.Run(() => SessionActivatedHandler());
            Task.Run(() => SessionDeactivatedHandler());
        }
        #endregion

        #region Methods
        private async void SessionActivatedHandler()
        {
            while (true)
            {
                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
                MMDevice device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                if (device.AudioSessionManager.Sessions.Count > ActiveSessions)
                {
                    for (int i = 0; i < device.AudioSessionManager.Sessions.Count; i++)
                    {
                        AudioSessionControl session = device.AudioSessionManager.Sessions[i];
                        bool isInList = false;
                        string name = string.Empty;

                        Process p = Process.GetProcessById((int)session.GetProcessID);
                        SimpleAudioVolume vol = session.SimpleAudioVolume;

                        if (device.State == DeviceState.Active)
                        {

                            for (int j = 0; j < Sessions.Count; j++)
                            {
                                if (Sessions[j].PID == session.GetProcessID)
                                {
                                    isInList = true;
                                }
                            }
                            if (!isInList)
                            {
                                if (p.MainWindowTitle != "")
                                    name = p.MainWindowTitle.ToString();
                                else if (session.DisplayName != "")
                                    name = session.DisplayName.ToString();
                                else if (p.ProcessName != "")
                                    name = p.ProcessName.ToString();

                                var sessionActivated = new SessionDataEventArgs
                                {
                                    Name = name,
                                    Volume = vol.Volume,
                                    PID = session.GetProcessID
                                };
                                SessionActivated.Invoke(this, sessionActivated);
                            }
                        }
                    }
                }
                await Task.Delay(1);
            }
        }

        private async void SessionDeactivatedHandler()
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            while (true)
            {
                if (device.AudioSessionManager.Sessions.Count < ActiveSessions)
                {
                    for (int i = 0; i < Sessions.Count; i++)
                    {
                        bool isNotInList = true;
                        for (int j = 0; j < device.AudioSessionManager.Sessions.Count; j++)
                        {
                            AudioSessionControl session = device.AudioSessionManager.Sessions[j];

                            if (Sessions[i].PID == session.GetProcessID)
                            {
                                isNotInList = false;
                                continue;
                            }
                        }
                        if (isNotInList)
                        {
                            var sessionDeactivated = new SessionDataEventArgs
                            {
                                Name = Sessions[i].Name,
                                Volume = Sessions[i].Volume,
                                PID = Sessions[i].PID
                            };
                            SessionDeactivated.Invoke(this, sessionDeactivated);
                        }
                    }
                }
                await Task.Delay(1);
            }
        }
        #endregion

        #region Events
        public event EventHandler<SessionDataEventArgs> SessionActivated;
        public event EventHandler<SessionDataEventArgs> SessionDeactivated;
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
    }

    public class SessionDataEventArgs : EventArgs
    {
        public string Name { get; set; }

        public double Volume { get; set; }

        public uint PID { get; set; }
    }
}
