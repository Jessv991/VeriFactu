using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static VeriFactu.apiVerifactu;
using static VeriFactu.pathServer;

namespace VeriFactu
{
    public partial class VeriFactu : Form
    {
        //Conection conection;
        apiVerifactu.Invoice apiInvoice;
        apiVerifactu.Issuer apiIssuer;
        apiVerifactu.Recipient apiRecipient;
        apiVerifactu.Id apiId;
        apiVerifactu.Description apiDescription;
        apiVerifactu.VatLine apiVatLine;
        apiVerifactu.CreditNote apiCreditNote;
        apiVerifactu.InvoiceId apiInvoiceId;
        apiVerifactu.jsonInvoice apijsonInvoice;
        public VeriFactu()
        {
            InitializeComponent();
            apiInvoice = new apiVerifactu.Invoice();
            apiIssuer = new apiVerifactu.Issuer();
            apiRecipient = new apiVerifactu.Recipient();
            apiId = new apiVerifactu.Id();
            apiDescription = new apiVerifactu.Description();
            apiVatLine = new apiVerifactu.VatLine();
            apiCreditNote = new apiVerifactu.CreditNote();
            apiInvoiceId = new apiVerifactu.InvoiceId();
            apijsonInvoice = new apiVerifactu.jsonInvoice();
        }

        static string respuesta;
        static string sql_maxTbaiQueue = System.String.Empty;
        static string sql_getVentas = System.String.Empty;
        static string sql_getTbaiCodi = System.String.Empty;
        static string sql_getTbaiCodiReenvio = System.String.Empty;
        static string sql_checkTbaiCodi = System.String.Empty;
        static string sql_getListaParametros = System.String.Empty;
        static string sql_getVenFactAbono = System.String.Empty;
        static string sql_getFechaVentaAbono = System.String.Empty;
        static string sql_updateFactura = System.String.Empty;
        static string sql_updateFactura_KO = System.String.Empty;
        static string sql_getVenImportTotal = System.String.Empty;
        static string sql_recargoEquivalenciaCliente = System.String.Empty;
        static string sql_recargoEquivalencia = System.String.Empty;
        static string sql_datosIssureTienda = System.String.Empty;
        static string recargo_equivalencia_empresa = System.String.Empty;
        static string recargo_equivalencia_cliente = System.String.Empty;
        static string TicketBai_DB = System.String.Empty;
        static string TicketBai_ApiKey = System.String.Empty;
        static string ImportTotalString = System.String.Empty;
        static decimal ImporTotalCheck = 0;
        static string serie = System.String.Empty;
        static string descripcion = System.String.Empty;
        static string sqlSiguenteNumFact = System.String.Empty;
        static string filename2 = System.String.Empty;
        static string venCodigo = "";
        static string rectificactiva = "N";
        static string factura_a_cliente = "S";
        static decimal importe_minimo;
        static string[]? subs;
        static string id_vendafact = System.String.Empty;
        static DataTable ven_abono = new DataTable();
        static DataTable vendaTable = new DataTable();
        static DataTable datosIssureTiendaTable = new DataTable();
        static DateTime date_abo = new DateTime();
        static string date_abo_formated = "";
        static string factura_abono_year = System.String.Empty;
        static string factura_abono = System.String.Empty;
        static string filename = System.String.Empty;
        static DateTime date = new DateTime();
        static bool consultAgainTBAI_Codi = false;


        TimeSpan hour = new TimeSpan();
        static string tbai_codifactura = System.String.Empty;
        static string date_formated = System.String.Empty;
        static string nombre_cliente = System.String.Empty;

        //=============ARRAYS DE LA API PARA ARMAR INVOICE
        static ArrayList ids = new ArrayList();
        static apiVerifactu.Invoice InvoiceArray = new apiVerifactu.Invoice();
        static apiVerifactu.Issuer IssuerArray = new apiVerifactu.Issuer();
        static apiVerifactu.Recipient recipientArray = new apiVerifactu.Recipient();
        static apiVerifactu.Id idArray = new apiVerifactu.Id();
        static apiVerifactu.Description descriptionArray = new apiVerifactu.Description();
        static apiVerifactu.VatLine vatLineArray = new apiVerifactu.VatLine();
        static apiVerifactu.InvoiceId InvoiceIdArray = new apiVerifactu.InvoiceId();
        static apiVerifactu.CreditNote CreditNoteArray = new apiVerifactu.CreditNote();

        //=============VARIABLES DE TOTAL NETO PARA CALCULAR SEG�N EL VATLINE

        static Decimal total_neto_21 = 0;
        static Decimal total_neto_10 = 0;
        static Decimal total_neto_4 = 0;
        static Decimal total_neto_0 = 0;
        static Decimal total_neto = 0;
        static Decimal totalAmountWithVat = 0;
        static Decimal precio_linea_sin_iva_con_dto = 0;
        static Decimal precio_linea_con_iva_sin_dto = 0;
        static Decimal precio_linea_con_iva_con_dto = 0;
        static Decimal precio_linea_sin_iva_sin_dto = 0;
        static Decimal precio_unidad_con_iva_sin_dto = 0;
        static Decimal precio_unidad_con_iva_con_dto = 0;
        static Decimal precio_unidad_sin_iva_sin_dto = 0;
        static Decimal vat_calc = 0;

        //=============VARIABLES PARA COMPARAR Y VERIFICAR IVA
        static int LVN_Uds;
        static Decimal LVN_IVA = 0;


        static string path_server = pathServer.getPathServer();


