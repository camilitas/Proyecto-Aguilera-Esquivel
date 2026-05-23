using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            dtpFechaIni.Checked = false;
            dtpFechaFin.Checked = false;
            CargarCombos();
            CargarGrilla();
        }

        private void CargarCombos()
        {
            cmbLogin.Items.Clear();
            cmbLogin.Items.Add("");
            foreach (var l in BitacoraBLL.Instancia.ObtenerLogins())
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
            string login = cmbLogin.SelectedItem?.ToString();
            DateTime? fechaIni = dtpFechaIni.Checked ? dtpFechaIni.Value.Date : (DateTime?)null;
            DateTime? fechaFin = dtpFechaFin.Checked ? dtpFechaFin.Value.Date : (DateTime?)null;
            string modulo = cmbModulo.SelectedItem?.ToString();
            string evento = cmbEvento.SelectedItem?.ToString();
            int? criticidad = null;
            if (cmbCriticidad.SelectedIndex > 0)
                criticidad = int.Parse(cmbCriticidad.SelectedItem.ToString());

            var lista = BitacoraBLL.Instancia.ObtenerFiltrado(login, fechaIni, fechaFin, modulo, evento, criticidad);

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
            if(dgvBitacora.SelectedRows.Count == 0) return;

            var fila = dgvBitacora.SelectedRows[0];
            string nombreUsuario = fila.Cells["Login"].Value?.ToString() ?? "";

            // Traemos Nombre y Apellido con una query
            Usuario usuario = UsuarioBLL.Instancia.ObtenerPorNombreUsuario(nombreUsuario);

            if (usuario != null)
            {
                txtNombre.Text = usuario.Nombre;
                txtApellido.Text = usuario.Apellido;
            }
            else
            {
                txtNombre.Text = "";
                txtApellido.Text = "";
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            MostrarNombreApellido();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (dtpFechaIni.Checked && dtpFechaFin.Checked)
            {
                if (dtpFechaIni.Value.Date > dtpFechaFin.Value.Date)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha de fin.",
                        "Fechas invalidas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            CargarGrilla();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            new FormPrincipal().Show();
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(ImprimirBitacora);

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.ShowDialog();
        }

        private void ImprimirBitacora(object sender, PrintPageEventArgs e)
        {
            float y = 50;
            float x = 40;
            Font fontTitulo = new Font("Arial", 14, FontStyle.Bold);
            Font fontHeader = new Font("Arial", 9, FontStyle.Bold);
            Font fontData = new Font("Arial", 8);

            // Titulo
            e.Graphics.DrawString("Bitácora de Eventos", fontTitulo, Brushes.Black, x, y);
            y += 30;

            // Fecha de impresion
            e.Graphics.DrawString("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                fontData, Brushes.Black, x, y);
            y += 25;

            // Headers
            e.Graphics.DrawString("Login", fontHeader, Brushes.Black, x, y);
            e.Graphics.DrawString("Fecha", fontHeader, Brushes.Black, x + 120, y);
            e.Graphics.DrawString("Hora", fontHeader, Brushes.Black, x + 200, y);
            e.Graphics.DrawString("Módulo", fontHeader, Brushes.Black, x + 260, y);
            e.Graphics.DrawString("Evento", fontHeader, Brushes.Black, x + 350, y);
            e.Graphics.DrawString("Criticidad", fontHeader, Brushes.Black, x + 500, y);
            y += 20;

            // Linea separadora
            e.Graphics.DrawLine(Pens.Black, x, y, 760, y);
            y += 10;

            // Datos de la grilla
            foreach (DataGridViewRow fila in dgvBitacora.Rows)
            {
                if (y > e.PageBounds.Height - 50)
                {
                    e.HasMorePages = true;
                    return;
                }

                e.Graphics.DrawString(fila.Cells["Login"].Value?.ToString() ?? "",
                    fontData, Brushes.Black, x, y);
                e.Graphics.DrawString(fila.Cells["Fecha"].Value?.ToString() ?? "",
                    fontData, Brushes.Black, x + 120, y);
                e.Graphics.DrawString(fila.Cells["Hora"].Value?.ToString() ?? "",
                    fontData, Brushes.Black, x + 200, y);
                e.Graphics.DrawString(fila.Cells["Modulo"].Value?.ToString() ?? "",
                    fontData, Brushes.Black, x + 260, y);
                e.Graphics.DrawString(fila.Cells["Evento"].Value?.ToString() ?? "",
                    fontData, Brushes.Black, x + 350, y);
                e.Graphics.DrawString(fila.Cells["Criticidad"].Value?.ToString() ?? "",
                    fontData, Brushes.Black, x + 500, y);
                y += 18;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbLogin.SelectedIndex = 0;
            cmbModulo.SelectedIndex = 0;
            cmbEvento.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
            dtpFechaIni.Checked = false;
            dtpFechaIni.Value = DateTime.Today; // resetea la fecha
            dtpFechaFin.Checked = false;
            dtpFechaFin.Value = DateTime.Today; // resetea la fecha
            CargarGrilla();
        }
    }
}
