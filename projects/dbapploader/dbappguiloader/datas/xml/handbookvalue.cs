using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LoaderDbApp {
    public class XMLDataHandbookValue {

        [XmlAttribute("code")]
        public int Code { get; set; }
        [XmlElement("Phrases")]
        public XMLDataPhrases Phrases { get; set; }

    }
}
