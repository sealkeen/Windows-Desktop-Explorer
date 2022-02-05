using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Explorer
{
    public class SettingsReader
    {
        public static string ReadLastElement(System.IO.Stream stream)
        {
            try {
                string e;
                XDocument xD = XDocument.Load(stream);
                string result = xD.Document.Elements().First().Elements().Last().Value;
                return result;
                //using (XmlReader reader = XmlReader.Create(stream)) {
                //    while (reader.Read()) {
                //        switch (reader.NodeType) {
                //            case XmlNodeType.Element: //Console.WriteLine("Start Element {0}", reader.Name);
                //                e = reader.Value;
                //                break;
                //        }
                //        return reader.Value;
                //    }
                //}
            } catch (Exception ex) { }
            return null;
        }

        public static IEnumerable<XElement> GetElements(string fileName)
        {
            try
            {
                return XDocument.Load(fileName).Elements().First().Elements();
            } catch {
                return new List<XElement>();
            }
        }
        
    }
}
