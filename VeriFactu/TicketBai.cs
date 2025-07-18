using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static VeriFactu.apiTicketBai;
using static VeriFactu.pathServer;

namespace VeriFactu
{
    public partial class TicketBai : Form
    {
        apiTicketBai.Json_Oddo apijsonInvoice;
        public TicketBai()
        {
            InitializeComponent();
            apijsonInvoice = new apiTicketBai.Json_Oddo();
            //MessageBox.Show("Es TicketBai");
        }

        static string respuesta;
        static string sql_maxTbaiQueue = System.String.Empty;
        static string sql_getVentas = System.String.Empty;
        static string sql_getTbaiCodi = System.String.Empty;
        static string sql_getTbaiCodiReenvio = System.String.Empty;
        static string sql_recargoEquivalenciaCliente = System.String.Empty;
        static string sql_checkTbaiCodi = System.String.Empty;
        static string sql_getListaParametros = System.String.Empty;
        static string sql_getVenFactAbono = System.String.Empty;
        static string sql_getFechaVentaAbono = System.String.Empty;
        static string sql_updateFactura = System.String.Empty;
        static string sql_updateFactura_KO = System.String.Empty;
        static string sql_getVenImportTotal = System.String.Empty;
        static string sql_updateStateDelete = System.String.Empty;
        static string sql_recargoEquivalencia = System.String.Empty;
        static string sql_datosIssureTienda = System.String.Empty;
        static string sql_facturaCancel= System.String.Empty;
        static string recargo_equivalencia_empresa = System.String.Empty;
        static string recargo_equivalencia_cliente = System.String.Empty;
        static string TicketBai_DB = System.String.Empty;
        static string TicketBai_ApiKey = System.String.Empty;
        static string ImportTotalString = System.String.Empty;
        static decimal ImporTotalCheck = 0;
        static string serie = System.String.Empty;
        static string descripcion = System.String.Empty;
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


        TimeSpan hour = new TimeSpan();
        static string tbai_codifactura = System.String.Empty;
        static string date_formated = System.String.Empty;
        static string nombre_cliente = System.String.Empty;

        //=============ARRAYS DE LA API PARA ARMAR INVOICE
        static ArrayList ids = new ArrayList();
        static apiTicketBai.Invoice InvoiceArray = new apiTicketBai.Invoice();
        static apiTicketBai.Invoice_id Invoice_idArray = new apiTicketBai.Invoice_id();
        static apiTicketBai.Recipient recipientArray = new apiTicketBai.Recipient();
        static apiTicketBai.CreditNote CreditNoteArray = new apiTicketBai.CreditNote();
        static apiTicketBai.CreditNote_ids CreditNote_idsArray = new apiTicketBai.CreditNote_ids();
        static apiTicketBai.Description descriptionArray = new apiTicketBai.Description();
        static apiTicketBai.Issuer IssuerArray = new apiTicketBai.Issuer();
        static apiTicketBai.Line lineArray = new apiTicketBai.Line();
        static apiTicketBai.VatLine vatLineArray = new apiTicketBai.VatLine();

        static apiVerifactu.Id idArray = new apiVerifactu.Id();
        static apiVerifactu.InvoiceId InvoiceIdArray = new apiVerifactu.InvoiceId();

        //=============VARIABLES DE TOTAL NETO PARA CALCULAR SEGÚN EL VATLINE

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
        static string contTBAI_Codi = System.String.Empty;
        static Decimal LVN_IVA = 0;


        static string path_server = pathServer.getPathServer();

        private void TicketBai_Load(object sender, EventArgs e)
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

            //recargo_equivalencia_cliente = Conection.SelectOnce(sql_recargoEquivalenciaCliente); //consulta para optiner 'recargo_equivalencia_empresa'

            //=========================importe minimo para pasar datos del cliente=========================

            string sql_getImporteMinimo = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_Key = 'TicketBai_ImporteMinimo'";
            importe_minimo = Convert.ToDecimal(Conection.SelectOnce(sql_getImporteMinimo));

            //============================CONSULTAR FACTURA CANCELADA============================
            sql_facturaCancel = "SELECT COUNT(*) FROM FACTURAS_HACIENDA_AUTO WHERE TBAI_Codi IN( SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE TBAI_Queue = " + txtFactura.Text + " ) AND TBAI_Queue IS NOT NULL";
            contTBAI_Codi = Conection.SelectOnce(sql_facturaCancel);

