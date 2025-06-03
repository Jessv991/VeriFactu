using System.Data.SqlTypes;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace VeriFactu
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            string tienda = args[1];
            string sql = $"SELECT PAR_Valor FROM \"indo\".\"TIENDADATOS\" WHERE TIE_Codigo = '{tienda}' AND PAR_Key = 'TicketBai_Tipo';";

            string TicketBai_Tipo = Conection.SelectOnce(sql);


            if (TicketBai_Tipo == "VeriFactu") {

                Inicio form = new Inicio();
                if (args.Length >= 4)
                {
                    form.getVEN_Tipo(args[0]);
                    form.getVEN_Tienda(args[1]);
                    form.getVEN_Numfactura(args[2]);
                    form.getVEN_Year(args[3]);

                    if (args.Length >= 5)
                    {
                        form.getReenvio(args[4]);
                    }
                }
                Application.Run(form);
            }
            else if (TicketBai_Tipo == "TicketBai")
            {
                TicketBai form = new TicketBai();

                if (args.Length >= 4)
                {
                    form.getVEN_Tipo(args[0]);
                    form.getVEN_Tienda(args[1]);
                    form.getVEN_Numfactura(args[2]);
                    form.getVEN_Year(args[3]);

                    if (args.Length >= 5)
                    {
                        form.getReenvio(args[4]);
                    }
                }
                Application.Run(form);
            }





        }
    }
}
