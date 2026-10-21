using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Laboranforderung.ViewModels.Main
{
    public partial class MainWindowViewModel
    {
        #region Commands
        public ICommand ClearFilterCommand { get; }
        public ICommand ToggleSucheVisibleCommand { get; }
        #endregion
        #region Fields
        private string m_FilterText = "";
        private string m_FilterPhrase = "";
        private List<CheckLaborwertgruppe> m_SuchLaborwertgruppen = new List<CheckLaborwertgruppe>();
        private bool m_SucheVisible = false;
      
        #endregion

        #region Properties
        public string FilterText
        {
            get => m_FilterText;
            set
            {
                if (m_FilterText != value)
                {
                    m_FilterText = value;
                    NotifyPropertyChanged();
                    FilterPhrase= GetFilterPhrase(m_FilterText);
                }
            }
        }

        public string FilterPhrase
        {
            get => m_FilterPhrase;
            set
            {
                if (m_FilterPhrase != value)
                {
                    m_FilterPhrase = value;
                    OnFilterPhraseChanged();
                }
            }
        }

        public List<CheckLaborwertgruppe> SuchLaborwertgruppen
        {
            get => m_SuchLaborwertgruppen;
            set
            {
                if (m_SuchLaborwertgruppen != value)
                {
                    m_SuchLaborwertgruppen = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool SucheVisible
        {
            get => m_SucheVisible;
            set
            {
                if (m_SucheVisible != value)
                {
                    m_SucheVisible = value;
                    NotifyPropertyChanged();
                    if(!m_SucheVisible)
                    {
                        FilterText = string.Empty;
                    }
                }
            }
        }

        private void OnFilterPhraseChanged()
        {
            List<CheckLaborwertgruppe> newSuchLaborwertgruppen = new List<CheckLaborwertgruppe>();
            if(string.IsNullOrEmpty(FilterPhrase))
            {
                foreach (var checkLaborwertgruppe in m_CheckLaborwertgruppen)
                {
                    newSuchLaborwertgruppen.Add(checkLaborwertgruppe);
                }   
            }
            else
            {
                foreach (var checkLaborwertgruppe in m_CheckLaborwertgruppen)
                {
                    CheckLaborwertgruppe newgruppe = new CheckLaborwertgruppe(checkLaborwertgruppe.Laborwertgruppe);
                    foreach (var checkLaborwert in checkLaborwertgruppe.CheckLaborwerte)
                    {

                        if (checkLaborwert.Name.Contains(FilterPhrase, StringComparison.OrdinalIgnoreCase))
                        {
                            newgruppe.CheckLaborwerte.Add(checkLaborwert);
                        }
                        else if (checkLaborwert.Laborwert.Bezeichnung.Contains(FilterPhrase, StringComparison.OrdinalIgnoreCase))
                        {
                            newgruppe.CheckLaborwerte.Add(checkLaborwert);
                        }
                        else if (checkLaborwert.Laborwert.Synonyme.Contains(FilterPhrase, StringComparison.OrdinalIgnoreCase))
                        {
                            newgruppe.CheckLaborwerte.Add(checkLaborwert);
                        }
                    }

                    if (newgruppe.CheckLaborwerte.Count > 0)
                    {
                        newSuchLaborwertgruppen.Add(newgruppe);
                    }
                }
                foreach (var checkLaborwertgruppe in newSuchLaborwertgruppen)
                {
                    checkLaborwertgruppe.IsExpanded = true;
                }
            }

            SuchLaborwertgruppen = newSuchLaborwertgruppen;
        }
        #endregion
        #region Methods
        private void ToggleSucheVisible()
        {
            SucheVisible = !SucheVisible;
        }
        private string GetFilterPhrase(string filterText)
        {
            if(string.IsNullOrEmpty(filterText))
            {
                return string.Empty;
            }
            string trimmedFilterText = filterText.Trim();
            if (string.IsNullOrEmpty(trimmedFilterText))
            {
                return string.Empty;
            }
            if(trimmedFilterText.Length < 2)
            {
                return string.Empty;
            }

            return trimmedFilterText;
        }


        private void ClearFilter()
        {
            FilterText = string.Empty;
        }
        #endregion
    }
}
