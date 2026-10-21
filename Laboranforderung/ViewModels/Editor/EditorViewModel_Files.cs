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
        public ICommand ResetFileCommand { get; }
        public ICommand ExportFileCommand { get; }
        public ICommand ImportFileCommand { get; }
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
            _fileManager.EditValue = GetDatensammlung();
            _fileManager.CreateNewFile();
            ReadDatensammlung(_fileManager.EditValue);
        }

        private void OpenFile()
        {
            _fileManager.EditValue = GetDatensammlung();
            _fileManager.LoadFile();
            ReadDatensammlung(_fileManager.EditValue);
        }

        private void SaveFile()
        {
            _fileManager.EditValue = GetDatensammlung();
            _fileManager.Save();
        }

        private void SaveFileAs()
        {
            _fileManager.EditValue = GetDatensammlung();
            _fileManager.SaveAs();
        }

        private void ResetFile()
        {
            _fileManager.EditValue = GetDatensammlung();
           if( _fileManager.ResetChanges())
           {
                // Handle the case when changes were successfully reset
                ReadDatensammlung(_fileManager.EditValue);
           }
        }

        private void ExportFile()
        {
            _fileManager.EditValue = GetDatensammlung();
            _fileManager.Export();
        }

        private void ImportFile()
        {
            _fileManager.EditValue = GetDatensammlung();
            _fileManager.Import();
            ReadDatensammlung(_fileManager.EditValue);
        }
        #endregion
    }
}
