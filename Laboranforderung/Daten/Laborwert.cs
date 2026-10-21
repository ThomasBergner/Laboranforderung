using Laboranforderung.Daten.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Laboranforderung.Daten
{
    public class Laborwert : INotifyPropertyChanged, IEquatable<Laborwert>, ILaborwert
    {

        #region Fields
        private string m_Name;
        private string m_Bezeichnung;
        private string m_Synonyme;
        private string m_Findehilfe;
        private string m_Roehrchenart;

        #endregion

        #region Constructors
        public Laborwert(ILaborwert wert)
        {
            m_Name = wert.Name;
            m_Bezeichnung = wert.Bezeichnung;
            m_Synonyme = wert.Synonyme;
            Index = wert.Index;
            m_Findehilfe = wert.Findehilfe;
            m_Roehrchenart = wert.Roehrchenart;
        }
        public Laborwert(string name, string bezeichnung, string synonyme, string findehilfe, string roehrchenart, int index)
        {
            m_Name = name;
            m_Bezeichnung = bezeichnung;
            m_Synonyme = synonyme;
            m_Findehilfe = findehilfe;
            m_Roehrchenart = roehrchenart;
            Index = index;
        }

        public Laborwert(Laborwert wert)
        {
            m_Name = wert.Name;
            m_Bezeichnung = wert.Bezeichnung;
            m_Synonyme = wert.Synonyme;
            Index = wert.Index;
            m_Findehilfe = wert.Findehilfe;
            m_Roehrchenart = wert.Roehrchenart;
        }
        public Laborwert(string name, string bezeichnung)
        {
            m_Name = name;
            m_Bezeichnung = bezeichnung;
            m_Synonyme = "";
            m_Findehilfe = "";
            m_Roehrchenart = "";
        }

        public Laborwert(XmlNode node)
        {
            m_Name = node.Attributes["Name"].Value;
            m_Bezeichnung = node.Attributes["Bezeichnung"].Value;
            m_Synonyme = node.Attributes["Synonyme"].Value;
            m_Findehilfe = node.Attributes["Findehilfe"].Value;
            m_Roehrchenart = node.Attributes["Roehrchenart"].Value;
        }
        #endregion

        #region Properties

        public string Name
        {
            get { return m_Name; }
            set { if (m_Name != value.Trim()) { m_Name = value.Trim(); NotifyPropertyChanged(); } }
        }
        public string Bezeichnung
        {
            get { return m_Bezeichnung; }
            set { if (m_Bezeichnung != value.Trim()) { m_Bezeichnung = value.Trim(); NotifyPropertyChanged(); } }
        }

        public string Synonyme
        {
            get { return m_Synonyme; }
            set { if (m_Synonyme != value.Trim()) { m_Synonyme = value.Trim(); NotifyPropertyChanged(); } }
        }

        public int Index { get; set; }

        public string Findehilfe
        {
            get { return m_Findehilfe; }
            set { if (m_Findehilfe != value.Trim()) { m_Findehilfe = value.Trim(); NotifyPropertyChanged(); } }
        }
        public string Roehrchenart
        {
            get { return m_Roehrchenart; }
            set { if (m_Roehrchenart != value.Trim()) { m_Roehrchenart = value.Trim(); NotifyPropertyChanged(); } }
        }


        #endregion

        #region Methods

        public void Write_Xml(XmlTextWriter writer)
        {
            writer.WriteStartElement("Laborwert");
            writer.WriteAttributeString("Name", m_Name);
            writer.WriteAttributeString("Bezeichnung", m_Bezeichnung);
            writer.WriteAttributeString("Synonyme", m_Synonyme);
            writer.WriteAttributeString("Findehilfe", m_Findehilfe);
            writer.WriteAttributeString("Roehrchenart", m_Roehrchenart);
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            return m_Name + " - " + m_Bezeichnung;
        }
        public bool Equals(Laborwert? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (m_Name != other.m_Name) return false;
            if (m_Bezeichnung != other.m_Bezeichnung) return false;
            if (m_Synonyme != other.m_Synonyme) return false;
            if(m_Findehilfe != other.m_Findehilfe) return false;
            if(m_Roehrchenart != other.m_Roehrchenart) return false;
            return true;
        }
        public Laborwert CreateClone()
        {
            Laborwert result = new Laborwert(this);
            result.Index = this.Index;
            result.Findehilfe = this.Findehilfe;
            result.Roehrchenart = this.Roehrchenart;
            return result;
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
