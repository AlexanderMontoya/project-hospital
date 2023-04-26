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
    public partial class tCitasPaciente : Form
    {
        public tCitasPaciente()
        {
            InitializeComponent();
        }

        private void tCitasPaciente_Load(object sender, EventArgs e)
        {
            MostrarCitasPaciente();
            lblIDHistoria.Text = HistoriaPaciente.NHistoria;
        }

        void MostrarCitasPaciente()
        {
            string cmd = string.Format("select * from tCitaMedica where fkHistoria=" + HistoriaPaciente.NHistoria);
            dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lblNCita.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtFec.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtDiagnostico.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtEstatura.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtPeso.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtIDMedico.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            lblIDHistoria.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string NCita = txtBuscar.Text;
            if (NCita == "")
            {
                MostrarCitasPaciente();
            }
            else
            {
                string cmd = string.Format("select * from tCitaMedica where NCita=" + NCita);//
                dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            DateTime fec = DateTime.Parse(txtFec.Text);
            string diagnostico = txtDiagnostico.Text;
            string estat = txtEstatura.Text;
            string peso = txtPeso.Text;
            string IDMedico = txtIDMedico.Text;
            string IDHistoria = lblIDHistoria.Text;
  
            try
            {
                IngresarCita(fec,diagnostico, estat, peso,IDMedico, IDHistoria);
                MessageBox.Show("Los datos han sido registrados");
                MostrarCitasPaciente();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool IngresarCita(DateTime fec, string diagnostico, string estat, string peso, string IDMedico, string IDHistoria)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_CitaMedica_Insertar @fec,@diag,@estat,@peso,@IDMedico,@IDHistoria", Con);
            cmd.Parameters.Add("@fec", SqlDbType.DateTime);
            cmd.Parameters.Add("@diag", SqlDbType.VarChar);
            cmd.Parameters.Add("@estat", SqlDbType.Char);
            cmd.Parameters.Add("@peso", SqlDbType.Char);
            cmd.Parameters.Add("@IDMedico", SqlDbType.Int);
            cmd.Parameters.Add("@IDHistoria", SqlDbType.Int);
            cmd.Parameters["@fec"].Value = fec;
            cmd.Parameters["@diag"].Value = diagnostico;
            cmd.Parameters["@estat"].Value = estat;
            cmd.Parameters["@peso"].Value = peso;
            cmd.Parameters["@IDMedico"].Value = IDMedico;
            cmd.Parameters["@IDHistoria"].Value = IDHistoria;
            int resultado = cmd.ExecuteNonQuery();
            Con.Close();
            if (resultado > 0)
            {
                return true;
            }
            else return false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            lblNCita.Text = "";
            txtFec.Text = "";
            txtDiagnostico.Text = "";
            txtEstatura.Text = "";
            txtIDMedico.Text = "";
            txtPeso.Text = "";
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            string NCita = lblNCita.Text;
            DateTime fec = DateTime.Parse(txtFec.Text);
            string diagnostico = txtDiagnostico.Text;
            string estat = txtEstatura.Text;
            string peso = txtPeso.Text;
            string IDMedico = txtIDMedico.Text;
            string IDHistoria = lblIDHistoria.Text;
            try
            {
                ModificarCita(NCita, fec, diagnostico, estat,peso, IDMedico, IDHistoria);
                MessageBox.Show("Los datos han sido Modificados de la cita N°"+NCita);
                MostrarCitasPaciente();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool ModificarCita(string NCita, DateTime fec, string diagnostico, string estat, string peso, string IDMedico, string IDHistoria)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_CitaMedica_Modificar @NCita,@fec,@diag,@estat,@peso,@IDMedico,@IDHistoria", Con);
            cmd.Parameters.Add("@NCita", SqlDbType.Int);
            cmd.Parameters.Add("@fec", SqlDbType.DateTime);
            cmd.Parameters.Add("@diag", SqlDbType.VarChar);
            cmd.Parameters.Add("@estat", SqlDbType.Char);
            cmd.Parameters.Add("@peso", SqlDbType.Char);
            cmd.Parameters.Add("@IDMedico", SqlDbType.Int);
            cmd.Parameters.Add("@IDHistoria", SqlDbType.Int);
            cmd.Parameters["@NCita"].Value = NCita;
            cmd.Parameters["@fec"].Value = fec;
            cmd.Parameters["@diag"].Value = diagnostico;
            cmd.Parameters["@estat"].Value = estat;
            cmd.Parameters["@peso"].Value = peso;
            cmd.Parameters["@IDMedico"].Value = IDMedico;
            cmd.Parameters["@IDHistoria"].Value = IDHistoria;
            int resultado = cmd.ExecuteNonQuery();
            Con.Close();
            if (resultado > 0)
            {
                return true;
            }
            else return false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string Ncit = lblNCita.Text;
            try
            {
                EliminarCita(Ncit);
                MessageBox.Show("Cita Eliminada");
                MostrarCitasPaciente();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        public bool EliminarCita(string NCita)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_CitaMedica_Eliminar @NCit", Con);
            cmd.Parameters.Add("@Ncit", SqlDbType.Int);
            cmd.Parameters["@Ncit"].Value = NCita;
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
