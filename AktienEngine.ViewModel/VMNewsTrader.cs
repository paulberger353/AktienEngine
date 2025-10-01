using AktienEngine.Model.Helper;
using AktienEngine.Model.NewsTrader;
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
    public class VMNewsTrader : INotifyPropertyChanged
    {
        //Methode und Propertys zum aktualisieren der View
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Konstruktor der VMNewsTrader Klasse
        /// Merkt sich das LauncherWindow und initialisiert die Buttons
        /// </summary>
        /// <param name="_mainVM">Aktuelle Instanz des Launchers</param>
        public VMNewsTrader(VMMainWindow _mainVM)
        {
            //LauncherWindow merken
            mainVM = _mainVM;

            //Buttons initialisieren
            InitializeButtons(_mainVM);

            //Scoreboard initialisieren und abbilden
            sb = new NGScoreboard();
            ShowScoreboard();
        }

        /// <summary>
        /// Methode wird vom Konstruktor aufgerufen, um die Buttons zu initialisieren
        /// </summary>
        private void InitializeButtons(VMMainWindow _mainVM)
        {
            BackCommand = new RelayCommand(_ =>
            {
                //Spielstand speichern
                sb.SetScoreboard();

                //Zurück zum Launcher
                mainVM.CurrentGame = _mainVM;
            });
        }

        #region Propertys und Commands (Buttons)
        public ICommand BackCommand { get; set; }   //Button um zurück zum Launcher zu kommen
        
        private readonly VMMainWindow mainVM;       //Instanz des MainWindow um es später zu laden
        private NGScoreboard sb;                    //Instanz des Scoreboards 

        #endregion


        /// <summary>
        /// Methode wird nach dem Starten oder dem einfügen von ienem neuen HIghscore aufgerufen.
        /// Zeigt das geordnete Scoreboard in der View an
        /// </summary>
        private void ShowScoreboard()
        {
            //Hole dir das aktuelle scoreboard
            List<(DateTime zeitpunkt, int kontostand)> currentSB = sb.GetScoreboard();
        }
    }
}
