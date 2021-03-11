using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace TennisScoreBoard.App.ViewModel
{
    public class MainBoardViewModel : ViewModelBase
    { 
        public int test { get; set; }
         
        private void notify()   
        {
            RaisePropertyChanged(() => test);
        }

    }
}
