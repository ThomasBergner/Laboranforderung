using Laboranforderung.Commands;
using Laboranforderung.Daten;
using Laboranforderung.Daten.Interfaces;
using Laboranforderung.ViewModels.FileMangement;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Laboranforderung.ViewModels.Editor
{
    public partial class EditorViewModel : INotifyPropertyChanged
    {

        #region Commands
        public ICommand CmdGruppenitemAdd { get; }
        public ICommand CmdGruppenlaborwertitemAdd { get; }
        public ICommand CmdGruppenentryRemove { get; }
        public ICommand CmdGruppenentryMoveUp { get; }
        public ICommand CmdGruppenentryMoveDown { get; }
        public ICommand CmdGruppenentryExpandAll { get; }
        public ICommand CmdGruppenentryCollapseAll { get; }
        public ICommand CmdMoveLaborwertToProfil { get; }
        #endregion

        #region Fields
        private FileManager _fileManager;

        private Options _options;
        private List<Gruppenitem> m_Gruppenitems = new List<Gruppenitem>();
        private List<Gruppenlaborwertitem> m_SortedLaborwerte = new List<Gruppenlaborwertitem>();

        #endregion 

        public void ReadDatensammlung(Datensammlung datensammlung)
        {
            Gruppenentry = null;
            Profilentry = null;
            m_Gruppenitems.Clear();
            m_SortedLaborwerte.Clear();
            m_Gruppenentries.Clear();
            m_Profilentries.Clear();
            m_Profilitems.Clear();
            Dictionary<Laborwert, Gruppenlaborwertitem> laborwertToItemMap = new Dictionary<Laborwert, Gruppenlaborwertitem>();

            foreach (Laborwertgruppe gruppe in datensammlung.Gruppen)
            {
                Gruppenitem item = new Gruppenitem(gruppe.Name, m_Gruppenentries);

                foreach (var laborwert in gruppe.Werte)
                {
                    Gruppenlaborwertitem laborwertitem = new Gruppenlaborwertitem(item, laborwert);
                    item.Children.Add(laborwertitem);
                    laborwertToItemMap[laborwert] = laborwertitem;
                }
                m_Gruppenitems.Add(item);
                m_Gruppenentries.Add(item);
            }
            foreach (Laborprofil profil in datensammlung.Profile)
            {
                Profilitem item = new Profilitem(profil.Profilname, profil.Number, m_Profilentries);
                foreach (var laborwert in profil.Werte)
                {
                    item.Children.Add(new Profillaborwertitem(item, laborwertToItemMap[laborwert]));
                }

                m_Profilitems.Add(item);
                m_Profilentries.Add(item);
            }

            RefreshSortedLaborwerte();
        }

        public Datensammlung GetDatensammlung()
        {
            Datensammlung datensammlung = new Datensammlung();
            int index = 0;
            Dictionary<Gruppenlaborwertitem, Laborwert> clones = new Dictionary<Gruppenlaborwertitem, Laborwert>();

            foreach (Gruppenitem item in m_Gruppenitems)
            {
                List<Laborwert> itemLaborwerte = new List<Laborwert>();
                foreach (Gruppenlaborwertitem laborwertitem in item.Children)
                {
                    laborwertitem.Index = index++;
                    Laborwert clone =  new Laborwert(laborwertitem);
                    itemLaborwerte.Add(clone);
                    clones[laborwertitem] = clone;
                }
                Laborwertgruppe newgruppe = new Laborwertgruppe(item.Name);
                newgruppe.Werte.AddRange(itemLaborwerte);
                datensammlung.Gruppen.Add(newgruppe);
            }
            
            foreach (Profilitem item in m_Profilitems)
            {
                List<Laborwert> itemLaborwerte = new List<Laborwert>();
                foreach (Profillaborwertitem laborwertitem in item.Children)
                {
                    itemLaborwerte.Add(clones[laborwertitem.Wert]);
                }
                itemLaborwerte.Sort(new LaborwertComparer());
                Laborprofil profil = new Laborprofil(item.Profilname, item.Number);
                profil.Werte.AddRange(itemLaborwerte);
                datensammlung.Profile.Add(profil);
            }
            return datensammlung;
        }

        #region Constructor

        public EditorViewModel()
        {
            CmdGruppenitemAdd = new RelayCommand(GruppenitemAdd, () => GruppenitemAddEnabled);
            CmdGruppenlaborwertitemAdd = new RelayCommand(GruppenlaborwertitemAdd, () => GruppenlaborwertitemAddEnabled);
            CmdGruppenentryRemove = new RelayCommand(GruppenentryRemove, () => GruppenentryRemoveEnabled);
            CmdGruppenentryMoveUp = new RelayCommand(GruppenentryMoveUp, () => GruppenentryMoveUpEnabled);
            CmdGruppenentryMoveDown = new RelayCommand(GruppenentryMoveDown, () => GruppenentryMoveDownEnabled);
            CmdGruppenentryExpandAll = new RelayCommand(GruppenentryExpandAll);
            CmdGruppenentryCollapseAll = new RelayCommand(GruppenentryCollapseAll);

            CmdMoveLaborwertToProfil = new RelayCommand(MoveLaborwertToProfil, () => MoveLaborwertToProfilEnabled);
            CmdProfilentryAdd = new RelayCommand(ProfilentryAdd, () => ProfilentryAddEnabled);
            CmdProfilentryRemove = new RelayCommand(ProfilentryRemove, () => ProfilentryRemoveEnabled);
            CmdProfilentryMoveUp = new RelayCommand(ProfilentryMoveUp, () => ProfilentryMoveUpEnabled);
            CmdProfilentryMoveDown = new RelayCommand(ProfilentryMoveDown, () => ProfilentryMoveDownEnabled);
            CmdProfilentryCollapseAll = new RelayCommand(ProfilentryCollapseAll);
            CmdSortieren = new RelayCommand(Sortieren);
            CmdProfilentryExpandAll = new RelayCommand(ProfilentryExpandAll);

            NewFileCommand = new RelayCommand(NewFile);
            OpenFileCommand = new RelayCommand(OpenFile);
            SaveFileCommand = new RelayCommand(SaveFile);
            SaveFileAsCommand = new RelayCommand(SaveFileAs);

            GruppenentryUpdateCommands();
            ProfilentryUpdateCommands();
            _fileManager = FileManager.Instance;
            ReadDatensammlung(_fileManager.Datensammlung.CreateClone());
            _options = Options.Instance;
            ExportFileCommand = new RelayCommand(ExportFile);
            ImportFileCommand = new RelayCommand(ImportFile);
            LoadDemoFileCommand = new RelayCommand(LoadDemoFile);
        }

        #endregion

        #region Events
        public event EventHandler? GruppenentryMoved;
        #endregion

        #region Properties

        private Object? m_SelectedValue = null;
        public Object? SelectedValue
        {
            get => m_SelectedValue;
            set
            {
                if (m_SelectedValue != value)
                {
                    m_SelectedValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Gruppenentry> m_Gruppenentries = new ObservableCollection<Gruppenentry>();
        public ObservableCollection<Gruppenentry> Gruppenentries
        {
            get => m_Gruppenentries;
            //set
            //{
            //    if (m_Gruppenentries != value)
            //    {
            //        Gruppenentry = null;
            //        m_Gruppenentries = value;
            //        NotifyPropertyChanged();
            //        if (m_Gruppenentries != null && m_Gruppenentries.Count > 0) Gruppenentry = m_Gruppenentries[0];
            //        GruppenentryUpdateCommands();
            //    }
            //}
        }
        private Gruppenentry? m_Gruppenentry = null;
        public Gruppenentry? Gruppenentry
        {
            get => m_Gruppenentry;
            set
            {
                if (m_Gruppenentry != value)
                {
                    m_Gruppenentry = value;
                    NotifyPropertyChanged();
                    GruppenentryUpdateCommands();
                    if(m_Gruppenentry == null)
                    {
                        SelectedValue = m_Gruppenentry;
                    }
                    else if (m_Gruppenentry is Gruppenitem gruppenitem)
                    {
                        SelectedValue = gruppenitem;
                    }
                    else if (m_Gruppenentry is Gruppenlaborwertitem laborwertitem)
                    {
                        SelectedValue = laborwertitem;
                    }
                }
            }
        }
        private bool m_GruppenitemAddEnabled = false;
        public bool GruppenitemAddEnabled
        {
            get => m_GruppenitemAddEnabled;
            set
            {
                if (m_GruppenitemAddEnabled != value)
                {
                    m_GruppenitemAddEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdGruppenitemAdd).RaiseCanExecuteChanged();
                }
            }
        }

        private bool m_GruppenlaborwertitemAddEnabled = false;
        public bool GruppenlaborwertitemAddEnabled
        {
            get => m_GruppenlaborwertitemAddEnabled;
            set
            {
                if (m_GruppenlaborwertitemAddEnabled != value)
                {
                    m_GruppenlaborwertitemAddEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdGruppenlaborwertitemAdd).RaiseCanExecuteChanged();
                }
            }
        }

        private bool m_GruppenentryRemoveEnabled = false;
        public bool GruppenentryRemoveEnabled
        {
            get => m_GruppenentryRemoveEnabled;
            set
            {
                if (m_GruppenentryRemoveEnabled != value)
                {
                    m_GruppenentryRemoveEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdGruppenentryRemove).RaiseCanExecuteChanged();
                }
            }
        }
        private bool m_GruppenentryMoveUpEnabled = false;
        public bool GruppenentryMoveUpEnabled
        {
            get => m_GruppenentryMoveUpEnabled;
            set
            {
                if (m_GruppenentryMoveUpEnabled != value)
                {
                    m_GruppenentryMoveUpEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdGruppenentryMoveUp).RaiseCanExecuteChanged();
                }
            }
        }
        private bool m_GruppenentryMoveDownEnabled = false;
        public bool GruppenentryMoveDownEnabled
        {
            get => m_GruppenentryMoveDownEnabled;
            set
            {
                if (m_GruppenentryMoveDownEnabled != value)
                {
                    m_GruppenentryMoveDownEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdGruppenentryMoveDown).RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region Methods

        public void HandleGruppenDrop(Gruppenentry source, Gruppenentry target)
        {
            if (source is Gruppenitem sourcegruppe)
            {
                Gruppenitem targetgruppe = target is Gruppenlaborwertitem laborwertitem ? laborwertitem.Parent : (Gruppenitem)target;
                if (sourcegruppe == targetgruppe) return; // No move needed

                int sourceIndex = m_Gruppenitems.IndexOf(sourcegruppe);
                int targetIndex = m_Gruppenitems.IndexOf(targetgruppe);

                bool isMovingDown = sourceIndex < targetIndex;

                m_Gruppenitems.RemoveAt(sourceIndex);
                m_Gruppenitems.Insert(targetIndex, sourcegruppe);

                bool isSourceExpanded = sourcegruppe.IsExpanded;
                bool isTargetExpanded = targetgruppe.IsExpanded;
                sourcegruppe.IsExpanded = false;
                targetgruppe.IsExpanded = false;

                int sourceFlatIndex = m_Gruppenentries.IndexOf(sourcegruppe);
                int targetFlatIndex = m_Gruppenentries.IndexOf(targetgruppe);

                m_Gruppenentries.Move(sourceFlatIndex, targetFlatIndex);

                sourcegruppe.IsExpanded = isSourceExpanded;
                targetgruppe.IsExpanded = isTargetExpanded;

                RefreshSortedLaborwerte();
                GruppenentryUpdateCommands();
                GruppenentryNotifyMoved();

            }
            else if (source is Gruppenlaborwertitem sourcelaborwertitem)
            {
                Gruppenitem sourceparent = sourcelaborwertitem.Parent;
                Gruppenitem targetparent = target is Gruppenlaborwertitem laborwertitem ? laborwertitem.Parent : (Gruppenitem)target;

                if (sourceparent == targetparent)
                {
                    // Bewegen innerhalb einer expandierten Gruppe
                    if (target is Gruppenlaborwertitem)
                    {
                        int sourceIdx = sourceparent.Children.IndexOf(sourcelaborwertitem);
                        int targetIdx = sourceparent.Children.IndexOf((Gruppenlaborwertitem)target);
                        if (sourceIdx == targetIdx) return; // No move needed

                        sourceparent.IsExpanded = false;
                        sourceparent.Children.RemoveAt(sourceIdx);
                        sourceparent.Children.Insert(targetIdx, sourcelaborwertitem);
                        sourceparent.IsExpanded = true;

                        RefreshSortedLaborwerte();
                        Gruppenentry = sourcelaborwertitem;

                        GruppenentryUpdateCommands();
                        GruppenentryNotifyMoved();
                    }
                    else if (target is Gruppenitem)
                    {
                        // Bewegen auf die Gruppe selbst, also an das Ende der Liste
                        int sourceIdx = sourceparent.Children.IndexOf(sourcelaborwertitem);
                        if (sourceIdx == sourceparent.Children.Count - 1) return; // No move needed
                        sourceparent.IsExpanded = false;
                        sourceparent.Children.RemoveAt(sourceIdx);
                        sourceparent.Children.Add(sourcelaborwertitem);
                        sourceparent.IsExpanded = true;
                        RefreshSortedLaborwerte();
                        Gruppenentry = sourcelaborwertitem;
                        GruppenentryUpdateCommands();
                        GruppenentryNotifyMoved();
                    }
                    return;
                }
                else
                {
                    // Bewegen von der einen zur anderen Gruppe
                    if (target is Gruppenlaborwertitem)
                    {
                        int targetIdx = targetparent.Children.IndexOf((Gruppenlaborwertitem)target);
                        sourceparent.IsExpanded = false;
                        targetparent.IsExpanded = false;
                        sourceparent.Children.Remove(sourcelaborwertitem);
                        targetparent.Children.Insert(targetIdx, sourcelaborwertitem);
                        sourceparent.IsExpanded = true;
                        targetparent.IsExpanded = true;
                        RefreshSortedLaborwerte();
                        Gruppenentry = sourcelaborwertitem;
                        GruppenentryUpdateCommands();
                        GruppenentryNotifyMoved();
                    }
                    else if (target is Gruppenitem)
                    {
                        sourceparent.IsExpanded = false;
                        targetparent.IsExpanded = false;
                        sourceparent.Children.Remove(sourcelaborwertitem);
                        targetparent.Children.Add(sourcelaborwertitem);
                        sourceparent.IsExpanded = true;
                        targetparent.IsExpanded = true;
                        RefreshSortedLaborwerte();
                        Gruppenentry = sourcelaborwertitem;
                        GruppenentryUpdateCommands();
                        GruppenentryNotifyMoved();
                    }
                    return;
                }

            }

        }


        private void RefreshSortedLaborwerte()
        {
            int index = 0;
            m_SortedLaborwerte.Clear();
            foreach (Gruppenitem item in m_Gruppenitems)
            {
                foreach (Gruppenlaborwertitem laborwertitem in item.Children)
                {
                    laborwertitem.Index = index++;
                    m_SortedLaborwerte.Add(laborwertitem);
                }
            }
        }

        private void GruppenentryNotifyMoved()
        {
            EventHandler? moved = GruppenentryMoved;
            if (moved == null) return;
            moved.Invoke(this, new EventArgs());
        }
        private void GruppenentryUpdateCommands()
        {
            GruppenitemAddEnabled = true;
            if (m_Gruppenentry == null)
            {
                GruppenlaborwertitemAddEnabled = false;
                GruppenentryRemoveEnabled = false;
                GruppenentryMoveUpEnabled = false;
                GruppenentryMoveDownEnabled = false;
                MoveLaborwertToProfilEnabled = false;
            }
            else
            {
                GruppenlaborwertitemAddEnabled = true;
                if (m_Gruppenentry is Gruppenitem gruppenitem)
                {
                    GruppenentryRemoveEnabled = gruppenitem.Children.Count == 0;
                    GruppenentryMoveUpEnabled = !(gruppenitem == m_Gruppenitems[0]);
                    GruppenentryMoveDownEnabled = !(gruppenitem == m_Gruppenitems[m_Gruppenitems.Count - 1]);
                    MoveLaborwertToProfilEnabled = false;
                }
                else
                {
                    GruppenentryRemoveEnabled = true;
                    Gruppenlaborwertitem gruppenlaborwertitem = (Gruppenlaborwertitem)m_Gruppenentry;
                    GruppenentryMoveUpEnabled = !(gruppenlaborwertitem == m_SortedLaborwerte[0]);
                    GruppenentryMoveDownEnabled = !(gruppenlaborwertitem == m_SortedLaborwerte[m_SortedLaborwerte.Count - 1]);
                    CheckAddLaborwertToProfil();
                }
            }
        }
        private void GruppenitemAdd()
        {
            Gruppenitem item = new Gruppenitem(CreateNextGruppeName(), m_Gruppenentries);
            m_Gruppenitems.Add(item);
            m_Gruppenentries.Add(item);
            Gruppenentry = item;
            GruppenentryNotifyMoved();
        }

        private string CreateNextGruppeName()
        {
            int index = 1;
            string name;

            do
            {
                name = $"Neue Gruppe {index}";
                index++;
            }
            while (m_Gruppenitems.Any(g => g.Name == name));

            return name;
        }

        public void GruppenlaborwertitemAdd()
        {
            if (m_Gruppenentry == null) throw new InvalidOperationException("Gruppenentry is null");

            Gruppenitem parent;
            if (m_Gruppenentry is Gruppenitem gruppenitem)
            {
                parent = gruppenitem;
            }
            else
            {
                parent = ((Gruppenlaborwertitem)m_Gruppenentry).Parent;
            }


            string newName = CreateNextLaborwertName();
            Laborwert newwert = new Laborwert(newName, newName);

            Gruppenlaborwertitem newitem = new Gruppenlaborwertitem(parent, newwert);
            parent.IsExpanded = false;
            parent.Children.Add(newitem);
            RefreshSortedLaborwerte();
            parent.IsExpanded = true;

            Gruppenentry = newitem;
            GruppenentryNotifyMoved();
        }

        private string CreateNextLaborwertName()
        {
            int index = 1;
            string name;

            do
            {
                name = $"Wert{index}";
                index++;
            }
            while (m_SortedLaborwerte.Any(w => w.Name == name));

            return name;
        }


        private void GruppenentryRemove()
        {
            if (m_Gruppenentry == null) throw new InvalidOperationException("Gruppenentry is null");
            
            Gruppenentry? nextselecteditem = null;

            if (m_Gruppenentry is Gruppenitem gruppenitem)
            {
                if (gruppenitem.Children.Count > 0) throw new InvalidOperationException("Gruppenitem has children, cannot remove.");
                int gruppenindex = m_Gruppenitems.IndexOf(gruppenitem);
                if (gruppenindex > 0)
                    nextselecteditem = m_Gruppenitems[gruppenindex - 1];
                else if (gruppenindex < m_Gruppenitems.Count - 1)
                    nextselecteditem = m_Gruppenitems[gruppenindex + 1];

                Gruppenentry = nextselecteditem;
                m_Gruppenitems.RemoveAt(gruppenindex);
                m_Gruppenentries.Remove(gruppenitem);
            }
            else
            {
                Gruppenlaborwertitem selitem = (Gruppenlaborwertitem)m_Gruppenentry;
                Gruppenitem gruppe = selitem.Parent;
                int laborwertindex = gruppe.Children.IndexOf(selitem);

                if (laborwertindex > 0)
                    nextselecteditem = gruppe.Children[laborwertindex - 1];
                else if (laborwertindex < gruppe.Children.Count - 1)
                    nextselecteditem = gruppe.Children[laborwertindex + 1];
                else
                    nextselecteditem = gruppe;

                Gruppenentry = nextselecteditem;
                gruppe.Children.Remove(selitem);
                m_Gruppenentries.Remove(selitem);
                RefreshSortedLaborwerte();

               // Laborwert sellaborwert = selitem.Wert;

                List<Profillaborwertitem> itemsToRemove = new List<Profillaborwertitem>();
                foreach (Profilitem item in m_Profilitems)
                {
                    itemsToRemove.Clear();
                    foreach (Profillaborwertitem laborwertitem in item.Children)
                    {
                        if (laborwertitem.Wert == selitem)
                        {
                            itemsToRemove.Add(laborwertitem);
                        }

                    }
                    foreach (Profillaborwertitem laborwertitem in itemsToRemove)
                    {
                        item.Children.Remove(laborwertitem);
                        if (item.IsExpanded)
                        {
                            m_Profilentries.Remove(laborwertitem);
                        }
                    }

                }


           
            }

            ProfilentryUpdateCommands();
            GruppenentryUpdateCommands();
        }


        private void GruppenentryMoveUp()
        {
            if (m_Gruppenentry == null) throw new InvalidOperationException("Gruppenentry is null");
            int flatindex = m_Gruppenentries.IndexOf(m_Gruppenentry);
           
            if (m_Gruppenentry is Gruppenitem gruppenitem)
            {
                int gruppenindex = m_Gruppenitems.IndexOf(gruppenitem);
                Gruppenitem previtem = m_Gruppenitems[gruppenindex - 1];

                int prevflatindex = m_Gruppenentries.IndexOf(previtem);
                m_Gruppenitems.RemoveAt(gruppenindex);
                m_Gruppenitems.Insert(gruppenindex - 1, gruppenitem);
                
                
                m_Gruppenentries.Move(flatindex, prevflatindex);


                if(gruppenitem.IsExpanded)
                {
                    foreach (Gruppenlaborwertitem laborwertitem in gruppenitem.Children)
                    {
                        int laborwertflatindex = m_Gruppenentries.IndexOf(laborwertitem);
                        m_Gruppenentries.Move(laborwertflatindex, prevflatindex + 1);
                        prevflatindex++;
                    }
                }
            }
            else
            {
                Gruppenlaborwertitem selitem = (Gruppenlaborwertitem)m_Gruppenentry;
                Gruppenitem gruppe = selitem.Parent;
                int laborwertindex = gruppe.Children.IndexOf(selitem);

                if(laborwertindex > 0)
                {
                    gruppe.Children.Move(laborwertindex, laborwertindex-1);
                    m_Gruppenentries.Move(flatindex, flatindex-1);
                }
                else
                {
                    int gruppenindex = m_Gruppenitems.IndexOf(gruppe);
                    Gruppenitem previtem = m_Gruppenitems[gruppenindex - 1];
                    
                    gruppe.Children.Remove(selitem);
                    m_Gruppenentries.Remove(selitem);
                    previtem.IsExpanded = false;
                    previtem.Children.Add(selitem);
                    previtem.IsExpanded = true;

                    Gruppenentry = selitem;
                }
            }

            RefreshSortedLaborwerte();
            GruppenentryUpdateCommands();
            GruppenentryNotifyMoved();
        }
        private void GruppenentryMoveDown()
        {
            if (m_Gruppenentry == null) throw new InvalidOperationException("Gruppenentry is null");
            int flatindex = m_Gruppenentries.IndexOf(m_Gruppenentry);

            if (m_Gruppenentry is Gruppenitem gruppenitem)
            {

                int gruppenindex = m_Gruppenitems.IndexOf(gruppenitem);
                Gruppenitem nextitem = m_Gruppenitems[gruppenindex + 1];
                int nextflatindex = m_Gruppenentries.IndexOf(nextitem);
                m_Gruppenitems.RemoveAt(gruppenindex);
                m_Gruppenitems.Insert(gruppenindex + 1, gruppenitem);


                m_Gruppenentries.Move(nextflatindex, flatindex);
                if(nextitem.IsExpanded)
                {
                    foreach (Gruppenlaborwertitem laborwertitem in nextitem.Children)
                    {
                        int laborwertflatindex = m_Gruppenentries.IndexOf(laborwertitem);
                        m_Gruppenentries.Move(laborwertflatindex, flatindex + 1);
                        flatindex++;
                    }
                }
            }
            else
            {
                Gruppenlaborwertitem selitem = (Gruppenlaborwertitem)m_Gruppenentry;
                Gruppenitem gruppe = selitem.Parent;
                int laborwertindex = gruppe.Children.IndexOf(selitem);


                if (laborwertindex < gruppe.Children.Count - 1)
                {
                    gruppe.Children.Move(laborwertindex, laborwertindex + 1);
                    m_Gruppenentries.Move(flatindex, flatindex + 1);
                }
                else
                {
                    int gruppenindex = m_Gruppenitems.IndexOf(gruppe);
                    Gruppenitem nextitem = m_Gruppenitems[gruppenindex + 1];
                    gruppe.Children.Remove(selitem);
                    m_Gruppenentries.Remove(selitem);
                    nextitem.IsExpanded = false;
                    nextitem.Children.Insert(0, selitem);
                    nextitem.IsExpanded = true;
                    Gruppenentry = selitem;
                }
            }

            RefreshSortedLaborwerte();
            GruppenentryUpdateCommands();
            GruppenentryNotifyMoved();
        }

        public void GruppenentryCollapseAll()
        {
            foreach (Gruppenitem item in m_Gruppenitems)
            {
                item.IsExpanded = false;
            }
        }
        public void GruppenentryExpandAll()
        {
            foreach (Gruppenitem item in m_Gruppenitems)
            {
                item.IsExpanded = true;
            }
        }
        public void MoveLaborwertToProfil()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region INotifyPropertyChanged
        public ConfirmationDelegate DeleteRequest { private get; set; } = message => true;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
