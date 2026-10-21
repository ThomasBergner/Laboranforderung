using Laboranforderung.Daten;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Laboranforderung.ViewModels.Main
{
    public class CheckLaborwert : INotifyPropertyChanged
    {
        
        #region Fields
        private bool m_IsChecked;
        private bool m_IsLocked;
        private Laborwert m_Laborwert;
        private ObservableCollection<CheckLaborprofil> m_LockSources;
        private string m_LockText;
        #endregion

        #region Constructors

        public CheckLaborwert(Laborwert laborwert)
        {
            m_IsChecked = false;
            m_IsLocked = false;
            m_Laborwert = laborwert;
            m_LockSources = new ObservableCollection<CheckLaborprofil>();
            m_LockSources.CollectionChanged += LockSources_CollectionChanged;
            m_LockText = "";
        }

        #endregion

        #region Properties

        public bool IsChecked
        {
            get { return m_IsChecked; }
            set { if (m_IsChecked != value) { m_IsChecked = value; NotifyPropertyChanged(); OnCheckedChanged(); } }
        }
        
        public bool IsLocked
        {
            get { return m_IsLocked; }
            private set { if (m_IsLocked != value) { m_IsLocked = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(IsEnabled)); } }
        }

        public bool IsEnabled
        {
            get { return !m_IsLocked; }
        }

        public Laborwert Laborwert
        {
            get { return m_Laborwert; }
        }
        
        public ObservableCollection<CheckLaborprofil> LockSources
        {
            get { return m_LockSources; }
        }
        public string LockText
        {
            get { return m_LockText; }
            private set { if (m_LockText != value.Trim()) { m_LockText = value.Trim(); NotifyPropertyChanged(); } }
        }
        public string Name
        {
            get { return m_Laborwert.Name; }
        }
        public string InfoText
        {
            get { return BuildInfoText(); }
        }

        #endregion

        #region Methods
        private string BuildInfoText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Laborwert.Bezeichnung);
          
            if(!string.IsNullOrEmpty(m_Laborwert.Synonyme))
            {
                List<string> synonyme = GetsynonymeList();
                foreach (var syn in synonyme)
                {
                    sb.Append(",");
                    sb.AppendLine();
                    sb.Append(syn);
                }
            }


           if(m_LockSources.Count > 0)
            {
                sb.AppendLine();
                sb.Append("Ausgewählt in Profilen:");
                foreach (var source in m_LockSources)
                {
                    sb.AppendLine();
                    sb.Append("- " + source.Laborprofil.Profilname);
                }
            }

            return sb.ToString();
        }

        private List<string> GetsynonymeList()
        {
            List<string> synonymeList = new List<string>();
            if (!string.IsNullOrEmpty(m_Laborwert.Synonyme))
            {
                string synonymeString = m_Laborwert.Synonyme;
                bool contains125Comma = synonymeString.Contains("1,25");
                if(contains125Comma)
                {
                    synonymeString = synonymeString.Replace("1,25", "1.25");
                }

                string[] synonymeArray = synonymeString.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var syn in synonymeArray)
                {
                    synonymeList.Add(syn.Trim());
                }
            }
            return synonymeList;
        }

        private void LockSources_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            IsLocked = m_LockSources.Count > 0;
            if(IsLocked)
            {
                string pretext = m_LockSources.Count == 1 ? "Ausgewählt im Profil " : "Ausgewählt in den Profilen ";
                LockText = pretext + string.Join(", ", m_LockSources.Select(x => x.Laborprofil.Profilname));
            }
            else
            {
                LockText = "";
            }
            NotifyPropertyChanged(nameof(InfoText));
        }
        #endregion

        #region Events
        public event EventHandler? CheckedChanged;
        private void OnCheckedChanged()
        {
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
