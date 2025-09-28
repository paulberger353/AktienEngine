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

        public ICommand BackCommand { get; }    //Button um zurück zum Launcher zu kommen
        private readonly VMMainWindow mainVM;   //Instanz des MainWindow um es später zu laden
        private NGScoreboard sb;        //Instanz des Scoreboards 

        /// <summary>
        /// Konstruktor der VMNewsGame Klasse
        /// Merkt sich das LauncherWindow und initialisiert den Button um zurück zu kommen
        /// </summary>
        /// <param name="_mainVM">Aktuelle Instanz des Launchers</param>
        public VMNewsTrader(VMMainWindow _mainVM)
        {
            //LauncherWindow merken
            mainVM = _mainVM;

            //Scoreboard initialisieren und abbilden
            sb = new NGScoreboard();
            ShowScoreboard();

            BackCommand = new RelayCommand(_ =>
            {
                //Spielstand speichern
                sb.SetScoreboard();

                //Zurück zum Launcher
                mainVM.CurrentGame = _mainVM;
            });
        }

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
