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
            SelectImageCommand = new RelayCommand(param =>
            {
                if (int.TryParse(param?.ToString(), out int index))
                {
                    //Klickt er COB?
                    if (index == 0)
                    {
                        _labGametitel = "Crash or Boom";
                    }
                    else if (index == 1)
                    {
                        _labGametitel = "News Trader";
                    }
                    else if (index == 2)
                    {
                        //Spiel nicht vorhanden
                    }

                    RaisePropertyChanged(nameof(LabGametitel));
                }
            });


            //Launcher als Standard-Window und COB als Standart-Spiel
            CurrentGame = this;
            SelectImageCommand.Execute(0);
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
        public ICommand SelectImageCommand { get; }

        // Property für den Titel des Spiels
        private string _labGametitel;
        public string LabGametitel
        {
            get => _labGametitel;
            set
            {
                if (_labGametitel != value)
                {
                    _labGametitel = value;
                    RaisePropertyChanged(); // meldet Änderung an die View
                }
            }
        }
}
}
