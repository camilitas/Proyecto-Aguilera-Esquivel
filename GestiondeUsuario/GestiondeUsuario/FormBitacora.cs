using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;

namespace GestiondeUsuario
{
    public partial class FormBitacora : Form
    {
        public FormBitacora()
        {
            InitializeComponent();
        }

        private void FormBitacora_Load(object sender, EventArgs e)
        {
            CargarCombos();
            CargarGrilla();
        }

        private void CargarCombos()
        {
            BitacoraDAL dal = new BitacoraDAL();

            cmbLogin.Items.Clear();
            cmbLogin.Items.Add("");
            foreach (var l in dal.ObtenerLogins())
                cmbLogin.Items.Add(l);
            cmbLogin.SelectedIndex = 0;

            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("");
            cmbModulo.Items.Add("Usuarios");
            cmbModulo.Items.Add("Ventas");
            cmbModulo.Items.Add("Compras");
            cmbModulo.Items.Add("Maestro");
            cmbModulo.SelectedIndex = 0;

            cmbEvento.Items.Clear();
            cmbEvento.Items.Add("");
            cmbEvento.Items.Add("Login");
            cmbEvento.Items.Add("Logout");
            cmbEvento.Items.Add("Cambiar Clave");
            cmbEvento.Items.Add("Crear Usuario");
            cmbEvento.Items.Add("Bloquear Usuario");
            cmbEvento.Items.Add("Login fallido");
            cmbEvento.SelectedIndex = 0;

            cmbCriticidad.Items.Clear();
            cmbCriticidad.Items.Add("");
            for (int i = 1; i <= 5; i++)
                cmbCriticidad.Items.Add(i.ToString());
            cmbCriticidad.SelectedIndex = 0;
        }

        private void CargarGrilla()
        {
            BitacoraDAL dal = new BitacoraDAL();

            string login = cmbLogin.SelectedItem?.ToString();
            DateTime? fechaIni = dtpFechaIni.Checked ? dtpFechaIni.Value.Date : (DateTime?)null;
            DateTime? fechaFin = dtpFechaFin.Checked ? dtpFechaFin.Value.Date : (DateTime?)null;
            string modulo = cmbModulo.SelectedItem?.ToString();
            string evento = cmbEvento.SelectedItem?.ToString();
            int? criticidad = null;
            if (cmbCriticidad.SelectedIndex > 0)
                criticidad = int.Parse(cmbCriticidad.SelectedItem.ToString());

            var lista = dal.ObtenerFiltrado(
                login, fechaIni, fechaFin, modulo, evento, criticidad);

            // Proyectamos para separar Fecha y Hora en columnas distintas
            var vista = new List<object>();
            foreach (var b in lista)
            {
                vista.Add(new
                {
                    Login = b.Usuario,
                    Fecha = b.Fecha.ToShortDateString(),
                    Hora = b.Fecha.ToString("HH:mm"),
                    Modulo = b.Modulo,
                    Evento = b.Accion,
                    Criticidad = b.Criticidad,
                    _nombre = b.Nombre,
                    _apellido = b.Apellido
                });
            }

            dgvBitacora.DataSource = null;
            dgvBitacora.DataSource = vista;

            if (dgvBitacora.Columns.Contains("_nombre"))
                dgvBitacora.Columns["_nombre"].Visible = false;
            if (dgvBitacora.Columns.Contains("_apellido"))
                dgvBitacora.Columns["_apellido"].Visible = false;

            // Seleccionar primer registro por defecto
            if (dgvBitacora.Rows.Count > 0)
            {
                dgvBitacora.Rows[0].Selected = true;
                MostrarNombreApellido();
            }
            else
            {
                txtNombre.Text = "";
                txtApellido.Text = "";
            }
        }

        private void MostrarNombreApellido()
        {
            if (dgvBitacora.SelectedRows.Count == 0) return;
            var fila = dgvBitacora.SelectedRows[0];
            txtNombre.Text = fila.Cells["_nombre"].Value?.ToString() ?? "";
            txtApellido.Text = fila.Cells["_apellido"].Value?.ToString() ?? "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            MostrarNombreApellido();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            cmbLogin.SelectedIndex = 0;
            cmbModulo.SelectedIndex = 0;
            cmbEvento.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
            dtpFechaIni.Checked = false;
            dtpFechaFin.Checked = false;
            CargarGrilla();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            new FormPrincipal().Show();
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función de impresión no implementada aún.",
               "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
