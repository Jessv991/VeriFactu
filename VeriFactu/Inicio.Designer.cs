namespace VeriFactu
{
    partial class Inicio
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnEscribir = new Button();
            txtTipo = new TextBox();
            numFactura = new TextBox();
            SuspendLayout();
            // 
            // btnEscribir
            // 
            btnEscribir.Location = new Point(50, 357);
            btnEscribir.Name = "btnEscribir";
            btnEscribir.Size = new Size(124, 61);
            btnEscribir.TabIndex = 0;
            btnEscribir.Text = "button1";
            btnEscribir.UseVisualStyleBackColor = true;
            btnEscribir.Click += btnEscribir_Click;
            // 
            // txtNombre
            // 
            txtTipo.Location = new Point(21, 26);
            txtTipo.Name = "txtNombre";
            txtTipo.Size = new Size(108, 23);
            txtTipo.TabIndex = 1;
            // 
            // textBox1
            // 
            numFactura.Location = new Point(145, 30);
            numFactura.Name = "textBox1";
            numFactura.Size = new Size(100, 23);
            numFactura.TabIndex = 2;
            numFactura.TextChanged += textBox1_TextChanged;
            // 
            // Inicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(numFactura);
            Controls.Add(txtTipo);
            Controls.Add(btnEscribir);
            Name = "Inicio";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnEscribir;
        private TextBox txtTipo;
        private TextBox numFactura;
    }
}
