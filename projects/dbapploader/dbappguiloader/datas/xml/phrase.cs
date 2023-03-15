using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LoaderDbApp {
    public class XMLDataPhrase {
        [XmlAttribute("language")]
        public string Lang { get; set; }
        [XmlText]
        public string Text { get; set; }

    }
}
