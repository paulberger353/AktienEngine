using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AktienEngine.Model
{
    public class HighscoreEintrag
    {
        public HighscoreEintrag(string _zeitpunkt, string _kontostand) 
        {
            //Zeitpunkt und Kontostand lokal
            Zeitpunkt = _zeitpunkt;
            Kontostand = _kontostand;
        }

        /// <summary>
        /// Getter/Setter für den Zeitpunkt und den Kontostand des Eintrags
        /// </summary>
        public string Zeitpunkt { get; set; }
        public string Kontostand { get; set; }
    }

}
