using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GestiondeUsuario
{
    public partial class FormRestore : Form
    {
        public FormRestore()
        {
            InitializeComponent();
        }

        private void FormRestore_Load(object sender, EventArgs e)
        {
            lblMensaje.Text = "Seleccioná el archivo de backup para restaurar.\n" +
                             "Se recomienda elegir el backup más reciente para minimizar la pérdida de datos.";
            CargarBackups();
        }

        private void CargarBackups()
        {
            lstBackups.Items.Clear();
            string carpeta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Backups");

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
                return;
            }

            foreach (var archivo in Directory.GetFiles(carpeta, "*.bak"))
                lstBackups.Items.Add(Path.GetFileName(archivo));
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (lstBackups.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un backup primero.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "¿Estás seguro que querés restaurar este backup?\n" +
                "Se perderán todos los datos posteriores a ese backup.",
                "Confirmar restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                MessageBox.Show("Restore ejecutado. El sistema volverá al login.",
                    "Restore completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new Form1().Show();
                this.Close();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Close();
        }
    }
}
