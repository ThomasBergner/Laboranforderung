using System.Xml;

namespace Laboranforderung.Daten
{
    public interface IXmlReadWritable
    {
        void Read_Xml(XmlNode node);
        void Write_Xml(XmlTextWriter writer);
    }
}
