namespace VeriFactu
{
    public partial class Inicio : Form
    {
        Conection conection;
        public Inicio()
        {
            InitializeComponent();
            conection = new Conection();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            conection.eso();
            string tipoFactura = txtTipo.Text;
            string respuesta;

            if (tipoFactura == "F")
            {
                respuesta = this.factura(1);
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tipoFactura == "FA")
            {
                respuesta = this.facturaAbono();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tipoFactura == "T")
            {
                respuesta = this.ticket();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tipoFactura == "TA")
            {
                respuesta = this.ticketAbono();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tipoFactura == "D")
            {
                respuesta = this.delete();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tipoFactura == "S")
            {
                respuesta = this.state();
            }
            else
            {
                MessageBox.Show("Error: El tipo de factura es incorrecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        //tipo VatType España es "S1", pero debo averiguar que tipo le toca a cada país
        //tipo de documento IrsIdType se consulta en la BBDD tabla client columna  CLI_EstatCivil

        public string factura(int codigo)
        {

            return "Factura creada correctamente";
        }

        public string facturaAbono()
        {

            return "Factura de abono creada correctamente";
        }

        public string ticket()
        {

            return "Ticket creado correctamente";
        }

        public string ticketAbono()
        {
            return "Ticket abono creado correctamente";
        }

        public string delete()
        {
            return "Eliminado correctamente";
        }

        public string state()
        {
            return "(pendiente de pasar el state)";
        }

    }
}
