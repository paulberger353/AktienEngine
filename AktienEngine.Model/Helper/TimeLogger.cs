using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AktienEngine.Model
{
    public class TimeLogger
    {

        //Pfad zur Exceldatei
        private string filePath;

        //DataSet für Exceldatei
        private DataTable dt { get; set; }


        /// <summary>
        /// Konstruktor der Klasse TimeLogger
        /// Initialisiert das DS und füllt es mit
        /// den Daten aus der Exceldatei
        /// </summary>
        public TimeLogger()
        {
            //Hol dir Dateipfad
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TimeLogger.xlsx");

            //DataSet initialisieren mit Exceldatei befüllen
            dt = new DataTable("Zeiten") { Columns = { "Datum", "Stunden", "Arbeit" } };

            return;
            dt = LoadExcelFile();    
        }

        /// <summary>
        /// Methode holt sich Daten aus dem DS 
        /// und printet sie in die Konsole
        /// </summary>
        public void Show()
        {
            bool firstrow = true;

            //Gehe Spalte in jeder Row durch
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    //Erste Row sind Spaltennamen
                    if (firstrow)
                    {
                        Debug.Write(dc.ColumnName + "\t");
                        firstrow = false;
                    }
                    else
                    {
                        Debug.Write(dr[dc] + "\t");
                    }
                }
            }
        }

        /// <summary>
        /// Methode speichert Tätigkeit und  
        /// Stundenanzahl hierzu im DS
        /// </summary>
        /// <param name="taetigkeit"></param>
        /// <param name="laenge"></param>
        public void Add(string taetigkeit, double laenge)
        {
            int rows = dt.Rows.Count;   //Anzahl der aktuellen Rows
            
            //Füge die Werte in die nächste Row ein
            dt.Rows.Add(DateTime.Now.ToString("dd.MM.yyyy"), laenge.ToString(), taetigkeit);
        }

        /// <summary>
        /// Methode wird aufgerufen um die Daten aus der
        /// Exceldatei in das DataSet zu laden
        /// </summary>
        private DataTable LoadExcelFile()
        {
            try
            {
                //Existiert die Datei?
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Excel-Datei nicht gefunden!", filePath, null);
                }


                //Exceldatei öffnen und alle Tabellenblätter durchgehen
                using (var workbook = new XLWorkbook(filePath))
                {
                    foreach (var worksheet in workbook.Worksheets)
                    {
                        //Tabelle im DS erstellen
                        DataTable dt = new DataTable(worksheet.Name);

                        //Erste Zeile sind Spaltennamen, dann kommen Daten
                        bool firstRow = true;
                        foreach (var row in worksheet.RowsUsed())
                        {
                            if (firstRow)
                            {
                                foreach (var cell in row.Cells())
                                    dt.Columns.Add(cell.Value.ToString());
                                firstRow = false;
                            }
                            else
                            {
                                dt.Rows.Add(row.Cells().Select(c => c.Value).ToArray());
                            }
                        }

                        return dt;      //Gebe die erste gefundene Tabelle zurück
                    }
                }

                return null;
            }
            catch (FileNotFoundException e)
            {
                //Logge Fehler in Konsole
                Debug.WriteLine("\t-----------------------------\t");
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.FileName);

                return null;
            } 
            catch (Exception ex)
            {
                //Logge Fehler in Konsole
                Debug.WriteLine("\t-----------------------------\t");
                Debug.WriteLine(ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Methode wird aufgerufen um die Daten aus
        /// dem DS in die Exceldatei zu laden
        /// </summary>
        public void Save()
        {
            try
            {
                //Hol dir das erste Tabellenblatt und mache BackUp falls was schiefgeht
                var sheet = new XLWorkbook(filePath);
                var workbook = sheet.Worksheet("Tabelle1");
                var workbookBackup = workbook;

                workbook.Clear();                   //Lösche alte Daten

                //1x pro Spalte den Spaltennamen
                for (int col = 1; col < dt.Columns.Count +1; col++)
                {
                    workbook.Cell(1, col).Value = dt.Columns[col -1].ColumnName;
                
                    //Restelichen Daten der Spalte
                    for (int row = 2; row < dt.Rows.Count + 2; row++)
                    {
                        workbook.Cell(row, col).Value = dt.Rows[row - 2][col - 1].ToString();
                    }
                }

                //Exceldate speichern und schließen
                sheet.Save();
                sheet.Dispose();

            }
            catch (Exception ex)
            {


                //Logge Fehler in Konsole
                Debug.WriteLine("\t-----------------------------\t");
                Debug.WriteLine(ex.Message);
            }
        }
    }
}

