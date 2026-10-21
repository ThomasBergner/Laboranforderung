using Laboranforderung.Daten;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laboranforderung.ViewModels.Main
{
    public class CheckLaborprofil : INotifyPropertyChanged
    {
        
        #region Fields
        private Laborprofil m_Laborprofil;
        private bool m_IsChecked;
        private List<CheckLaborwert> m_CheckLaborwerte;
        private CheckLaborwertCollection m_CheckLaborwertCollection;
        private bool m_IsExpanded;
        #endregion

        #region Constructors

        public CheckLaborprofil(Laborprofil laborprofil, CheckLaborwertCollection checkLaborwertCollection)
        {
            m_Laborprofil = laborprofil;
            m_IsChecked = false;
            m_CheckLaborwerte = new List<CheckLaborwert>();
            m_CheckLaborwertCollection = checkLaborwertCollection;
            m_IsExpanded = false;
        }
        
        #endregion
        
        #region Properties
        
        public Laborprofil Laborprofil
        {
            get { return m_Laborprofil; }
        }

        public bool IsExpanded
        {
            get { return m_IsExpanded; }
            set { if (m_IsExpanded != value) { m_IsExpanded = value; NotifyPropertyChanged(); } }
        }
        
        public bool IsChecked
        {
            get { return m_IsChecked; }
            set { if (m_IsChecked != value) { m_IsChecked = value; NotifyPropertyChanged(); OnCheckedChanged(); } }
        }
        
        public List<CheckLaborwert> CheckLaborwerte
        {
            get { return m_CheckLaborwerte; }
        }

        public string Name
        {
            get { return m_Laborprofil.DisplayName; }
        }
        #endregion

        #region Methods


        #endregion

        #region Events
        public event EventHandler? CheckedChanged;
        private void OnCheckedChanged()
        {
            m_CheckLaborwertCollection.BeginUpdate();
            if (m_IsChecked)
            {
                foreach (var checkLaborwert in m_CheckLaborwerte)
                {
                    checkLaborwert.IsChecked = true;
                    checkLaborwert.LockSources.Add(this);
                }
            }
            else
            {
                foreach (var checkLaborwert in m_CheckLaborwerte)
                {
                    checkLaborwert.LockSources.Remove(this);
                }
            }
            m_CheckLaborwertCollection.EndUpdate();
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
