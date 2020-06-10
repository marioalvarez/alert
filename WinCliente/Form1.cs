using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace WinCliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            escuchar es = new escuchar();
            DataTable dt = es.msoObtenerUltimaNotificacion();
            if(dt!=null)
            {
                if(dt.Rows.Count > 0)
                {

                    try
                    {
                        oLb_funcionario.Text = dt.Rows[0]["nombre_completo"].ToString();
                        try
                        {
                            WindowsMediaPlayer wplayer = new WindowsMediaPlayer();
                            string sPath = System.AppDomain.CurrentDomain.BaseDirectory;
                            wplayer.URL = sPath + @"\\sound.mp3";
                            wplayer.controls.play();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("No tiene el componente de audio", "WindowsMediaPlayer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }
}
