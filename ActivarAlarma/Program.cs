using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ActivarAlarma
{
    class Program
    {
        static void Main(string[] args)
        {
            string rut = string.Empty;
            int sector = 0;
            // Start with XmlReader object  
            //here, we try to setup Stream between the XML file nad xmlReader  
            string sPath = System.AppDomain.CurrentDomain.BaseDirectory;

            using (XmlReader reader = XmlReader.Create(sPath + @"\\configuracion.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //return only when you have START tag  
                        switch (reader.Name.ToString())
                        {
                            case "rut":
                                rut = reader.ReadString();
                                break;
                            case "sector":
                                sector = int.Parse(reader.ReadString());
                                break;
                        }
                    }
                }
                escuchar e = new escuchar();
                e.minGuardar(rut, sector);
            }

        }
    }
}
