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

            MostrarModoAgregar();
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            UsuarioBLL bll = new UsuarioBLL();
            List<Usuario> lista;

            if (rbActivos.Checked)
                lista = UsuarioBLL.Instancia.ObtenerActivos();
            else
                lista = UsuarioBLL.Instancia.ObtenerTodos();

            dgvUsuarios.DataSource = null;
            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.DataSource = lista;
        }
        private void MostrarModoAgregar()
        {
            LimpiarCampos();
            _idSeleccionado = -1;
            lblModo.Text = "Modo Agregar";
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
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
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
            lblModo.Text = "Modo Modificar";
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

            DialogResult confirm = MessageBox.Show(
                "¿Estás segura que querés deshabilitar este usuario?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                bool ok = UsuarioBLL.Instancia.Deshabilitar(_idSeleccionado);
                if (ok)
                {
                    MessageBox.Show("Usuario deshabilitado.", "Éxito",
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
    }
}
