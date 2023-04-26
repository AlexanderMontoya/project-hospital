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
    public partial class tAnalisisPaciente : Form
    {
        public tAnalisisPaciente()
        {
            InitializeComponent();
        }

        private void tAnalisisPaciente_Load(object sender, EventArgs e)
        {
            MostrarAnalisisPaciente();
        }

        void MostrarAnalisisPaciente()
        {
            string cmd = string.Format("select id_Analisis,fkCodAnalisis,fkPaciente,Fecha from tAnalisisPaciente where fkPaciente=" + HistoriaPaciente.NHistoria);
            dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fo = new OpenFileDialog();
            DialogResult rs = fo.ShowDialog();
            if (rs == DialogResult.OK)
            {
                pbResultado.Image = Image.FromFile(fo.FileName);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lblID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtCodA.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtNHistoria.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtFec.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            string idA = lblID.Text;
            string cmd2 = string.Format("select DocAnalisis from tAnalisisPaciente where id_Analisis=" + idA);
            DataSet ds2 = Utilidades.Ejecutar(cmd2);
            byte[] FotoResultado = (byte[])ds2.Tables[0].Rows[0]["DocAnalisis"];
            MemoryStream ms = new MemoryStream(FotoResultado);
            pbResultado.Image = Image.FromStream(ms);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string codA = txtBuscar.Text;
            if (codA== "")
            {
                MostrarAnalisisPaciente();
            }
            else
            {
                string cmd = string.Format("select id_Analisis,fkCodAnalisis,fkPaciente,Fecha from tAnalisisPaciente where fkCodAnalisis='{0}'", codA);
                dataGridView1.DataSource = Utilidades.Ejecutar(cmd).Tables[0];
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            lblID.Text = "";
            txtCodA.Text = "";
            txtFec.Text = "";
            txtNHistoria.Text = "";
            pbResultado.Image = null;

        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string CodA = txtCodA.Text;
            string NHistoria = txtNHistoria.Text;
            DateTime fec = DateTime.Parse(txtFec.Text);
            PictureBox pb = pbResultado;
            try
            {
                IngresarAnalisisPac(CodA, NHistoria, fec, pb);
                MessageBox.Show("Los datos han sido registrados");
                MostrarAnalisisPaciente();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool IngresarAnalisisPac(string CodA, string NHistoria, DateTime Fec, PictureBox pb)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_AnalisisPaciente_Insertar @codA, @NHistoria, @Fec, @pb", Con);
            cmd.Parameters.Add("@codA", SqlDbType.Char);
            cmd.Parameters.Add("@NHistoria", SqlDbType.Int);
            cmd.Parameters.Add("@Fec", SqlDbType.DateTime);
            cmd.Parameters.Add("@pb", SqlDbType.VarBinary);
            cmd.Parameters["@codA"].Value = CodA;
            cmd.Parameters["@Fec"].Value = Fec;
            cmd.Parameters["@NHistoria"].Value = NHistoria;
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

        private void btnModificar_Click(object sender, EventArgs e)
        {
            string idAPac = lblID.Text;
            string codA = txtCodA.Text;
            string nHistoria = txtNHistoria.Text;
            DateTime fec = DateTime.Parse(txtFec.Text);
            PictureBox pb = pbResultado;
            try
            {
                ModificarAnalisisPac(idAPac, codA,nHistoria, fec, pb);
                MessageBox.Show("Los datos han sido Modificados");
                MostrarAnalisisPaciente();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error " + error);
            }
        }

        public bool ModificarAnalisisPac( string idAPaciente, string CodA, string NHistoria, DateTime Fec, PictureBox pb)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_AnalisisPaciente_Modificar @idAPac,@codA,@NHistoria,@fec,@pb", Con);
            cmd.Parameters.Add("@idAPac", SqlDbType.Int);
            cmd.Parameters.Add("@codA", SqlDbType.Char);
            cmd.Parameters.Add("@NHistoria", SqlDbType.Int);
            cmd.Parameters.Add("@fec", SqlDbType.DateTime);
            cmd.Parameters.Add("@pb", SqlDbType.VarBinary);
            cmd.Parameters["@idAPac"].Value = idAPaciente;
            cmd.Parameters["@codA"].Value = CodA;
            cmd.Parameters["@NHistoria"].Value = NHistoria;
            cmd.Parameters["@fec"].Value = Fec;
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
            string idapac = lblID.Text;
            try
            {
                EliminarAnalisisPac(idapac);
                MessageBox.Show("Analisis Eliminado");
                MostrarAnalisisPaciente();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }
        public bool EliminarAnalisisPac(string idAPac)
        {
            SqlConnection Con = new SqlConnection("Data Source=DESKTOP-68QERQ7;Initial Catalog=BDHospital;Integrated Security=True");
            Con.Open();
            SqlCommand cmd = new SqlCommand("exec pa_AnalisisPaciente_Eliminar @idapac", Con);
            cmd.Parameters.Add("@idapac", SqlDbType.Int);
            cmd.Parameters["@idapac"].Value = idAPac;
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
