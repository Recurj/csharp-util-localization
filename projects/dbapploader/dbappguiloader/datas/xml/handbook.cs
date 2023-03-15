using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LoaderDbApp {
    public class XMLDataHandbook {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        [XmlElement("Value")] 
        public List<XMLDataHandbookValue> Values { get; set; }
    }
}
