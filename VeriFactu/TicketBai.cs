using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static VeriFactu.apiVerifactu;
using static VeriFactu.pathServer;

namespace VeriFactu
{
    public partial class TicketBai : Form
    {
        public TicketBai()
        {
            InitializeComponent();
            //MessageBox.Show("Es TicketBai");
        }

        static string respuesta;
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
        static apiVerifactu.Invoice InvoiceArray = new apiVerifactu.Invoice();
        static apiVerifactu.Issuer IssuerArray = new apiVerifactu.Issuer();
        static apiVerifactu.Recipient recipientArray = new apiVerifactu.Recipient();
        static apiVerifactu.Id idArray = new apiVerifactu.Id();
        static apiVerifactu.Description descriptionArray = new apiVerifactu.Description();
        static apiVerifactu.VatLine vatLineArray = new apiVerifactu.VatLine();
        static apiVerifactu.InvoiceId InvoiceIdArray = new apiVerifactu.InvoiceId();
        static apiVerifactu.CreditNote CreditNoteArray = new apiVerifactu.CreditNote();

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
        static Decimal LVN_IVA = 0;


        static string path_server = pathServer.getPathServer();

        private void Inicio_Load(object sender, EventArgs e)
        {
            //client.Timeout = -1;
            date_formated = date.ToString("yyyy-MM-dd");
            //conection.openConection();

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

            sql_datosIssureTienda = "SELECT (SELECT PAR_Valor FROM indo.TIENDADATOS WHERE PAR_Key = 'NIF' AND TIE_Codigo = " + txtTienda.Text + ") AS NIF, (SELECT PAR_Valor FROM indo.TIENDADATOS WHERE PAR_Key = 'RegMercantil' AND TIE_Codigo = " + txtTienda.Text + ") AS RegMercantil;";

            //recargo_equivalencia_cliente = Conection.SelectOnce(sql_recargoEquivalenciaCliente); //consulta para optiner 'recargo_equivalencia_empresa'

            //=========================importe minimo para pasar datos del cliente=========================

            string sql_getImporteMinimo = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_Key = 'TicketBai_ImporteMinimo'";
            importe_minimo = Convert.ToDecimal(Conection.SelectOnce(sql_getImporteMinimo));

            //=========================CONDICIÓN SEGÚN EL TIPO DE FACTURA=========================

            if (txtTipo.Text == "F")
            {
                respuesta = this.factura(1);
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtTipo.Text == "FA")
            {
                respuesta = this.facturaAbono();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtTipo.Text == "T")
            {
                respuesta = this.ticket();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtTipo.Text == "TA")
            {
                respuesta = this.ticketAbono();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtTipo.Text[0] == 'D')//cogemos la pprimera letra para identificar que tiene 'D' y así saber que es delete, una vez hecho eso en la función identifico el tipo de delete
            {
                respuesta = this.deleteOrState();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtTipo.Text[0] == 'S')//cogemos la pprimera letra para identificar que tiene 'S' y así saber que es state, una vez hecho eso en la función identifico el tipo de state
            {
                respuesta = this.deleteOrState();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: El tipo de factura es incorrecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        public void sameSQLFacturas()
        {
            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "'";

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='" + txtTipo.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_Estado=?, TBAI_UserId=?, TBAI_mensaje = ? WHERE " +
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

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_Hash  = ?, TBAI_Estado=?, TBAI_UserId=?, TBAI_mensaje = ? WHERE " +
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

        public string factura(int codigo)
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

            string msj = "Factura creada correctamente";

            string checkIfExist = Conection.SelectOnce(sql_checkTbaiCodi);

            if (string.IsNullOrEmpty(checkIfExist) || txtReenvio.Text == "R")
            {

                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");



                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);

                vendaTable = Conection.SelectRows(sql_getVentas);

                Decimal CLI_Codi = 0;

                if (vendaTable.Rows[0].Field<Decimal>("CLI_Codi").ToString() != null)
                {
                    //apiRecipient.irsId = vendaTable.Rows[0].Field<string>("TipoDoc").ToString(); //IrsIdType
                    CLI_Codi = vendaTable.Rows[0].Field<Decimal>("CLI_Codi");
                }
                else
                {
                    MessageBox.Show("Error: Falta el Pa�s del Cliente");
                    System.Windows.Forms.Application.Exit();
                }

                if ((ImporTotalCheck > importe_minimo))
                {
                    if (CLI_Codi == 0)
                    {
                        InvoiceArray.type = "F2";
                        InvoiceArray.simple = true;

                    }
                    else
                    {
                        //this.datosCliente();
                    }
                }
                else
                {
                    InvoiceArray.type = "F2";
                    InvoiceArray.simple = true;
                }

                //this.buildJson();
            }
            else
            {
                msj = "No se puede crear factura duplicada";
            }


            return msj;



        }

        public string facturaAbono()
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

            string msj = "Factura creada correctamente";

            string checkIfExist = Conection.SelectOnce(sql_checkTbaiCodi);

            if (string.IsNullOrEmpty(checkIfExist) || txtReenvio.Text == "R")
            {

                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");



                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);

                vendaTable = Conection.SelectRows(sql_getVentas);

                Decimal CLI_Codi = 0;

                if (vendaTable.Rows[0].Field<Decimal>("CLI_Codi").ToString() != null)
                {
                    //apiRecipient.irsId = vendaTable.Rows[0].Field<string>("TipoDoc").ToString(); //IrsIdType
                    CLI_Codi = vendaTable.Rows[0].Field<Decimal>("CLI_Codi");
                }
                else
                {
                    MessageBox.Show("Error: Falta el Pa�s del Cliente");
                    System.Windows.Forms.Application.Exit();
                }


                //================DATOS DEL CLIENTE================
                if ((ImporTotalCheck > importe_minimo))
                {
                    if (CLI_Codi == 0)
                    {
                        //this.buildCreditNote();
                    }
                    else
                    {
                        //this.buildCreditNote(CLI_Codi);
                        //this.datosCliente();

                    }
                }
                else
                {
                    //this.buildCreditNote();

                }

                //this.buildJson();
            }
            else
            {
                msj = "No se puede crear factura duplicada";
            }

