using Laboranforderung.Daten.Interfaces;
using System.Xml;

namespace Laboranforderung.Daten
{
    public class Datensammlung : IXmlReadWritable, IEquatable<Datensammlung>
    {

        #region Fields
        private List<Laborwertgruppe> m_Gruppen;
        private List<Laborprofil> m_Profile;
        #endregion

        #region Constructors

        public Datensammlung()
        {
            m_Gruppen = new List<Laborwertgruppe>();
            m_Profile = new List<Laborprofil>();
        }

        public Datensammlung(List<Laborwertgruppe> gruppen, List<Laborprofil> profile)
        {
            m_Gruppen = gruppen;
            m_Profile = profile;
        }

        public Datensammlung(XmlNode node)
        {
            List<Laborwert> tempwerte = new List<Laborwert>();
            m_Gruppen = new List<Laborwertgruppe>();
            foreach (XmlNode nd in node.ChildNodes[0].ChildNodes)
                m_Gruppen.Add(new Laborwertgruppe(nd));


            foreach (var gruppe in m_Gruppen)
                tempwerte.AddRange(gruppe.Werte);

            m_Profile = new List<Laborprofil>();
            foreach (XmlNode nd in node.ChildNodes[1].ChildNodes)
                m_Profile.Add(new Laborprofil(nd, tempwerte));
        }
        #endregion

        #region Properties


        public List<Laborwertgruppe> Gruppen
        {
            get { return m_Gruppen; }
        }

        public List<Laborprofil> Profile
        {
            get { return m_Profile; }
        }
        #endregion

        #region Methods



        public Datensammlung CreateClone()
        {
            Datensammlung clone = new Datensammlung();

            Dictionary<Laborwert, Laborwert> wertMapping = new Dictionary<Laborwert, Laborwert>();
            foreach (var gruppe in m_Gruppen)
            {

                Laborwertgruppe clonedGruppe = new Laborwertgruppe(gruppe.Name);
                clone.Gruppen.Add(clonedGruppe);
                foreach (var wert in gruppe.Werte)
                {
                    Laborwert clonedWert = new Laborwert(wert);
                    wertMapping[wert] = clonedWert;
                    clonedGruppe.Werte.Add(clonedWert);
                }
            }
            foreach (var profil in m_Profile)
            {

                Laborprofil clonedProfil = new Laborprofil(profil.Profilname, profil.Number);
                clone.Profile.Add(clonedProfil);
                foreach (var wert in profil.Werte)
                {
                    clonedProfil.Werte.Add(wertMapping[wert]);
                }
            }

            return clone;
        }


        public void WerteEnumerieren()
        {
            int index = 0;
            foreach (var gruppe in m_Gruppen)
                foreach (var wert in gruppe.Werte)
                    wert.Index = index++;
        }

        public void Read_Xml(XmlNode node)
        {
            m_Gruppen.Clear();
            m_Profile.Clear();

            foreach (XmlNode nd in node.ChildNodes[0].ChildNodes)
                m_Gruppen.Add(new Laborwertgruppe(nd));


            List<Laborwert> tempwerte = new List<Laborwert>();

            foreach (var gruppe in m_Gruppen)
                tempwerte.AddRange(gruppe.Werte);

            foreach (XmlNode nd in node.ChildNodes[1].ChildNodes)
                m_Profile.Add(new Laborprofil(nd, tempwerte));
        }

        public void Write_Xml(XmlTextWriter writer)
        {
            writer.WriteStartElement("Datensammlung");
            writer.WriteStartElement("Gruppen");
            foreach (var member in m_Gruppen)
                member.Write_Xml(writer);
            writer.WriteEndElement();

            WerteEnumerieren();

            writer.WriteStartElement("Profile");
            foreach (var member in m_Profile)
                member.Write_Xml(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public bool Equals(Datensammlung? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (m_Gruppen.Count != other.m_Gruppen.Count) return false;
            if (m_Profile.Count != other.m_Profile.Count) return false;

            for (int i = 0; i < m_Gruppen.Count; i++)
            {
                if (!m_Gruppen[i].Equals(other.m_Gruppen[i])) return false;
            }

            for (int i = 0; i < m_Profile.Count; i++)
            {
                if (!m_Profile[i].Equals(other.m_Profile[i])) return false;
            }

            return true;
        }
        #endregion

    }
}
