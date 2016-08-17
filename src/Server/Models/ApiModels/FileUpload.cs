using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Server.Models.ApiModels
{
    [XmlRoot("file")]
    public class FileUpload
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("content")]
        public string Content { get; set; }

        [XmlElement("user")]
        public string UserToken { get; set; }
    }
}
