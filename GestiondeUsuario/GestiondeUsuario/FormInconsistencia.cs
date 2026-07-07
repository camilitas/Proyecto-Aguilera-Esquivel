using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace GestiondeUsuario
{
    public partial class FormInconsistencia : Form
    {
        public FormInconsistencia()
        {
            InitializeComponent();
        }

        private void FormInconsistencia_Load(object sender, EventArgs e)
        {
            lblMensaje.Text = "Se detectó una inconsistencia en los datos del sistema.\n" +
                             "Por favor seleccioná una opción para continuar.";
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                DigitoVerificadorBLL.Instancia.RecalcularYGuardar();
                MessageBox.Show("Dígito verificador recalculado correctamente.\n" +
                                "El sistema aceptará los datos actuales como válidos.",
                    "Recalculado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new Form1().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recalcular: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            new FormRestore().Show();
            this.Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
