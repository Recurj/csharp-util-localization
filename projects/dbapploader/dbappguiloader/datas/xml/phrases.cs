using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LoaderDbApp {
    public class XMLDataPhrases {
        [XmlElement("For")]
        public List<XMLDataPhrase> Phrases { get; set; }
    }
}
