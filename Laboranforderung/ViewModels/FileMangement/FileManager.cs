using Laboranforderung.Daten;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Laboranforderung.ViewModels.FileMangement
{
    public class FileManager : INotifyPropertyChanged
    {

        private static FileManager? _instance;
        private Datensammlung? _EditValue;
        private Datensammlung? _DisplayClone;

        private IMessageReplyer? _messageReplyer;

        private bool _isDirty;
        public static FileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileManager();
                }
                return _instance;
            }
        }

        public IMessageReplyer? MessageReplyer
        {
            get => _messageReplyer;
            set
            {
                if (_messageReplyer != value)
                {
                    _messageReplyer = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Datensammlung EditValue
        {
            get
            {
                if (_EditValue == null)
                {
                    _EditValue = LoadInitialDatensammlung();
                }
                return _EditValue;
            }
            set
            {
                if (_EditValue != value)
                {
                    _EditValue = value;
                    NotifyPropertyChanged();

                    IsDirty = !DisplayClone.Equals(_EditValue);
                }
            }
        }

        public Datensammlung DisplayClone
        {
            get
            {
                if (_DisplayClone == null)
                {
                    _DisplayClone = EditValue.CreateClone();
                }
                return _DisplayClone;
            }
            private set
            {
                if (_DisplayClone != value)
                {
                    _DisplayClone = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    NotifyPropertyChanged();
                }
            }
        }

        internal void Export()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {
                    ExportImport.ExportToCsv(filePath, EditValue);
                    ShowMessage("Daten erfolgreich exportiert.", "Export erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Exportieren der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        internal void Import()
        {
            if (!DirtyValuesSaved())
                return;
            
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {

                    Datensammlung loadedData = ExportImport.ImportFromCsv(filePath);
                    EditValue = loadedData;
                    DisplayClone = EditValue.CreateClone(); // Update the main collection after loading
                    Options.Instance.FilePath = ""; // Update the file path in options
                    IsDirty = false; // Reset the dirty flag after loading

                    ShowMessage("Daten erfolgreich importiert.", "Import erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Importieren der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool DirtyValuesSaved()
        {
            if (IsDirty)
            {
                var result = ShowMessage("Es gibt ungespeicherte Änderungen. Möchten Sie die Änderungen speichern?", "Ungespeicherte Änderungen", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                    return false;
                if (result == MessageBoxResult.Yes)
                {
                    var saveResult = Save();
                    if (saveResult != SaveResults.Success)
                        return false;
                }
            }
            return true;
        }


        public void CreateNewFile()
        {
            if(!DirtyValuesSaved())
                return;

            EditValue = new Datensammlung();
            DisplayClone = EditValue.CreateClone();
            Options.Instance.FilePath = string.Empty; // Reset the file path in options
            IsDirty = false;
        }

        public void LoadDemoFile()
        {
            if (!DirtyValuesSaved())
                return;

            EditValue = new DemoDatensammlung();
            DisplayClone = EditValue.CreateClone();
            Options.Instance.FilePath = string.Empty; // Reset the file path in options
            IsDirty = false;
        }

        public SaveResults Save()
        {
            if (string.IsNullOrEmpty(Options.Instance.FilePath))
            {
                return SaveAs();
            }
            else
            {
                if (!IsDirty) return SaveResults.Success; // No changes to save
                try
                {
                    XmlReaderWriter.Write(EditValue, Options.Instance.FilePath);
                    DisplayClone = EditValue.CreateClone(); // Update the main collection after saving
                    IsDirty = false; // Reset the dirty flag after saving
                    return SaveResults.Success;
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Speichern der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return SaveResults.Failure;
                }
            }
        }

        public SaveResults SaveAs()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";


            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {
                    bool wasDirty = IsDirty; // Store the current dirty state
                    XmlReaderWriter.Write(EditValue, filePath);
                    if (wasDirty)
                    {
                        DisplayClone = EditValue.CreateClone(); // Update the main collection after saving
                    }
                    Options.Instance.FilePath = filePath; // Update the file path in options
                    IsDirty = false; // Reset the dirty flag after saving
                    return SaveResults.Success;
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Speichern der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return SaveResults.Failure;
                }
            }

            return SaveResults.Cancelled;
        }

        public void LoadFile()
        {
            if (!DirtyValuesSaved())
                return;

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {
                    Datensammlung loadedData = new Datensammlung();
                    XmlReaderWriter.Read(loadedData, filePath);
                    EditValue = loadedData;
                    DisplayClone = EditValue.CreateClone(); // Update the main collection after loading
                    Options.Instance.FilePath = filePath; // Update the file path in options
                    IsDirty = false; // Reset the dirty flag after loading
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Laden der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public bool ResetChanges()
        {
            if (IsDirty)
            {
                var result = ShowMessage("Es gibt ungespeicherte Änderungen. Möchten Sie die Änderungen verwerfen?", "Ungespeicherte Änderungen", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    return false;
            }
            EditValue = DisplayClone.CreateClone(); // Reset to the last saved state
            IsDirty = false; // Reset the dirty flag
            return true;
        }

        public bool EditorWindow_Closing_Cancelled()
        {
            if (IsDirty)
            {
                if (Options.Instance.AutoSave)
                {
                    var saveResult = Save();
                    if (saveResult != SaveResults.Success)
                        return true; // Cancel the closing if save failed
                }
                else
                {
                    var result = ShowMessage("Es gibt ungespeicherte Änderungen. Möchten Sie die Änderungen speichern?", "Ungespeicherte Änderungen", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Cancel)
                        return true; // Cancel the closing
                    if (result == MessageBoxResult.Yes)
                    {
                        var saveResult = Save();
                        if (saveResult != SaveResults.Success)
                            return true; // Cancel the closing if save failed
                    }
                    if (result == MessageBoxResult.No)
                    {
                        EditValue = DisplayClone.CreateClone();
                        IsDirty = false; // Reset the dirty flag
                        return false; // Allow the closing
                    }
                }
            }
            return false; // Allow the closing
        }

        private Datensammlung LoadInitialDatensammlung()
        {
            // Implement the logic to load the initial Datensammlung from a file or database
            // For example, you can read from a JSON file or a database and deserialize it into a Datensammlung object
            // Here, we'll just return a new instance for demonstration purposes

            if (Options.Instance.AutoOpen && !string.IsNullOrEmpty(Options.Instance.FilePath))
            {
                string FilePath = Options.Instance.FilePath;
                if (System.IO.File.Exists(FilePath))
                {
                    try
                    {
                        Datensammlung result = new Datensammlung();
                        XmlReaderWriter.Read(result, FilePath);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, e.g., log the error
                        ShowMessage("Fehler beim Öffnen der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        Options.Instance.FilePath = string.Empty; // Reset the file path in options
                        return new Datensammlung();
                    }
                }
            }


            // If AutoOpen is false or FilePath is empty, return a new Datensammlung
            Options.Instance.FilePath = string.Empty;
            return new Datensammlung();
        }

        private MessageBoxResult ShowMessage(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            if (MessageReplyer != null)
            {
                return MessageReplyer.ReplyMessage(messageBoxText, caption, button, icon);
            }
            else
            {
                return MessageBox.Show(messageBoxText, caption, button, icon);
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
