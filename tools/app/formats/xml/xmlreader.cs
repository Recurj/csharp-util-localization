using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace RJToolsApp.formats.xml {
    public class XMLDataLoader<T> {
        const string xmlHeader = "<?xml version = \"1.0\" encoding=\"utf-8\"?>";
        public const int UnknownNode = 0;
        public const int UnknownElement = 0;
        public const int UnknownAttribute = 0;
        private readonly Action<string, int> _act;
        public XMLDataLoader(Action<string, int> act) {
            _act = act;
        }
        public T LoadFile(string fn) {
            using FileStream stream = new(fn, FileMode.Open);
            return Load(stream, new XmlSerializer(typeof(T)));
        }
        public T LoadData(string xml, string root) {
            XmlRootAttribute xRoot = new();
            xRoot.ElementName = root;
            xRoot.IsNullable = true;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlHeader + "<" + root + ">" + xml + "</" + root + ">"))) {
                return Load(stream, new XmlSerializer(typeof(T), xRoot));
            }
        }
        public T Load(Stream stream, XmlSerializer xmlSerializer) {
            xmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(OnUnknownAttribute);
            xmlSerializer.UnknownElement += new XmlElementEventHandler(OnUnknownElement);
            xmlSerializer.UnknownNode += new XmlNodeEventHandler(OnUnknownNode);
            return (T)xmlSerializer.Deserialize(stream);
        }
        void OnUnknownNode(object sender, XmlNodeEventArgs e) {
            _act($"Unknown Node {e.Name} found at the line {e.LineNumber}", UnknownNode);
        }
        void OnUnknownElement(object sender, XmlElementEventArgs e) {
            _act($"Unknown Element {e.Element.Name} found at the line {e.LineNumber}", UnknownElement);
        }
        void OnUnknownAttribute(object sender, XmlAttributeEventArgs e) {
            _act($"Unknown Attribute {e.Attr.Name} found at the line {e.LineNumber}", UnknownAttribute);
        }
    }
}