        private void Inicio_Load(object sender, EventArgs e)
        {

            //string mensaje = $"Tipo: {txtTipo.Text}\nFactura: {txtFactura.Text}\nTienda: {txtTienda.Text}\nAño: {txtYear.Text}";
            //MessageBox.Show(mensaje, "Datos de Factura");

            date_formated = date.ToString("yyyy-MM-dd");

            sql_getListaParametros = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_Key = 'TicketBai_DB'";
            TicketBai_DB = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_DB'
            if (string.IsNullOrEmpty(TicketBai_DB))
            {
                sql_getListaParametros = "SELECT PAR_Valor FROM LISTA_PARAMETROS WHERE PAR_Key = 'TicketBai_DB'";
                TicketBai_DB = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_DB'
                sql_getListaParametros = "SELECT PAR_Valor FROM LISTA_PARAMETROS WHERE PAR_Key = 'TicketBai_ApiKey'";
                TicketBai_ApiKey = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_ApiKey'
            }
            else
            {
                sql_getListaParametros = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_Key = 'TicketBai_ApiKey'";
                TicketBai_ApiKey = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_ApiKey'
            }

            sql_recargoEquivalencia = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_key = 'RecargodeEquivalencia'";
            recargo_equivalencia_empresa = Conection.SelectOnce(sql_recargoEquivalencia); //consulta para optiner 'recargo_equivalencia_empresa'

            if (recargo_equivalencia_empresa == null || recargo_equivalencia_empresa == "")
            {
                sql_recargoEquivalencia = "SELECT PAR_Valor FROM LISTA_PARAMETROS WHERE PAR_key = 'RecargodeEquivalencia'";
                recargo_equivalencia_empresa = Conection.SelectOnce(sql_recargoEquivalencia); //consulta para optiner 'recargo_equivalencia_empresa'
            }

            //=====================CONSULTA PARA OBTENER ISSURE=======================================

            sql_datosIssureTienda = "SELECT (SELECT PAR_Valor FROM TIENDADATOS WHERE PAR_Key = 'NIF' AND TIE_Codigo = " + txtTienda.Text + ") AS NIF, (SELECT PAR_Valor FROM indo.TIENDADATOS WHERE PAR_Key = 'RegMercantil' AND TIE_Codigo = " + txtTienda.Text + ") AS RegMercantil;";

            //=========================importe minimo para pasar datos del cliente=========================

            string sql_getImporteMinimo = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_Key = 'TicketBai_ImporteMinimo'";
            importe_minimo = Convert.ToDecimal(Conection.SelectOnce(sql_getImporteMinimo));

            //=========================CONDICI�N SEG�N EL TIPO DE FACTURA=========================

            if (txtTipo.Text == "F")
            {
                this.factura();
            }
            else if (txtTipo.Text == "FA")
            {
                this.facturaAbono();
            }
            else if (txtTipo.Text == "T")
            {
                this.ticket();
            }
            else if (txtTipo.Text == "TA")
            {
                this.ticketAbono();
            }
            else if (txtTipo.Text[0] == 'D' || txtTipo.Text[0] == 'S')
            {
                this.deleteOrState();
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                MessageBox.Show("Error: El tipo de factura o ticket es incorrecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        //tipo VatType Espa�a es "S1", pero debo averiguar que tipo le toca a cada pa�s
        //tipo de documento IrsIdType se consulta en la BBDD tabla client columna  CLI_EstatCivil

        public void sameSQLFacturas()
        {
            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "'";

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_UserId=?, TBAI_Estado=?, TBAI_mensaje = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_ImgQr = ? WHERE " +
                                        "VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = '" + txtTipo.Text +
                                        "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "'";

            sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE " +
                                        "VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = '" + txtTipo.Text +
                                        "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "'";

        }

        public void sameSQLTicket()
        {
            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_Ticket = '" + txtFactura.Text + "'";//esto es para DF (no va aqu�)

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND VEN_Ticket = '" + txtFactura.Text + "' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_UserId=?, TBAI_Estado=?, TBAI_mensaje = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_ImgQr = ? WHERE " +
                                        "VEN_Ticket = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = '" + txtTipo.Text +
                                        "' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "'";

            sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE " +
                                        "VEN_Ticket = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = '" + txtTipo.Text +
                                        "' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "'";

        }

        public void factura()
        {
            this.sameSQLFacturas();

            serie = "F";

            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                        "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                        "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                        "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                        "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                        "AND v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                        "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                                        "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) " +
                                        "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Codi,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";


            sql_recargoEquivalenciaCliente = "SELECT CLI_RecEquivalencia " +
                                        "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact WHERE " +
                                        "v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                        "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                                        "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) ";
            descripcion = "Factura ";

            sql_getVenImportTotal = "SELECT SUM(v.VEN_ImportTotal) FROM VENDA v WHERE v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                          "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                          "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                          "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) ";

            //================DATOS DEL CLIENTE================

            string checkIfExist = Conection.SelectOnce(sql_checkTbaiCodi);

            if (string.IsNullOrEmpty(checkIfExist) || txtReenvio.Text == "R")
            {

                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");



                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);

                vendaTable = Conection.SelectRows(sql_getVentas);

                if (vendaTable.Rows[0].Field<string>("Nom").ToString() != null)
                {
                    nombre_cliente = vendaTable.Rows[0].Field<string>("Nom").ToString();
                }
                else
                {
                    MessageBox.Show("Error: Falta el Nombre del Cliente");
                    System.Windows.Forms.Application.Exit();
                }

                if ((ImporTotalCheck > importe_minimo))
                {
                    this.datosCliente();
                }
                else
                {
                    InvoiceArray.type = "F2";
                    InvoiceArray.simple = true;
                }

                this.buildJson();
                //MessageBox.Show("Factura creada correctamente");
                System.Windows.Forms.Application.Exit();
            }
            else
            {

                //MessageBox.Show("No se puede crear factura duplicada");
                System.Windows.Forms.Application.Exit();
            }



        }

        public void facturaAbono()
        {
            this.sameSQLFacturas();
            serie = "FA";
            rectificactiva = "S";//ojo! puede que no lo necesites (seguro)
            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                       "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                       "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                       "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                       "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                       "AND v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                                       "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                       "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                                       "AND v.VEN_Referencia = 'ABONO' " +
                                       "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Codi,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";

            sql_getVenFactAbono = "SELECT VEN_ImportBrut FROM VENDA WHERE VEN_NumFactura = '" + txtFactura.Text + "' " + "AND VEN_Tienda = '" + txtTienda.Text + "' " + "AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' " + "AND VEN_Referencia = 'ABONO'";

            id_vendafact = Conection.SelectOnce(sql_getVenFactAbono); //consulta para traer el valor de 'sql_getVenFactAbono'

            subs = id_vendafact.Split(',');

            sql_getFechaVentaAbono = "SELECT TBAI_Codi, V.VEN_DataFactura, V.VEN_NumFactura FROM VENDA V " +
                                       "INNER JOIN FACTURAS_HACIENDA_AUTO FHA ON (FHA.VEN_NumFactura = V.VEN_NumFactura AND V.VEN_DataFactura = FHA.VEN_DataFactura) WHERE " +
                                       "V.VEN_Codi = '" + subs[0] + "' " +
                                       "AND V.VEN_Tienda = '" + subs[1].Substring(0, 2) + "'";
            ven_abono = Conection.SelectRows(sql_getFechaVentaAbono);

            if (ven_abono == null || ven_abono.Rows.Count == 0)
            {
                MessageBox.Show("No puede facturar un Abono de una Venta no facturada (" + subs[0] + "/" + subs[1].Substring(0, 2) + ")");
                System.Windows.Forms.Application.Exit();
            }

            date_abo = ven_abono.Rows[0].Field<DateTime>("VEN_DataFactura");
            date_abo_formated = date_abo.ToString("yyyy-MM-dd");
            factura_abono_year = date_abo.ToString("yyyy");
            factura_abono = ven_abono.Rows[0].Field<Int32>("TBAI_Codi").ToString();



            sql_recargoEquivalenciaCliente = "SELECT CLI_RecEquivalencia " +
                    "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact WHERE " +
                    "v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                    "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                    "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                    "AND v.VEN_Referencia = 'ABONO'";

            descripcion = "Factura Abono ";

            sql_getVenImportTotal = "SELECT SUM(v.VEN_ImportTotal) FROM VENDA v WHERE v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                                             "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                             "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                                             "AND v.VEN_Referencia = 'ABONO'";

            string checkIfExist = Conection.SelectOnce(sql_checkTbaiCodi);

            if (string.IsNullOrEmpty(checkIfExist) || txtReenvio.Text == "R")
            {

                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");



                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);

                vendaTable = Conection.SelectRows(sql_getVentas);

                if (vendaTable.Rows[0].Field<string>("Nom").ToString() != null)
                {
                    nombre_cliente = vendaTable.Rows[0].Field<string>("Nom").ToString();
                }
                else
                {
                    MessageBox.Show("Error: Falta el Nombre del Cliente");
                    System.Windows.Forms.Application.Exit();
                }

                //================DATOS DEL CLIENTE================
                if ((ImporTotalCheck > importe_minimo))
                {
                    InvoiceArray.type = "R1";
                    this.buildCreditNote();
                    this.datosCliente();
                }
                else
                {
                    InvoiceArray.type = "R5";
                    InvoiceArray.simple = true;
                    this.buildCreditNote();
                }

                this.buildJson();
                //MessageBox.Show("Factura Abono creada correctamente");
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                //MessageBox.Show("No se puede crear factura Abono duplicada");
                System.Windows.Forms.Application.Exit();
            }
        }

