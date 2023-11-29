using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Audio_Interface.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für Equalizer.xaml
    /// </summary>

    public partial class EqualizerView : Window
    {
        MVVM.ViewModel.MainViewModel? vm;
        public EqualizerView()
        {
            InitializeComponent();
            vm = DataContext as MVVM.ViewModel.MainViewModel;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
