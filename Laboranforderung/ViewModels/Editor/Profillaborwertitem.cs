using Laboranforderung.Daten;

namespace Laboranforderung.ViewModels.Editor
{
    public class Profillaborwertitem: Profilentry
    {
        #region Fields
        private Gruppenlaborwertitem m_Laborwert;
        private Profilitem m_Parent;
        #endregion

        #region Constructor
        public Profillaborwertitem(Profilitem parent, Gruppenlaborwertitem laborwert)
        {
            m_Parent = parent;
            m_Laborwert = laborwert;
            m_Laborwert.PropertyChanged += Laborwert_PropertyChanged;
        }


        #endregion

        #region Properties
        public override string Name
        { 
         get => m_Laborwert.Name;
        set
            {
                if (m_Laborwert.Name != value)
                {
                    m_Laborwert.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public override int Indent => 16;

        public Gruppenlaborwertitem Wert => m_Laborwert;
        public Profilitem Parent
        {
            get => m_Parent;
            internal set => m_Parent = value;
        }
        #endregion

        #region Methods
        private void Laborwert_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Gruppenlaborwertitem.Name))
            {
                NotifyPropertyChanged(nameof(Name));
            }  
        }
        #endregion

    }
}
