using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestiondeUsuario
{
    public partial class FormGU : Form
    {
        private int _idSeleccionado = -1;

        public FormGU()
        {
            InitializeComponent();
        }

        private void FormGU_Load(object sender, EventArgs e)
        {
            cmbRol.Items.Clear(); // Cargamos los roles en el combo
            cmbRol.Items.Add("General");
            cmbRol.Items.Add("Admin");
            cmbRol.SelectedIndex = 0;

            rbActivos.Checked = true;
            CargarGrilla();
            

            HabilitarBotonera();
            lblModo.Text = "Seleccioná una opción de la botonera";
            DeshabilitarCampos();
        }

        private void ModoAccion()
        {
            btnAgregar.Enabled = false;
            btnModificar.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnDeshabilitar.Enabled = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void DeshabilitarCampos()
        {
            txtDNI.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtNombreUsuario.Enabled = false;
            txtCorreo.Enabled = false;
            cmbRol.Enabled = false;
            rbSi.Enabled = false;
            rbNo.Enabled = false;
        }
        private void HabilitarCampos()
        {
            txtDNI.Enabled = true;
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtNombreUsuario.Enabled = true;
            txtCorreo.Enabled = true;
            cmbRol.Enabled = true;
            rbSi.Enabled = true;
            rbNo.Enabled = true;
        }

        private void HabilitarBotonera()
        {
            btnAgregar.Enabled = true;
            btnModificar.Enabled = true;
            btnDesbloquear.Enabled = true;
            btnDeshabilitar.Enabled = true;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void CargarGrilla()
        {
            List<Usuario> lista;

            if (rbActivos.Checked)
                lista = UsuarioBLL.Instancia.ObtenerActivos();
            else
                lista = UsuarioBLL.Instancia.ObtenerTodos();

            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.DataSource = null;
            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.DataSource = lista;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            if (dgvUsuarios.Columns.Contains("Contraseña"))
                dgvUsuarios.Columns["Contraseña"].Visible = false;
            if (dgvUsuarios.Columns.Contains("IntentosFallidos"))
                dgvUsuarios.Columns["IntentosFallidos"].Visible = false;
            if (dgvUsuarios.Columns.Contains("FechaCreacion"))
                dgvUsuarios.Columns["FechaCreacion"].Visible = false;
            if (dgvUsuarios.Columns.Contains("PrimerIngreso"))
                dgvUsuarios.Columns["PrimerIngreso"].Visible = false;
            if (dgvUsuarios.Columns.Contains("Bloqueado"))
                dgvUsuarios.Columns["Bloqueado"].Visible = false;

            _idSeleccionado = -1;
            dgvUsuarios.ClearSelection();
            dgvUsuarios.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }
        private void MostrarModoAgregar()
        {
            HabilitarCampos();
            LimpiarCampos();
            _idSeleccionado = -1;
            lblModo.Text = "Modo Agregar";

            // Habilitamos todos los campos para agregar
            txtDNI.ReadOnly = false;
            txtNombre.ReadOnly = false;
            txtApellido.ReadOnly = false;
            txtCorreo.ReadOnly = false;
            cmbRol.Enabled = true;
            txtDNI.BackColor = Color.White;
            txtNombre.BackColor = Color.White;
            txtApellido.BackColor = Color.White;
            txtCorreo.BackColor = Color.White;

            txtNombreUsuario.ReadOnly = true;
            txtNombreUsuario.BackColor = Color.LightGray;
            txtNombreUsuario.Text = "Se genera automaticamente.";

            btnDeshabilitar.Text = "Deshabilitar"; // resetea el texto
            ModoAccion(); // al agregar solo quedan Aplicar y Cancelar
        }
        private void LimpiarCampos()
        {
            txtDNI.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtNombreUsuario.Text = "";
            txtCorreo.Text = "";
            cmbRol.SelectedIndex = 0;
            rbSi.Checked = true;
        }
       
        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            MostrarModoAgregar();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text) ||
            string.IsNullOrEmpty(txtApellido.Text) ||
            string.IsNullOrEmpty(txtDNI.Text) ||
            string.IsNullOrEmpty(txtCorreo.Text))
            {
                MessageBox.Show("Completá todos los campos.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDNI.Text, out int dni))
            {
                MessageBox.Show("El DNI debe ser un número.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_idSeleccionado == -1)
            {
                // MODO AGREGAR
                Usuario nuevo = new Usuario()
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Email = txtCorreo.Text,
                    DNI = dni,
                    Rol = cmbRol.SelectedItem.ToString(),
                    // Contraseña inicial: DNI + Nombre
                    Contraseña = txtApellido.Text + txtDNI.Text,
                    Activo = true,
                    PrimerIngreso = true
                };
            try
                {
                    bool ok = UsuarioBLL.Instancia.CrearUsuario(nuevo);

                    if (ok)
                    {
                      MessageBox.Show("Usuario creado exitosamente.\nNombre de usuario: " + nuevo.Apellido + dni.ToString(),
                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       CargarGrilla();
                       LimpiarCampos();
                       HabilitarBotonera();
                       lblModo.Text = "Seleccioná una opción de la botonera";
                       DeshabilitarCampos();
                    }
                }
            catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                // MODO MODIFICAR
                Usuario modificado = new Usuario()
                {
                    Id = _idSeleccionado,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Email = txtCorreo.Text,
                    DNI = dni,
                    Rol = cmbRol.SelectedItem.ToString()
                };

                bool ok = UsuarioBLL.Instancia.Modificar(modificado);

                if (ok)
                {
                    MessageBox.Show("Usuario modificado exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrilla();
                    LimpiarCampos();
                    HabilitarBotonera();
                    lblModo.Text = "Seleccioná una opción de la botonera";
                    DeshabilitarCampos();
                }
                else
                {
                    MessageBox.Show("Error al modificar el usuario.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un usuario primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool estaActivo = btnDeshabilitar.Text == "Deshabilitar";
            string accion = estaActivo ? "deshabilitar" : "habilitar";

            DialogResult confirm = MessageBox.Show(
                "¿Queres " + accion + " este usuario?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                bool ok = estaActivo
                    ? UsuarioBLL.Instancia.Deshabilitar(_idSeleccionado)
                    : UsuarioBLL.Instancia.Habilitar(_idSeleccionado);

                if (ok)
                {
                    MessageBox.Show("Usuario " + accion + "do exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrilla();
                    HabilitarBotonera();
                    LimpiarCampos();
                    lblModo.Text = "Seleccioná una opción de la botonera";
                    DeshabilitarCampos();
                }
            }
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un usuario primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná un usuario primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow fila = dgvUsuarios.SelectedRows[0];
            bool estaBloqueado = Convert.ToBoolean(fila.Cells["Bloqueado"].Value);

            if (!estaBloqueado)
            {
                MessageBox.Show("El usuario ya estaba desbloqueado.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblModo.Text = "Modo Desbloquear";

            bool ok = UsuarioBLL.Instancia.Desbloquear(_idSeleccionado);
            if (ok)
            {
                MessageBox.Show("Usuario desbloqueado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrilla();
                HabilitarBotonera();
                LimpiarCampos();
                lblModo.Text = "Seleccioná una opción de la botonera";
                DeshabilitarCampos();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DeshabilitarCampos();
            LimpiarCampos();
            HabilitarEdicion(false);
            HabilitarBotonera();
            lblModo.Text = "Seleccioná una opción de la botonera";
        }

        private void rbActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActivos.Checked)
            {
                CargarGrilla();
                LimpiarCampos();
                DeshabilitarCampos();
                lblModo.Text = "Seleccioná una opción de la botonera";
            }
        }

        private void rbTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTodos.Checked)
            {
                CargarGrilla();
                LimpiarCampos();
                DeshabilitarCampos();
                lblModo.Text = "Seleccioná una opción de la botonera";
            }
        }

        private void dgvUsuarios_SelectionChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count == 0) return;

                DataGridViewRow fila = dgvUsuarios.SelectedRows[0];
                _idSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);
                txtDNI.Text = fila.Cells["DNI"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
                txtNombreUsuario.Text = fila.Cells["NombreUsuario"].Value.ToString();
                txtCorreo.Text = fila.Cells["Email"].Value.ToString();
                cmbRol.SelectedItem = fila.Cells["Rol"].Value.ToString();
                bool activo = Convert.ToBoolean(fila.Cells["Activo"].Value);
                rbSi.Checked = activo;
                rbNo.Checked = !activo;
                btnDeshabilitar.Text = activo ? "Deshabilitar" : "Habilitar";
                HabilitarEdicion(false);
                lblModo.Text = "Usuario seleccionado";
            }
            catch
            {
                // Ignoramos errores de índice al recargar la grilla
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            HabilitarCampos();
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un usuario primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HabilitarEdicion(true);
            lblModo.Text = "Modo Modificar";
            ModoAccion();
        }

        private void HabilitarEdicion(bool habilitar)
        {
            // DNI, Nombre, Apellido y NombreUsuario siempre bloqueados
            txtDNI.ReadOnly = true;
            txtNombre.ReadOnly = true;
            txtApellido.ReadOnly = true;
            txtNombreUsuario.ReadOnly = true;
            txtDNI.BackColor = Color.LightGray;
            txtNombre.BackColor = Color.LightGray;
            txtApellido.BackColor = Color.LightGray;
            txtNombreUsuario.BackColor = Color.LightGray;

            // Solo Email y Rol se habilitan al modificar
            txtCorreo.ReadOnly = !habilitar;
            cmbRol.Enabled = habilitar;
            txtCorreo.BackColor = habilitar ? Color.White : Color.LightGray;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            new FormPrincipal().Show();
            this.Close();

        }

        private void dgvUsuarios_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Suprimimos el error de índice al recargar
             e.ThrowException = false;
        }
    }
}
