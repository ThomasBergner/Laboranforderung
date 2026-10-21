using Laboranforderung.Daten;
using Laboranforderung.ViewModels.FileMangement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Laboranforderung.ViewModels.Main
{
    public partial class MainWindowViewModel
    {
        #region Commands
        public ICommand OptimizeProfilesCommand { get; }
        #endregion
        #region Fields
        private string m_ProfileText = "";
        private string m_RestLaborwerteText = "";
        #endregion

        #region Properties
        public string ProfileText
        {
            get => m_ProfileText;
            set
            {
                if (m_ProfileText != value)
                {
                    m_ProfileText = value;
                    NotifyPropertyChanged(nameof(ProfileText));
                }
            }
        }
        public string RestLaborwerteText
        {
            get => m_RestLaborwerteText;
            set
            {
                if (m_RestLaborwerteText != value)
                {
                    m_RestLaborwerteText = value;
                    NotifyPropertyChanged(nameof(RestLaborwerteText));
                }
            }
        }
        #endregion

        #region Methods
        private void OptimizeProfiles()
        {
            // Implement the logic to optimize the profile here
            // This could involve processing data, adjusting settings, etc.
            if (m_CheckLaborwertCollection == null) return;
            if (m_CheckLaborwertCollection.Result.Count == 0) return;
    
            List<Laborwert> selectedLaborwerte = new List<Laborwert>(m_CheckLaborwertCollection.Result);
            List<Laborprofil> matchingprofiles = new List<Laborprofil>();
            foreach (var profile in FileManager.Instance.DisplayClone.Profile)
            {
                if(profile.Werte.Count == 0) continue;
                if (IsProfileMatching(profile, selectedLaborwerte))
                    matchingprofiles.Add(profile);

            }

            List<Laborwert> restlaborwerte = new List<Laborwert>();
            foreach (var wert in selectedLaborwerte)
            {
                bool isInAnyProfile = false;
                foreach (var profile in matchingprofiles)
                {
                    if (profile.Werte.Contains(wert))
                    {
                        isInAnyProfile = true;
                        break;
                    }
                }
                if (!isInAnyProfile)
                {
                    restlaborwerte.Add(wert);
                }
            }
            
            matchingprofiles.Sort(new LaborprofilWerteCountComparer());
            for(int i = matchingprofiles.Count-1; i >= 1; i--)
            {
                var profile = matchingprofiles[i];
                var previouswerte  = new List<Laborwert>();
              
                for(int j = 0; j < i; j++)
                {
                    var previousprofile = matchingprofiles[j];
                    foreach (var wert in previousprofile.Werte)
                    {
                        if (!previouswerte.Contains(wert))
                        {
                            previouswerte.Add(wert);
                        }
                    }
                }

                if(IsProfileMatching(profile, previouswerte))
                {
                    matchingprofiles.RemoveAt(i);
                }
            }

            // Now matchingprofiles contains the optimized list of profiles
            // restlaborwerte contains the labor values that are not part of any profile

            m_CheckLaborwertCollection.BeginUpdate();
            Restart();

            Dictionary<Laborprofil, CheckLaborprofil> profileCheckMap = new Dictionary<Laborprofil, CheckLaborprofil>();
            foreach (var checkProfile in m_CheckLaborprofile)
            {
                profileCheckMap[checkProfile.Laborprofil] = checkProfile;
            }

            Dictionary<Laborwert, CheckLaborwert> wertCheckMap = new Dictionary<Laborwert, CheckLaborwert>();
            foreach (var gruppe in m_CheckLaborwertgruppen)
            {
                foreach (var checkWert in gruppe.CheckLaborwerte)
                {
                    wertCheckMap[checkWert.Laborwert] = checkWert;
                }
            }

            foreach (var profile in matchingprofiles)
            {
                profileCheckMap[profile].IsChecked = true;
            }
            foreach (var wert in restlaborwerte)
            {
                wertCheckMap[wert].IsChecked = true;
            }
            
            m_CheckLaborwertCollection.EndUpdate();

            ProfileText = string.Join(", ", matchingprofiles.Select(p => p.DisplayName));
            RestLaborwerteText = string.Join(", ", restlaborwerte.Select(w => w.Name));
        }

        private class LaborprofilWerteCountComparer : IComparer<Laborprofil>
        {
            public int Compare(Laborprofil x, Laborprofil y)
            {
                if (x == null || y == null) return 0;
                return y.Werte.Count.CompareTo(x.Werte.Count);
            }
        }

        private bool IsProfileMatching(Laborprofil profile, List<Laborwert> selectedLaborwerte)
        {
            foreach (var wert in profile.Werte)
            {
                if(!selectedLaborwerte.Contains(wert))
                {
                    return false;
                }
            }
            return true;
        }



        #endregion
    }
}
