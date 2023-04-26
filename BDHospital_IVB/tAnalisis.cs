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
using System.Data.SqlClient;

namespace BDHospital_IVB
{
    public partial class tAnalisis : Form
    {
        public tAnalisis()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void MostrarAnalisis()
        {
            string cmd = string.Format("select * from tAnalisis");
            dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void tAnalisis_Load(object sender, EventArgs e)
        {
            MostrarAnalisis();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCod.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtNomA.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string Cod = txtBuscar.Text;
            if (Cod == "")
            {
                MostrarAnalisis();
            }
            else
            {
                string cmd = string.Format("select * from tAnalisis where Cod_Analisis='{0}'", Cod);//
                dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            string cmd = string.Format("exec pa_GenerarCodigo");
            DataSet ds = Utilidades.Ejecutar(cmd);
            string codigo = ds.Tables[0].Rows[0]["codigo"].ToString().Trim();
            lblCod.Text = codigo;
            txtNomA.Text = "";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string cod = lblCod.Text;
            try
            {
                EliminarAnalisis(cod);
                MessageBox.Show("Analisis Eliminado");
                MostrarAnalisis();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }
        public bool EliminarAnalisis(string cod)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Analisis_Delete @cod", Con);
            cmd.Parameters.Add("@cod", SqlDbType.Char);
            cmd.Parameters["@cod"].Value = cod;
            int resultado = cmd.ExecuteNonQuery();
            Con.Close();
            if (resultado > 0)
            {
                return true;
            }
            else return false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            string cod = lblCod.Text;
            string nom = txtNomA.Text;
            try
            {
                ModificarAnalisis(cod, nom);
                MessageBox.Show("Los datos han sido Modificados");
                MostrarAnalisis();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool ModificarAnalisis(string cod, string nom)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Analisis_Modificar @cod,@nom", Con);
            cmd.Parameters.Add("@cod", SqlDbType.VarChar);
            cmd.Parameters.Add("@nom", SqlDbType.VarChar);
            cmd.Parameters["@cod"].Value = cod;
            cmd.Parameters["@nom"].Value = nom;
            int resultado = cmd.ExecuteNonQuery();
            Con.Close();
            if (resultado > 0)
            {
                return true;
            }
            else return false;
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string cod = lblCod.Text;
            string nom = txtNomA.Text;
            try
            {
                IngresarAnalisis(cod, nom);
                MessageBox.Show("Los datos han sido registrados");
                MostrarAnalisis();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool IngresarAnalisis(string cod, string nombre)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Analisis_Insertar @cod,@nom", Con);
            cmd.Parameters.Add("@cod", SqlDbType.Char);
            cmd.Parameters.Add("@nom", SqlDbType.VarChar);
            cmd.Parameters["@cod"].Value = cod;
            cmd.Parameters["@nom"].Value = nombre;
            int resultado = cmd.ExecuteNonQuery();
            Con.Close();
            if (resultado > 0)
            {
                return true;
            }
            else return false;
        }
    }
}
