using Laboranforderung.Daten;
using Laboranforderung.Daten.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;

namespace Laboranforderung.ViewModels.Editor
{
    public class Gruppenitem : Gruppenentry, ILaborwertgruppe
    {
        #region Fields
        private ObservableCollection<Gruppenentry> m_FlatItems;

        private Laborwertgruppe m_Gruppe;
        private ObservableCollection<Gruppenlaborwertitem> m_Children;

        private string m_Name;

        #endregion

        #region Constructor
        public Gruppenitem(string name, ObservableCollection<Gruppenentry> flatItems)
        {
            m_Name = name;
            m_FlatItems = flatItems;
            m_Children = new ObservableCollection<Gruppenlaborwertitem>();
            m_Children.CollectionChanged += Children_CollectionChanged;
            HasChildren = false;
        }

        #endregion
      
        #region Properties
        public override string Name
        {
            get => m_Name;
            set
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    m_Gruppe.Name = value;
                    NotifyPropertyChanged();
                }
            }
}   

        //public Laborwertgruppe Gruppe => m_Gruppe;
        public ObservableCollection<Gruppenlaborwertitem> Children => m_Children;

        public IList<ILaborwert> Werte => m_Children.Cast<ILaborwert>().ToList();
        #endregion

        #region Methods
        private void Children_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasChildren = m_Children.Count > 0;
            if(e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is Gruppenlaborwertitem laborwertItem)
                    {
                        laborwertItem.Parent = this;
                    }
                }
            }
        }

        private void Gruppe_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Laborwertgruppe.Name))
            {
                NotifyPropertyChanged(nameof(Name));
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
