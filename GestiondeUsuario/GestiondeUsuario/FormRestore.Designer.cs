namespace GestiondeUsuario
{
    partial class FormRestore
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMensaje = new System.Windows.Forms.Label();
            this.lstBackups = new System.Windows.Forms.ListBox();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.lblBackups = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Location = new System.Drawing.Point(40, 25);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(35, 13);
            this.lblMensaje.TabIndex = 0;
            this.lblMensaje.Text = "label1";
            // 
            // lstBackups
            // 
            this.lstBackups.FormattingEnabled = true;
            this.lstBackups.Location = new System.Drawing.Point(43, 68);
            this.lstBackups.Name = "lstBackups";
            this.lstBackups.Size = new System.Drawing.Size(570, 212);
            this.lstBackups.TabIndex = 1;
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(142, 325);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(137, 32);
            this.btnRestore.TabIndex = 2;
            this.btnRestore.Text = "Restaurar";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(368, 325);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(137, 32);
            this.btnSalir.TabIndex = 3;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // lblBackups
            // 
            this.lblBackups.AutoSize = true;
            this.lblBackups.Location = new System.Drawing.Point(40, 52);
            this.lblBackups.Name = "lblBackups";
            this.lblBackups.Size = new System.Drawing.Size(35, 13);
            this.lblBackups.TabIndex = 4;
            this.lblBackups.Text = "label1";
            // 
            // FormRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 427);
            this.Controls.Add(this.lblBackups);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.lstBackups);
            this.Controls.Add(this.lblMensaje);
            this.Name = "FormRestore";
            this.Text = "FormRestore";
            this.Load += new System.EventHandler(this.FormRestore_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.ListBox lstBackups;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label lblBackups;
    }
}