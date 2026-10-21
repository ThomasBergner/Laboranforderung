using Laboranforderung.Daten;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Laboranforderung.ViewModels.FileMangement
{
    public class FileManager : INotifyPropertyChanged
    {

        #region Fields
        private static FileManager? _instance;
        private IMessageReplyer? _messageReplyer;

        private IDatensammlungEditSource? _editSource;
        private Datensammlung? _Datensammlung;
        #endregion

        #region Properties

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
                    _messageReplyer = value;
            }
        }
        public IDatensammlungEditSource? EditSource
        {
            get => _editSource;
            set
            {
                _editSource = value;
            }
        }


        public Datensammlung Datensammlung
        {
            get { return _Datensammlung ??= LoadInitialDatensammlung(); }
            private set
            {
                if (_Datensammlung != value && !Datensammlung.Equals(value))
                {
                    _Datensammlung = value;
                    NotifyPropertyChanged();
                }
            }
        }


        #endregion

        #region Methods

        public bool EditorWindow_Closing_Cancelled()
        {
            if (!ChangesSavedAccepted()) return true; // User canceled the operation
            return false;
        }
        public void ExportCsvFile()
        {
            Datensammlung? editedValue = GetEditedValue();
            if (editedValue == null) return;

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {
                    ExportImport.ExportToCsv(filePath, editedValue);
                    ShowMessage("Daten erfolgreich exportiert.", "Export erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Exportieren der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void ImportCsvFile()
        {
            if (!ChangesSavedAccepted()) return; // User canceled the operation

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {

                    Datensammlung newValue = ExportImport.ImportFromCsv(filePath);
                    Datensammlung = newValue;
                    Options.Instance.FilePath = string.Empty; // Reset the file path in options
                    EditSource?.SetEditDatensammlung(newValue.CreateClone());
                    ShowMessage("Daten erfolgreich importiert.", "Import erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Importieren der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }



        public void CreateNewFile()
        {
            if (!ChangesSavedAccepted()) return; // User canceled the operation
            Datensammlung newValue= new Datensammlung();
            Datensammlung = newValue;
            Options.Instance.FilePath = string.Empty; // Reset the file path in options
            EditSource?.SetEditDatensammlung(newValue.CreateClone());
        }

        public void LoadFile()
        {
            if (!ChangesSavedAccepted()) return; // User canceled the operation
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {
                    Datensammlung result = new Datensammlung();
                    XmlReaderWriter.Read(result, filePath);
                    Datensammlung = result;
                    Options.Instance.FilePath = filePath; // Update the file path in options
                    EditSource?.SetEditDatensammlung(result.CreateClone());
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    ShowMessage("Fehler beim Öffnen der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void LoadDemoFile()
        {
            if(!ChangesSavedAccepted()) return; // User canceled the operation


            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Laboranforderung.Demodaten.xml";
            using Stream? resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream is not null)
            {
                try
                {
                    Datensammlung result = new Datensammlung();
                    XmlReaderWriter.Read(result, resourceStream);
                    Datensammlung = result;
                    Options.Instance.FilePath = string.Empty; // Reset the file path in options
                    EditSource?.SetEditDatensammlung(result.CreateClone());

                }
                catch (Exception ex)
                {
                    // vorhandenes Fehlerhandling verwenden
                    throw new InvalidOperationException($"Fehler beim Lesen der eingebetteten Ressource '{resourceName}'.", ex);
                }
            }
            else
            {
                ShowMessage("Demo-Datei nicht gefunden: " + resourceName, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ChangesSavedAccepted()
        {
            Datensammlung? editedValue = GetEditedValue();
            if (editedValue == null) return false;


            if (!Datensammlung.Equals(editedValue))
            {
                MessageBoxResult result = ShowMessage("Es gibt ungespeicherte Änderungen. Möchten Sie die Änderungen speichern?", "Ungespeicherte Änderungen", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SaveResults saveResult = Save(editedValue);
                    return saveResult == SaveResults.Success;
                }
                else if (result == MessageBoxResult.No)
                {
                    return true; // Discard changes
                }
                else
                {
                    return false; // Cancel the operation
                }
            }
            return true; // No changes to save
        }




        public SaveResults Save(Datensammlung? editedValue = null)
        {
            if (editedValue == null) editedValue = GetEditedValue();
            if (editedValue == null) return SaveResults.Failure;


            if (string.IsNullOrEmpty(Options.Instance.FilePath))
            {
                return SaveAs(editedValue);
            }
            else
            {
                if (Datensammlung.Equals(editedValue)) return SaveResults.Success; // No changes to save
                
                try
                {
                    XmlReaderWriter.Write(editedValue, Options.Instance.FilePath);
                    Datensammlung = editedValue.CreateClone(); // Update the main collection after saving
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


        public SaveResults SaveAs(Datensammlung? editedValue = null)
        {
            if (editedValue == null) editedValue = GetEditedValue();
            if (editedValue == null) return SaveResults.Failure;
            
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";


            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                try
                {
                    XmlReaderWriter.Write(editedValue, filePath);
                    Datensammlung = editedValue.CreateClone(); // Update the main collection after saving
                    Options.Instance.FilePath = filePath; // Update the file path in options
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

        private Datensammlung? GetEditedValue()
        {
            if (EditSource != null)
            {
                return EditSource.GetEditedDatensammlung();
            }
            else
            {
                return this.Datensammlung;
            }
        }



        #endregion 



        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
