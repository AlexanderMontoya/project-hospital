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

namespace BDHospital_IVB
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public static string IdUser = "";

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string CMD = string.Format("Select * from TUsuarios where Usuario='{0}' and Contraseña='{1}'",txtUser.Text.Trim(),txtPass.Text.Trim());
                DataSet ds = Utilidades.Ejecutar(CMD);
                IdUser = ds.Tables[0].Rows[0]["id_Usuario"].ToString().Trim();
                string User = ds.Tables[0].Rows[0]["Usuario"].ToString().Trim();
                string contra = ds.Tables[0].Rows[0]["Contraseña"].ToString().Trim();
                string NomUser = ds.Tables[0].Rows[0]["Nombres"].ToString().Trim();
                if (User == txtUser.Text.Trim() && contra==txtPass.Text.Trim())
                {
                    MessageBox.Show("Bienvenido "+ NomUser, "Bienvenido");
                    Pantalla_Principal frm = new Pantalla_Principal();
                    frm.Show();
                    this.Hide();
                }
            }
            catch 
            {
                MessageBox.Show("Usuario o Contraseña Incorrecta");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
