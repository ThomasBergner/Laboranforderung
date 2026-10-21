using Laboranforderung.Daten.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboranforderung.Daten
{
    public class ExportImport
    {

        public static Datensammlung ImportFromCsv(string filePath)
        {
            var datensammlung = new Datensammlung();
            var lines = System.IO.File.ReadAllLines(filePath);
            string currentGroupName = "";
            string currentProfileName = "";
            bool inProfileSection = false;
            Laborwertgruppe? currentGroup = null;
            Laborprofil? currentProfile = null;

            Dictionary<string, Laborwert> werteDictionary = new Dictionary<string, Laborwert>();
            Laborwertgruppe? weitereWerte = null;

            foreach (var line in lines)
            {
                var laborwertLine = LaborwertLine.ParseCsv(line);
                if (laborwertLine.GroupProfilname == "Laborwerte" || laborwertLine.GroupProfilname == "Gruppenname")
                {
                    inProfileSection = false;
                    continue;
                }
                else if (laborwertLine.GroupProfilname == "Profile" || laborwertLine.GroupProfilname == "Profilnummer")
                {
                    inProfileSection = true;
                    continue;
                }
                if (!inProfileSection)
                {
                    if (!string.IsNullOrEmpty(laborwertLine.GroupProfilname))
                    {
                        currentGroup= new Laborwertgruppe(laborwertLine.GroupProfilname);
                        datensammlung.Gruppen.Add(currentGroup);
                    }
                    if (!string.IsNullOrEmpty(laborwertLine.Laborwertname))
                    {
                        var laborwert = new Laborwert(laborwertLine.Laborwertname, laborwertLine.Langname, laborwertLine.Synonyme, laborwertLine.Roehrchenart, laborwertLine.Findehilfe, 0);
                        currentGroup?.Werte.Add(laborwert);
                        werteDictionary[laborwert.Name] = laborwert;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(laborwertLine.Laborwertname))
                    {
                        currentProfile = new Laborprofil(laborwertLine.Laborwertname, "");
                        datensammlung.Profile.Add(currentProfile);
                        if (!string.IsNullOrEmpty(laborwertLine.GroupProfilname))
                        {
                            currentProfile.Number = laborwertLine.GroupProfilname;
                        }
                    }
                    if (!string.IsNullOrEmpty(laborwertLine.Langname))
                    {
                        if (werteDictionary.TryGetValue(laborwertLine.Langname, out var laborwert))
                        {
                            currentProfile?.Werte.Add(laborwert);
                        }
                        else
                        {
                            // Laborwert existiert nicht in den Gruppen, also erstellen wir einen neuen Laborwert mit leeren Eigenschaften
                            var newLaborwert = new Laborwert(laborwertLine.Langname, laborwertLine.Langname, "", "", "", 0);
                            currentProfile?.Werte.Add(newLaborwert);
                            if(weitereWerte == null)
                            {
                                weitereWerte = new Laborwertgruppe("Weitere Werte");
                                datensammlung.Gruppen.Add(weitereWerte);
                            }
                            weitereWerte.Werte.Add(newLaborwert);
                            werteDictionary[newLaborwert.Name] = newLaborwert;
                        }

                    }
                }
          
            }
            return datensammlung;
        }
        public static void ExportToCsv(string filePath, Datensammlung datensammlung)
        {
            List <LaborwertLine> laborwertLines = new List<LaborwertLine>();
            laborwertLines.Add(new LaborwertLine("Laborwerte", "", "", "", "", ""));
            laborwertLines.Add(new LaborwertLine("Gruppenname", "Laborwert-Name/Kürzel", "Lange Bezeichnung", "Synonyme (kommagetrennt)", "Röhrchenart", "Findehilfe")); // Header


            var csvLines = new List<string>();
            foreach (var laborwertgruppe in datensammlung.Gruppen)
            {
                laborwertLines.Add(new LaborwertLine(laborwertgruppe.Name, "", "", "", "", ""));
                foreach (var laborwert in laborwertgruppe.Werte)
                {
                    var line = new LaborwertLine("", laborwert.Name, laborwert.Bezeichnung, laborwert.Synonyme, laborwert.Roehrchenart, laborwert.Findehilfe);
                    laborwertLines.Add(line);
                }
            }

            laborwertLines.Add(new LaborwertLine("Profile", "", "", "", "", ""));
            laborwertLines.Add(new LaborwertLine("Profilnummer", "Profilname", "Laborwert-Name/Kürzel", "", "", ""));
            foreach (var laborprofil in datensammlung.Profile)
            {
                laborwertLines.Add(new LaborwertLine(laborprofil.Number, laborprofil.Profilname, "", "", "", ""));

                foreach (var laborwert in laborprofil.Werte)
                {
                    var line = new LaborwertLine("", "", laborwert.Name, "", "", "");
                    laborwertLines.Add(line);
                }
            }


            
            foreach (var laborwertline in laborwertLines)
            {
                csvLines.Add(laborwertline.ToCsvLine());
            }

            System.IO.File.WriteAllLines(filePath, csvLines);
        }





        private class LaborwertLine
        {
            public static LaborwertLine ParseCsv(string csvLine)
            {
                // Es gibt Zellen, die mit Anführungszeichen eingeschlossen sind, um Kommas innerhalb der Zelle zu behandeln.
                // Wir müssen diese Zellen korrekt parsen.
                var parts = new List<string>();
                bool inQuotes = false;
                StringBuilder currentPart = new StringBuilder();
                for (int i = 0; i < csvLine.Length; i++)
                {
                    char c = csvLine[i];
                    if (c == '\"')
                    {
                        inQuotes = !inQuotes;
                    }
                    else if (c == ',' && !inQuotes)
                    {
                        parts.Add(currentPart.ToString().Trim());
                        currentPart.Clear();
                    }
                    else
                    {
                        currentPart.Append(c);
                    }
                }
                parts.Add(currentPart.ToString().Trim());
                // Entferne Anführungszeichen von den Teilen
                for (int i = 0; i < parts.Count; i++)
                {
                    parts[i] = parts[i].Trim('\"');
                }
                if (parts.Count != 6)
                {
                    throw new FormatException("CSV line does not have the correct number of fields.");
                }
                return new LaborwertLine(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]); 


            }


            private string m_GroupProfilname = "";
            private string m_Laborwertname = "";
            private string m_Langname = "";
            private string m_Synonyme = "";
            private string m_Roehrchenart = "";
            private string m_Findehilfe = "";
            

            public string GroupProfilname => m_GroupProfilname;
            public string Laborwertname => m_Laborwertname;
            public string Langname => m_Langname;
            public string Synonyme => m_Synonyme;
            public string Roehrchenart => m_Roehrchenart;
            public string Findehilfe => m_Findehilfe;
            

            public LaborwertLine(string groupProfilname, string laborwertname, string langname, string synonyme, string roehrchenart, string findehilfe)
            {
                m_GroupProfilname = groupProfilname;
                m_Laborwertname = laborwertname;
                m_Langname = langname;
                m_Synonyme = synonyme;
                m_Roehrchenart = roehrchenart;
                m_Findehilfe = findehilfe;
               
            }
           
            public string ToCsvLine()
            {
                string[] parts = new string[] { m_GroupProfilname, m_Laborwertname, m_Langname, m_Synonyme, m_Roehrchenart, m_Findehilfe };
                for (int i = 0; i < parts.Length; i++)
                {
                    if(parts[i].Contains("\""))
                    {
                        parts[i] = parts[i].Replace("\"", " ").Trim();
                    }
                    if (parts[i].Contains(",") || parts[i].Contains("\""))
                    {
                        parts[i] = "\"" + parts[i] + "\"";

                    }
                }

                return string.Join(",", parts);
            }
        }
    }
}
