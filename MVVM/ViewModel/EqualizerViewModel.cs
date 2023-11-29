using Audio_Interface.Commands;
using Audio_Interface.MVVM.Model.Equalizer;
using NAudio.CoreAudioApi;
using NAudio.Extras;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Audio_Interface.MVVM.ViewModel
{
    public class EqualizerViewModel : BaseViewModel
    {
        public EqualizerViewModel()
        {
            //EqualizerCommand = new RelayCommand(EqualizerCommander, CanExecute);
            this.PropertyChanged += OnPropertyChanged;
        }

        //private string[] commandList = { "Add", "New", "Edit", "Remove" };

        private List<BetterEqualizerBand> _bands;
        public Equalizer equalizer;
        private BetterEqualizerBand _selectedEqualizer;

        public float MinimumGain { get; } = -12;
        public float MaximumGain { get; } = 12;

        public List<BetterEqualizerBand> Bands
        {
            get => _bands;
            set => Set(ref _bands, value);
        }

        public RelayCommand EqualizerCommand { get; set; }

        public BetterEqualizerBand SelectedEqualizer
        {
            get => _selectedEqualizer;
            set => Set(ref _selectedEqualizer, value);
        }

        public string EqualizerName { get; set; }

        public EqualizerBand[] SliderValues { get; set; } = new EqualizerBand[10];

        #region Methods
        //private void LoadEqualizer(ref ObservableCollection<EqualizerViewModel> liste)
        //{
        //    if (File.Exists("Equalizer.xml"))
        //    {
        //        FileStream fs = new FileStream("Equalizer.xml", FileMode.Open);
        //        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Equalizer>));
        //        var tempListe = (ObservableCollection<Equalizer>)serializer.Deserialize(fs);
        //        foreach (var item in tempListe)
        //        {
        //            liste.Add(new EqualizerViewModel(item));
        //        }
        //    }
        //}

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            equalizer = new Equalizer(ToSample(), SelectedEqualizer.Bands);
            equalizer?.Update();
        }

        private ISampleProvider ToSample()
        {
            var enumerator = new MMDeviceEnumerator();
            var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
            var capture = new WasapiOut(device, AudioClientShareMode.Shared, false, 100);
            return capture as ISampleProvider;
        }
        #endregion

        #region Commander and CanExecute
        //private void EqualizerCommander(object parameter)
        //{
        //    string command = parameter as string;
        //    switch (command)
        //    {
        //        case "Add":
        //            Bands.Add(new BetterEqualizerBand() { Bands = SliderValues, Name = EqualizerName, IsEditable = true });
        //            RaisePropertyChanged(nameof(Bands));
        //            break;
        //        case "Edit":
        //            SelectedEqualizer.Bands = SliderValues;
        //            SelectedEqualizer.Name = EqualizerName;
        //            RaisePropertyChanged(nameof(Bands));
        //            break;
        //        case "Remove":
        //            Bands.Remove(SelectedEqualizer);
        //            RaisePropertyChanged(nameof(Bands));
        //            break;
        //    }
        //}

        //private bool CanExecute(object parameter) => SelectedEqualizer.IsEditable;
        #endregion
    }
}