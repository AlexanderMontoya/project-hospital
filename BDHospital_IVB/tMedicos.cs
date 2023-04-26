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
using System.Data.SqlClient;

namespace BDHospital_IVB
{
    public partial class tMedicos : Form
    {
        public tMedicos()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void MostrarMedicos()
        {
            string cmd = string.Format("select idMedico,nombre,apellido,sexo,fechaNacimiento,fkEspecialidad,tEspecialidad.nomEspelidad from tMedico join tEspecialidad on fkEspecialidad=idEspecialidad");
            dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
        }

        private void tMedicos_Load(object sender, EventArgs e)
        {
            MostrarMedicos();
        }

        private void btnBuscarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fo = new OpenFileDialog();
            DialogResult rs = fo.ShowDialog();
            if (rs == DialogResult.OK)
            {
                pbFotoMedico.Image = Image.FromFile(fo.FileName);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lblID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtNom.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtAP.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtSexo.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtFec.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtIDEspecialidad.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            lblNomEspecialidad.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            string idMedico = lblID.Text;
            string cmd2 = string.Format("select fotoMedico from tMedico where idMedico=" + idMedico);
            DataSet ds2 = Utilidades.Ejecutar(cmd2);
            byte[] FotoUser = (byte[])ds2.Tables[0].Rows[0]["fotoMedico"];
            MemoryStream ms = new MemoryStream(FotoUser);
            pbFotoMedico.Image = Image.FromStream(ms);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtBuscar.Text;
            if (id == "")
            {
                MostrarMedicos();
            }
            else
            {
                string cmd = string.Format("select idMedico,nombre,apellido,sexo,fechaNacimiento,fkEspecialidad,tEspecialidad.nomEspelidad from tMedico join tEspecialidad on fkEspecialidad=idEspecialidad where idMedico=" + id);//
                dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            lblID.Text = "";
            txtNom.Text = "";
            txtAP.Text = "";
            txtFec.Text = "";
            txtIDEspecialidad.Text = "";
            txtSexo.Text = "";
            lblNomEspecialidad.Text = "";
            pbFotoMedico.Image = null;
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string nom = txtNom.Text;
            string ape = txtAP.Text;
            string sex = txtSexo.Text;
            DateTime fec = DateTime.Parse(txtFec.Text);
            string esp = txtIDEspecialidad.Text;
            PictureBox pb = pbFotoMedico;
            try
            {
                IngresarMedico( nom, ape, sex, fec, esp, pb);
                MessageBox.Show("Los datos han sido registrados");
                MostrarMedicos();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool IngresarMedico(string nom, string ap, string sexo, DateTime fec, string esp, PictureBox pb)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Medico_Insertar @nom, @ap, @sexo, @fec, @esp, @pb", Con);
            cmd.Parameters.Add("@nom", SqlDbType.VarChar);
            cmd.Parameters.Add("@ap", SqlDbType.VarChar);
            cmd.Parameters.Add("@sexo", SqlDbType.Char);
            cmd.Parameters.Add("@fec", SqlDbType.DateTime);
            cmd.Parameters.Add("@esp", SqlDbType.Int);
            cmd.Parameters.Add("@pb", SqlDbType.VarBinary);
            cmd.Parameters["@nom"].Value = nom;
            cmd.Parameters["@ap"].Value = ap;
            cmd.Parameters["@sexo"].Value = sexo;
            cmd.Parameters["@fec"].Value = fec;
            cmd.Parameters["@esp"].Value = esp;
            MemoryStream ms = new MemoryStream();
            pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            cmd.Parameters["@pb"].Value = ms.GetBuffer();
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
            string idMed = lblID.Text;
            try
            {
                EliminarMedico(idMed);
                MessageBox.Show("Medico Eliminado");
                MostrarMedicos();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        public bool EliminarMedico(string idMed)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Medico_Eliminar @idmed", Con);
            cmd.Parameters.Add("@idmed", SqlDbType.Int);
            cmd.Parameters["@idmed"].Value = idMed;
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
            string ID = lblID.Text;
            string nom = txtNom.Text;
            string ap = txtAP.Text;
            string sexo = txtSexo.Text;
            DateTime fec = DateTime.Parse(txtFec.Text);
            string esp = txtIDEspecialidad.Text;
            PictureBox pb = pbFotoMedico;
            try
            {
                ModificarMedico(ID,nom, ap,sexo,fec,esp, pb);
                MessageBox.Show("Los datos han sido Modificados");
                MostrarMedicos();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool ModificarMedico(string ID,string nom, string ap, string sexo, DateTime fec, string esp, PictureBox pb)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Medico_Modificar @id ,@nom, @ap, @sexo, @fec, @esp, @pb", Con);
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters.Add("@nom", SqlDbType.VarChar);
            cmd.Parameters.Add("@ap", SqlDbType.VarChar);
            cmd.Parameters.Add("@sexo", SqlDbType.Char);
            cmd.Parameters.Add("@fec", SqlDbType.Date);
            cmd.Parameters.Add("@esp", SqlDbType.VarChar);
            cmd.Parameters.Add("@pb", SqlDbType.VarBinary);
            cmd.Parameters["@id"].Value = ID;
            cmd.Parameters["@nom"].Value = nom;
            cmd.Parameters["@ap"].Value = ap;
            cmd.Parameters["@sexo"].Value = sexo;
            cmd.Parameters["@fec"].Value = fec;
            cmd.Parameters["@esp"].Value = esp;
            MemoryStream ms = new MemoryStream();
            pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            cmd.Parameters["@pb"].Value = ms.GetBuffer();
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
