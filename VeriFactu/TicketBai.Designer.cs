namespace VeriFactu
{
    partial class TicketBai
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
            label1 = new TextBox();
            label2 = new TextBox();
            label3 = new TextBox();
            label4 = new TextBox();
            label5 = new TextBox();
            txtTipo = new TextBox();
            txtFactura = new TextBox();
            txtTienda = new TextBox();
            txtYear = new TextBox();
            txtReenvio = new TextBox();
            txtJson = new TextBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = SystemColors.Control;
            label1.BorderStyle = BorderStyle.None;
            label1.Location = new Point(24, 31);
            label1.Name = "label1";
            label1.Size = new Size(110, 16);
            label1.TabIndex = 11;
            label1.Text = "Tipo F FA T TA D S";
            // 
            // label2
            // 
            label2.BackColor = SystemColors.Control;
            label2.BorderStyle = BorderStyle.None;
            label2.Location = new Point(140, 31);
            label2.Name = "label2";
            label2.Size = new Size(108, 16);
            label2.TabIndex = 12;
            label2.Text = "Codigo de Factura";
            // 
            // label3
            // 
            label3.BackColor = SystemColors.Control;
            label3.BorderStyle = BorderStyle.None;
            label3.Location = new Point(254, 31);
            label3.Name = "label3";
            label3.Size = new Size(108, 16);
            label3.TabIndex = 13;
            label3.Text = "Codigo de Tienda";
            // 
            // label4
            // 
            label4.BackColor = SystemColors.Control;
            label4.BorderStyle = BorderStyle.None;
            label4.Location = new Point(368, 31);
            label4.Name = "label4";
            label4.Size = new Size(108, 16);
            label4.TabIndex = 14;
            label4.Text = "Año";
            // 
            // label5
            // 
            label5.BackColor = SystemColors.Control;
            label5.BorderStyle = BorderStyle.None;
            label5.Location = new Point(482, 31);
            label5.Name = "label5";
            label5.Size = new Size(108, 16);
            label5.TabIndex = 15;
            label5.Text = "Reenviar";
            // 
            // txtTipo
            // 
            txtTipo.BackColor = SystemColors.Window;
            txtTipo.Location = new Point(24, 53);
            txtTipo.Name = "txtTipo";
            txtTipo.Size = new Size(108, 23);
            txtTipo.TabIndex = 16;
            // 
            // txtFactura
            // 
            txtFactura.Location = new Point(140, 53);
            txtFactura.Name = "txtFactura";
            txtFactura.Size = new Size(108, 23);
            txtFactura.TabIndex = 17;
            // 
            // txtTienda
            // 
            txtTienda.Location = new Point(254, 53);
            txtTienda.Name = "txtTienda";
            txtTienda.Size = new Size(108, 23);
            txtTienda.TabIndex = 18;
            // 
            // txtYear
            // 
            txtYear.Location = new Point(368, 53);
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(108, 23);
            txtYear.TabIndex = 19;
            // 
            // txtReenvio
            // 
            txtReenvio.Location = new Point(482, 53);
            txtReenvio.Name = "txtReenvio";
            txtReenvio.Size = new Size(108, 23);
            txtReenvio.TabIndex = 20;
            txtReenvio.TextChanged += txtReenvio_TextChanged;
            // 
            // txtJson
            // 
            txtJson.Location = new Point(24, 95);
            txtJson.Multiline = true;
            txtJson.Name = "txtJson";
            txtJson.Size = new Size(630, 123);
            txtJson.TabIndex = 21;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(660, 95);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(141, 124);
            pictureBox1.TabIndex = 22;
            pictureBox1.TabStop = false;
            // 
            // TicketBai
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(837, 263);
            Controls.Add(pictureBox1);
            Controls.Add(txtJson);
            Controls.Add(txtReenvio);
            Controls.Add(txtYear);
            Controls.Add(txtTienda);
            Controls.Add(txtFactura);
            Controls.Add(txtTipo);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "TicketBai";
            Text = "TicketBai";
            Load += TicketBai_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox label1;
        private TextBox label2;
        private TextBox label3;
        private TextBox label4;
        private TextBox label5;
        private TextBox txtTipo;
        private TextBox txtFactura;
        private TextBox txtTienda;
        private TextBox txtYear;
        private TextBox txtReenvio;
        private TextBox txtJson;
        private PictureBox pictureBox1;
    }
}