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

            Inicio form = new Inicio();

            if (args.Length >= 4)
            {
                form.getVEN_Tipo(args[0]);
                form.getVEN_Numfactura(args[1]);
                form.getVEN_Tienda(args[2]);
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
