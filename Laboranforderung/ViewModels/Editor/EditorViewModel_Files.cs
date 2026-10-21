using Laboranforderung.ViewModels.FileMangement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Laboranforderung.ViewModels.Editor
{
    public partial class EditorViewModel : INotifyPropertyChanged
    {
        #region Commands
        public ICommand NewFileCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand SaveFileAsCommand { get; }
        public ICommand ExportFileCommand { get; }
        public ICommand ImportFileCommand { get; }
        public ICommand LoadDemoFileCommand { get; }
        #endregion

        #region Properties
        public bool AutoSave
        {
            get => _options.AutoSave;
            set
            {
                _options.AutoSave = value;
                NotifyPropertyChanged(nameof(AutoSave));
            }
        }
        public bool AutoOpen
        {
            get => _options.AutoOpen;
            set
            {
                _options.AutoOpen = value;
                NotifyPropertyChanged(nameof(AutoOpen));
            }
        }


        #endregion


        #region Methods

        private void NewFile()
        {
            _fileManager.CreateNewFile();
        }

        private void OpenFile()
        {
            _fileManager.LoadFile();
        }

        private void LoadDemoFile()
        {
            _fileManager.LoadDemoFile();
        }

        private void SaveFile()
        {
            _fileManager.Save();
        }

        private void SaveFileAs()
        {
            _fileManager.SaveAs();
        }

        private void ExportFile()
        {
            _fileManager.ExportCsvFile();
        }

        private void ImportFile()
        {
            _fileManager.ImportCsvFile();
        }
        #endregion
    }
}
