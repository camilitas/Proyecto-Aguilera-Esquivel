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
    public partial class FormGestionRoles : Form
    {
        private int _idSeleccionado = -1;
        public FormGestionRoles()
        {
            InitializeComponent();
        }

        private void FormGestionRoles_Load(object sender, EventArgs e)
        {
            rbPatente.Checked = true;
            CargarRoles();
            CargarDisponibles();
            HabilitarBotonera();
            DeshabilitarCampos();
            lblModo.Text = "Seleccioná una opción";
        }

        private void HabilitarBotonera()
        {
            btnNuevo.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;
        }
        private void ModoAccion()
        {
            btnNuevo.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
        }
        private void DeshabilitarCampos()
        {
            txtNombre.Enabled = false;
            txtDescripcion.Enabled = false;
        }
        private void HabilitarCampos()
        {
            txtNombre.Enabled = true;
            txtDescripcion.Enabled = true;
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            lstContenido.Items.Clear();
        }

        private void CargarRoles()
        {
            lstRoles.Items.Clear();
            var lista = RolBLL.Instancia.ObtenerTodos();
            foreach (var r in lista)
                lstRoles.Items.Add(r);
        }
        private void CargarDisponibles()
        {
            lstDisponibles.Items.Clear();
            if (rbPatente.Checked)
            {
                var patentes = PatenteBLL.Instancia.ObtenerTodos();
                foreach (var p in patentes)
                    lstDisponibles.Items.Add(p);
            }
            else
            {
                var familias = FamiliaBLL.Instancia.ObtenerTodos();
                foreach (var f in familias)
                    lstDisponibles.Items.Add(f);
            }
        }
        private void CargarContenido(int idRol)
        {
            lstContenido.Items.Clear();
            var patentes = RolBLL.Instancia.ObtenerPatentes(idRol);
            foreach (var p in patentes)
                lstContenido.Items.Add("[P] " + p.Nombre);

            var familias = RolBLL.Instancia.ObtenerFamilias(idRol);
            foreach (var f in familias)
                lstContenido.Items.Add("[F] " + f.Nombre);
        }

        private void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItem == null) return;

            var rol = (Rol)lstRoles.SelectedItem;
            _idSeleccionado = rol.Id;
            txtNombre.Text = rol.Nombre;
            txtDescripcion.Text = rol.Descripcion;
            CargarContenido(rol.Id);
            CargarDisponibles();
            lblModo.Text = "Rol seleccionado";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _idSeleccionado = -1;
            LimpiarCampos();
            HabilitarCampos();
            ModoAccion();
            lblModo.Text = "Modo Nuevo";
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un rol primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HabilitarCampos();
            ModoAccion();
            lblModo.Text = "Modo Modificar";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un rol primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "¿Querés eliminar este rol?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool ok = RolBLL.Instancia.Eliminar(_idSeleccionado);
                    if (ok)
                    {
                        MessageBox.Show("Rol eliminado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        DeshabilitarCampos();
                        HabilitarBotonera();
                        _idSeleccionado = -1;
                        CargarRoles();
                        lblModo.Text = "Seleccioná una opción";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_idSeleccionado == -1)
            {
                // MODO NUEVO
                Rol nuevo = new Rol
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text
                };

                bool ok = RolBLL.Instancia.Insertar(nuevo);
                if (ok)
                {
                    MessageBox.Show("Rol creado exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarRoles();
                    LimpiarCampos();
                    DeshabilitarCampos();
                    HabilitarBotonera();
                    lblModo.Text = "Seleccioná una opción";
                }
            }
            else
            {
                // MODO MODIFICAR
                Rol modificado = new Rol
                {
                    Id = _idSeleccionado,
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text
                };

                bool ok = RolBLL.Instancia.Modificar(modificado);
                if (ok)
                {
                    MessageBox.Show("Rol modificado exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarRoles();
                    LimpiarCampos();
                    DeshabilitarCampos();
                    HabilitarBotonera();
                    lblModo.Text = "Seleccioná una opción";
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            DeshabilitarCampos();
            HabilitarBotonera();
            lblModo.Text = "Seleccioná una opción";
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            new FormPrincipal().Show();
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un rol primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lstDisponibles.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un elemento para agregar.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (rbPatente.Checked)
                {
                    var patente = (Patente)lstDisponibles.SelectedItem;
                    RolBLL.Instancia.AgregarPatente(_idSeleccionado, patente.Id);
                }
                else
                {
                    var familia = (Familia)lstDisponibles.SelectedItem;
                    RolBLL.Instancia.AgregarFamilia(_idSeleccionado, familia.Id);
                }
                CargarContenido(_idSeleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1 || lstContenido.SelectedItem == null) return;

            string item = lstContenido.SelectedItem.ToString();

            try
            {
                if (item.StartsWith("[P]"))
                {
                    string nombre = item.Replace("[P] ", "");
                    var patentes = RolBLL.Instancia.ObtenerPatentes(_idSeleccionado);
                    var patente = patentes.FirstOrDefault(p => p.Nombre == nombre);
                    if (patente != null)
                        RolBLL.Instancia.EliminarPatente(_idSeleccionado, patente.Id);
                }
                else
                {
                    string nombre = item.Replace("[F] ", "");
                    var familias = RolBLL.Instancia.ObtenerFamilias(_idSeleccionado);
                    var familia = familias.FirstOrDefault(f => f.Nombre == nombre);
                    if (familia != null)
                        RolBLL.Instancia.EliminarFamilia(_idSeleccionado, familia.Id);
                }
                CargarContenido(_idSeleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbPatente_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPatente.Checked) CargarDisponibles();
        }

        private void rbFamilia_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFamilia.Checked) CargarDisponibles();
        }
    }
}
