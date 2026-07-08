namespace GestiondeUsuario
{
    partial class FormGestionRespaldo
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
            this.BackUp = new System.Windows.Forms.Button();
            this.Restore = new System.Windows.Forms.Button();
            this.Volver = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BackUp
            // 
            this.BackUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackUp.Location = new System.Drawing.Point(43, 118);
            this.BackUp.Name = "BackUp";
            this.BackUp.Size = new System.Drawing.Size(158, 83);
            this.BackUp.TabIndex = 0;
            this.BackUp.Text = "BackUp";
            this.BackUp.UseVisualStyleBackColor = true;
            this.BackUp.Click += new System.EventHandler(this.BackUp_Click);
            // 
            // Restore
            // 
            this.Restore.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Restore.Location = new System.Drawing.Point(218, 118);
            this.Restore.Name = "Restore";
            this.Restore.Size = new System.Drawing.Size(158, 83);
            this.Restore.TabIndex = 1;
            this.Restore.Text = "Restore";
            this.Restore.UseVisualStyleBackColor = true;
            this.Restore.Click += new System.EventHandler(this.Restore_Click);
            // 
            // Volver
            // 
            this.Volver.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Volver.Location = new System.Drawing.Point(137, 242);
            this.Volver.Name = "Volver";
            this.Volver.Size = new System.Drawing.Size(140, 52);
            this.Volver.TabIndex = 2;
            this.Volver.Text = "Volver";
            this.Volver.UseVisualStyleBackColor = true;
            this.Volver.Click += new System.EventHandler(this.Volver_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(120, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Gestion de Respaldo";
            // 
            // FormGestionRespaldo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 349);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Volver);
            this.Controls.Add(this.Restore);
            this.Controls.Add(this.BackUp);
            this.Name = "FormGestionRespaldo";
            this.Text = "FormGestionRespaldo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BackUp;
        private System.Windows.Forms.Button Restore;
        private System.Windows.Forms.Button Volver;
        private System.Windows.Forms.Label label1;
    }
}