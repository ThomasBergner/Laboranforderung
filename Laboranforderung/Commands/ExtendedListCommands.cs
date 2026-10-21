using System.Windows.Input;

namespace Laboranforderung.Commands
{
    public class ExtendedListCommands : ListCommands, IExtendedListCommands
    {
        private bool _AddKategorieEnabled;

        public ICommand AddKategorie { get; }

        public ExtendedListCommands()
        {
            AddKategorie = new RelayCommand(OnAddKategorie, () => AddKategorieEnabled);
        }

        private void OnAddKategorie()
        {
            ExecuteAddKategorie?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? ExecuteAddKategorie;

        public bool AddKategorieEnabled
        {
            get => _AddKategorieEnabled;
            set
            {
                if (_AddKategorieEnabled != value)
                {
                    _AddKategorieEnabled = value;
                    ((RelayCommand)AddKategorie).RaiseCanExecuteChanged();
                }
            }
        }
    }
}
