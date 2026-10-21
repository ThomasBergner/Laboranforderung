using Laboranforderung.Commands;
using Laboranforderung.Daten;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Versioning;
using System.Windows.Input;

namespace Laboranforderung.ViewModels.Editor
{
    public partial class EditorViewModel : INotifyPropertyChanged
    {
        
        #region Commands
        public ICommand CmdProfilentryAdd { get; }
        public ICommand CmdProfilentryRemove { get; }
        public ICommand CmdProfilentryMoveUp { get; }
        public ICommand CmdProfilentryMoveDown { get; }
        public ICommand CmdProfilentryCollapseAll { get; }
        public ICommand CmdSortieren { get; }
        public ICommand CmdProfilentryExpandAll { get; }
        #endregion
        
        #region Constructor
        

        
        #endregion
        
        #region Events
        public event EventHandler? ProfilentryMoved;
        #endregion
        
        #region Properties
        private ObservableCollection<Profilentry> m_Profilentries = new ObservableCollection<Profilentry>();
        private List<Profilitem> m_Profilitems = new List<Profilitem>();

        public ObservableCollection<Profilentry> Profilentries
        {
            get => m_Profilentries;
            //set
            //{
            //    if (m_Profilentries != value)
            //    {
            //        Profilentry = null;
            //        m_Profilentries = value;
            //        NotifyPropertyChanged();
            //        if (m_Profilentries != null && m_Profilentries.Count > 0) Profilentry = m_Profilentries[0];
            //        ProfilentryUpdateCommands();
            //    }
            //}
        }
        private Profilentry? m_Profilentry = null;
        public Profilentry? Profilentry
        {
            get => m_Profilentry;
            set
            {
                if (m_Profilentry != value)
                {
                    m_Profilentry = value;
                    NotifyPropertyChanged();
                    ProfilentryUpdateCommands();
                    if(m_Profilentry == null)
                    {
                        SelectedValue = null;
                    }
                    else if (m_Profilentry is Profilitem profilitem)
                    {
                        SelectedValue = profilitem;
                    }
                    else if (m_Profilentry is Profillaborwertitem profillaborwertitem)
                    {
                        SelectedValue = profillaborwertitem.Wert;
                    }
                }
            }
        }

        private bool m_MoveLaborwertToProfilEnabled = false;
        public bool MoveLaborwertToProfilEnabled
        {
            get => m_MoveLaborwertToProfilEnabled;
            set
            {
                if (m_MoveLaborwertToProfilEnabled != value)
                {
                    m_MoveLaborwertToProfilEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdMoveLaborwertToProfil).RaiseCanExecuteChanged();
                }
            }
        }

