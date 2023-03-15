using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LoaderDbApp {
    public class XMLDataLang {
        [XmlAttribute("default")]
        public int Default { get; set; } = 0;
        [XmlAttribute("label")]
        public string Label { get; set; }
        [XmlAttribute("iso")]
        public string Iso { get; set; }
        [XmlElement("Phrases")]
        public XMLDataPhrases Phrases { get; set; }
    }
}
