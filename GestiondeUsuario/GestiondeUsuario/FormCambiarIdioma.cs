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
    public partial class FormCambiarIdioma : Form
    {
        public FormCambiarIdioma()
        {
            InitializeComponent();
        }

        private void FormCambiarIdioma_Load(object sender, EventArgs e)
        {
            cmbIdiomas.Items.Clear();
            cmbIdiomas.Items.Add("español");
            cmbIdiomas.Items.Add("ingles");
            cmbIdiomas.Items.Add("coreano");

            // Seleccionamos el idioma actual
            cmbIdiomas.SelectedItem = SessionManager.Instancia.ObtenerIdioma();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cmbIdiomas.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un idioma.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idioma = cmbIdiomas.SelectedItem.ToString();
            GestorIdioma.Instancia.CambiarIdioma(idioma);

            MessageBox.Show("Idioma cambiado a: " + idioma, "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            new FormPrincipal().Show();
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cmbIdiomas.SelectedItem = SessionManager.Instancia.ObtenerIdioma();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            new FormPrincipal().Show();
            this.Close();
        }
    }
}
