using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laboranforderung.ViewModels.FileMangement
{
    public class Options: INotifyPropertyChanged
    {
        private string _FilePath;
        private bool _AllowChanges;
        private bool _AutoSave;
        private bool _AutoOpen;

        private static readonly Options _instance = new Options();

        private Options()
        {
            _FilePath = Properties.Settings.Default.FilePath;
            _AllowChanges = Properties.Settings.Default.AllowChanges;
            _AutoSave = Properties.Settings.Default.AutoSave;
            _AutoOpen = Properties.Settings.Default.AutoOpen;
        }

        public static Options Instance
        {
            get
            {
                return _instance;
            }
        }

        public string FilePath
        {
            get => _FilePath;
            set
            {
                if (_FilePath != value)
                {
                    _FilePath = value;
                    NotifyPropertyChanged();
                    Properties.Settings.Default.FilePath = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool AllowChanges
        {
            get => _AllowChanges;
            set
            {
                if (_AllowChanges != value)
                {
                    _AllowChanges = value;
                    NotifyPropertyChanged();
                    Properties.Settings.Default.AllowChanges = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool AutoSave
        {
            get => _AutoSave;
            set
            {
                if (_AutoSave != value)
                {
                    _AutoSave = value;
                    NotifyPropertyChanged();
                    Properties.Settings.Default.AutoSave = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool AutoOpen
        {
            get => _AutoOpen;
            set
            {
                if (_AutoOpen != value)
                {
                    _AutoOpen = value;
                    NotifyPropertyChanged();
                    Properties.Settings.Default.AutoOpen = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
