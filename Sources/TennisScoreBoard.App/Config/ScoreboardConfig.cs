using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TennisScoreBoard.App.Config
{
    [Serializable]
    public class ScoreboardConfig 
    {
        [XmlElement("ConnectionString")]
        public string ConnectionString { get; set; }
    }
}
