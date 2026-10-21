namespace Laboranforderung.Daten
{
    public class DemoDatensammlung : Datensammlung
    {
        public DemoDatensammlung() : base()
        {
            // Create demo data for the Datensammlung
            Laborwertgruppe currentgruppe = CreateGruppe("Blutbild");

            Laborwert klBlutbild = CreateLaborwert(currentgruppe, "kl. BB", "kleines Blutbild");
            CreateLaborwert(currentgruppe, "Diff-BB", "Differenzialblutbild");
            CreateLaborwert(currentgruppe, "Diff-BB (mikrosk.)", "Differenzialblutbild (mikroskopisch)");
            currentgruppe = CreateGruppe("Elektrolyte");
            Laborwert na = CreateLaborwert(currentgruppe, "Na", "Natrium");
            Laborwert k = CreateLaborwert(currentgruppe, "K", "Kalium");
            CreateLaborwert(currentgruppe, "Ca", "Kalzium");
            CreateLaborwert(currentgruppe, "Cl", "Chlorid");

            currentgruppe = CreateGruppe("Entzündungsparameter");
            Laborwert crp = CreateLaborwert(currentgruppe, "CRP", "C-reaktives Protein");
            Laborwert bsg = CreateLaborwert(currentgruppe, "BSG", "Blutsenkungsgeschwindigkeit");

            currentgruppe = CreateGruppe("Leberwerte");
            Laborwert asat = CreateLaborwert(currentgruppe, "ASAT", "Aspartat-Aminotransferase");
            Laborwert alat = CreateLaborwert(currentgruppe, "ALAT", "Alanin-Aminotransferase");
            CreateLaborwert(currentgruppe, "ALP", "Alkalische Phosphatase");
            CreateLaborwert(currentgruppe, "GGT", "Gamma-Glutamyltransferase");
            currentgruppe = CreateGruppe("Bauchspeicheldrüse");
            CreateLaborwert(currentgruppe, "Amyl", "Amylase");
            Laborwert lip = CreateLaborwert(currentgruppe, "Lip", "Lipase");
            currentgruppe = CreateGruppe("Zucker");
            Laborwert bz = CreateLaborwert(currentgruppe, "BZ", "Blutzucker");
            CreateLaborwert(currentgruppe, "HbA1c", "Glykosyliertes Hämoglobin");

            currentgruppe = CreateGruppe("Fette");
            Laborwert chol= CreateLaborwert(currentgruppe, "Chol", "Cholesterin");
            Laborwert trig= CreateLaborwert(currentgruppe, "Trig", "Triglyzeride");
            Laborwert hdl= CreateLaborwert(currentgruppe, "HDL", "High Density Lipoprotein");
            Laborwert ldl= CreateLaborwert(currentgruppe, "LDL", "Low Density Lipoprotein");
            CreateLaborwert(currentgruppe, "Lp(a)", "Lipoprotein(a)");

            currentgruppe = CreateGruppe("Nierenwerte");
            CreateLaborwert(currentgruppe, "Krea", "Kreatinin");
            CreateLaborwert(currentgruppe, "Harnstoff", "Harnstoff");

            currentgruppe = CreateGruppe("Schilddrüse");
            Laborwert tsh = CreateLaborwert(currentgruppe, "TSH", "Thyreoidea-stimulierendes Hormon");
            Laborwert ft3 = CreateLaborwert(currentgruppe, "fT3", "freies Trijodthyronin");
            Laborwert ft4 = CreateLaborwert(currentgruppe, "fT4", "freies Thyroxin");
            Laborwert antiTPO = CreateLaborwert(currentgruppe, "Anti-TPO", "Antikörper gegen Thyreoperoxidase");
            Laborwert antiTG = CreateLaborwert(currentgruppe, "Anti-TG", "Antikörper gegen Thyreoglobulin");
            Laborwert trak = CreateLaborwert(currentgruppe, "TRAK", "Antikörper gegen TSH-Rezeptor");

            currentgruppe = CreateGruppe("Rheumawerte");
            CreateLaborwert(currentgruppe, "RF", "Rheumafaktor");
            CreateLaborwert(currentgruppe, "HLAB27", "HLAB27");





            currentgruppe = CreateGruppe("Weitere Werte");

            Laborprofil currentprofil = CreateLaborprofil("kleine Aufnahme", "1");
            currentprofil.Werte.Add (klBlutbild);
            currentprofil.Werte.Add (asat);
            currentprofil.Werte.Add (alat);
            currentprofil.Werte.Add (lip);
            currentprofil.Werte.Add (bz);
            currentprofil.Werte.Add (tsh);


            currentprofil = CreateLaborprofil("große Aufnahme", "2");
            currentprofil.Werte.Add(klBlutbild);
            currentprofil.Werte.Add(na);
            currentprofil.Werte.Add(k);
            currentprofil.Werte.Add(crp);
            currentprofil.Werte.Add(bsg);
            currentprofil.Werte.Add(chol);
            currentprofil.Werte.Add(trig);
            currentprofil.Werte.Add(hdl);
            currentprofil.Werte.Add(ldl);

            currentprofil.Werte.Add(asat);
            currentprofil.Werte.Add(alat);
            currentprofil.Werte.Add(lip);
            currentprofil.Werte.Add(bz);
            currentprofil.Werte.Add(tsh);
            currentprofil.Werte.Add(ft3);
            currentprofil.Werte.Add(ft4);

            currentprofil = CreateLaborprofil("Fette", "3");
            currentprofil.Werte.Add(chol);
            currentprofil.Werte.Add(trig);
            currentprofil.Werte.Add(hdl);
            currentprofil.Werte.Add(ldl);

            currentprofil = CreateLaborprofil("Thyreoiditis", "4");
            currentprofil.Werte.Add(tsh);
            currentprofil.Werte.Add(ft3);
            currentprofil.Werte.Add(ft4);
            currentprofil.Werte.Add(antiTPO);
            currentprofil.Werte.Add(antiTG);
            currentprofil.Werte.Add(trak);

            currentprofil = CreateLaborprofil("Transaminasen", "5");
            currentprofil.Werte.Add (asat);
            currentprofil.Werte.Add (alat);


        }
        private Laborwertgruppe CreateGruppe(string name)
        {
            Laborwertgruppe gruppe = new Laborwertgruppe(name);
            Gruppen.Add(gruppe);
            return gruppe;
        }

        public Laborwert CreateLaborwert(Laborwertgruppe gruppe, string name, string bezeichnung, string synonyme = "")
        {
            Laborwert wert = new Laborwert(name, bezeichnung);
            if(!string.IsNullOrEmpty(synonyme))
                wert.Synonyme=synonyme;
            gruppe.Werte.Add(wert);
            return wert;
        }

        private Laborprofil CreateLaborprofil(string profilname, string number)
        {
            Laborprofil profil = new Laborprofil(profilname, number);
            Profile.Add(profil);
            return profil;
        }
    }
}
