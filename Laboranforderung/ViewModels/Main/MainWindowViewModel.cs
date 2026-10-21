using Laboranforderung.Commands;
using Laboranforderung.Daten;
using Laboranforderung.ViewModels.FileMangement;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Laboranforderung.ViewModels.Main
{

    public partial class MainWindowViewModel: INotifyPropertyChanged 
    {
        #region Commands
        public ICommand ResultTextKopierenCommand { get; }
        public ICommand ExpandAllGroupsCommand => new RelayCommand(ExpandAllGroups);
        public ICommand CollapseAllGroupsCommand => new RelayCommand(CollapseAllGroups);
        public ICommand ExpandAllProfilesCommand => new RelayCommand(ExpandAllProfiles);
        public ICommand CollapseAllProfilesCommand => new RelayCommand(CollapseAllProfiles);
        public ICommand RestartCommand => new RelayCommand(Restart);
        #endregion

        private List<CheckLaborwertgruppe> m_CheckLaborwertgruppen = new List<CheckLaborwertgruppe>();
        private List<CheckLaborprofil> m_CheckLaborprofile = new List<CheckLaborprofil>();

        private CheckLaborwertCollection? m_CheckLaborwertCollection;
        private string m_ResultText;
        private Datensammlung m_Datensammlung;

       private Options _options;
        public MainWindowViewModel()
        {
            _options = Options.Instance;
            _options.PropertyChanged += Options_PropertyChanged;
            m_ResultText = "";
            m_Datensammlung = new Datensammlung();
            Initialize(m_Datensammlung);
            ResultTextKopierenCommand = new RelayCommand(ResultTextKopieren);
            ClearFilterCommand = new RelayCommand(ClearFilter);
            ToggleSucheVisibleCommand = new RelayCommand(ToggleSucheVisible);
            OptimizeProfilesCommand = new RelayCommand(OptimizeProfiles);
        }



        #region Properties

        public bool AllowChanges
        {
            get => _options.AllowChanges;
            set { }
        }

        public Datensammlung Datensammlung
        {
            get => m_Datensammlung;
            set
            {
                if (m_Datensammlung != value)
                {
                    m_Datensammlung = value;
                    Initialize(m_Datensammlung);
                }
            }
        }

        public List<CheckLaborwertgruppe> CheckLaborwertgruppen
        {
            get => m_CheckLaborwertgruppen;
            set
            {
                if (m_CheckLaborwertgruppen != value)
                {
                    m_CheckLaborwertgruppen = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<CheckLaborprofil> CheckLaborprofile
        {
            get => m_CheckLaborprofile;
            set
            {
                if (m_CheckLaborprofile != value)
                {
                    m_CheckLaborprofile = value;
                    NotifyPropertyChanged();
                }
            }
        }
        

        public string ResultText
        {
            get => m_ResultText;
            set
            {
                if (m_ResultText != value)
                {
                    m_ResultText = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion
        #region Methods

        private void Options_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Options.AllowChanges))
            {
                NotifyPropertyChanged(nameof(AllowChanges));
            }
        }
        public void ExpandAllGroups()
        {
            foreach (var gruppe in m_CheckLaborwertgruppen)
            {
                gruppe.IsExpanded = true;
            }
            foreach (var gruppe in m_SuchLaborwertgruppen)
            {
                gruppe.IsExpanded = true;
            }
        }
        public void CollapseAllGroups()
        {
            foreach (var gruppe in m_CheckLaborwertgruppen)
            {
                gruppe.IsExpanded = false;
            }
            foreach (var gruppe in m_SuchLaborwertgruppen)
            {
                gruppe.IsExpanded = false;
            }
        }

        public void ExpandAllProfiles()
        {
            foreach (var profil in m_CheckLaborprofile)
            {
                profil.IsExpanded = true;
            }
        }

        public void CollapseAllProfiles()
        {
            foreach (var profil in m_CheckLaborprofile)
            {
                profil.IsExpanded = false;
            }
        }

        private void Restart()
        {
            m_CheckLaborwertCollection.BeginUpdate();   
            foreach (var profil in m_CheckLaborprofile)
            {
                profil.IsChecked = false;
            }

            foreach (var gruppe in m_CheckLaborwertgruppen)
            {
                foreach (var laborwert in gruppe.CheckLaborwerte)
                {
                    laborwert.IsChecked = false;
                }
            }
            m_CheckLaborwertCollection.EndUpdate();
            ProfileText = "";
            RestLaborwerteText = "";
        }

        private void ResultTextKopieren()
        {
            if (string.IsNullOrWhiteSpace(ResultText))
            {
                return;
            }
            System.Windows.Clipboard.SetText(ResultText);
            ShowToast("Ergebnis in die Zwischenablage kopiert.");
        }

        private void Initialize(Datensammlung source)
        {
            m_FilterText = string.Empty;
            m_FilterPhrase = string.Empty;

            ResultText = "";
            List<CheckLaborwertgruppe> newCheckLaborwertgruppen = new List<CheckLaborwertgruppe>();
            List<CheckLaborprofil> newCheckLaborprofile = new List<CheckLaborprofil>();

            List<CheckLaborwert> checkLaborwerte = new List<CheckLaborwert>();

            Dictionary<Laborwert, CheckLaborwert> laborwertMap = new Dictionary<Laborwert, CheckLaborwert>();
            foreach (var laborwertgruppe in source.Gruppen)
            {
                CheckLaborwertgruppe gruppe = new CheckLaborwertgruppe(laborwertgruppe);
                foreach (var laborwert in laborwertgruppe.Werte)
                {
                    CheckLaborwert checkLaborwert = new CheckLaborwert(laborwert);
                    gruppe.CheckLaborwerte.Add(checkLaborwert);
                    laborwertMap[laborwert] = checkLaborwert;
                    checkLaborwerte.Add(checkLaborwert);
                }
                newCheckLaborwertgruppen.Add(gruppe);
            }
            
            if(m_CheckLaborwertCollection != null)
            {
                m_CheckLaborwertCollection.PropertyChanged -= CheckLaborwertCollection_PropertyChanged;
            }

            m_CheckLaborwertCollection = new CheckLaborwertCollection(checkLaborwerte);

            foreach (var laborprofil in source.Profile)
            {
                var checkLaborprofil = new CheckLaborprofil(laborprofil, m_CheckLaborwertCollection);
                foreach (var laborwert in laborprofil.Werte)
                {
                    checkLaborprofil.CheckLaborwerte.Add(laborwertMap[laborwert]);
                }
                newCheckLaborprofile.Add(checkLaborprofil);
            }

            CheckLaborwertgruppen = newCheckLaborwertgruppen;
            CheckLaborprofile = newCheckLaborprofile;

            m_CheckLaborwertCollection.PropertyChanged += CheckLaborwertCollection_PropertyChanged;

            OnFilterPhraseChanged();
            ProfileText="";
            RestLaborwerteText="";
            NotifyPropertyChanged(nameof(FilterText));
        }

        private void CheckLaborwertCollection_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
           if (e.PropertyName == nameof(CheckLaborwertCollection.Result))
            {
                ResultText = string.Join(", ", m_CheckLaborwertCollection!.Result.Select(lw => lw.Name));
            }
        }

        #endregion


        #region Events
        public event Action<string>? ToastRequested;

        public void ShowToast(string message)
        {
            ToastRequested?.Invoke(message);
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
