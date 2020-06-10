using System;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Runtime.InteropServices;

namespace ClienteEscucha
{
    class Program
    {
        private static int id_ultima_notificacion = 0;

        private static string rut_fun = "";
        private static int sector = 0;

        static public void Tick(Object stateInfo)
        {

            //Console.WriteLine("Tick");
            escuchar e = new escuchar();
            //Console.WriteLine("última notificacion: "+ id_ultima_notificacion);
            DataTable dt = e.msoBuscarNotificacion();
            if(dt!=null)
            {
                if(dt.Rows.Count > 0)
                {
                    int id = int.Parse((dt.Rows[0]["id"].ToString())=="" ? "0": dt.Rows[0]["id"].ToString());
                    if (id > id_ultima_notificacion)
                    {
                        id_ultima_notificacion = id;
                        Process myProcess = new Process();

                        try
                        {
                            string rut_quien_activo = dt.Rows[0]["rut_func"].ToString().Trim();
                            int sector_quien_activo = int.Parse(dt.Rows[0]["sector"].ToString());
                            if (rut_fun != rut_quien_activo)
                            {
                                if (sector == sector_quien_activo)
                                {
                                    myProcess.StartInfo.UseShellExecute = false;
                                    // You can start any process, HelloWorld is a do-nothing example.
                                    string sPath = System.AppDomain.CurrentDomain.BaseDirectory;
                                    myProcess.StartInfo.FileName = sPath + @"WinCliente.exe";
                                    myProcess.StartInfo.CreateNoWindow = true;
                                    myProcess.Start();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nada que leer");
                }
            }
        }

        [DllImport("user32.dll")]
        internal static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
        static Int32 WM_SYSCOMMAND = 0x0112;
        static Int32 SC_MINIMIZE = 0x0F020;


        static void Main(string[] args)
        {
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
            CargaPropioXml();

            escuchar e = new escuchar();
            id_ultima_notificacion = e.msoObtenerUltimaNotificacion();

            TimerCallback callback = new TimerCallback(Tick);

            Console.WriteLine("Favor minimizar ventana - escucha activada a las {0}\n",
                               DateTime.Now.ToString("h:mm:ss"));

            // create a one second timer tick
            Timer stateTimer = new Timer(callback, null, 0, 4000);

            // loop here forever
            for (; ; )
            {
                // add a sleep for 100 mSec to reduce CPU usage
                Thread.Sleep(4000);
            }
        }

        static void CargaPropioXml()
        {
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
                                rut_fun = reader.ReadString();
                                break;
                            case "sector":
                                sector = int.Parse(reader.ReadString());
                                break;
                        }
                    }
                }
            }
        }


    }
}
