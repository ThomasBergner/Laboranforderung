using Laboranforderung.Daten;
using Laboranforderung.Daten.Interfaces;
using System.Collections.ObjectModel;

namespace Laboranforderung.ViewModels.Editor
{
    public class Profilitem: Profilentry, ILaborprofil
    {
        #region Fields
        private ObservableCollection<Profilentry> m_FlatItems;
        private ObservableCollection<Profillaborwertitem> m_Children;
        private string m_Profilname;
        private string m_Number;
        #endregion

        #region Constructor

        public Profilitem(string profilname, string number, ObservableCollection<Profilentry> flatItems)
        {
            m_Profilname = profilname;
            m_Number = number;
            m_FlatItems = flatItems;
            m_Children = new ObservableCollection<Profillaborwertitem>();
            m_Children.CollectionChanged += Children_CollectionChanged;
            HasChildren = false;
        }

        #endregion

        #region Properties
        public string Profilname
        {
            get => m_Profilname;
            set
            {
                if (m_Profilname != value)
                {
                    m_Profilname = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Number
        {
            get => m_Number;
            set
            {
                if (m_Number != value)
                {
                    m_Number = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        public override string Name
        {
            get { return $"{m_Number} - {m_Profilname}"; }
            set { }
        }

        public ObservableCollection<Profillaborwertitem> Children => m_Children;
        #endregion

        #region Methods
        
        private void Children_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasChildren = m_Children.Count > 0;
            if(e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is Profillaborwertitem laborwertItem)
                    {
                        laborwertItem.Parent = this;
                    }
                }
            }
        }
        internal override void OnIsExpandedChanged()
        {
            if (m_Children.Count == 0) return;

            int MyIndex = m_FlatItems.IndexOf(this);

            if (IsExpanded)
            {
                int i = 1;
                foreach (var item in m_Children)
                {
                    m_FlatItems.Insert(MyIndex + i, item);
                    i++;
                }
            }
            else
            {
                int index = MyIndex + 1;
                foreach (var item in m_Children)
                {
                    m_FlatItems.RemoveAt(index);
                }
            }

        }

        #endregion
      
    }
}
