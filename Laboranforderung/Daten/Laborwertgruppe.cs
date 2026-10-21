using Laboranforderung.Daten.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Laboranforderung.Daten
{
    public class Laborwertgruppe : INotifyPropertyChanged, IEquatable<Laborwertgruppe>, ILaborwertgruppe
    {

        #region Fields
        private string m_Name;
        private List<Laborwert> m_Werte;
        #endregion

        #region Constructors

        public Laborwertgruppe(string name)
        {
            m_Name = name;
            m_Werte = new List<Laborwert>();
        }



        public Laborwertgruppe(XmlNode node)
        {
            m_Name = node.Attributes["Name"].Value;
            m_Werte = new List<Laborwert>();
            foreach (XmlNode nd in node.ChildNodes[0].ChildNodes)
                m_Werte.Add(new Laborwert(nd));
        }
        #endregion

        #region Properties

        public List<Laborwert> Werte
        {
            get { return m_Werte; }
        }

        public string Name
        {
            get { return m_Name; }
            set { if (m_Name != value.Trim()) { m_Name = value.Trim(); NotifyPropertyChanged(); } }
        }

        #endregion

        #region Methods
        public void Write_Xml(XmlTextWriter writer)
        {
            writer.WriteStartElement("Laborwertgruppe");
            writer.WriteAttributeString("Name", m_Name);
            writer.WriteStartElement("Werte");
            foreach (var member in m_Werte)
                member.Write_Xml(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        public bool Equals(Laborwertgruppe? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (m_Werte.Count != other.m_Werte.Count) return false;
            if (m_Name != other.m_Name) return false;

            for (int i = 0; i < m_Werte.Count; i++)
            {
                if (!m_Werte[i].Equals(other.m_Werte[i])) return false;
            }

            return true;
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
