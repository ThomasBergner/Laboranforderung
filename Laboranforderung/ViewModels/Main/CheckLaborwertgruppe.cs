using Laboranforderung.Daten;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laboranforderung.ViewModels.Main
{
    public class CheckLaborwertgruppe : INotifyPropertyChanged
    {
        #region Fields
        private Laborwertgruppe m_Laborwertgruppe;
        private List<CheckLaborwert> m_CheckLaborwerte;
        private bool m_IsExpanded;
        #endregion

        #region Constructors

        public CheckLaborwertgruppe(Laborwertgruppe laborwertgruppe)
        {
            m_Laborwertgruppe = laborwertgruppe;
            m_CheckLaborwerte = new List<CheckLaborwert>();
            m_IsExpanded = false;
        }
        
        #endregion
        
        #region Properties
        
        public Laborwertgruppe Laborwertgruppe
        {
            get { return m_Laborwertgruppe; }
        }

        public string Name
        {
            get { return m_Laborwertgruppe.Name; }
        }

        public List<CheckLaborwert> CheckLaborwerte
        {
            get { return m_CheckLaborwerte; }
        }

        public bool HasChildren
        {
            get => m_CheckLaborwerte.Count > 0; 
        }
        public bool IsExpanded
        {
            get => m_IsExpanded;
            set
            {
                if (m_IsExpanded != value)
                {
                    m_IsExpanded = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods


        #endregion
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

    }
}
