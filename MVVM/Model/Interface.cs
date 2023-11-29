using Audio_Interface.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio_Interface.MVVM.Model
{
    public class Interface : BaseViewModel
    {
        #region Fields
        private int _firstSlider;
        private int _secondSlider;
        private int _thirdSlider;
        private int _fourthSlider;
        private int _fifthSlider;
        #endregion

        #region Properties
        public int FirstSlider
        {
            get => _firstSlider;
            set => Set(ref _firstSlider, value);
        }

        public int SecondSlider
        {
            get => _secondSlider;
            set => Set(ref _secondSlider, value);
        }

        public int ThirdSlider
        {
            get => _thirdSlider;
            set => Set(ref _thirdSlider, value);
        }

        public int FourthSlider
        {
            get => _fourthSlider;
            set => Set(ref _fourthSlider, value);
        }

        public int FifthSlider
        {
            get => _fifthSlider;
            set => Set(ref _fifthSlider, value);
        }
        #endregion
    }
}
