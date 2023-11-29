using Audio_Interface.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Audio_Interface.MVVM.Model
{
    public class Serial : MVVM.ViewModel.BaseViewModel
    {
        #region Fields
        private SerialPort _serialPort = new SerialPort();
        private int _baudRate = 115200;
        #endregion

        #region Properties
        public string PortName
        {
            get => _serialPort.PortName;
            set
            {
                _serialPort.PortName = value;
            }
        }

        public int BaudRate
        {
            get => _serialPort.BaudRate;
            private set => _serialPort.BaudRate = value;
        }

        public bool IsOpen
        {
            get => _serialPort.IsOpen;
        }
        #endregion

        #region Methods
        public void OpenConnection()
        {
            CloseConnection();

            PortName = GetPort();
            _serialPort.BaudRate = _baudRate;

            try
            {
                _serialPort.Open();
                _serialPort.DiscardInBuffer();
                Task.Run(() => DataReceived());
            }
            catch (Exception)
            {
                MessageBox.Show($"Konnte Verbindung zu {_serialPort.PortName} nicht herstellen!");
            }
        }

        public void CloseConnection()
        {
            if (IsOpen)
                _serialPort.Close();
        }

        static private string GetPort()
        {
            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * from Win32_SerialPort"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                if (device.GetPropertyValue("Description").ToString() == "Silicon Labs CP210x USB to UART Bridge")
                {
                    return device.GetPropertyValue("DeviceID").ToString();
                }
            }
            throw new Exception("No Port found!");
        }

        private async void DataReceived()
        {
            string zeile = "";
            while (true)
            {
                if (_serialPort.BytesToRead > 0)
                {
                    char empfZeichen = (char)_serialPort.ReadChar();
                    if (empfZeichen == '\n')
                    {
                        zeile = zeile.Replace("\r", "");
                        zeile = zeile.Replace("\n", "");

                        var serialReceivedEvArgs = new SerialReceivedEventArgs { Line = zeile };
                        SerialReceived.Invoke(this, serialReceivedEvArgs);

                        _serialPort.DiscardInBuffer();
                        zeile = "";
                    }
                    else
                        zeile += empfZeichen;
                }
                await Task.Delay(1);
            }
        }
        #endregion

        #region Event
        public event EventHandler<SerialReceivedEventArgs>? SerialReceived;
        #endregion

        #region Deconstructor
        ~Serial()
        {
            _serialPort.Close();
            _serialPort.DiscardInBuffer();
            //SerialPort.DataReceived -= DataReceivedHandler;
            _serialPort.Dispose();
        }
        #endregion
    }

    public class SerialReceivedEventArgs : EventArgs
    {
        public string? Line { get; set; }
    }
}
