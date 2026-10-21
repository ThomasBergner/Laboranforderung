using Laboranforderung.Daten.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml;

namespace Laboranforderung.Daten
{
    public class Laborprofil : INotifyPropertyChanged, IEquatable<Laborprofil>, ILaborprofil
    {

        #region Fields
        private string m_Profilname;
        private string m_Number;
        private List<Laborwert> m_Werte;
        #endregion

        #region Constructors

        public Laborprofil(string profilname, string number)
        {
            m_Profilname = profilname;
            m_Number = number;
            m_Werte = new List<Laborwert>();
        }



        public Laborprofil(XmlNode node, IList<Laborwert> werteitems)
        {
            string[] vals;
            m_Profilname = node.Attributes["Profilname"].Value;
            m_Number = node.Attributes["Number"].Value;
            m_Werte = new List<Laborwert>();

            string wertevalue = node.Attributes["Werte"].Value;
            if(!string.IsNullOrWhiteSpace(wertevalue))
            {
                vals = wertevalue.Split();
                for (int i = 0; i <= vals.Length - 1; i++)
                { 
                    m_Werte.Add(werteitems[Convert.ToInt32(vals[i])]);
                }
            }
        }

        #endregion

        #region Properties

        public string Profilname
        {
            get { return m_Profilname; }
            set { if (m_Profilname != value.Trim()) { m_Profilname = value.Trim(); NotifyPropertyChanged(); } }
        }

        public string Number
        {
            get { return m_Number; }
            set { if (m_Number != value.Trim()) { m_Number = value.Trim(); NotifyPropertyChanged(); } }
        }

        public string DisplayName
        {
            get { return $"{m_Number} - {m_Profilname}"; }
        }

        public List<Laborwert> Werte
        {
            get { return m_Werte; }
        }

        #endregion

        #region Methods

        public void Write_Xml(XmlTextWriter writer)
        {
            writer.WriteStartElement("Laborprofil");
            List<string> vals = new List<string>();
            writer.WriteAttributeString("Profilname", m_Profilname);
            writer.WriteAttributeString("Number", m_Number);
            vals.Clear();
            foreach (var m_member in m_Werte)
                vals.Add(m_member.Index.ToString());
            writer.WriteAttributeString("Werte", string.Join(" ", vals.ToArray()));
            writer.WriteEndElement();
        }

        public bool Equals(Laborprofil? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (m_Werte.Count != other.m_Werte.Count) return false;
            if (m_Profilname != other.m_Profilname) return false;
            if (m_Number != other.m_Number) return false;
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
