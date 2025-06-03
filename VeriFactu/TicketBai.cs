using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeriFactu
{
    public partial class TicketBai : Form
    {
        public TicketBai()
        {
            InitializeComponent();
            //MessageBox.Show("Es TicketBai");
        }


        public void getVEN_Tipo(string value)
        {
            txtTipo.Text = value;
        }

        public void getVEN_Tienda(string value)
        {
            txtTienda.Text = value;
        }

        public void getVEN_Numfactura(string value)
        {
            txtFactura.Text = value;
        }

        public void getVEN_Year(string value)
        {
            txtYear.Text = value;
        }

        public void getReenvio(string value)
        {
            txtReenvio.Text = value;
        }

        private void TicketBai_Load(object sender, EventArgs e)
        {

        }
    }
}
