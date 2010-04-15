using System.Xml.Serialization;
using System.IO;

namespace Reanimator.Forms
{
    public static class XmlUtilities<T>
    {
        public static void Serialize(T type, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter tw = new StreamWriter(path);
            serializer.Serialize(tw, type);
            tw.Close();
        }

        public static T Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextReader tr = new StreamReader(path);
            T type = (T)serializer.Deserialize(tr);
            tr.Close();

            return type;
        }
    }
}
