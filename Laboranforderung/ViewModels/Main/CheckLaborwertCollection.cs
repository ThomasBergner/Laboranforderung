using Laboranforderung.Daten;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laboranforderung.ViewModels.Main
{
    public class CheckLaborwertCollection : INotifyPropertyChanged
    {
        
        #region Fields
        private List<CheckLaborwert> m_CheckLaborwerte;
        private int m_UpdateCounter;
        private List<Laborwert> m_Result;
        private bool m_HasChanges;
        #endregion

        #region Constructors

        public CheckLaborwertCollection(List<CheckLaborwert> checkLaborwerte)
        {
            m_CheckLaborwerte = checkLaborwerte;
            m_UpdateCounter = 0;
            m_Result = new List<Laborwert>();
            foreach (var checkLaborwert in m_CheckLaborwerte)
            {
                checkLaborwert.CheckedChanged += CheckLaborwert_CheckedChanged;
            }
        }

        #endregion

        #region Properties

        public List<CheckLaborwert> CheckLaborwerte
        {
            get { return m_CheckLaborwerte; }
        }
        
        public List<Laborwert> Result
        {
            get { return m_Result; }
            private set { if (m_Result != value) { m_Result = value; NotifyPropertyChanged(); } }
        }
        #endregion

        #region Methods
        private void CheckLaborwert_CheckedChanged(object? sender, EventArgs e)
        {
            BeginUpdate();
            m_HasChanges = true;
            EndUpdate();
        }
        public void BeginUpdate()
        {
            m_UpdateCounter++;
        }
        
        public void EndUpdate()
        {
            m_UpdateCounter--;
            if (m_UpdateCounter == 0 && m_HasChanges)
            {
                m_HasChanges = false;
                List<Laborwert> newResult = m_CheckLaborwerte.Where(clw => clw.IsChecked).Select(clw => clw.Laborwert).ToList();
                Result = newResult;
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