        private bool m_ProfilentryAddEnabled = false;
        public bool ProfilentryAddEnabled
        {
            get => m_ProfilentryAddEnabled;
            set
            {
                if (m_ProfilentryAddEnabled != value)
                {
                    m_ProfilentryAddEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdProfilentryAdd).RaiseCanExecuteChanged();
                }
            }
        }
        private bool m_ProfilentryRemoveEnabled = false;
        public bool ProfilentryRemoveEnabled
        {
            get => m_ProfilentryRemoveEnabled;
            set
            {
                if (m_ProfilentryRemoveEnabled != value)
                {
                    m_ProfilentryRemoveEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdProfilentryRemove).RaiseCanExecuteChanged();
                }
            }
        }
        private bool m_ProfilentryMoveUpEnabled = false;
        public bool ProfilentryMoveUpEnabled
        {
            get => m_ProfilentryMoveUpEnabled;
            set
            {
                if (m_ProfilentryMoveUpEnabled != value)
                {
                    m_ProfilentryMoveUpEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdProfilentryMoveUp).RaiseCanExecuteChanged();
                }
            }
        }
        private bool m_ProfilentryMoveDownEnabled = false;
        public bool ProfilentryMoveDownEnabled
        {
            get => m_ProfilentryMoveDownEnabled;
            set
            {
                if (m_ProfilentryMoveDownEnabled != value)
                {
                    m_ProfilentryMoveDownEnabled = value;
                    NotifyPropertyChanged();
                    ((RelayCommand)CmdProfilentryMoveDown).RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region Methods
        public void HandleGruppenLaborwertDropToProfil(Gruppenlaborwertitem source, Profilentry target)
        {
            Profilitem targetprofil = target is Profilitem profilitem ? profilitem : ((Profillaborwertitem)target).Parent;
            if (!CanAddLaborwertToProfil(source, targetprofil)) return; // No action needed if the target Profilitem already contains the Laborwert

            targetprofil.IsExpanded = false;
            Profillaborwertitem newitem = new Profillaborwertitem(targetprofil, source);
            targetprofil.Children.Add(newitem);
            targetprofil.IsExpanded = true;
            Profilentry = newitem;

            ProfilentryUpdateCommands();
            ProfilentryNotifyMoved();
        }

        public void HandleProfilDrop(Profilentry source, Profilentry target)
        {
            Profilitem targetprofil = target is Profilitem profilitem ? profilitem : ((Profillaborwertitem)target).Parent;
            if(source is Profilitem sourceitem)
            {
                // Handle the case where the source is a Profilitem
                if(sourceitem == targetprofil) return; // No action needed if the source and target are the same)
                int sourceindex = m_Profilitems.IndexOf(sourceitem);
                int targetindex = m_Profilitems.IndexOf(targetprofil);

                m_Profilitems.RemoveAt(sourceindex);
                m_Profilitems.Insert(targetindex, sourceitem);
                bool isSourceExpanded = sourceitem.IsExpanded;
                bool isTargetExpanded = targetprofil.IsExpanded;
                sourceitem.IsExpanded = false;
                targetprofil.IsExpanded = false;

                int sourceFlatIndex = m_Profilentries.IndexOf(sourceitem);
                int targetFlatIndex = m_Profilentries.IndexOf(targetprofil);

                m_Profilentries.Move(sourceFlatIndex, targetFlatIndex);

                sourceitem.IsExpanded = isSourceExpanded;
                targetprofil.IsExpanded = isTargetExpanded;

                ProfilentryUpdateCommands();
                ProfilentryNotifyMoved();

            }
            else if(source is Profillaborwertitem sourcelaborwertitem)
            {
                // Handle the case where the source is a Profillaborwertitem
                Profilitem sourceprofil = sourcelaborwertitem.Parent;
                if(sourceprofil == targetprofil) return; // No action needed if the source and target are in the same Profilitem    
               
                if(!CanAddLaborwertToProfil(sourcelaborwertitem.Wert, targetprofil)) return; // No action needed if the target Profilitem already contains the Laborwert

                sourceprofil.IsExpanded = false;
                targetprofil.IsExpanded = false;

                sourceprofil.Children.Remove(sourcelaborwertitem);
                targetprofil.Children.Add(sourcelaborwertitem);
                sourceprofil.IsExpanded = true;
                targetprofil.IsExpanded = true;

                Profilentry= sourcelaborwertitem;

                ProfilentryUpdateCommands();
                ProfilentryNotifyMoved();


            }
        }



        private void ProfilentryNotifyMoved()
        {
            EventHandler? moved = ProfilentryMoved;
            if (moved == null) return;
            moved.Invoke(this, new EventArgs());
        }
        private void ProfilentryUpdateCommands()
        {
            ProfilentryAddEnabled = true;
            if (m_Profilentry == null)
            {
                ProfilentryRemoveEnabled = false;
                ProfilentryMoveUpEnabled = false;
                ProfilentryMoveDownEnabled = false;
                MoveLaborwertToProfilEnabled = false;
            }
            else
            {
                ProfilentryRemoveEnabled = true;
                if (m_Profilentry is Profilitem profilitem)
                {
                    ProfilentryMoveUpEnabled = !(profilitem == m_Profilitems[0]);
                    ProfilentryMoveDownEnabled = !(profilitem == m_Profilitems[m_Profilitems.Count - 1]);
                }
                else
                {
                    ProfilentryMoveUpEnabled = false;
                    ProfilentryMoveDownEnabled = false;
                }
                CheckAddLaborwertToProfil();
            }
        }


        private void CheckAddLaborwertToProfil()
        {

            if(m_Profilentry != null && m_Gruppenentry is Gruppenlaborwertitem gruppenlaborwertitem)
            {
                Profilitem affectedprofilitem = m_Profilentry is Profilitem profilitem ? profilitem : ((Profillaborwertitem)m_Profilentry).Parent;
                MoveLaborwertToProfilEnabled = CanAddLaborwertToProfil(gruppenlaborwertitem, affectedprofilitem);
                return;
            }
            MoveLaborwertToProfilEnabled = false;

        }


        private bool CanAddLaborwertToProfil(Gruppenlaborwertitem laborwert, Profilitem profilitem)
        {
            foreach (var item in profilitem.Children)
            {
                if (item.Wert == laborwert)
                    return false;
            }
            
            return true;
        }

        private void ProfilentryAdd()
        {
            Profilitem profilitem = new Profilitem(CreateNextProfilName(), CreateNextProfilNumber(), m_Profilentries);
            m_Profilentries.Add(profilitem);
            m_Profilitems.Add(profilitem);
            Profilentry = profilitem;
            ProfilentryNotifyMoved();
        }

        private string CreateNextProfilName()
        {
            int index = 1;
            string name;

            do
            {
                name = $"Neues Profil {index}";
                index++;
            }
            while (m_Profilitems.Any(p => p.Profilname == name));

            return name;
        }

        private string CreateNextProfilNumber()
        {
            int index = 1;
            string number;
            do
            {
                number = index.ToString();
                index++;
            }
            while (m_Profilitems.Any(p => p.Number == number));
            return number;
        }


        private void ProfilentryRemove()
        {
            if(m_Profilentry == null) throw new Exception("ProfilentryRemove called with null Profilentry");
            
            Profilentry? nextselecteditem = null;

            if(m_Profilentry is Profillaborwertitem profillaborwertitem)
            {
                Profilitem parent = profillaborwertitem.Parent;
                int index = parent.Children.IndexOf(profillaborwertitem);
                if(index > 0)
                    nextselecteditem = parent.Children[index - 1];
                else if (index < parent.Children.Count - 1)
                    nextselecteditem = parent.Children[index + 1];
                else
                    nextselecteditem = parent;
                Profilentry = nextselecteditem;

                parent.Children.Remove(profillaborwertitem);
                m_Profilentries.Remove(profillaborwertitem);
            }
            else
            {
                Profilitem item = (Profilitem)m_Profilentry;
                int index = m_Profilitems.IndexOf(item);

                if(index > 0)
                    nextselecteditem = m_Profilitems[index - 1];
                else if (index < m_Profilitems.Count - 1)
                    nextselecteditem = m_Profilitems[index + 1];
                else
                    nextselecteditem = null;

                Profilentry = nextselecteditem;

                m_Profilentries.Remove(item);
                m_Profilitems.Remove(item);
            }

            ProfilentryUpdateCommands();
        }
        private void ProfilentryMoveUp()
        {
            if (m_Profilentry == null) throw new Exception("ProfilentryMoveUp called with null Profilentry");
            if (m_Profilentry is Profillaborwertitem) throw new Exception("ProfilentryMoveUp called with Profillaborwertitem");


            int flatindex = m_Profilentries.IndexOf(m_Profilentry);

            Profilitem profilitem = (Profilitem)m_Profilentry;

            int profilindex = m_Profilitems.IndexOf(profilitem);
            Profilitem previtem = m_Profilitems[profilindex - 1];

            int prevflatindex = m_Profilentries.IndexOf(previtem);
            m_Profilitems.RemoveAt(profilindex);
            m_Profilitems.Insert(profilindex - 1, profilitem);

            m_Profilentries.Move(flatindex, prevflatindex);

            if(profilitem.IsExpanded)
            {
                foreach (var child in profilitem.Children)
                {
                    int childflatindex = m_Profilentries.IndexOf(child);
                    m_Profilentries.Move(childflatindex, prevflatindex + 1);
                    prevflatindex++;
                }
            }

            ProfilentryUpdateCommands();
            ProfilentryNotifyMoved();
        }
        private void ProfilentryMoveDown()
        {
            if (m_Profilentry == null) throw new Exception("ProfilentryMoveDown called with null Profilentry");
            if (m_Profilentry is Profillaborwertitem) throw new Exception("ProfilentryMoveDown called with Profillaborwertitem");

            int flatindex = m_Profilentries.IndexOf(m_Profilentry);

            Profilitem profilitem = (Profilitem)m_Profilentry;

            int profilindex = m_Profilitems.IndexOf(profilitem);

            Profilitem nextitem = m_Profilitems[profilindex + 1];

            int nextflatindex = m_Profilentries.IndexOf(nextitem);


            m_Profilitems.RemoveAt(profilindex);
            m_Profilitems.Insert(profilindex + 1, profilitem);

            m_Profilentries.Move(nextflatindex, flatindex);

            if(nextitem.IsExpanded)
            {
                foreach (Profillaborwertitem profillaborwertitem in nextitem.Children)
                {
                    int laborwertflatindex = m_Profilentries.IndexOf(profillaborwertitem);
                    m_Profilentries.Move(laborwertflatindex, flatindex + 1);
                    flatindex++;
                }
            }

            ProfilentryUpdateCommands();
            ProfilentryNotifyMoved();
        }
        public void Sortieren()
        {
            foreach (Profilitem profilitem in m_Profilitems)
            {
                if(profilitem.Children.Count > 1)
                {
                    int profilitemflatindex = m_Profilentries.IndexOf(profilitem);

                    List<Profillaborwertitem> sortedChildren = profilitem.Children.OrderBy(c => c.Wert.Index).ToList();
                    
                    for (int i = sortedChildren.Count - 1; i >= 1; i-- )
                    {
                        Profillaborwertitem child = sortedChildren[i];
                        int currentindex = profilitem.Children.IndexOf(child);
                        if(currentindex != i)
                        {
                            profilitem.Children.Move(currentindex, i);
                            if(profilitem.IsExpanded)
                            {
                                int oldflatindex = m_Profilentries.IndexOf(child);
                                int delta = i - currentindex;
                                m_Profilentries.Move(oldflatindex, oldflatindex + delta);
                            }
                        }
                    }
                }
            }
        }
        public void ProfilentryCollapseAll()
        {
            foreach (Profilitem profilitem in m_Profilitems)
            {
                profilitem.IsExpanded = false;
            }
        }
        public void ProfilentryExpandAll()
        {
            foreach(Profilitem profilitem in m_Profilitems)
            {
                profilitem.IsExpanded = true;
            }
        }
        #endregion
        

    }
}
