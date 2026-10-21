using System.Windows.Input;

namespace Laboranforderung.Commands
{
    public class ListCommands : IListCommands
    {
        private bool _AddEnabled;
        private bool _EditEnabled;
        private bool _DeleteEnabled;
        private bool _MoveUpEnabled;
        private bool _MoveDownEnabled;

        public ICommand Add { get; }

        public ICommand Edit { get; }

        public ICommand Delete { get; }

        public ICommand MoveUp { get; }

        public ICommand MoveDown { get; }
        public ListCommands()
        {
            Add = new RelayCommand(OnAdd, () => AddEnabled);
            Edit = new RelayCommand(OnEdit, () => EditEnabled);
            Delete = new RelayCommand(OnDelete, () => DeleteEnabled);
            MoveUp = new RelayCommand(OnMoveUp, () => MoveUpEnabled);
            MoveDown = new RelayCommand(OnMoveDown, () => MoveDownEnabled);
        }

        private void OnAdd()
        {
            ExecuteAdd?.Invoke(this, EventArgs.Empty);
        }
        private void OnEdit()
        {
            ExecuteEdit?.Invoke(this, EventArgs.Empty);
        }
        private void OnDelete()
        {
            ExecuteDelete?.Invoke(this, EventArgs.Empty);
        }
        private void OnMoveUp()
        {
            ExecuteMoveUp?.Invoke(this, EventArgs.Empty);
        }
        private void OnMoveDown()
        {
            ExecuteMoveDown?.Invoke(this, EventArgs.Empty);
        }


        public bool AddEnabled
        {
            get => _AddEnabled;
            set
            {
                if (_AddEnabled != value)
                {
                    _AddEnabled = value;
                    ((RelayCommand)Add).RaiseCanExecuteChanged();
                }
            }
        }
        public bool EditEnabled
        {
            get => _EditEnabled;
            set
            {
                if (_EditEnabled != value)
                {
                    _EditEnabled = value;
                    ((RelayCommand)Edit).RaiseCanExecuteChanged();
                }
            }
        }


        public bool DeleteEnabled
        {
            get => _DeleteEnabled;
            set
            {
                if (_DeleteEnabled != value)
                {
                    _DeleteEnabled = value;
                    ((RelayCommand)Delete).RaiseCanExecuteChanged();
                }
            }
        }

        public bool MoveUpEnabled
        {
            get => _MoveUpEnabled;
            set
            {
                if (_MoveUpEnabled != value)
                {
                    _MoveUpEnabled = value;
                    ((RelayCommand)MoveUp).RaiseCanExecuteChanged();
                }
            }
        }

        public bool MoveDownEnabled
        {
            get => _MoveDownEnabled;
            set
            {
                if (_MoveDownEnabled != value)
                {
                    _MoveDownEnabled = value;
                    ((RelayCommand)MoveDown).RaiseCanExecuteChanged();
                }
            }
        }

        public event EventHandler? ExecuteAdd;
        public event EventHandler? ExecuteEdit;
        public event EventHandler? ExecuteDelete;
        public event EventHandler? ExecuteMoveUp;
        public event EventHandler? ExecuteMoveDown;


    }

}
