using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiLibreria;
using System.IO;

namespace BDHospital_IVB
{
    public partial class HistoriaPaciente : Form
    {
        public HistoriaPaciente()
        {
            InitializeComponent();
        }

        public static string NHistoria = "";

        private void HistoriaPaciente_Load(object sender, EventArgs e)
        {
            try
            {
                string CMD = string.Format("select idHistoria,FkNumeroSeguro,tPaciente.fotoPaciente from tHistoria join tPaciente on FkNumeroSeguro=tPaciente.idNumeroSeguro where FkNumeroSeguro=" + tPacientes.IdPac);
                DataSet ds = Utilidades.Ejecutar(CMD);
                NHistoria = ds.Tables[0].Rows[0]["idHistoria"].ToString().Trim();
                string IDPac = ds.Tables[0].Rows[0]["FkNumeroSeguro"].ToString().Trim();
                byte[] FotoPac= (byte[])ds.Tables[0].Rows[0]["fotoPaciente"];
                MemoryStream ms = new MemoryStream(FotoPac);
                lblNHistoria.Text = NHistoria;
                lblIDPac.Text = IDPac;
                pbFoto.Image = Image.FromStream(ms);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error : " + error);
            }
        }

        private void btnCitas_Click(object sender, EventArgs e)
        {
            tCitasPaciente frmCitas = new tCitasPaciente();
            frmCitas.Show();
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            tAnalisisPaciente frmAnalisis = new tAnalisisPaciente();
            frmAnalisis.Show();
        }
    }
}
