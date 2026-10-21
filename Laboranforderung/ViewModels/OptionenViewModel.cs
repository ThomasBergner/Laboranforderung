using Laboranforderung.Commands;
using Laboranforderung.ViewModels.FileMangement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Laboranforderung.ViewModels
{
    public class OptionenViewModel: INotifyPropertyChanged
    {
        private Options _options;
        private FileManager _fileManager;

        public ICommand OpenFileCommand { get; }

        public ICommand LoadDemoFileCommand { get; }
        public OptionenViewModel()
        {
            _options = Options.Instance;
            _options.PropertyChanged += Options_PropertyChanged;
            _fileManager = FileManager.Instance;
            OpenFileCommand = new RelayCommand(OpenFile, () => AllowChanges);
            LoadDemoFileCommand = new RelayCommand(LoadDemoFile, () => AllowChanges);
        }
        private void LoadDemoFile()
        {
            _fileManager.LoadDemoFile();
        }
        private void OpenFile()
        {
            _fileManager.LoadFile();
        }

        public string FilePath
        {
            get => _options.FilePath;
            set
            {
                    _options.FilePath = value;
            }
        }


        public bool AllowChanges
        {
            get => _options.AllowChanges;
            set
            {
                _options.AllowChanges = value;
            }
        }

        public bool AutoSave
        {
            get => _options.AutoSave;
            set
            {
                _options.AutoSave = value;
            }
        }

        public bool AutoOpen
        {
            get => _options.AutoOpen;
            set
            {
                _options.AutoOpen = value;
            }
        }

        private void Options_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Options.FilePath))
            {
                NotifyPropertyChanged(nameof(FilePath));
            }
            else if (e.PropertyName == nameof(Options.AllowChanges))
            {
                NotifyPropertyChanged(nameof(AllowChanges));
                ((RelayCommand)OpenFileCommand).RaiseCanExecuteChanged();
                ((RelayCommand)LoadDemoFileCommand).RaiseCanExecuteChanged();
            }
            else if (e.PropertyName == nameof(Options.AutoSave))
            {
                NotifyPropertyChanged(nameof(AutoSave));
            }
            else if (e.PropertyName == nameof(Options.AutoOpen))
            {
                NotifyPropertyChanged(nameof(AutoOpen));
            }
 
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
