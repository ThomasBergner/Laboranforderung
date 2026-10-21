using System.IO;
using System.Text;
using System.Xml;

namespace Laboranforderung.Daten
{
    public class XmlReaderWriter
    {
        public static void Read(IXmlReadWritable obj, string path)
        {
            if (File.Exists(path))
            {
                var reader = new XmlTextReader(path);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                
                // XML-Dokument erstellen und über den Reader laden
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                // Reader kann geschlossen werden
                reader.Close();
                obj.Read_Xml(xmlDoc.DocumentElement.ChildNodes[0]);
            }
        }
        
        public static void Read(IXmlReadWritable obj, Stream stream)
        {
            var reader = new XmlTextReader(stream);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            
            // XML-Dokument erstellen und über den Reader laden
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            // Reader kann geschlossen werden
            reader.Close();
            obj.Read_Xml(xmlDoc.DocumentElement.ChildNodes[0]);
        }
        
        public static void Write(IXmlReadWritable obj, string path)
        {
            var writer = new XmlTextWriter(path, Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Content");
            obj.Write_Xml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
