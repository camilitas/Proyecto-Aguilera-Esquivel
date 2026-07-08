using BLL;
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
    public partial class FormGestionRespaldo : Form
    {
        public FormGestionRespaldo()
        {
            InitializeComponent();
        }

        private void BackUp_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Seleccioná la carpeta donde guardar el backup";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BackUpRestoreBLL.Instancia.RealizarBackUp(fbd.SelectedPath);
                    MessageBox.Show("Se realizó BackUp con éxito.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al realizar backup: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Backup files (*.bak)|*.bak";
            ofd.Title = "Seleccioná el archivo de backup";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DialogResult confirm = MessageBox.Show(
                    "¿Estás seguro que querés restaurar este backup?\nSe perderán todos los datos posteriores.",
                    "Confirmar Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        BackUpRestoreBLL.Instancia.RealizarRestore(ofd.FileName);
                        MessageBox.Show("Se realizó Restore con éxito.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new Form1().Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al realizar restore: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Volver_Click(object sender, EventArgs e)
        {
            new FormPrincipal().Show();
            this.Close();
        }
    }
}
