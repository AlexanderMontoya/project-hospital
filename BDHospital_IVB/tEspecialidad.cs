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
    public partial class tEspecialidad : Form
    {
        public tEspecialidad()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tEspecialidad_Load(object sender, EventArgs e)
        {
            MostrarEspecialidad();
        }

        void MostrarEspecialidad()
        {
            string cmd = string.Format("select * from tEspecialidad");
            dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lblID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtNom.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtDescripcion.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtEstado.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtBuscar.Text;
            if (id == "")
            {
                MostrarEspecialidad();
            }
            else
            {
                string cmd = string.Format("select * from tEspecialidad where idEspecialidad=" + id);//
                dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            lblID.Text = "";
            txtNom.Text = "";
            txtDescripcion.Text = "";
            txtEstado.Text = "";
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string nom = txtNom.Text;
            string des = txtDescripcion.Text;
            string estado = txtEstado.Text;
            try
            {
                IngresarEspecialidad(nom, des, estado);
                MessageBox.Show("Los datos han sido registrados");
                MostrarEspecialidad();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool IngresarEspecialidad( string nomEspecialidad, string descripcion, string estado )
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_tEspecialidad_Insertar @nom, @desc, @estado", Con);
            cmd.Parameters.Add("@nom", SqlDbType.VarChar);
            cmd.Parameters.Add("@desc", SqlDbType.VarChar);
            cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            cmd.Parameters["@nom"].Value = nomEspecialidad;
            cmd.Parameters["@desc"].Value = descripcion;
            cmd.Parameters["@estado"].Value = estado;
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
            string id = lblID.Text;
            string nom = txtNom.Text;
            string des = txtDescripcion.Text;
            string estado = txtEstado.Text;
            try
            {
                ModificarEspecialidad(id, nom, des, estado);
                MessageBox.Show("Los datos han sido Modificados");
                MostrarEspecialidad();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool ModificarEspecialidad(string id, string nomEspecialidad, string descripcion, string estado)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_tEspecialidad_Modificar @id ,@nom, @desc, @estado", Con);
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters.Add("@nom", SqlDbType.VarChar);
            cmd.Parameters.Add("@desc", SqlDbType.VarChar);
            cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            cmd.Parameters["@id"].Value = id;
            cmd.Parameters["@nom"].Value = nomEspecialidad;
            cmd.Parameters["@desc"].Value = descripcion;
            cmd.Parameters["@estado"].Value = estado;
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
            string id = lblID.Text;
            try
            {
                EliminarEspecialidad(id);
                MessageBox.Show("Especialidad Eliminada");
                MostrarEspecialidad();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        public bool EliminarEspecialidad(string id)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_tEspecialidad_Eliminar @id", Con);
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = id;
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
