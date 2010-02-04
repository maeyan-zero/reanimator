using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Reanimator
{
    class Mod
    {
        public static void Parse(string path)
        {
            XmlTextReader r = new XmlTextReader(path);
            XmlValidatingReader v = new XmlValidatingReader(r);

            v.ValidationType = ValidationType.Schema;
            v.ValidationEventHandler += new ValidationEventHandler(ValidationEventHandler);

            while(v.Read())
            {
                switch (v.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + v.Name);
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(v.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + v.Name);
                        Console.WriteLine(">");
                        break;
                }
            }
            v.Close();

            if (isValid)
            {
                Console.WriteLine("Document is valid");
            }
            else
            {
                Console.WriteLine("Document is invalid");
            }

        }

        public static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            isValid = false;
            Console.WriteLine("Validation event\n" + args.Message);
        }

        static bool isValid = true;

        enum Function
        {

        }
#pragma warning disable 0649
        class Modification
        {
            public string title;
            public string description;
            public string version;
            public string pack;
            public string file;
            public Function function;
            public int action;
            public int col;
            public int row;
            public object data;
        }
#pragma warning restore 0649
    }
}
