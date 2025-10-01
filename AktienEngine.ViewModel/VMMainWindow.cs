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

        /// <summary>
        /// Konstruktor des VMMainWindow
        /// Initialisiert die Commands der Buttons und setzt Standartwerte fest
        /// </summary>
        public VMMainWindow()
        {
            //Initialisierung der Commands
            SelectCOB = new RelayCommand(_ => CurrentGame = new VMCrashOrBoom(this));
            SelectNG = new RelayCommand(_ => CurrentGame = new VMNewsTrader(this));
            SelectBOB = new RelayCommand(_ => CurrentGame = new VMBullOrBear(this));
            SelectImageCommand = new RelayCommand(param =>
            {
                //Wandle Tag in Index um
                if (int.TryParse(param?.ToString(), out int index))
                {
                    //Klickt er COB?
                    if (index == 0)
                    {
                        _labGametitel = "Crash or Boom";

                        _labGamebeschreibung = "Stell dich dem riskanten Markt und triff clevere Entscheidungen, " +
                                                "kauf oder verkauf, bevor ein Crash dein Kapital zerstört – nur wer klug abwägt, kann richtig wachsen.";
                    }
                    else if (index == 1)
                    {
                        _labGametitel = "News Trader";

                        _labGamebeschreibung = "Verwalte fünf Aktien, reagiere schnell auf neue News und stell die Schwierigkeit ein, " +
                                                "um dein Vermögen effektiv zu steigern. Schaffst du es, die News zu meistern und dein Kapital zu maximieren?";
                    }
                    else if (index == 2)
                    {
                        _labGametitel = "Bull or Bear";

                        _labGamebeschreibung = "Setz auf steigende oder fallende Kurse oder leg einen Bereich fest, in dem du den Markt siehst. " +
                                                "Dann heißt es nur noch zuschauen, wie dein Kapital wächst oder schwindet – pure Spannung!";
                    }

                    RaisePropertyChanged(nameof(LabGametitel));
                    RaisePropertyChanged(nameof(LabGamebeschreibung));
                }
            });

            SelectGame = new RelayCommand(_ => 
            {
                //Gucke nach dem ersten buchstaben vom gametitel
                if (_labGametitel.Substring(0, 1) == "C")
                {
                    SelectCOB.Execute(null);
                }
                else if (_labGametitel.Substring(0, 1) == "N")
                {
                    SelectNG.Execute(null);
                }
                else if (_labGametitel.Substring(0, 1) == "B")
                {
                    SelectBOB.Execute(null);
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

        # region Commands für die Buttons
        public ICommand SelectGame { get; }
        public ICommand SelectCOB { get; }
        public ICommand SelectNG { get; }
        public ICommand SelectBOB { get; }
        public ICommand SelectImageCommand { get; }
        #endregion

        #region Binding Variablen
        private string _labGametitel;
        public string LabGametitel
        {
            get
            {
                if (string.IsNullOrEmpty(_labGametitel))
                    return string.Empty;

                var sb = new StringBuilder();
                foreach (char c in _labGametitel)
                    sb.Append(c).Append(' ');

                return sb.ToString().TrimEnd();
            }
            set
            {
                if (_labGametitel == value) return;
                _labGametitel = value;
                RaisePropertyChanged();
            }
        }
               
        private string _labGamebeschreibung;
        public string LabGamebeschreibung
        {
            get { return _labGamebeschreibung; }
            set
            {
                if (_labGamebeschreibung == value) { return; }
                
                _labGamebeschreibung = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}
