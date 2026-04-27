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

            // Solo Email y Rol son editables
            txtDNI.ReadOnly = true;
            txtNombre.ReadOnly = true;
            txtApellido.ReadOnly = true;
            txtNombreUsuario.ReadOnly = true;
            txtDNI.BackColor = Color.LightGray;
            txtNombre.BackColor = Color.LightGray;
            txtApellido.BackColor = Color.LightGray;
            txtNombreUsuario.BackColor = Color.LightGray;

            MostrarModoAgregar();
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            List<Usuario> lista;

            if (rbActivos.Checked)
                lista = UsuarioBLL.Instancia.ObtenerActivos();
            else
                lista = UsuarioBLL.Instancia.ObtenerTodos();

            dgvUsuarios.DataSource = null;
            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.DataSource = lista;

            dgvUsuarios.Columns["Contraseña"].Visible = false;
            dgvUsuarios.Columns["IntentosFallidos"].Visible = false;
            dgvUsuarios.Columns["FechaCreacion"].Visible = false;
            dgvUsuarios.Columns["PrimerIngreso"].Visible = false;
        }
        private void MostrarModoAgregar()
        {
            LimpiarCampos();
            _idSeleccionado = -1;
            lblModo.Text = "Modo Agregar";

            // Habilitamos todos los campos para agregar
            txtDNI.ReadOnly = false;
            txtNombre.ReadOnly = false;
            txtApellido.ReadOnly = false;
            txtNombreUsuario.ReadOnly = false;
            txtCorreo.ReadOnly = false;
            cmbRol.Enabled = true;
            txtDNI.BackColor = Color.White;
            txtNombre.BackColor = Color.White;
            txtApellido.BackColor = Color.White;
            txtNombreUsuario.BackColor = Color.White;
            txtCorreo.BackColor = Color.White;

            btnDeshabilitar.Text = "Deshabilitar"; // resetea el texto
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

                bool ok = UsuarioBLL.Instancia.CrearUsuario(nuevo);

                if (ok)
                {
                    MessageBox.Show("Usuario creado exitosamente.\nNombre de usuario: " + nuevo.Apellido + dni.ToString(),
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrilla();
                    MostrarModoAgregar();
                }
                else
                {
                    MessageBox.Show("Error al crear el usuario.", "Error",
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
                    MostrarModoAgregar();
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
                bool ok;
                if (estaActivo)
                {
                    ok = UsuarioBLL.Instancia.Deshabilitar(_idSeleccionado);
                    lblModo.Text = "Modo Deshabilitar";
                }
                else
                {
                    ok = UsuarioBLL.Instancia.Habilitar(_idSeleccionado);
                    lblModo.Text = "Modo Habilitar";
                }

                if (ok)
                {
                    MessageBox.Show("Usuario " + accion + "do exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrilla();
                    MostrarModoAgregar();
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

            lblModo.Text = "Modo Desbloquear";

            bool ok = UsuarioBLL.Instancia.Desbloquear(_idSeleccionado);
            if (ok)
            {
                MessageBox.Show("Usuario desbloqueado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrilla();
                MostrarModoAgregar();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MostrarModoAgregar();
        }

        private void rbActivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void rbTodos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void dgvUsuarios_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0) return;
            // Cargamos los datos del usuario seleccionado en los campos
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
            lblModo.Text = "Usuario seleccionado";

            // Cambia el texto del botón según estado
            btnDeshabilitar.Text = activo ? "Deshabilitar" : "Habilitar";

            // Campos bloqueados hasta que se clickee Modificar
            HabilitarEdicion(false);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un usuario primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HabilitarEdicion(true);
            lblModo.Text = "Modo Modificar";
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
    }
}
