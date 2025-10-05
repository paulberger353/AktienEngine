using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AktienEngine.Model.CrashOrBoom
{
    public class COBScoreboard
    {
        private string path;    //Pfad zum speichern des Scoreboard
        private List<HighscoreEintrag> highscorelist;   //Scoreboard

        /// <summary>
        /// Konstruktor für die Klasse Scoreboard
        /// Stellt zunächst den Pfad zusammen und überprüft, ob die Datei existiert.
        /// Falls ja, werden Daten eingelesen und lokal gespeichert
        /// </summary>
        public COBScoreboard()
        {
            //Stelle dir Pfad zusammen und hole dir Scoreboard falls existent
            path = GetPath();
            highscorelist = GetHighscore(path);
        }

        /// <summary>
        /// Methode stellt den Pfad zur Scoreboard Datei zusammen
        /// </summary>
        /// <returns>Pfad zur Scoreboard Datei</returns>
        private string GetPath()
        {
            //Hole dir den Pfad zur Scoreboard Datei
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, ".AktienEngine\\COB_Highscore.txt");

            //Gib ihn zurück            
            return path;
        }

        /// <summary>
        /// Methode holt sich Anhand des Pfades die Highscoreliste.
        /// Wenn die Datei nicht existiert oder ein Fehler auftritt, wird null zurückgegeben
        /// </summary>
        /// <param name="path">Pfad in der die Highscoreliste liegt (liegen sollte)</param>
        /// <returns>Scoreboard oder null</returns>
        private List<HighscoreEintrag> GetHighscore(string path)
        {
            //Liste initialisieren
            List<HighscoreEintrag> highscore = new List<HighscoreEintrag>();

            //Existiert die Datei?
            if (!File.Exists(path))
            {
                //Nein, gib leeres Scoreboard zurück
                return highscore;
            }

            try
            {
                //StreamReader initialisieren und anfangen Datei zu lesen
                StreamReader sr = new StreamReader(path);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Teile die Zeile am Semikolon, und füg den Eintrag in die Liste
                    var teile = line.Split(';');

                    highscore.Add(new HighscoreEintrag(teile[0], teile[1]));
                }
                sr.Close();

                //Alles hat geklappt, gib die Highscoreliste zurück
                return highscore;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   //Fehlermeldung loggen

                //Falls ein Fehler beim einlesen auftritt, gib das Scoreboard zum aktuellen Stand zurück
                return highscorelist;
            }
        }

        /// <summary>
        /// Methode ordner ein Scoreboard und gibt es zurück, falls kein Fehler kommt
        /// </summary>
        /// <param name="highscorelist_old">Ungeordnetes Scoreboard</param>
        /// <returns>Geordnetes Scorboard</returns>
        private List<HighscoreEintrag> OrderScoreboard(List<HighscoreEintrag> highscorelist_unordered)
        {
            //Wenn es kein Scoreboard gibt, übergib ein leeres/ neues
            if (highscorelist_unordered == null || highscorelist_unordered.Count == 0)
            {
                return new List<HighscoreEintrag>();
            }

            try
            {
                //Sortiere übergebene Highscoreliste nach dem Kontostand und gib sie zurück 
                highscorelist_unordered.Sort((a, b) => Convert.ToInt32(b.Kontostand).CompareTo(Convert.ToInt32(a.Kontostand)));

                return highscorelist_unordered;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   //Fehlermeldung loggen

                //Bei Fehler altes/ungeordnetes Scoreboard zurückgeben
                return highscorelist;
            }
        }

        /// <summary>
        /// Methode schreibt das Scoreboard in die Datei, egal ob alt oder neu
        /// </summary>
        /// <param name="highscore">Highscorelist die in die Datei geschrieben wird</param>
        private void WriteScoreboard(List<HighscoreEintrag> highscore)
        {
            //Falls die Datei/Unterordner nicht existieren, lege sie an
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            //StreamWriter initialisieren und anfangen Datei zu schreiben
            StreamWriter sw = new StreamWriter(path, false);

            foreach (var eintrag in highscore)
            {
                //Schreibe jeden Eintrag in die Datei
                sw.WriteLine($"{eintrag.Zeitpunkt};{eintrag.Kontostand}");
            }

            //Scoreboard ist fertig geschrieben, beende StreamWriter
            sw.Close();
        }


        /// <summary>
        /// Methode wird aufgerufen wenn das Programm geschlossen wird.
        /// Speichert Scoreboard neu, falls es ein Fehler gibt wird das alte beibehalten.
        /// </summary>
        /// <param name="newScoreboard">Liste des neuen Scoreboardes</param>
        public void SetScoreboard()
        {
            try
            {
                //Highscoreliste in Datei schreiben
                WriteScoreboard(OrderScoreboard(highscorelist));
            }
            catch (Exception e)
            {
                //Fehlermeldung loggen wenn Fehlerauftritt
                Console.WriteLine(e.Message);  
            }
        }

        /// <summary>
        /// Methode gibt ein geordnetes Scoreboard zurück, auch wenn es unbefüllt ist.
        /// </summary>
        /// <returns>Geordnetes Scoreboard</returns>
        public List<HighscoreEintrag> GetScoreboard()
        {
            return OrderScoreboard(highscorelist);
        }

        /// <summary>
        /// Methode fügt einen neuen Score zum Highscoreboard hinzu
        /// </summary>
        /// <param name="zeitpunkt">Datetime des Scores</param>
        /// <param name="score">Erzielter Kontostand</param>
        /// <returns>Ergänztes und geordnetes Highscoreboard</returns>
        public List<HighscoreEintrag> AddToScoreboard(string score)
        {
            string zeitpunkt = DateTime.Now.ToString("dd.MM.yy  HH:mm");
           
            //Überprüfe ob ein Scoreboard existiert
            if (highscorelist == null)
            {
                //Falls nicht erstelle es neu mit dem Eintrag, und gib es zurück
                return new List<HighscoreEintrag>()
                {
                    new HighscoreEintrag(zeitpunkt, score)
                };
            }
            else
            {
                //Füge Daten an Scoreboard an, und gib geordnetes zurück
                highscorelist.Add(new HighscoreEintrag(zeitpunkt, score));
                
                return OrderScoreboard(highscorelist);
            }
        }
    }
}
