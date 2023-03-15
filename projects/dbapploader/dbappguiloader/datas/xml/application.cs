using System.Xml.Serialization;

namespace LoaderDbApp {
    public class XMLDataApplication {
        [XmlAttribute("label")]
        public string Label { get; set; }

        [XmlAttribute("status")]
        public int Status { get; set; }
        [XmlElement("Phrases")]
        public XMLDataPhrases Phrases { get; set; }
    }
}
