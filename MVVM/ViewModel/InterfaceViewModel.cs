using Audio_Interface.Commands;
using Audio_Interface.MVVM.Model;
using Audio_Interface.MVVM.View;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Audio_Interface.MVVM.ViewModel
{
    public class InterfaceViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Session> _sessions = new ObservableCollection<Session>();
        private Capture _selectedCapture;
        private Session _selectedSessionOne;
        private Session _selectedSessionTwo;
        private Session _selectedSessionThree;
        private Session _selectedSessionFour;
        #endregion

        public Serial serial = new Serial();

        #region Properties
        private SessionEvents SessionEvents { get; set; }

        public ObservableCollection<Session> Sessions
        {
            get => _sessions;
            private set => Set(ref _sessions, value);
        }

        public Capture SelectedCapture
        {
            get => _selectedCapture;
            set => Set(ref _selectedCapture, value);
        }

        public Session SelectedSessionOne
        {
            get => _selectedSessionOne;
            set
            {
                if ((value == SelectedSessionTwo ||
                    value == SelectedSessionThree ||
                    value == SelectedSessionFour) &&
                    value != null)
                    value = null;

                Set(ref _selectedSessionOne, value);
            }
        }

        public Session SelectedSessionTwo
        {
            get => _selectedSessionTwo;
            set
            {
                if ((value == SelectedSessionOne ||
                    value == SelectedSessionThree ||
                    value == SelectedSessionFour) &&
                    value != null)
                    value = null;

                Set(ref _selectedSessionTwo, value);
            }
        }

        public Session SelectedSessionThree
        {
            get => _selectedSessionThree;
            set
            {
                if ((value == SelectedSessionTwo ||
                    value == SelectedSessionThree ||
                    value == SelectedSessionFour) &&
                    value != null)
                    value = null;

                Set(ref _selectedSessionThree, value);
            }
        }

        public Session SelectedSessionFour
        {
            get => _selectedSessionFour;
            set
            {
                if ((value == SelectedSessionOne ||
                    value == SelectedSessionTwo ||
                    value == SelectedSessionThree) &&
                    value != null)
                    value = null;

                Set(ref _selectedSessionFour, value);
            }
        }

        public RelayCommand Equalizer => new RelayCommand(ExecuteEqualizer);

        public RelayCommand Minimize => new RelayCommand(ExecuteMinimize);

        public RelayCommand Close => new RelayCommand(ExecuteClose);
        #endregion

        #region Constructor
        public InterfaceViewModel()
        {
            AddToSession(ref _sessions);
            //SessionEvents = new SessionEvents(Sessions);
            //SessionEvents.SessionActivated += SessionActivated;
            //SessionEvents.SessionDeactivated += SessionDeactivated;

            serial.OpenConnection();
            serial.SerialReceived += SerialReceivedHandler;
        }
        #endregion

        #region Methods
        private void ExecuteEqualizer(object parameter)
        {
            EqualizerView eqv = new EqualizerView();
            eqv.Show();
        }

        private void ExecuteMinimize(object parameter)
        {
            var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            activeWindow.WindowState = WindowState.Minimized;
        }

        private void ExecuteClose(object parameter)
        {
            var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            activeWindow.Close();
        }

        private static void AddToSession(ref ObservableCollection<Session> list)
        {
            MMDeviceEnumerator _devEnum = new MMDeviceEnumerator();
            MMDevice _devices = _devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            SessionCollection sessions = _devices.AudioSessionManager.Sessions;

            for (int i = 0; i < sessions.Count; i++)
            {
                string name = string.Empty;
                Process p = Process.GetProcessById((int)sessions[i].GetProcessID);
                SimpleAudioVolume vol = sessions[i].SimpleAudioVolume;

                if (_devices.State == DeviceState.Active)
                {
                    if (p.ProcessName != "")
                        name = p.ProcessName.ToString();
                    else if (p.MainWindowTitle != "")
                        name = p.MainWindowTitle.ToString();
                    else if (sessions[i].DisplayName != "")
                        name = sessions[i].DisplayName.ToString();
                }

                float volume = vol.Volume;
                uint pid = sessions[i].GetProcessID;
                list.Add(new Session(name, volume, pid));
            }
        }

        private void SessionActivated(object sender, SessionDataEventArgs e)
        {
            Sessions.Add(new Session(e.Name, e.Volume, e.PID));

            SessionEvents.Sessions = Sessions;
        }

        private void SessionDeactivated(object? sender, SessionDataEventArgs e)
        {
            for (int i = 0; i < Sessions.Count; i++)
            {
                if (Sessions[i].PID == e.PID)
                {
                    Sessions.Remove(Sessions[i]);
                }
            }
            SessionEvents.Sessions = Sessions;
        }
        #endregion

        #region EventHandler
        private void SerialReceivedHandler(object sender, SerialReceivedEventArgs e)
        {
            if (e.Line == null)
                return;

            string? input = e.Line;

            string[] strData = input.Split('|');

            if (strData.Length != 5)
                return;

            double[] doubleData = new double[5];
            for (int i = 0; i < strData.Length; i++)
            {
                doubleData[i] = Convert.ToDouble(strData[i]);
            }
            //if (SelectedCapture != null)
            //    SelectedCapture.Volume = doubleData[0] / 100;
            if (SelectedSessionOne != null)
                SelectedSessionOne.Volume = doubleData[1] / 100;
            if (SelectedSessionTwo != null)
                SelectedSessionTwo.Volume = doubleData[2] / 100;
            if (SelectedSessionThree != null)
                SelectedSessionThree.Volume = doubleData[3] / 100;
            if (SelectedSessionFour != null)
                SelectedSessionFour.Volume = doubleData[4] / 100;
        }
        #endregion
    }
}