            return msj;
        }

        public string ticket()
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

            string msj = "Ticket creado correctamente";

            string checkIfExist = Conection.SelectOnce(sql_checkTbaiCodi);

            if (string.IsNullOrEmpty(checkIfExist) || txtReenvio.Text == "R")
            {
                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");
                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);

                vendaTable = Conection.SelectRows(sql_getVentas);


                InvoiceArray.type = "F2";
                InvoiceArray.simple = true;

                //this.buildJson();
            }
            else
            {
                msj = "No se puede enviar un ticket duplicado";
            }

            return msj;
        }

        public string ticketAbono()
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




            string msj = "Ticket creado correctamente";

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

                ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");

                ImporTotalCheck = Convert.ToDecimal(ImportTotalString);
                //this.buildCreditNote();
                //this.buildJson();
            }
            else
            {
                msj = "No se puede enviar un ticket duplicado";
            }

            return msj;

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
            string sql_tipoForT = "SELECT FACT_Tipo FROM FACTURAS_HACIENDA_AUTO WHERE TBAI_Queue = '" + txtFactura.Text + "'";
            string tipoForT = Conection.SelectOnce(sql_tipoForT);

            var client = new RestClient(new RestClientOptions(TicketBai_DB.Trim())
            {
                MaxTimeout = -1 // -1 significa sin timeout
            });

            RestRequest request = null;

            if (txtTipo.Text == "S")
            {
                sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Estado =? WHERE TBAI_Queue = '" + txtFactura.Text + "' AND FACT_Tiend = '" + txtTienda.Text + "'";
                request = new RestRequest("verifactu/v1/invoice_state/" + txtFactura.Text, Method.Post);
            }
            else if (txtTipo.Text == "D")
            {
                if (tipoForT == "F")
                {
                    sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_UserId=?, TBAI_Data = ?, TBAI_Id = ?, TBAI_Hash  = ?, TBAI_Xml = ?, TBAI_Queue = ?, TBAI_Estado=?, TBAI_mensaje = ? WHERE " +
                                        "VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'F' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "'";

                }
                else if (tipoForT == "T")
                {
                    sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_UserId=?, TBAI_Data = ?, TBAI_Id = ?, TBAI_Hash  = ?, TBAI_Xml = ?, TBAI_Queue = ?, TBAI_Estado=?, TBAI_mensaje = ? WHERE " +
                                        "VEN_Ticket = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'T' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "'";
                }
                request = new RestRequest("verifactu/v1/invoice_cancel", Method.Post);
                request.AddStringBody(txtFactura.Text, DataFormat.Json);
            }




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
                        System.Data.Odbc.OdbcParameter paramTBAI_Id = new System.Data.Odbc.OdbcParameter();
                        paramTBAI_Id.OdbcType = System.Data.Odbc.OdbcType.Text;
                        paramTBAI_Id.Value = new_factura.chainInfo.number;
                        cmd.Parameters.Add(paramTBAI_Id);
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
                        paramTBAI_Estado.Value = "Factura Cancelada";
                        cmd.Parameters.Add(paramTBAI_Estado);
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
                    MessageBox.Show("Error: " + new_factura.message);
                    WriteLog("Error: " + new_factura.message, path_server, txtTipo.Text, txtFactura.Text, txtTienda.Text, txtYear.Text);
                    cmd.CommandText = sql_updateFactura_KO;
                    System.Data.Odbc.OdbcParameter paramTBAI_Mensaje = new System.Data.Odbc.OdbcParameter();
                    paramTBAI_Mensaje.OdbcType = System.Data.Odbc.OdbcType.Text;
                    paramTBAI_Mensaje.Value = new_factura.message;
                    cmd.Parameters.Add(paramTBAI_Mensaje);
                    Conection.ExecuteCommand(cmd);
                }
            }
            //System.Windows.Forms.Application.Exit();



            return "Eliminado correctamente";
        }

        /*public string deleteState()
        {
            // Definir la parte base del SELECT
            string baseQuery = "SELECT TBAI_Id FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo=";

            // Determinar el FACT_Tipo y la columna de fecha según el tipo de documento
            string factTipo = "";
            string fechaColumna = "";
            string numFacturaColumna = "";

            string texto = txtTipo.Text;

            if (texto.Length > 1)
            {
                factTipo = texto.Substring(1); // Ignora el primer carácter y toma el resto
            }

            if (txtTipo.Text == "SF" || txtTipo.Text == "SFA")
            {
                fechaColumna = "VEN_DataFactura";
                numFacturaColumna = "VEN_NumFactura";
            }
            else if (txtTipo.Text == "ST" || txtTipo.Text == "STA")
            {
                fechaColumna = "VEN_DataTicket";
                numFacturaColumna = "VEN_Ticket";
            }

            // Construcción final de la consulta SQL
            serie = "D";
            sql_checkTbaiCodi = $"{baseQuery}'{factTipo}' AND YEAR({fechaColumna}) = '{txtYear.Text}' " +
                                 $"AND FACT_Tienda = '{txtTienda.Text}' AND {numFacturaColumna} = '{txtFactura.Text}'";


            return "Eliminado correctamente";
        }*/

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

        private void TicketBai_Load(object sender, EventArgs e)
        {

        }
    }
}
