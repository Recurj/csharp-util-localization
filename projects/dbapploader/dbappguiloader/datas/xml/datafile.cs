using System.Collections.Generic;
using System.Xml.Serialization;

namespace LoaderDbApp {

    [XmlRoot("DBImport")]
    public class XMLDataFile {
        [XmlElement("Lang")]
        public List<XMLDataLang> Langs { get; set; }
        [XmlElement("Handbook")]
        public List<XMLDataHandbook> Handbooks { get; set; }
    }
}

