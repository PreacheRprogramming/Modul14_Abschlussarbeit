//using NAudio.CoreAudioApi;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Audio_Interface.MVVM.Model
//{
//    public class SessionObservableCollection<T> : ObservableCollection<T>
//    {
//        public event EventHandler<SessionDataEventArgs> SessionActivated;
//        public event EventHandler<SessionDataEventArgs> SessionDeactivated;

//        public int ActiveSessions { get; private set; }

//        private void SessionActivatedHandler()
//        {
//            while (true)
//            {
//                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
//                MMDevice device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

//                if (device.AudioSessionManager.Sessions.Count > ActiveSessions)
//                {
//                    for (int i = 0; i < device.AudioSessionManager.Sessions.Count; i++)
//                    {
//                        AudioSessionControl session = device.AudioSessionManager.Sessions[i];
//                        bool isInList = false;
//                        string name = string.Empty;

//                        Process p = Process.GetProcessById((int)session.GetProcessID);
//                        SimpleAudioVolume vol = session.SimpleAudioVolume;

//                        if (device.State == DeviceState.Active)
//                        {
//                            foreach (var item in Sessions)
//                            {
//                                if (item.PID == session.GetProcessID)
//                                {
//                                    isInList = true;
//                                }
//                            }
//                            if (!isInList)
//                            {
//                                if (p.MainWindowTitle != "")
//                                    name = p.MainWindowTitle.ToString();
//                                else if (session.DisplayName != "")
//                                    name = session.DisplayName.ToString();
//                                else if (p.ProcessName != "")
//                                    name = p.ProcessName.ToString();

//                                var sessionActivated = new SessionDataEventArgs
//                                {
//                                    SessionName = name,
//                                    Volume = vol.Volume,
//                                    PID = session.GetProcessID
//                                };
//                                SessionActivated.Invoke(this, sessionActivated);
//                                ActiveSessions++;
//                            }
//                        }
//                    }
//                    Thread.Sleep(1);
//                }
//            }
//        }

//        private void SessionDeactivatedHandler()
//        {
//            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
//            MMDevice device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

//            while (true)
//            {
//                if (device.AudioSessionManager.Sessions.Count < ActiveSessions)
//                {
//                    for (int i = 0; i < Sessions.Count; i++)
//                    {
//                        bool isNotInList = true;
//                        for (int j = 0; j < device.AudioSessionManager.Sessions.Count; j++)
//                        {
//                            AudioSessionControl session = device.AudioSessionManager.Sessions[j];

//                            if (Sessions[i].PID == session.GetProcessID)
//                            {
//                                isNotInList = false;
//                                continue;
//                            }
//                        }
//                        if (isNotInList)
//                        {
//                            var sessionDeactivated = new SessionDataEventArgs
//                            {
//                                SessionName = Sessions[i].SessionName,
//                                Volume = Sessions[i].Volume,
//                                PID = Sessions[i].PID
//                            };
//                            SessionDeactivated.Invoke(this, sessionDeactivated);
//                            ActiveSessions--;
//                        }
//                    }
//                }
//                Thread.Sleep(1);
//            }
//        }
//    }

//    public class SessionDataEventArgs : EventArgs
//    {
//        public string SessionName { get; set; }

//        public float Volume { get; set; }

//        public uint PID { get; set; }
//    }
//}
