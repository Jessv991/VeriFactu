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
            txtTipo = new TextBox();
            txtFactura = new TextBox();
            txtYear = new TextBox();
            txtRenvio = new TextBox();
            txtTienda = new TextBox();
            label5 = new TextBox();
            label4 = new TextBox();
            label3 = new TextBox();
            label2 = new TextBox();
            label1 = new TextBox();
            textBox1 = new TextBox();
            txtJson = new TextBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // txtTipo
            // 
            txtTipo.BackColor = SystemColors.Window;
            txtTipo.Location = new Point(21, 45);
            txtTipo.Name = "txtTipo";
            txtTipo.Size = new Size(108, 23);
            txtTipo.TabIndex = 5;
            // 
            // txtFactura
            // 
            txtFactura.Location = new Point(135, 45);
            txtFactura.Name = "txtFactura";
            txtFactura.Size = new Size(108, 23);
            txtFactura.TabIndex = 2;
            // 
            // txtYear
            // 
            txtYear.Location = new Point(363, 45);
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(108, 23);
            txtYear.TabIndex = 3;
            // 
            // txtRenvio
            // 
            txtRenvio.Location = new Point(477, 45);
            txtRenvio.Name = "txtRenvio";
            txtRenvio.Size = new Size(108, 23);
            txtRenvio.TabIndex = 4;
            // 
            // txtTienda
            // 
            txtTienda.Location = new Point(249, 45);
            txtTienda.Name = "txtTienda";
            txtTienda.Size = new Size(108, 23);
            txtTienda.TabIndex = 5;
            // 
            // label5
            // 
            label5.BackColor = SystemColors.Control;
            label5.BorderStyle = BorderStyle.None;
            label5.Location = new Point(477, 23);
            label5.Name = "label5";
            label5.Size = new Size(108, 16);
            label5.TabIndex = 6;
            label5.Text = "Reenviar";
            // 
            // label4
            // 
            label4.BackColor = SystemColors.Control;
            label4.BorderStyle = BorderStyle.None;
            label4.Location = new Point(363, 23);
            label4.Name = "label4";
            label4.Size = new Size(108, 16);
            label4.TabIndex = 7;
            label4.Text = "Año";
            // 
            // label3
            // 
            label3.BackColor = SystemColors.Control;
            label3.BorderStyle = BorderStyle.None;
            label3.Location = new Point(249, 23);
            label3.Name = "label3";
            label3.Size = new Size(108, 16);
            label3.TabIndex = 8;
            label3.Text = "Codigo de Tienda";
            // 
            // label2
            // 
            label2.BackColor = SystemColors.Control;
            label2.BorderStyle = BorderStyle.None;
            label2.Location = new Point(135, 23);
            label2.Name = "label2";
            label2.Size = new Size(108, 16);
            label2.TabIndex = 9;
            label2.Text = "Codigo de Factura";
            // 
            // label1
            // 
            label1.BackColor = SystemColors.Control;
            label1.BorderStyle = BorderStyle.None;
            label1.Location = new Point(21, 23);
            label1.Name = "label1";
            label1.Size = new Size(108, 16);
            label1.TabIndex = 10;
            label1.Text = "Tipo F FA T TA D S";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.Location = new Point(21, 74);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(0, 23);
            textBox1.TabIndex = 4;
            // 
            // txtJson
            // 
            txtJson.Location = new Point(21, 74);
            txtJson.Multiline = true;
            txtJson.Name = "txtJson";
            txtJson.Size = new Size(630, 123);
            txtJson.TabIndex = 11;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(657, 74);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(141, 124);
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // Inicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 232);
            Controls.Add(pictureBox1);
            Controls.Add(txtJson);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(txtTienda);
            Controls.Add(txtRenvio);
            Controls.Add(txtYear);
            Controls.Add(txtFactura);
            Controls.Add(txtTipo);
            Name = "Inicio";
            Text = "Form1";
            Load += Inicio_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtTipo;
        private TextBox txtFactura;
        private TextBox txtYear;
        private TextBox txtRenvio;
        private TextBox txtTienda;
        private TextBox label5;
        private TextBox label4;
        private TextBox label3;
        private TextBox label2;
        private TextBox label1;
        private TextBox textBox1;
        private TextBox txtJson;
        private PictureBox pictureBox1;
    }
}
