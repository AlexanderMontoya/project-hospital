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
    public partial class Pantalla_Principal : Form
    {
        public Pantalla_Principal()
        {
            InitializeComponent();
            
            DatosUsuario();

        }
        void DatosUsuario()
        {
            try
            {
                string CMD = string.Format("Select * from TUsuarios where id_Usuario="+Login.IdUser);
                DataSet ds = Utilidades.Ejecutar(CMD);
                string NomUser = ds.Tables[0].Rows[0]["Nombres"].ToString().Trim();
                string ApUser = ds.Tables[0].Rows[0]["Apellidos"].ToString().Trim();
                byte[] FotoUser = (byte[])ds.Tables[0].Rows[0]["FotoUsuario"];
                MemoryStream ms = new MemoryStream(FotoUser);
                lblNomUser.Text = NomUser;
                lblApUser.Text = ApUser;
                pbFotoUser.Image = Image.FromStream(ms);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error : " + error);
            }
        }

        private void Pantalla_Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMedicos_Click(object sender, EventArgs e)
        {
            tMedicos frmMedicos = new tMedicos();
            frmMedicos.Show();
        }

        private void btnPacientes_Click(object sender, EventArgs e)
        {
            tPacientes frmPacientes = new tPacientes();
            frmPacientes.Show();
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            tAnalisis frmAnalisis = new tAnalisis();
            frmAnalisis.Show();
        }

        private void btnEspealidad_Click(object sender, EventArgs e)
        {
            tEspecialidad frmEspecialidad = new tEspecialidad();
            frmEspecialidad.Show();
        }
    }
}
