using Laboranforderung.Daten.Interfaces;

namespace Laboranforderung.ViewModels.Editor
{
    public class Gruppenlaborwertitem: Gruppenentry, ILaborwert
    {
        #region Fields
        private Gruppenitem m_Parent;
        private string m_Name;
        private string m_Bezeichnung;
        private string m_Synonyme;
        private string m_Findehilfe;
        private string m_Roehrchenart;

        #endregion

        #region Constructor
        public Gruppenlaborwertitem(Gruppenitem parent, ILaborwert laborwert)
        {
            m_Name = laborwert.Name;
            m_Bezeichnung = laborwert.Bezeichnung;
            m_Synonyme = laborwert.Synonyme;
            m_Findehilfe = laborwert.Findehilfe;
            m_Roehrchenart = laborwert.Roehrchenart;
            m_Parent = parent;
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
                    NotifyPropertyChanged();
                }
            }
        }
        public string Bezeichnung
        {
            get => m_Bezeichnung;
            set
            {
                if (m_Bezeichnung != value)
                {
                    m_Bezeichnung = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Synonyme
        {
            get => m_Synonyme;
            set
            {
                if (m_Synonyme != value)
                {
                    m_Synonyme = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Findehilfe
        {
            get => m_Findehilfe;
            set
            {
                if (m_Findehilfe != value)
                {
                    m_Findehilfe = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Roehrchenart
        {
            get => m_Roehrchenart;
            set
            {
                if (m_Roehrchenart != value)
                {
                    m_Roehrchenart = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Index { get; set;}
        public override int Indent => 16;

        public Gruppenitem Parent {
            get => m_Parent;
            internal set
            {
                m_Parent = value;
            }
        }
        #endregion

        #region Methods

        #endregion

    }
}