            int result_contTBAI_Codi = int.Parse(contTBAI_Codi);
            
            //=========================CONDICIÓN SEGÚN EL TIPO DE FACTURA=========================

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
                if ((txtTipo.Text[0] == 'D' && result_contTBAI_Codi < 2) || (txtTipo.Text[0] == 'S'))
                {
                    this.deleteOrState();
                }
                else
                {
                    MessageBox.Show("Esta factura ya fue cancelada.");
                }
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                MessageBox.Show("Error: El tipo de factura o ticket es incorrecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        public void sameSQLFacturas()
        {
            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "'";

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_Estado=?, TBAI_mensaje = ? WHERE " +
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

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_Estado=?, TBAI_mensaje = ? WHERE " +
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
                    this.datosCliente();
                    CreditNoteArray.reason = "R1";
                }
                else
                {
                    CreditNoteArray.reason = "R5";
                    InvoiceArray.simple = true;
                }

                this.buildCreditNote();
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

                if (vendaTable.Rows[0].Field<string>("Nom").ToString() != null)
                {
                    nombre_cliente = vendaTable.Rows[0].Field<string>("Nom").ToString();
                }
                else
                {
                    MessageBox.Show("Error: Falta el Nombre del Cliente");
                    System.Windows.Forms.Application.Exit();
                }

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

                InvoiceArray.simple = true;
                CreditNoteArray.reason = "R5";
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

        public void datosCliente()
        {
            //=====================RECIPIENT=====================

            //=====================numero de TIPO DE DOCUMENTO (irsId)

            /*if (vendaTable.Rows[0].Field<string>("TipoDoc").ToString() != null)
            {
                if (vendaTable.Rows[0].Field<string>("Pais") != null)
                {
                    if (vendaTable.Rows[0].Field<string>("TipoDoc").ToString() != "02") //IrsIdType
                    {
                        recipientArray.idType = vendaTable.Rows[0].Field<string>("TipoDoc").ToString(); //IrsIdType
                    }

                }
                else
                {
                    MessageBox.Show("Error: Falta Pais del cliente");
                    System.Windows.Forms.Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Error: Falta tipo de documento de identidad");
                System.Windows.Forms.Application.Exit();
            }*/

            if (vendaTable.Rows[0].Field<string>("CLI_Dni") != null)
            {
                string dni = vendaTable.Rows[0].Field<string>("CLI_Dni").ToString();
                dni = dni.Replace("-", "");
                dni = dni.Replace("_", "");
                dni = dni.Replace(" ", "");
                char[] charsToTrim = { '*', ' ', '\'' };
                dni = dni.Trim(charsToTrim);

                if (vendaTable.Rows[0].Field<string>("TipoDoc") != null)
                {
                    if (vendaTable.Rows[0].Field<string>("Pais") != null)
                    {
                        if (vendaTable.Rows[0].Field<string>("TipoDoc").ToString() == "02")//IrsIdType
                        {
                            recipientArray.irsId = dni;
                        }
                        else
                        {
                            recipientArray.id = dni;
                            recipientArray.idType = vendaTable.Rows[0].Field<string>("TipoDoc").ToString(); //IrsIdType
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Falta Pais del cliente");
                        System.Windows.Forms.Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Error: Falta Tipo de documento de identidad");
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
            

            if (vendaTable.Rows[0].Field<string>("Pais") != null)
            {
                recipientArray.country = vendaTable.Rows[0].Field<string>("Pais").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el País del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            if (vendaTable.Rows[0].Field<string>("CLI_CodiPostal") != null)
            {
                recipientArray.postal = vendaTable.Rows[0].Field<string>("CLI_CodiPostal").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el Código Postal del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            if (vendaTable.Rows[0].Field<string>("Domicili") != null)
            {
                recipientArray.address = vendaTable.Rows[0].Field<string>("Domicili").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta la Dirección del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            if (recargo_equivalencia_cliente == "S")
            {
                recipientArray.vat2 = true;
            }

            InvoiceArray.simple = false;

            InvoiceArray.recipient = recipientArray;

        }

        public void buildJson()
        {

            //=====================URL JSON===================

            var client = new RestClient(new RestClientOptions(TicketBai_DB.Trim())
            {
                MaxTimeout = -1 // -1 significa sin timeout
            });

            var request = new RestRequest("ticketbai_api/v1/invoice", Method.Post);

            //=====================ISSURE=====================

            datosIssureTiendaTable = Conection.SelectRows(sql_datosIssureTienda);

            if (datosIssureTiendaTable.Rows[0].Field<string>("RegMercantil").ToString() != null)
            {
                IssuerArray.name = datosIssureTiendaTable.Rows[0].Field<string>("RegMercantil").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el Registro Mercantil de la optica del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            if (datosIssureTiendaTable.Rows[0].Field<string>("NIF").ToString() != null)
            {
                IssuerArray.irsId = datosIssureTiendaTable.Rows[0].Field<string>("NIF").ToString();
            }
            else
            {
                MessageBox.Show("Error: Falta el NIF de la optica del Cliente");
                System.Windows.Forms.Application.Exit();
            }

            InvoiceArray.issuer = IssuerArray; //AGREGAMOS ISSURE A INVOICE

            if (txtTipo.Text == "F" || txtTipo.Text == "FA")
            {
                date = vendaTable.Rows[0].Field<DateTime>("VEN_DataFactura");
            }
            else if (txtTipo.Text == "T" || txtTipo.Text == "TA")
            {
                date = vendaTable.Rows[0].Field<DateTime>("VEN_DataTicket");
            }

            tbai_codifactura = Conection.SelectOnce(sql_getTbaiCodi);

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

            string date_formated = date.ToString("yyyy-MM-dd");
            TimeSpan hour = vendaTable.Rows[0].Field<TimeSpan>("VEN_Hora");
            string venCodigo = vendaTable.Rows[0].Field<int>("VEN_Codi").ToString();

            //=====================ID=====================
            Invoice_idArray.number = tbai_codifactura;
            Invoice_idArray.serie = serie + "/" + txtTienda.Text + "/" + txtYear.Text;
            Invoice_idArray.issuedTime = date_formated + " " + hour;

            InvoiceArray.id = Invoice_idArray;

            //=====================DESCRIPCION=====================

            

            descriptionArray.operationDate = date_formated + " " + hour;
            descriptionArray.text = descripcion + serie + "/" + txtTienda.Text + "/" + txtYear.Text + " EMITIDA A " + nombre_cliente + " DE LA VENTA " + txtYear.Text + "/" + venCodigo;

            InvoiceArray.description = descriptionArray;

            //=====================LINES=====================


            bool extrajero = false;
            
            InvoiceArray.lines = new List<apiTicketBai.Line>();

            foreach (DataRow row in vendaTable.Rows)
            {
                var lineArray = new apiTicketBai.Line();
                extrajero = false;

                if ((ImporTotalCheck > importe_minimo) && (!txtTipo.Text.Equals("T") && !txtTipo.Text.Equals("TA")))
                {

                    if (vendaTable.Rows[0].Field<string>("Pais") != null)
                    {
                        if (vendaTable.Rows[0].Field<string>("Pais").ToString() != "ES")
                        {

                            lineArray.merchandiseOrService = "merchandise";
                            extrajero = true;
                        }
                        else
                        {
                            extrajero = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Falta el País del Cliente");
                        System.Windows.Forms.Application.Exit();
                    }

                }

                lineArray.description = row.Field<string>("PRD_Item").ToString();
                lineArray.quantity = Math.Abs(row.Field<Int16>("LVN_Uds"));
                lineArray.vat = Convert.ToInt16(row.Field<Decimal>("LVN_IVA"));


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
                lineArray.price = Math.Round(((row.Field<Decimal>("LVN_PreuDtoLineaVenta") / vat_calc) / lineArray.quantity), 2);
                lineArray.amountWithVat = precio_linea_con_iva_con_dto;
                totalAmountWithVat = totalAmountWithVat + lineArray.amountWithVat;
                total_neto = total_neto + precio_linea_con_iva_con_dto;

                if (precio_linea_sin_iva_con_dto == 0)
                {
                    lineArray.price = 0;
                }else
                {

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


                InvoiceArray.lines.Add(lineArray);

            }
               InvoiceArray.vatLines = new List<apiTicketBai.VatLine>();


            if ((total_neto_21 > 0) || (total_neto_21 < 0))
            {
                var vatLine21 = new VatLine
                {
                    amount = Math.Round(total_neto_21 * Convert.ToDecimal(0.21), 2),
                    @base = Math.Round(total_neto_21, 2),
                    rate = 21,
                };

                if (extrajero == true)
                {
                vatLine21.merchandiseOrService = "merchandise";
                }

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine21.isUsingSimplifiedRegime = true;
                }

                InvoiceArray.vatLines.Add(vatLine21);
            }

            if ((total_neto_10 > 0) || (total_neto_10 < 0))
            {
                var vatLine10 = new VatLine
                {
                    amount = Math.Round(total_neto_10 * Convert.ToDecimal(0.10), 2),
                    @base = Math.Round(total_neto_10, 2),
                    rate = 10,
                };

                if (extrajero == true)
                {
                    vatLine10.merchandiseOrService = "merchandise";
                }

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine10.isUsingSimplifiedRegime = true;
                }

                InvoiceArray.vatLines.Add(vatLine10);

            }

            if ((total_neto_4 > 0) || (total_neto_4 < 0))
            {
                var vatLine4 = new VatLine
                {
                    amount = Math.Round(total_neto_4 * Convert.ToDecimal(0.04), 2),
                    @base = Math.Round(total_neto_4, 2),
                    rate = 4,
                };

                if (extrajero == true)
                {
                    vatLine4.merchandiseOrService = "merchandise";
                }

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine4.isUsingSimplifiedRegime = true;
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
                    exemptionReason = "E1",
                };

                if (extrajero == true)
                {
                    vatLine0.merchandiseOrService = "merchandise";
                }

                if (recargo_equivalencia_empresa == "S")
                {
                    vatLine0.isUsingSimplifiedRegime = true;
                }

                InvoiceArray.vatLines.Add(vatLine0);
            }

            InvoiceArray.total = total_neto;

            Decimal compensacion = Math.Abs(total_neto - totalAmountWithVat);
            if (compensacion > 0)
            {
                Line line_compensacion = new Line();
                line_compensacion.description = "Compensación";
                line_compensacion.quantity = 1;
                line_compensacion.price = compensacion;
                line_compensacion.amountWithVat = compensacion;
                line_compensacion.vat = 0;
                InvoiceArray.lines.Add(line_compensacion);
            }

            if (recargo_equivalencia_empresa == "S")
            {
                InvoiceArray.vatKeys[0] = "51";//
            }
            else
            {
                InvoiceArray.vatKeys[0] = "01"; //VatKey 
            }

            apijsonInvoice.invoice = InvoiceArray;



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

            apiTicketBai.Factura new_factura = JsonConvert.DeserializeObject<apiTicketBai.Factura>(response.Content);

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
                        //TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_Hash = ?, TBAI_Estado=?, TBAI_mensaje = ?, 
                        cmd.CommandText = sql_updateFactura;
                        System.Data.Odbc.OdbcParameter paramTBAI_Codi = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Codi.OdbcType = System.Data.Odbc.OdbcType.Int;
                        paramTBAI_Codi.Value = tbai_codifactura;
                        cmd.Parameters.Add(paramTBAI_Codi);//====HECHO
                        System.Data.Odbc.OdbcParameter paramTBAI_Id = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Id.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Id.Value = new_factura.tbaiId;
                        cmd.Parameters.Add(paramTBAI_Id);//====HECHO
                        System.Data.Odbc.OdbcParameter paramTBAI_Qr = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Qr.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Qr.Value = new_factura.qrcode;
                        cmd.Parameters.Add(paramTBAI_Qr);//====HECHO
                        System.Data.Odbc.OdbcParameter paramTBAI_Url = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Url.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Url.Value = new_factura.tbaiUrl;//====HECHO
                        cmd.Parameters.Add(paramTBAI_Url);
                        System.Data.Odbc.OdbcParameter paramTBAI_Queue = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Queue.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Queue.Value = new_factura.queueId;
                        cmd.Parameters.Add(paramTBAI_Queue);//====HECHO
                        System.Data.Odbc.OdbcParameter paramTBAI_Xml = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Xml.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Xml.Value = new_factura.tbaiXml;//====HECHO
                        cmd.Parameters.Add(paramTBAI_Xml);
                        System.Data.Odbc.OdbcParameter paramTBAI_ImgQr = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_ImgQr.OdbcType = System.Data.Odbc.OdbcType.Image;
                        paramTBAI_ImgQr.Value = fileData;
                        cmd.Parameters.Add(paramTBAI_ImgQr);//====HECHO
                        System.Data.Odbc.OdbcParameter paramTBAI_Data = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Data.OdbcType = System.Data.Odbc.OdbcType.Date;
                        paramTBAI_Data.Value = issuedTime;
                        cmd.Parameters.Add(paramTBAI_Data);//====HECHO
                        System.Data.Odbc.OdbcParameter paramTBAI_Hash = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Hash.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Hash.Value = new_factura.chainInfo.hash;
                        cmd.Parameters.Add(paramTBAI_Hash);//====HECHO
                        System.Data.Odbc.OdbcParameter paramTBAI_Estado = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Estado.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Estado.Value = "En cola";
                        cmd.Parameters.Add(paramTBAI_Estado);//====HECHO
                        System.Data.Odbc.OdbcParameter paramDateCreated = new System.Data.Odbc.OdbcParameter();
                        paramDateCreated.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramDateCreated.Value = "Factura insertada correctamente";
                        cmd.Parameters.Add(paramDateCreated);

                        Conection.ExecuteCommand(cmd);

                        //using (OdbcConnection connection = new OdbcConnection(connectionString))
                        //{
                        //    cmd.Connection = connection;
                        //    connection.Open();
                        //    cmd.ExecuteNonQuery();
                        //}
                    }
                }

                //System.Windows.Forms.Application.Exit();

            }
            else
            {
                using (System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand())
                {

                    if (new_factura.message == "The invoice is duplicated")
                    {
                        string TBAI_Queue = "";
                        string sql_TBAI_QueueAnterior = "SELECT TBAI_Queue FROM FACTURAS_HACIENDA_AUTO WHERE TBAI_Codi = " + (int.Parse(tbai_codifactura) - 1);
                        string TBAI_QueueAnterior = Conection.SelectOnce(sql_TBAI_QueueAnterior);
                        sql_maxTbaiQueue = "SELECT TBAI_Queue FROM FACTURAS_HACIENDA_AUTO WHERE TBAI_Queue >= " + TBAI_QueueAnterior + "  ORDER BY TBAI_Queue ASC";

                        DataTable resultado = Conection.SelectRows(sql_maxTbaiQueue);

                        if (resultado != null && resultado.Rows.Count > 1)
                        {
                            List<int> cola = new List<int>();

                            foreach (DataRow fila in resultado.Rows)
                            {
                                if (int.TryParse(fila["TBAI_Queue"].ToString(), out int valor))
                                {
                                    cola.Add(valor);
                                }
                            }

                            List<int> faltantes = new List<int>();

                            for (int i = 0; i < cola.Count - 1; i++)
                            {
                                int actual = cola[i];
                                int siguiente = cola[i + 1];

                                if (siguiente - actual > 1)
                                {
                                    for (int j = actual + 1; j < siguiente; j++)
                                    {
                                        TBAI_Queue = j.ToString();
                                    }
                                }
                            }
                        }
                        else if (resultado != null && resultado.Rows.Count == 1)
                        {
                            if (int.TryParse(resultado.Rows[0]["TBAI_Queue"].ToString(), out int valor))
                            {
                                TBAI_Queue = (valor + 1).ToString();
                            }
                        }

                        var client2 = new RestClient(new RestClientOptions(TicketBai_DB.Trim())
                        {
                            MaxTimeout = -1 
                        });

                        var request2 = new RestRequest("ticketbai_api/v1/invoice_data/" + TBAI_Queue, Method.Get);

                       

                        WriteLog(txtJson.Text, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);

                        request2.AddHeader("Content-Type", "application/json; charset=utf-8");
                        request2.AddHeader("Accept", "application/json");
                        request2.AddHeader("User-Agent", "RestSharpClient/1.0");
                        request2.AddHeader("API-KEY", TicketBai_ApiKey.Trim());

                        RestResponse respuestaApi= client2.Execute(request2);

                        txtJson.Text = $"Status: {respuestaApi.StatusCode}\n\n{respuestaApi.Content}";

                        apiTicketBai.Factura old_factura = JsonConvert.DeserializeObject<apiTicketBai.Factura>(respuestaApi.Content);

                        // Convertir a XML
                        string base64Xml = old_factura.tbaiXml;
                        byte[] xmlBytes = Convert.FromBase64String(base64Xml);
                        string xmlString = Encoding.UTF8.GetString(xmlBytes);

                        // Convertir a JSON
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlString);

                        string jsonFromXml = JsonConvert.SerializeXmlNode(xmlDoc);

                        // Convertir el JSON a JObject
                        JObject jsonObject = JObject.Parse(jsonFromXml);

                        // Acceder a DatosFactura
                        //=====================OBTENER MONTO TOTAL DE LA FACTURA=====================
                        //decimal importeTotal;
                        //var importeTotalStr = jsonObject["tbai:TicketBai"]?["Factura"]?["DatosFactura"]?["ImporteTotalFactura"]?.ToString();
                        //
                        //decimal.TryParse(importeTotalStr, NumberStyles.Any, CultureInfo.InvariantCulture, out importeTotal);


                
                        //=====================OBTENER NOMBRE Y TIENDA DEL CLIENTE DE LA DESCRIPCIÓN=====================
                        var datosFactura = jsonObject["tbai:TicketBai"]?["Factura"]?["DatosFactura"]?["DescripcionFactura"]?.ToString();
                        string nombreCli = "";
                        int tienda = 0;
                        int ventaNumero = 0;

                        // Extraer nombre después de "EMITIDA A"
                        Match matchNombre = Regex.Match(datosFactura, @"EMITIDA A\s+(.+)$");

                        // Extraer número de tienda después de "F/"
                        Match matchTienda = Regex.Match(datosFactura, @"F\/(\d+)\/");

                        // Extraer número final de venta después de "DE LA VENTA"
                        Match matchVenta = Regex.Match(datosFactura, @"DE LA VENTA\s+[\d]+\/(\d+)");



                        if (matchNombre.Success)
                            nombreCli = matchNombre.Groups[1].Value.Trim();

                        if (matchTienda.Success)
                            int.TryParse(matchTienda.Groups[1].Value, out tienda);

                        if (matchVenta.Success)
                            int.TryParse(matchVenta.Groups[1].Value, out ventaNumero);


                        string consultaSQLventa = "";

                        if (txtTipo.Text == "F")
                        {
                            consultaSQLventa = "UPDATE VENDA SET VEN_NumFactura =?, VEN_Factura =?, VEN_DataFactura =?, VEN_DataDoc =? WHERE VEN_Codi = " + ventaNumero + "AND VEN_Tienda = " + tienda;
                            sql_updateFactura = "INSERT INTO FACTURAS_HACIENDA_AUTO (FACT_Codi, FACT_Tienda, FACT_Tipo, VEN_NumFactura, VEN_DataFactura, TBAI_Codi,TBAI_Id, TBAI_Qr, TBAI_Url, TBAI_Queue, TBAI_Xml, TBAI_ImgQr, TBAI_Data, TBAI_Mensaje, TBAI_Hash, TBAI_Estado, TBAI_UserId) VALUES(NULL, 'F', 12345, '2025-07-17', 67890, '2025-07-17', 10, 'abc123-tbai-id', 'QRDATA123456...', 'https://tbai.url/factura/123', '789', '<xml>...</xml>', NULL, '2025-07-17', 'Factura procesada correctamente', 'HASH987654321...', 'PROCESADA', 'usuario_api'); ";
                        }
                        else if (txtTipo.Text == "T")
                        {
                            consultaSQLventa = "UPDATE VENDA SET VEN_Ticket =?, VEN_DataTicket =?, VEN_DataDoc =? WHERE VEN_Codi = '" + ventaNumero + "AND VEN_Tienda = " + tienda;
                            sql_updateFactura = "INSERT INTO FACTURAS_HACIENDA_AUTO (FACT_Codi, FACT_Tienda, FACT_Tipo, VEN_Ticket, VEN_DataTicket, TBAI_Codi,TBAI_Id, TBAI_Qr, TBAI_Url, TBAI_Queue, TBAI_Xml, TBAI_ImgQr, TBAI_Data, TBAI_Mensaje, TBAI_Hash, TBAI_Estado, TBAI_UserId) VALUES(NULL, 'F', 12345, '2025-07-17', 67890, '2025-07-17', 10, 'abc123-tbai-id', 'QRDATA123456...', 'https://tbai.url/factura/123', '789', '<xml>...</xml>', NULL, '2025-07-17', 'Factura procesada correctamente', 'HASH987654321...', 'PROCESADA', 'usuario_api'); ";
                        }

                        Conection.PerformQuery(sql_updateFactura);

                        if (datosFactura == "Factura F/2025/3/12 EMITIDA A William Brown ")
                        {
                            MessageBox.Show("entró");
                        }


                    }
                    else
                    {
                        MessageBox.Show("Error: " + new_factura.message);
                        WriteLog("Error: " + new_factura.message, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);
                        cmd.CommandText = sql_updateFactura_KO;
                        System.Data.Odbc.OdbcParameter paramTBAI_Mensaje = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Mensaje.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Mensaje.Value = new_factura.message;
                        cmd.Parameters.Add(paramTBAI_Mensaje);
                    }

                    Conection.ExecuteCommand(cmd);
                }
            }
            //MessageBox.Show("Factura creada");
            System.Windows.Forms.Application.Exit();
        }

        public void buildCreditNote()
        {
            CreditNoteArray.ids = new List<CreditNote_ids>();
            CreditNoteArray.style = "I";
            CreditNote_idsArray.number = factura_abono;
            CreditNote_idsArray.serie = serie.Substring(0, 1) + "/" + txtTienda.Text + "/" + factura_abono_year;
            CreditNote_idsArray.issuedTime = date_abo_formated + " 00:00:00";
            CreditNoteArray.ids.Add(CreditNote_idsArray);

            InvoiceArray.creditNote = CreditNoteArray;
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

        public string deleteOrState()
        {
            string sql_tipoForT = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE TBAI_Queue = '" + txtFactura.Text + "'";
            string tipoForT = Conection.SelectOnce(sql_tipoForT);
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
                request = new RestRequest("ticketbai_api/v1/invoice_state", Method.Post);
                request.AddStringBody(txtFactura.Text, DataFormat.Json);
                msjReturn = "Estado actualizado correctamente";
            }
            else if (txtTipo.Text == "D")
            {
                sql_updateStateDelete = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Estado = 'Cancelada' WHERE " +
                                        "TBAI_Codi = '" + tipoForT + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND TBAI_Queue = " + txtFactura.Text;
                Conection.PerformQuery(sql_updateStateDelete);

                sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Xml = ?, TBAI_Id = ?, TBAI_Queue = ?, TBAI_Estado=?, TBAI_Data=? WHERE " +
                                        "TBAI_Codi = '" + tipoForT + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND TBAI_Queue IS NULL";
                sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE TBAI_Codi = '" + tipoForT + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND TBAI_Queue IS NULL";

                request = new RestRequest("ticketbai_api/v1/invoice_cancel", Method.Post);
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

            apiTicketBai.Factura new_factura = JsonConvert.DeserializeObject<apiTicketBai.Factura>(response.Content);



            if ((new_factura.message == "") || (new_factura.message is null) || (tipoForT == new_factura.chainInfo.number))
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

                        string issuedTime = DateTime.Now.ToString("yyyy/MM/dd");

                        System.Data.Odbc.OdbcParameter paramTBAI_Xml = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Xml.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Xml.Value = new_factura.tbaiXml;
                        cmd.Parameters.Add(paramTBAI_Xml);
                        System.Data.Odbc.OdbcParameter paramTBAI_Id = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Id.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Id.Value = new_factura.tbaiId;
                        cmd.Parameters.Add(paramTBAI_Id);
                        System.Data.Odbc.OdbcParameter paramTBAI_Queue = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Queue.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Queue.Value = new_factura.queueId;
                        cmd.Parameters.Add(paramTBAI_Queue);
                        System.Data.Odbc.OdbcParameter paramTBAI_Estado = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Estado.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Estado.Value = "Cancelada";
                        cmd.Parameters.Add(paramTBAI_Estado);
                        System.Data.Odbc.OdbcParameter paramTBAI_Data = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Data.OdbcType = System.Data.Odbc.OdbcType.Date;
                        paramTBAI_Data.Value = issuedTime;
                        cmd.Parameters.Add(paramTBAI_Data);
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
            //System.Windows.Forms.Application.Exit();



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