        public void ticket()
        {
            this.sameSQLTicket();

            serie = "T";
            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                      "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                      "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                      "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                      "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                      "AND v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                      "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                      "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                      "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) " +
                                      "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";
            factura_a_cliente = "N";

            sql_recargoEquivalenciaCliente = "SELECT CLI_RecEquivalencia " +
                                       "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact WHERE " +
                                       "v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                       "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                       "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                       "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) ";

            descripcion = "Ticket ";

            sql_getVenImportTotal = "SELECT SUM(v.VEN_ImportTotal) FROM VENDA v WHERE v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                            "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                            "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                            "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) ";

            string checkIfExist = Conection.SelectOnce(sql_checkTbaiCodi);

            if (string.IsNullOrEmpty(checkIfExist) || txtReenvio.Text == "R")
            {
                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");
                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);

                vendaTable = Conection.SelectRows(sql_getVentas);


                InvoiceArray.type = "F2";
                InvoiceArray.simple = true;

                this.buildJson();
                //MessageBox.Show("Ticket creado correctamente");
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                //MessageBox.Show("No se puede crear Ticket duplicado");
                System.Windows.Forms.Application.Exit();
            }
        }

        public void ticketAbono()
        {
            this.sameSQLTicket();
            serie = "TA";
            rectificactiva = "S";
            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                      "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' N� ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                      "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                      "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                      "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                      "AND v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                      "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                      "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                      "AND v.VEN_Referencia = 'ABONO' " +
                                      "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_DefAuditiu, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";

            factura_a_cliente = "N";

            sql_recargoEquivalenciaCliente = "SELECT CLI_RecEquivalencia " +
                                        "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact WHERE " +
                                        "v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                        "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                        "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                        "AND v.VEN_Referencia = 'ABONO'";

            descripcion = "Ticket Abono ";

            sql_getVenImportTotal = "SELECT SUM(v.VEN_ImportTotal) FROM VENDA v WHERE v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                             "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                             "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                              "AND v.VEN_Referencia = 'ABONO'";

            sql_getVenFactAbono = "SELECT VEN_ImportBrut FROM VENDA WHERE VEN_Ticket = '" + txtFactura.Text + "' " +
                                       "AND VEN_Tienda = '" + txtTienda.Text + "' " +
                                       "AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' " +
                                       "AND VEN_Referencia = 'ABONO'";


            string checkIfExist = Conection.SelectOnce(sql_checkTbaiCodi);

            if (string.IsNullOrEmpty(checkIfExist) || txtReenvio.Text == "R")
            {
                id_vendafact = Conection.SelectOnce(sql_getVenFactAbono);

                subs = id_vendafact.Split(',');

                sql_getFechaVentaAbono = "SELECT TBAI_Codi, V.VEN_DataTicket, V.VEN_Ticket FROM VENDA V " +
                                   "INNER JOIN FACTURAS_HACIENDA_AUTO FHA ON (FHA.VEN_Ticket = V.VEN_Ticket AND V.VEN_DataTicket = FHA.VEN_DataTicket) WHERE " +
                                   "V.VEN_Codi = '" + subs[0] + "' " +
                                   "AND V.VEN_Tienda = '" + subs[1].Substring(0, 2) + "'";

                ven_abono = Conection.SelectRows(sql_getFechaVentaAbono);

                if (ven_abono == null || ven_abono.Rows.Count == 0)
                {
                    MessageBox.Show("No puede facturar un Abono de un Ticket no facturado (" + subs[0] + "/" + subs[1].Substring(0, 2) + ")");
                    System.Windows.Forms.Application.Exit();
                }

                date_abo = ven_abono.Rows[0].Field<DateTime>("VEN_DataTicket");

                date_abo_formated = date_abo.ToString("yyyy-MM-dd");

                factura_abono_year = date_abo.ToString("yyyy");

                factura_abono = ven_abono.Rows[0].Field<Int32>("TBAI_Codi").ToString();



                vendaTable = Conection.SelectRows(sql_getVentas);

                if (vendaTable.Rows[0].Field<string>("Nom").ToString() != null)
                {
                    nombre_cliente = vendaTable.Rows[0].Field<string>("Nom").ToString();
                }
                else
                {
                    MessageBox.Show("Error: Falta el Nombre del Cliente");
                    System.Windows.Forms.Application.Exit();
                }

                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");

                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);
                InvoiceArray.type = "R5";
                InvoiceArray.simple = true;
                this.buildCreditNote();
                this.buildJson();
                //MessageBox.Show("Ticket Abono creado correctamente");
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                //MessageBox.Show("No se puede crear Ticket Abono duplicado");
                System.Windows.Forms.Application.Exit();
            }

        }

        //Funci�n para generar los datos del cliente si el valor total de la factura es mayor al minimo que viene de la base de dato

        public void datosCliente()
        {
            //=====================RECIPIENT=====================

            //=====================numero de TIPO DE DOCUMENTO (irsId)

            string tipo = "";

            /*if (vendaTable.Rows[0].Field<string>("TipoDoc").ToString() != null)
            {
                tipo = vendaTable.Rows[0].Field<string>("TipoDoc");

                if (tipo == "02")
                {
                    recipientArray.idType = null;
                }
                else
                {
                    recipientArray.idType = tipo;
                }
            }
            else
            {
                MessageBox.Show("Error: Falta el Tipo Documento del Cliente");
                System.Windows.Forms.Application.Exit();
            }*/

            //=====================numero de DNI (irsId)

            if (vendaTable.Rows[0].Field<string>("CLI_Dni") != null)
            {
                string dni = vendaTable.Rows[0].Field<string>("CLI_Dni").ToString();
                dni = dni.Replace("-", "");
                dni = dni.Replace("_", "");
                dni = dni.Replace(" ", "");
                char[] charsToTrim = { '*', ' ', '\'' };
                dni = dni.Trim(charsToTrim);

                if (vendaTable.Rows[0].Field<string>("TipoDoc").ToString() != null)
                {
                    tipo = vendaTable.Rows[0].Field<string>("TipoDoc");

                    if (tipo == "02")
                    {
                        recipientArray.irsId = dni;
                        recipientArray.idType = null;
                    }
                    else
                    {
                        recipientArray.id = dni;
                        recipientArray.idType = tipo;
                    }
                }
                else
                {
                    MessageBox.Show("Error: Falta el Tipo Documento del Cliente");
                    System.Windows.Forms.Application.Exit();
                }

            }
            else
            {
                MessageBox.Show("Error: Falta el DNI del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            //=====================numero de NOMBRE (name)

            recipientArray.name = nombre_cliente;

            //=====================numero de CIUDAD (country)


            if (vendaTable.Rows[0].Field<string>("Pais") != null)
            {
                recipientArray.country = vendaTable.Rows[0].Field<string>("Pais").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el Pais del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            //=====================INVOICE=====================
            //=====================type de (type)

            InvoiceArray.type = "F1";

            //=====================simple de (simple)

            InvoiceArray.simple = false;

            InvoiceArray.recipient = recipientArray;

        }

        public void buildJson()
        {
            //=====================ISSURE=====================

            datosIssureTiendaTable = Conection.SelectRows(sql_datosIssureTienda);

            if (datosIssureTiendaTable.Rows[0].Field<string>("RegMercantil").ToString() != null)
            {
                IssuerArray.name = datosIssureTiendaTable.Rows[0].Field<string>("RegMercantil").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el nombre del Registro Mercantil del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            if (datosIssureTiendaTable.Rows[0].Field<string>("NIF").ToString() != null)
            {
                IssuerArray.irsId = datosIssureTiendaTable.Rows[0].Field<string>("NIF").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el NIF de la Optica del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            InvoiceArray.issuer = IssuerArray; //AGREGAMOS ISSURE A INVOICE

            //=====================ID=====================

            //=====================numbre (numero de factura)

            //tbai_codifactura = "16";
            tbai_codifactura = Conection.SelectOnce(sql_getTbaiCodi);

            //MessageBox.Show(tbai_codifactura);

            if (txtReenvio.Text == "R")
            {
                tbai_codifactura = Conection.SelectOnce(sql_getTbaiCodiReenvio);
            }
            else
            {
                if ((tbai_codifactura is null) || (tbai_codifactura == ""))
                {
                    tbai_codifactura = "1";
                }
            }

            venCodigo = vendaTable.Rows[0].Field<int>("VEN_Codi").ToString();

            idArray.number = serie + "/" + tbai_codifactura + "/" + txtTienda.Text + "/" + txtYear.Text;

            //=====================issuedTime (issuedTime de factura)
            if (txtTipo.Text == "F" || txtTipo.Text == "FA")
            {
                date = vendaTable.Rows[0].Field<DateTime>("VEN_DataFactura");
            }
            else if (txtTipo.Text == "T" || txtTipo.Text == "TA")
            {
                date = vendaTable.Rows[0].Field<DateTime>("VEN_DataTicket");
            }

            idArray.issuedTime = date.ToString("yyyy-MM-dd");

            InvoiceArray.id = idArray;//CARGAMOS ID A INVOICE

            //=====================DESCRIPTION (description de factura)
            if (vendaTable.Rows[0].Field<string>("Nom").ToString() != null)
            {
                nombre_cliente = vendaTable.Rows[0].Field<string>("Nom").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el Nombre del Cliente");
                System.Windows.Forms.Application.Exit();
            }
            descriptionArray.text = descripcion + serie + "/" + tbai_codifactura + "/" + txtYear.Text + " EMITIDA A " + nombre_cliente + " DE LA VENTA " + txtTienda.Text + "/" + venCodigo;
            descriptionArray.operationDate = date.ToString("yyyy-MM-dd");

            InvoiceArray.description = descriptionArray;//AGREGAMOS DESCRIPTION AL MODELO JSON

            //=====================VATLINE (VatLine de factura)

            //vatLineArray.@base = 0;

            foreach (DataRow row in vendaTable.Rows)
            {
                LVN_Uds = Math.Abs(row.Field<Int16>("LVN_Uds"));
                LVN_IVA = Convert.ToInt16(row.Field<Decimal>("LVN_IVA"));

                vat_calc = (1 + (Decimal.Divide(LVN_IVA, 100)));

                precio_linea_con_iva_sin_dto = LVN_Uds * row.Field<Decimal>("LVN_PreuUnitat");
                precio_linea_con_iva_con_dto = row.Field<Decimal>("LVN_PreuDtoLineaVenta");
                precio_linea_sin_iva_sin_dto = Math.Round(((LVN_Uds * row.Field<Decimal>("LVN_PreuUnitat")) / vat_calc), 4);
                precio_linea_sin_iva_con_dto = Math.Round((row.Field<Decimal>("LVN_PreuDtoLineaVenta") / vat_calc), 4);
                precio_unidad_con_iva_sin_dto = row.Field<Decimal>("LVN_PreuUnitat");
                precio_unidad_con_iva_con_dto = row.Field<Decimal>("LVN_PreuDtoLineaVenta") / LVN_Uds;
                precio_unidad_sin_iva_sin_dto = Math.Round((row.Field<Decimal>("LVN_PreuUnitat") / vat_calc), 4);
                total_neto = total_neto + precio_linea_con_iva_con_dto;


                switch (LVN_IVA)
                {
                    case 21:
                        total_neto_21 += precio_linea_sin_iva_con_dto;
                        break;
                    case 10:
                        total_neto_10 += precio_linea_sin_iva_con_dto;
                        break;
                    case 4:
                        total_neto_4 += precio_linea_sin_iva_con_dto;
                        break;
                    case 0:
                        total_neto_0 += precio_linea_sin_iva_con_dto;
                        break;
                }
            }

            Decimal total = 0;
            Decimal totalAmount = 0;

            InvoiceArray.vatLines = new List<VatLine>();

            if ((total_neto_21 > 0) || (total_neto_21 < 0))
            {
                var vatLine21 = new VatLine
                {

                    @base = Math.Round(total_neto_21, 2),
                    rate = 21,
                    amount = Math.Round(total_neto_21 * Convert.ToDecimal(0.21), 2),
                    vatOperation = "S1",


                };

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine21.isUsingSimplifiedRegime = true;
                }

                total += vatLine21.@base + vatLine21.amount;
                totalAmount += vatLine21.amount;

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine21.vatKey = "18";
                }
                else
                {
                    vatLine21.vatKey = "01";
                }

                InvoiceArray.vatLines.Add(vatLine21);
            }

            if ((total_neto_10 > 0) || (total_neto_10 < 0))
            {
                var vatLine10 = new VatLine
                {
                    @base = Math.Round(total_neto_10, 2),
                    rate = 10,
                    amount = Math.Round(total_neto_10 * Convert.ToDecimal(0.10), 2),
                    vatOperation = "S1",

                };

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine10.isUsingSimplifiedRegime = true;
                }

                total += vatLine10.@base + vatLine10.amount;
                totalAmount += vatLine10.amount;

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine10.vatKey = "18";
                }
                else
                {
                    vatLine10.vatKey = "01";
                }

                InvoiceArray.vatLines.Add(vatLine10);
            }

            if ((total_neto_4 > 0) || (total_neto_4 < 0))
            {
                var vatLine4 = new VatLine
                {
                    @base = Math.Round(total_neto_4, 2),
                    rate = 4,
                    amount = Math.Round(total_neto_4 * Convert.ToDecimal(0.04), 2),
                    vatOperation = "S1",

                };
                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine4.isUsingSimplifiedRegime = true;
                }

                total += vatLine4.@base + vatLine4.amount;
                totalAmount += vatLine4.amount;

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine4.vatKey = "18";
                }
                else
                {
                    vatLine4.vatKey = "01";
                }

                InvoiceArray.vatLines.Add(vatLine4);
            }

            if ((total_neto_0 > 0) || (total_neto_0 < 0))
            {
                var vatLine0 = new VatLine
                {
                    amount = 0,
                    @base = Math.Round(total_neto_0, 2),
                    rate = 0,
                    vatOperation = "E1",
                };
                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine0.isUsingSimplifiedRegime = true;
                }

                total += vatLine0.@base + vatLine0.amount;
                totalAmount += vatLine0.amount;

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine0.vatKey = "18";
                }
                else
                {
                    vatLine0.vatKey = "01";
                }

                InvoiceArray.vatLines.Add(vatLine0);
            }

            //MessageBox.Show(total.ToString(), totalAmount.ToString());



            InvoiceArray.total = total;
            InvoiceArray.amount = totalAmount;


            //=============================BUILD JSON============================


            apijsonInvoice.invoice = InvoiceArray;

            this.apiConnectioSendJson();

        }

        public void apiConnectioSendJson()
        {
            if (consultAgainTBAI_Codi == true)
            {
                tbai_codifactura = Conection.SelectOnce(sql_getTbaiCodi);
                idArray.number = serie + "/" + tbai_codifactura + "/" + txtTienda.Text + "/" + txtYear.Text;
                apijsonInvoice.invoice = InvoiceArray;
            }

            //=====================URL JSON===================

            var client = new RestClient(new RestClientOptions(TicketBai_DB.Trim())
            {
                MaxTimeout = -1 // -1 significa sin timeout
            });

            var request = new RestRequest("verifactu/v1/invoice", Method.Post);


            string JSONresult = JsonConvert.SerializeObject(apijsonInvoice, Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });

            txtJson.Text = JSONresult;

            WriteLog(txtJson.Text, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);

            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("User-Agent", "RestSharpClient/1.0");
            request.AddHeader("API-KEY", TicketBai_ApiKey.Trim());
            request.AddStringBody(JSONresult, DataFormat.Json); // JSONresult es tu string JSON

            RestResponse response = client.Execute(request);

            //Uri fullUri = client.BuildUri(request);
            //MessageBox.Show(fullUri.ToString());

            txtJson.Text = $"Status: {response.StatusCode}\n\n{response.Content}";

            apiVerifactu.apiResponse new_factura = JsonConvert.DeserializeObject<apiVerifactu.apiResponse>(response.Content);

            if ((new_factura.message == "") || (new_factura.message is null))
            {
                string rawFecha = new_factura.chainInfo.issuedTime; // esto es un string ISO 8601
                DateTime fecha = DateTime.Parse(rawFecha);
                string issuedTime = fecha.ToString("yyyy-MM-dd");

                filename = createImageQr(new_factura.qrcode, tbai_codifactura, txtTienda.Text);
                pictureBox1.Image = Image.FromFile(filename);
                string sPathToFileToSave = filename;
                using (System.IO.FileStream fs = new System.IO.FileStream(sPathToFileToSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();

                    // insert this file in the database
                    using (System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand())
                    {
                        //TBAI_Codi = ?, TBAI_Id = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_UserId=?, TBAI_Estado=?, TBAI_mensaje = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_ImgQr = ?
                        cmd.CommandText = sql_updateFactura;
                        System.Data.Odbc.OdbcParameter paramTBAI_Codi = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Codi.OdbcType = System.Data.Odbc.OdbcType.Int;
                        paramTBAI_Codi.Value = tbai_codifactura;
                        cmd.Parameters.Add(paramTBAI_Codi);
                        System.Data.Odbc.OdbcParameter paramTBAI_Id = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Id.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Id.Value = new_factura.chainInfo.number;
                        cmd.Parameters.Add(paramTBAI_Id);
                        System.Data.Odbc.OdbcParameter paramTBAI_Queue = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Queue.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Queue.Value = new_factura.queueId;
                        cmd.Parameters.Add(paramTBAI_Queue);
                        System.Data.Odbc.OdbcParameter paramTBAI_Xml = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Xml.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Xml.Value = new_factura.verifactuXml;//====HECHO
                        cmd.Parameters.Add(paramTBAI_Xml);
                        System.Data.Odbc.OdbcParameter paramTBAI_Data = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Data.OdbcType = System.Data.Odbc.OdbcType.Date;
                        paramTBAI_Data.Value = issuedTime;
                        cmd.Parameters.Add(paramTBAI_Data);
                        System.Data.Odbc.OdbcParameter paramTBAI_Hash = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Hash.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Hash.Value = new_factura.chainInfo.hash;
                        cmd.Parameters.Add(paramTBAI_Hash);
                        System.Data.Odbc.OdbcParameter paramTBAI_UserId = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_UserId.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_UserId.Value = new_factura.chainInfo.issuerIrsId;
                        cmd.Parameters.Add(paramTBAI_UserId);
                        System.Data.Odbc.OdbcParameter paramTBAI_Estado = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Estado.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Estado.Value = "En cola";
                        cmd.Parameters.Add(paramTBAI_Estado);
                        System.Data.Odbc.OdbcParameter paramTBAI_mensaje = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_mensaje.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_mensaje.Value = "Factura insertada correctamente";
                        cmd.Parameters.Add(paramTBAI_mensaje);
                        System.Data.Odbc.OdbcParameter paramTBAI_Qr = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Qr.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Qr.Value = new_factura.qrcode;
                        cmd.Parameters.Add(paramTBAI_Qr);
                        System.Data.Odbc.OdbcParameter paramTBAI_Url = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Url.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Url.Value = new_factura.verifactuUrl;
                        cmd.Parameters.Add(paramTBAI_Url);
                        System.Data.Odbc.OdbcParameter paramTBAI_ImgQr = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_ImgQr.OdbcType = System.Data.Odbc.OdbcType.Image;
                        paramTBAI_ImgQr.Value = fileData;
                        cmd.Parameters.Add(paramTBAI_ImgQr);

                        Conection.ExecuteCommand(cmd);
                    }
                }

            }
            else
            {
                using (System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand())
                {
                    if (new_factura.message == "The invoice is duplicated")
                    {

                        this.consultDuplicate();


                    }
                    else
                    { 
                        WriteLog("Error: " + new_factura.message, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);
                        cmd.CommandText = sql_updateFactura_KO;
                        System.Data.Odbc.OdbcParameter paramTBAI_Mensaje = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Mensaje.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Mensaje.Value = new_factura.message;
                        cmd.Parameters.Add(paramTBAI_Mensaje);
                        Conection.ExecuteCommand(cmd);
                        MessageBox.Show("Error: " + new_factura.message);
                    }
                }
            }
            System.Windows.Forms.Application.Exit();
        }

        public void consultDuplicate()
        {
            //=======================DECLARACIÓN DE VARIABLES========================
            string nombreCli = "";
            string TipoFact = "";
            string TipoFactExpresion = "";
            string yearFactExpresion = "";
            string yearFact = "";
            string textTipoFact = "";
            string siguenteNumFact = "";
            int tienda = 0;
            int ventaNumero = 0;
            int numFacturaOld = 0;
            int VEN_CodiFactSent = 0;
            int.TryParse(venCodigo, out VEN_CodiFactSent);

            //=======================INICIO DE LA LOGICA========================

            sql_maxTbaiQueue = @"SELECT 
                        CAST(ISNULL((
                            SELECT MIN(f1.TBAI_Queue) + 1
                            FROM FACTURAS_HACIENDA_AUTO AS f1
                            WHERE 
                                (YEAR(f1.ven_dataFactura) = '" + txtYear.Text + "' OR YEAR(f1.ven_dataticket) = '" + txtYear.Text + @"')
                                AND NOT EXISTS (
                                    SELECT 1
                                    FROM FACTURAS_HACIENDA_AUTO AS f2
                                    WHERE f2.TBAI_Queue = f1.TBAI_Queue + 1
                                    AND (YEAR(f2.ven_dataFactura) = '" + txtYear.Text + "' OR YEAR(f2.ven_dataticket) = '" + txtYear.Text + @"')
                                )
                        ), 
                        (SELECT MAX(TBAI_Queue) + 1 
                         FROM FACTURAS_HACIENDA_AUTO
                         WHERE YEAR(ven_dataFactura) = '" + txtYear.Text + "' OR YEAR(ven_dataticket) = '" + txtYear.Text + @"')
                        ) AS INT) AS NextQueueValue";

            string TBAI_Queue = Conection.SelectOnce(sql_maxTbaiQueue);

            var client2 = new RestClient(new RestClientOptions(TicketBai_DB.Trim())
            {
                MaxTimeout = -1
            });

            var request2 = new RestRequest("verifactu/v1/invoice_data/" + TBAI_Queue, Method.Get);



            WriteLog(txtJson.Text, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);

            request2.AddHeader("Content-Type", "application/json; charset=utf-8");
            request2.AddHeader("Accept", "application/json");
            request2.AddHeader("User-Agent", "RestSharpClient/1.0");
            request2.AddHeader("API-KEY", TicketBai_ApiKey.Trim());

            RestResponse respuestaApi = client2.Execute(request2);

            txtJson.Text = $"Status: {respuestaApi.StatusCode}\n\n{respuestaApi.Content}";

            apiVerifactu.apiResponse old_factura = JsonConvert.DeserializeObject<apiVerifactu.apiResponse>(respuestaApi.Content);

            // Convertir a XML
            string base64Xml = old_factura.verifactuXml;
            byte[] xmlBytes = Convert.FromBase64String(base64Xml);
            string xmlString = Encoding.UTF8.GetString(xmlBytes);

            // Convertir a JSON
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            string jsonFromXml = JsonConvert.SerializeXmlNode(xmlDoc);

            // Convertir el JSON a JObject
            JObject jsonObject = JObject.Parse(jsonFromXml);

            //=====================FECHA EN LA QUE FUE FACTURADA LA FACTURA=====================
            var dataFacturaOld = old_factura.chainInfo.issuedTime;
            DateTime fechaOldFact = DateTime.Parse(dataFacturaOld);
            string dataFactura = fechaOldFact.ToString("yyyy-MM-dd");

            //=====================OBTENER EL TBAI_Codi DE LA RESPUESTA QUE NOS ARROJA LA API=====================
            var chainInfoNumber = old_factura.chainInfo.number;
            var matchTBAI_CodiOld = Regex.Match(chainInfoNumber, @"\b(F(A)?|T(A)?)\/(\d+)\/", RegexOptions.IgnoreCase);
            var matchTienda = Regex.Match(chainInfoNumber, @"\b(F(A)?|T(A)?)\/\d+/(\d+)\/", RegexOptions.IgnoreCase);
            var matchTipoFact = Regex.Match(chainInfoNumber, @"\b(F(A)?|T(A)?)\/", RegexOptions.IgnoreCase);
            var matchYearFact = Regex.Match(chainInfoNumber, @"\b(?:F|FA|T|TA)/\d+/\d+/(\d{4})", RegexOptions.IgnoreCase);


            int TBAI_CodiOld = 0;//ESTO HA CAMBIADO
            if (matchTBAI_CodiOld.Success && matchTienda.Success && matchTipoFact.Success && matchYearFact.Success)
            {
                int.TryParse(matchTBAI_CodiOld.Groups[4].Value, out TBAI_CodiOld);
                int.TryParse(matchTienda.Groups[4].Value, out tienda);
                TipoFact = matchTipoFact.Groups[1].Value.Trim();
                yearFact = matchYearFact.Groups[1].Value.Trim();
            }

            if (!string.IsNullOrEmpty(old_factura.qrcode))
            {
                //=====================OBTENER NOMBRE Y TIENDA DEL CLIENTE DE LA DESCRIPCIÓN=====================
                var datosFactura = jsonObject["sum:RegistroFactura"]?["RegistroAlta"]?["DescripcionOperacion"]?.ToString();
                System.Data.Odbc.OdbcCommand cmd2;

                // Extraer nombre después de "EMITIDA A"
                Match matchNombre = Regex.Match(datosFactura, @"EMITIDA A\s+(.+?)\s+DE LA VENTA");
                // Extraer número final de venta después de "DE LA VENTA"
                Match matchVenta = Regex.Match(datosFactura, @"DE LA VENTA\s+[\d]+\/(\d+)");


                if (matchNombre.Success && matchVenta.Success)
                {
                    nombreCli = matchNombre.Groups[1].Value.Trim();
                    int.TryParse(matchVenta.Groups[1].Value, out ventaNumero);
                }

                if (TipoFact == "F")
                {
                    textTipoFact = "la Factura";
                    sqlSiguenteNumFact = "SELECT TOP 1 VEN_NumFactura + 1 AS nextFactura FROM VENDA WHERE VEN_Tienda = " + tienda + " AND YEAR(VEN_Data) = '" + yearFact + "' AND (VEN_Referencia IS NULL OR VEN_Referencia = '') ORDER BY VEN_NumFactura DESC";
                }
                else if (TipoFact == "FA")
                {
                    textTipoFact = "la Factura Abono";
                    sqlSiguenteNumFact = "SELECT TOP 1 VEN_NumFactura + 1 AS nextFacturaAbono FROM VENDA WHERE VEN_Tienda = " + tienda + " AND YEAR(VEN_Data) = '" + yearFact + "' AND VEN_Referencia = 'ABONO' ORDER BY VEN_NumFactura DESC";
                }
                else if (TipoFact == "T")
                {
                    textTipoFact = "el Ticket";
                    sqlSiguenteNumFact = "SELECT TOP 1 VEN_Ticket + 1 AS nextTicket FROM VENDA WHERE VEN_Tienda = " + tienda + " AND YEAR(VEN_Data) = '" + yearFact + "' AND (VEN_Referencia IS NULL OR VEN_Referencia = '') ORDER BY VEN_Ticket DESC";
                }
                else if (TipoFact == "TA")
                {
                    textTipoFact = "el Ticket Abono";
                    sqlSiguenteNumFact = "SELECT TOP 1 VEN_Ticket + 1 AS nextTicketAbono FROM VENDA WHERE VEN_Tienda = " + tienda + " AND YEAR(VEN_Data) = '" + yearFact + "' AND VEN_Referencia = 'ABONO' ORDER BY VEN_Ticket DESC";
                }

                //=====================OBTENER EL VEN_NumFactura DE LA RESPUESTA QUE NOS ARROJA LA API=====================
                if (VEN_CodiFactSent == ventaNumero)
                {
                    siguenteNumFact = txtFactura.Text;
                }
                else
                {
                    siguenteNumFact = Conection.SelectOnce(sqlSiguenteNumFact);
                }
                
                int.TryParse(siguenteNumFact, out numFacturaOld);

                string consultaSQLventa = "";

                if (TipoFact == "F" || TipoFact == "FA")
                {

                    consultaSQLventa = "UPDATE VENDA SET VEN_NumFactura =?, VEN_Factura =?, VEN_DataFactura =?, VEN_DataDoc =? WHERE VEN_Codi = " + ventaNumero + " AND VEN_Tienda = " + tienda;

                }
                else
                {
                    consultaSQLventa = "UPDATE VENDA SET VEN_Ticket =?, VEN_DataTicket =?, VEN_DataDoc =? WHERE VEN_Codi = " + ventaNumero + " AND VEN_Tienda = " + tienda;

                }

                using (cmd2 = new System.Data.Odbc.OdbcCommand())
                {
                    cmd2.CommandText = consultaSQLventa;
                    System.Data.Odbc.OdbcParameter paramVEN_NumFactura = new System.Data.Odbc.OdbcParameter();
                    paramVEN_NumFactura.OdbcType = System.Data.Odbc.OdbcType.Int;
                    paramVEN_NumFactura.Value = numFacturaOld;
                    cmd2.Parameters.Add(paramVEN_NumFactura);
                    if (TipoFact == "F" || TipoFact == "FA")
                    {
                        System.Data.Odbc.OdbcParameter paramVEN_Factura = new System.Data.Odbc.OdbcParameter();
                        paramVEN_Factura.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramVEN_Factura.Value = "S";
                        cmd2.Parameters.Add(paramVEN_Factura);
                    }
                    System.Data.Odbc.OdbcParameter paramVEN_DataFactura = new System.Data.Odbc.OdbcParameter();
                    paramVEN_DataFactura.OdbcType = System.Data.Odbc.OdbcType.Date;
                    paramVEN_DataFactura.Value = dataFactura;
                    cmd2.Parameters.Add(paramVEN_DataFactura);
                    System.Data.Odbc.OdbcParameter paramVEN_DataDoc = new System.Data.Odbc.OdbcParameter();
                    paramVEN_DataDoc.OdbcType = System.Data.Odbc.OdbcType.Date;
                    paramVEN_DataDoc.Value = dataFactura;
                    cmd2.Parameters.Add(paramVEN_DataDoc);
                    Conection.ExecuteCommand(cmd2);
                }
            }
            else
            {
                var sqlNumFactCancelada = "SELECT VEN_NumFactura FROM FACTURAS_HACIENDA_AUTO where TBAI_Id = '" + chainInfoNumber + "'";

                siguenteNumFact = Conection.SelectOnce(sqlNumFactCancelada);

                int.TryParse(siguenteNumFact, out numFacturaOld);
            }

            using (var cmdInsert = new System.Data.Odbc.OdbcCommand())
            {

                if (TipoFact == "F" || TipoFact == "FA")
                {
                    if (VEN_CodiFactSent == ventaNumero)
                    {
                        cmdInsert.CommandText = sql_updateFactura;
                    }
                    else
                    {
                        cmdInsert.CommandText = @"
                                        INSERT INTO FACTURAS_HACIENDA_AUTO 
                                        (FACT_Codi, FACT_Tienda, FACT_Tipo, VEN_NumFactura, VEN_DataFactura, 
                                         TBAI_Codi, TBAI_Id, TBAI_Queue, 
                                         TBAI_Xml, TBAI_Data, TBAI_Hash, TBAI_UserId, TBAI_Estado, TBAI_mensaje, TBAI_Qr, TBAI_Url, TBAI_ImgQr)
                                        VALUES (NULL, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                        cmdInsert.Parameters.AddWithValue("@FACT_Tienda", tienda);
                        cmdInsert.Parameters.AddWithValue("@FACT_Tipo", TipoFact);
                        cmdInsert.Parameters.AddWithValue("@VEN_NumFactura", numFacturaOld);
                        cmdInsert.Parameters.AddWithValue("@VEN_DataFactura", dataFactura);
                    }
                        
                }
                else
                {
                    if (VEN_CodiFactSent == ventaNumero)
                    {
                        cmdInsert.CommandText = sql_updateFactura;
                    }
                    else
                    {
                        cmdInsert.CommandText = @"
                                    INSERT INTO FACTURAS_HACIENDA_AUTO 
                                    (FACT_Codi, FACT_Tienda, FACT_Tipo, VEN_Ticket, VEN_DataTicket, 
                                     TBAI_Codi, TBAI_Id, TBAI_Queue, 
                                     TBAI_Xml, TBAI_Data, TBAI_Hash, TBAI_UserId, TBAI_Estado, TBAI_mensaje, TBAI_Qr, TBAI_Url, TBAI_ImgQr)
                                    VALUES (NULL, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                        cmdInsert.Parameters.AddWithValue("@FACT_Tienda", tienda);
                        cmdInsert.Parameters.AddWithValue("@FACT_Tipo", TipoFact);
                        cmdInsert.Parameters.AddWithValue("@VEN_Ticket", numFacturaOld);
                        cmdInsert.Parameters.AddWithValue("@VEN_DataTicket", dataFactura);
                    }

                }

                // Comunes a ambos tipos

                cmdInsert.Parameters.AddWithValue("@TBAI_Codi", TBAI_CodiOld);
                cmdInsert.Parameters.AddWithValue("@TBAI_Id", old_factura.chainInfo.number);
                cmdInsert.Parameters.AddWithValue("@TBAI_Queue", old_factura.queueId);
                cmdInsert.Parameters.AddWithValue("@TBAI_Xml", old_factura.verifactuXml);
                cmdInsert.Parameters.AddWithValue("@TBAI_Data", dataFactura);
                cmdInsert.Parameters.AddWithValue("@TBAI_Hash", old_factura.chainInfo.hash);
                cmdInsert.Parameters.AddWithValue("@TBAI_UserId", old_factura.chainInfo.issuerIrsId);

                if (!string.IsNullOrEmpty(old_factura.qrcode))
                { 
                
                    filename2 = createImageQr(old_factura.qrcode, tbai_codifactura, txtTienda.Text);
                    pictureBox1.Image = Image.FromFile(filename2);
                    string sPathToFileToSave2 = filename2;

                    using (System.IO.FileStream fs2 = new System.IO.FileStream(sPathToFileToSave2, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        byte[] fileData = new byte[fs2.Length];
                        fs2.Read(fileData, 0, System.Convert.ToInt32(fs2.Length));
                        fs2.Close();
                        cmdInsert.Parameters.AddWithValue("@TBAI_Estado", "En cola");
                        cmdInsert.Parameters.AddWithValue("@TBAI_mensaje", "Factura insertada correctamente");
                        cmdInsert.Parameters.AddWithValue("@TBAI_Qr", old_factura.qrcode);
                        cmdInsert.Parameters.AddWithValue("@TBAI_Url", old_factura.verifactuUrl);
                        cmdInsert.Parameters.AddWithValue("@TBAI_ImgQr", fileData);
                    }
                }
                else
                {
                    cmdInsert.Parameters.AddWithValue("@TBAI_Estado", "Cancelada");
                    cmdInsert.Parameters.AddWithValue("@TBAI_mensaje", "Factura cancelada correctamente");
                    cmdInsert.Parameters.AddWithValue("@TBAI_Qr", "NULL");
                    cmdInsert.Parameters.AddWithValue("@TBAI_Url", "NULL");
                    cmdInsert.Parameters.AddWithValue("@TBAI_ImgQr", "NULL");
                }

                Conection.ExecuteCommand(cmdInsert);

            }


            MessageBox.Show("No estaba registrado en la Base de Datos " + textTipoFact + " Nº " + numFacturaOld + " del cliente " + nombreCli + " y con fecha " + dataFactura + ". Hemos realizado su registro");

            if (VEN_CodiFactSent == ventaNumero)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                consultAgainTBAI_Codi = true;
                this.apiConnectioSendJson();
            }


        }


        public string createImageQr(string qrcode, string ticketbai, string ven_tienda)
        {
            string filename;
            string path = getPathServerImg() + "qr\\";

            var base64Data = Regex.Match(qrcode, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            //long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string fileName = "QR_" + ticketbai + "_" + ven_tienda + "_" + DateTime.Now.ToString("yyyyMMdd") + ".BMP";
            filename = Path.Combine(path, fileName);

            if (!File.Exists(filename))
            {
                File.WriteAllBytes(filename, binData);
            }

            return filename;
        }

        public void buildCreditNote()
        {
            CreditNoteArray.ids = new List<InvoiceId>();
            CreditNoteArray.style = "I";//OJO hacer un array de apiCreditNote ***********
            //InvoiceIdArray.serie = serie.Substring(0, 1) + "/" + txtTienda.Text + "/" + factura_abono_year;
            InvoiceIdArray.number = factura_abono;
            InvoiceIdArray.issuedTime = date_formated;
            CreditNoteArray.ids.Add(InvoiceIdArray);

            InvoiceArray.creditNote = CreditNoteArray;
        }


        public string deleteOrState()
        {
            string sql_tipoForT= "SELECT TBAI_Id FROM FACTURAS_HACIENDA_AUTO WHERE TBAI_Queue = '" + txtFactura.Text + "'";
            string tipoForT= Conection.SelectOnce(sql_tipoForT);
            string msjReturn = "";

            var client = new RestClient(new RestClientOptions(TicketBai_DB.Trim())
            {
                MaxTimeout = -1 // -1 significa sin timeout
            });

            RestRequest request = null;

            if (txtTipo.Text == "S")
            {
                sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Estado =? WHERE TBAI_Queue = '" + txtFactura.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";
                sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE TBAI_Queue = '" + txtFactura.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";
                request = new RestRequest("verifactu/v1/invoice_state/" + txtFactura.Text, Method.Post);
                msjReturn = "Estado actualizado correctamente";
            }
            else if (txtTipo.Text == "D")
            {
                sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_UserId=?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_Xml = ?, TBAI_Queue = ?, TBAI_Estado=?, TBAI_mensaje = ? WHERE " +
                                        "TBAI_Id = '" + tipoForT + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND TBAI_Queue IS NULL";
                sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE TBAI_Id = '" + tipoForT + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND TBAI_Queue IS NULL";

                request = new RestRequest("verifactu/v1/invoice_cancel", Method.Post);
                request.AddStringBody(txtFactura.Text, DataFormat.Json);
                msjReturn = "Cancelada correctamente";
            }


            txtJson.Text = sql_updateFactura;

            WriteLog(txtJson.Text, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);

            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("User-Agent", "RestSharpClient/1.0");
            request.AddHeader("API-KEY", TicketBai_ApiKey.Trim());

            RestResponse response = client.Execute(request);

            //Uri fullUri = client.BuildUri(request);
            //MessageBox.Show(fullUri.ToString());

            txtJson.Text = $"Status: {response.StatusCode}\n\n{response.Content}";

            apiVerifactu.apiResponse new_factura = JsonConvert.DeserializeObject<apiVerifactu.apiResponse>(response.Content);

            

            if ((new_factura.message == "") || (new_factura.message is null))
            {


                // insert this file in the database
                using (System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand())
                {
                    //TBAI_Estado=?, 
                    cmd.CommandText = sql_updateFactura;
                    if (txtTipo.Text == "S")
                    {
                        System.Data.Odbc.OdbcParameter paramTBAI_Estado = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Estado.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Estado.Value = new_factura.state;
                        cmd.Parameters.Add(paramTBAI_Estado);
                    }
                    else if (txtTipo.Text == "D")
                    {
                        string rawFecha = new_factura.chainInfo.issuedTime; // esto es un string ISO 8601
                        DateTime fecha = DateTime.Parse(rawFecha);
                        string issuedTime = fecha.ToString("yyyy-MM-dd");

                        System.Data.Odbc.OdbcParameter paramTBAI_UserId = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_UserId.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_UserId.Value = new_factura.chainInfo.issuerIrsId;
                        cmd.Parameters.Add(paramTBAI_UserId);
                        System.Data.Odbc.OdbcParameter paramTBAI_Data = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Data.OdbcType = System.Data.Odbc.OdbcType.Date;
                        paramTBAI_Data.Value = issuedTime;
                        cmd.Parameters.Add(paramTBAI_Data);
                        System.Data.Odbc.OdbcParameter paramTBAI_Hash = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Hash.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Hash.Value = new_factura.chainInfo.hash;
                        cmd.Parameters.Add(paramTBAI_Hash);
                        System.Data.Odbc.OdbcParameter paramTBAI_Xml = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Xml.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Xml.Value = new_factura.verifactuXml;
                        cmd.Parameters.Add(paramTBAI_Xml);
                        System.Data.Odbc.OdbcParameter paramTBAI_Queue = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Queue.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Queue.Value = new_factura.queueId;
                        cmd.Parameters.Add(paramTBAI_Queue);
                        System.Data.Odbc.OdbcParameter paramTBAI_Estado = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Estado.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Estado.Value = "Cancelada";
                        cmd.Parameters.Add(paramTBAI_Estado);
                        System.Data.Odbc.OdbcParameter paramTBAI_Mensaje = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Mensaje.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Mensaje.Value = new_factura.message;
                        cmd.Parameters.Add(paramTBAI_Mensaje);
                    }

                    Conection.ExecuteCommand(cmd);

                }
                //System.Windows.Forms.Application.Exit();

                //MessageBox.Show("Proceso realizado correctamente");
            }
            else
            {
                using (System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand())
                {
                    //MessageBox.Show("Error: " + new_factura.message);
                    WriteLog("Error: " + new_factura.message, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);
                    cmd.CommandText = sql_updateFactura_KO;
                    System.Data.Odbc.OdbcParameter paramTBAI_Mensaje = new System.Data.Odbc.OdbcParameter();
                    paramTBAI_Mensaje.OdbcType = System.Data.Odbc.OdbcType.Text;
                    paramTBAI_Mensaje.Value = new_factura.message;
                    cmd.Parameters.Add(paramTBAI_Mensaje);
                    Conection.ExecuteCommand(cmd);

                    msjReturn = "Error: " + new_factura.message;
                }
            }
            System.Windows.Forms.Application.Exit();

            return msjReturn;
        }


        public static void WriteLog(string strLog, string path, string tipo, string numfra, string tiecodigo, string year)
        {
            string logFilePath = path + "Facturaslog\\" + tipo + "_" + numfra + "_" + tiecodigo + "_" + year + "_" + System.DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
            FileInfo logFileInfo = new FileInfo(logFilePath);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(strLog);
                }
            }
        }

        //cogemos los valores que pasan por parametro
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



    }
}