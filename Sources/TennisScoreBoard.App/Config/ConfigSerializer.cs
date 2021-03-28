using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TennisScoreBoard.App.Config
{
    public class ConfigSerializer
    {
        public ScoreboardConfig LoadScoreboardConfig(string xml)
        {
            var serializer = new XmlSerializer(typeof(ScoreboardConfig));
            if (File.Exists(xml))
            {
                var xmlStr = XDocument.Load(xml).ToString();
                using var stream = new StringReader(xmlStr);
                using var reader = XmlReader.Create(stream);
                var config = (ScoreboardConfig)serializer.Deserialize(reader);

                return config;
            }

            return null;
        }
    }
}
