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
    public partial class tPacientes : Form
    {
        public tPacientes()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tPacientes_Load(object sender, EventArgs e)
        {
            MostrarPacientes();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lblID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtDNI.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtNom.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtAp.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtSexo.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtFec.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtDirec.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            txtTel.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            string idPaciente = lblID.Text;
            string cmd2 = string.Format("select fotoPaciente from tPaciente where idNumeroSeguro=" + idPaciente);
            DataSet ds2 = Utilidades.Ejecutar(cmd2);
            byte[] FotoUser = (byte[])ds2.Tables[0].Rows[0]["fotoPaciente"];
            MemoryStream ms = new MemoryStream(FotoUser);
            pbFoto.Image = Image.FromStream(ms);
        }

        private void btnBuscarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fo = new OpenFileDialog();
            DialogResult rs = fo.ShowDialog();
            if (rs == DialogResult.OK)
            {
                pbFoto.Image = Image.FromFile(fo.FileName);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string dniPaciente = txtDNIBuscar.Text;
            if (dniPaciente=="")
            {
                MostrarPacientes();
            }
            else
            {
                string cmd = string.Format("Select idNumeroSeguro,dni,nombre,apellido,sexo,fechaNacimiento,direccion,telefono from tPaciente where dni="+dniPaciente);//
                dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
            }
            
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string dni = txtDNI.Text;
            string nom = txtNom.Text;
            string ape = txtAp.Text;
            string sex = txtSexo.Text;
            DateTime fec = DateTime.Parse(txtFec.Text);
            string direc = txtDirec.Text;
            string tel = txtTel.Text;
            PictureBox pb = pbFoto;
            try
            {
                IngresarPaciente(dni, nom, ape, sex, fec, direc, tel, pb);
                MessageBox.Show("Los datos han sido registrados");
                MostrarPacientes();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
            
        }
        
        public bool IngresarPaciente(string DNI,string nombre,string apellido,string sexo,DateTime fec,string direc,string tel,PictureBox pb)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd=new SqlCommand("exec pa_Paciente_Insertar @dni,@nom,@ap,@sexo,@fec,@direc,@tel,@pb",Con);
            cmd.Parameters.Add("@dni",SqlDbType.VarChar);
            cmd.Parameters.Add("@nom",SqlDbType.VarChar);
            cmd.Parameters.Add("@ap",SqlDbType.VarChar);
            cmd.Parameters.Add("@sexo",SqlDbType.Char);
            cmd.Parameters.Add("@fec",SqlDbType.Date);
            cmd.Parameters.Add("@direc",SqlDbType.VarChar);
            cmd.Parameters.Add("@tel",SqlDbType.Char);
            cmd.Parameters.Add("@pb",SqlDbType.VarBinary);
            cmd.Parameters["@dni"].Value=DNI;
            cmd.Parameters["@nom"].Value=nombre;
            cmd.Parameters["@ap"].Value=apellido;
            cmd.Parameters["@sexo"].Value=sexo;
            cmd.Parameters["@fec"].Value=fec;
            cmd.Parameters["@direc"].Value=direc;
            cmd.Parameters["@tel"].Value=tel;
            MemoryStream ms=new MemoryStream();
            pb.Image.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
            cmd.Parameters["@pb"].Value=ms.GetBuffer();
            int resultado=cmd.ExecuteNonQuery();
            Con.Close();
            if (resultado > 0)
            {
                return true;
            }
            else return false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            lblID.Text = "";
            txtDNI.Text = "";
            txtNom.Text = "";
            txtAp.Text = "";
            txtSexo.Text = "";
            txtFec.Text = "";
            txtDirec.Text = "";
            txtTel.Text = "";
            pbFoto.Image = null;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string idpac = lblID.Text;
            try
            {
                EliminarPaciente(idpac);
                MessageBox.Show("Paciente Eliminado");
                MostrarPacientes();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }

        }
        public bool EliminarPaciente(string idPac)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Paciente_Delete @idpac", Con);
            cmd.Parameters.Add("@idpac", SqlDbType.Int);
            cmd.Parameters["@idpac"].Value = idPac;
            int resultado = cmd.ExecuteNonQuery();
            Con.Close();
            if (resultado > 0)
            {
                return true;
            }
            else return false;
        }
        void MostrarPacientes()
        {
            string cmd = string.Format("Select idNumeroSeguro,dni,nombre,apellido,sexo,fechaNacimiento,direccion,telefono from tPaciente");
            dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            string dni = txtDNI.Text;
            string nom = txtNom.Text;
            string ape = txtAp.Text;
            string sex = txtSexo.Text;
            DateTime fec = DateTime.Parse(txtFec.Text);
            string direc = txtDirec.Text;
            string tel = txtTel.Text;
            PictureBox pb = pbFoto;
            try
            {
                ModificarPaciente(dni, nom, ape, sex, fec, direc, tel, pb);
                MessageBox.Show("Los datos han sido Modificados");
                MostrarPacientes();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }
        public bool ModificarPaciente(string DNI, string nombre, string apellido, string sexo, DateTime fec, string direc, string tel, PictureBox pb)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_Paciente_Modificar @dni,@nom,@ap,@sexo,@fec,@direc,@tel,@pb", Con);
            cmd.Parameters.Add("@dni", SqlDbType.VarChar);
            cmd.Parameters.Add("@nom", SqlDbType.VarChar);
            cmd.Parameters.Add("@ap", SqlDbType.VarChar);
            cmd.Parameters.Add("@sexo", SqlDbType.Char);
            cmd.Parameters.Add("@fec", SqlDbType.Date);
            cmd.Parameters.Add("@direc", SqlDbType.VarChar);
            cmd.Parameters.Add("@tel", SqlDbType.Char);
            cmd.Parameters.Add("@pb", SqlDbType.VarBinary);
            cmd.Parameters["@dni"].Value = DNI;
            cmd.Parameters["@nom"].Value = nombre;
            cmd.Parameters["@ap"].Value = apellido;
            cmd.Parameters["@sexo"].Value = sexo;
            cmd.Parameters["@fec"].Value = fec;
            cmd.Parameters["@direc"].Value = direc;
            cmd.Parameters["@tel"].Value = tel;
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

        public static string IdPac = "";

        private void btnVerHistoria_Click(object sender, EventArgs e)
        {
            if (lblID.Text=="")
            {
                MessageBox.Show("Se requiere de id del paciente");
            }
            else
            {
                IdPac = lblID.Text;
                HistoriaPaciente frmHistoria = new HistoriaPaciente();
                frmHistoria.Show();
            }
        }
    }
}
