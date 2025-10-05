using AktienEngine.Model.CrashOrBoom;
using AktienEngine.Model.Helper;
using AktienEngine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AktienEngine.ViewModel
{
    public class VMCrashOrBoom : INotifyPropertyChanged
    {
        //Methode und Propertys zum aktualisieren der View
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand BackToLauncher { get; }    //Button um zurück zum Launcher zu kommen

        private readonly VMMainWindow mainVM;  //Instanz des MainWindow um es später zu laden

        private COBScoreboard sb;


        /// <summary>
        /// Konstruktor der VMCrashOrBoom Klasse
        /// Merkt sich das LauncherWindow und initialisiert den Button um zurück zu kommen
        /// </summary>
        /// <param name="_mainVM">Aktuelle Instanz des Launchers</param>
        public VMCrashOrBoom(VMMainWindow _mainVM)
        {
            //LauncherWindow merken
            mainVM = _mainVM;

            BackToLauncher = new RelayCommand(_ =>
            {
                //Spielstand speichern
                sb.SetScoreboard();

                //Zurück zum Launcher
                mainVM.CurrentGame = _mainVM;
            });

            //Scoreboard initialisieren
            sb = new COBScoreboard();

            //Setze Standartwerte
            _labKontostand = "50000";
            _labOffenePositionen = "0";

            ShowScoreboard();
            RaisePropertyChanged(nameof(Highscorelist));
            RaisePropertyChanged(nameof(LabKontostand));
            RaisePropertyChanged(nameof(LabOffenePositionen));
        }

        /// <summary>
        /// Methode wird nach dem Starten oder dem einfügen von ienem neuen HIghscore aufgerufen.
        /// Zeigt das geordnete Scoreboard in der View an
        /// </summary>
        private void ShowScoreboard()
        {
            _highscorelist = new ObservableCollection<HighscoreEintrag>(sb.GetScoreboard());
            RaisePropertyChanged(nameof(Highscorelist));
        }

        #region Bindings
        private string _labKontostand;
        public string LabKontostand
        {
            get { return $"{_labKontostand} €"; }
            set
            {
                if (_labKontostand == value) { return; }

                _labKontostand = value;
                RaisePropertyChanged();
            }
        }

        private string _labOffenePositionen;
        public string LabOffenePositionen
        {
            get { return $"{_labOffenePositionen} €"; }
            set
            {
                if (_labOffenePositionen == value) { return; }

                _labOffenePositionen = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<HighscoreEintrag> _highscorelist;
        public ObservableCollection<HighscoreEintrag> Highscorelist
        {
            get 
            {
                return _highscorelist;
            }
            set
            {
                if (_highscorelist == value) { return;  }
                {
                    _highscorelist = value;
                    RaisePropertyChanged(nameof(Highscorelist));
                }   
            }
        }

        #endregion
    }
}
