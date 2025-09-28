using AktienEngine.Model.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AktienEngine.ViewModel
{
    public class VMMainWindow : INotifyPropertyChanged
    {
        //Methode und Propertys zum aktualisieren der View
        public event PropertyChangedEventHandler? PropertyChanged;         
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public VMMainWindow()
        {
            //Initialisierung der Commands
            SelectCOB = new RelayCommand(_ => CurrentGame = new VMCrashOrBoom(this));
            SelectNG = new RelayCommand(_ => CurrentGame = new VMNewsTrader(this));

            //Launcher als Standard-Window
            CurrentGame = this;
        }

        private object _currentGame;
        public object CurrentGame
        {
            get => _currentGame;
            set
            {
                _currentGame = value;
                RaisePropertyChanged();
            }
        }

        // Commands für die Buttons
        public ICommand SelectCOB { get; }
        public ICommand SelectNG { get; }



        /**private string _labText;

        //public string LabText
        //{
        //    get => _labText;
        //    set
        //    {
        //        if (_labText != value)
        //        {
        //            _labText = value;
        //            RaisePropertyChanged(); // meldet Änderung an die View
        //        }
        //    }
        }**/
    }
}
