using System.Data;

namespace VeriFactu
{
    public partial class Inicio : Form
    {
        //Conection conection;
        apiVerifactu.issuer apiIssuer;
        apiVerifactu.PartnerOther apiPartnerOther;
        apiVerifactu.InvoiceIdJson apiInvoiceIdJson;
        apiVerifactu.InvoiceDescriptionJson apiInvoiceDescriptionJson;
        apiVerifactu.VatLine apiVatLine;
        apiVerifactu.CreditNoteType apiCreditNoteType;
        public Inicio()
        {
            InitializeComponent();
            apiIssuer = new apiVerifactu.issuer();
            apiPartnerOther = new apiVerifactu.PartnerOther();
            apiInvoiceIdJson = new apiVerifactu.InvoiceIdJson();
            apiInvoiceDescriptionJson = new apiVerifactu.InvoiceDescriptionJson();
            apiVatLine = new apiVerifactu.VatLine();
            apiCreditNoteType = new apiVerifactu.CreditNoteType();
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
        static string recargo_equivalencia_empresa = System.String.Empty;
        static string recargo_equivalencia_cliente = System.String.Empty;
        static string TicketBai_DB = System.String.Empty;
        static string TicketBai_ApiKey = System.String.Empty;
        static string ImportTotalString = System.String.Empty;
        static Decimal ImporTotalCheck = 0;
        static string serie = System.String.Empty;
        static string descripcion = System.String.Empty;
        static string rectificactiva = "N";
        static string[] subs;
        static string id_vendafact = System.String.Empty;
        static DataTable ven_abono = new DataTable();
        static DataTable vendaTable = new DataTable();
        DateTime date = new DateTime();

        private void Inicio_Load(object sender, EventArgs e)
        {
            //conection.openConection();

            //sql_getListaParametros = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_Key = 'TicketBai_DB'";
            //TicketBai_DB = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_DB'
            //if (string.IsNullOrEmpty(TicketBai_DB))
            //{
            //    sql_getListaParametros = "SELECT PAR_Valor FROM LISTA_PARAMETROS WHERE PAR_Key = 'TicketBai_DB'";
            //    TicketBai_DB = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_DB'
            //    sql_getListaParametros = "SELECT PAR_Valor FROM LISTA_PARAMETROS WHERE PAR_Key = 'TicketBai_ApiKey'";
            //    TicketBai_ApiKey = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_ApiKey'
            //}
            //else
            //{
            //    sql_getListaParametros = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_Key = 'TicketBai_ApiKey'";
            //    TicketBai_ApiKey = Conection.SelectOnce(sql_getListaParametros); //consulta para optiner 'TicketBai_ApiKey'
            //}

            //sql_recargoEquivalencia = "SELECT PAR_Valor FROM TIENDADATOS WHERE TIE_Codigo = " + txtTienda.Text + " AND PAR_key = 'RecargodeEquivalencia'";
            //recargo_equivalencia_empresa = Conection.SelectOnce(sql_recargoEquivalencia); //consulta para optiner 'recargo_equivalencia_empresa'

            //if (recargo_equivalencia_empresa == null || recargo_equivalencia_empresa == "")
            //{
            //    sql_recargoEquivalencia = "SELECT PAR_Valor FROM LISTA_PARAMETROS WHERE PAR_key = 'RecargodeEquivalencia'";
            //    recargo_equivalencia_empresa = Conection.SelectOnce(sql_recargoEquivalencia); //consulta para optiner 'recargo_equivalencia_empresa'
            //}

            //recargo_equivalencia_cliente = Conection.SelectOnce(sql_recargoEquivalenciaCliente); //consulta para optiner 'recargo_equivalencia_empresa'


            //===========CONDICIÓN SEGÚN EL TIPO DE FACTURA===========

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
                respuesta = this.deleteState();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtTipo.Text[0] == 'S')//cogemos la pprimera letra para identificar que tiene 'S' y así saber que es state, una vez hecho eso en la función identifico el tipo de state
            {
                respuesta = this.deleteState();
                MessageBox.Show("OK: " + respuesta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            serie = "F";
            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                        "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                        "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                        "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                        "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                        "AND v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                        "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                                        "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) " +
                                        "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";

            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='F' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "'";

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='F' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='F' AND VEN_NumFactura = '" + txtFactura.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getVenFactAbono = "SELECT VEN_ImportBrut FROM VENDA WHERE VEN_NumFactura = '" + txtFactura.Text + "' " +
                                       "AND VEN_Tienda = '" + txtTienda.Text + "' " +
                                       "AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' " +
                                       "AND VEN_Referencia = 'ABONO'";
            //id_vendafact = SelectOnce(connectionString, sql_getVenFactAbono); consulta para traer el valor de 'sql_getVenFactAbono'

            subs = id_vendafact.Split('.');//tratando el resultado de 'id_vendafact'
            //sql_getFechaVentaAbono = "SELECT TBAI_Codi, V.VEN_DataFactura, V.VEN_NumFactura FROM VENDA V " +
            //                   "INNER JOIN FACTURAS_HACIENDA_AUTO FHA ON (FHA.VEN_NumFactura = V.VEN_NumFactura AND V.VEN_DataFactura = FHA.VEN_DataFactura) WHERE " +
            //                   "V.VEN_Codi = '" + subs[0] + "' " +
            //                   "AND V.VEN_Tienda = '" + subs[1].Substring(0, 2) + "'";
            //ven_abono = SelectRows(connectionString, sql_getFechaVentaAbono); consulta para conseguir el abono de la venta

            //if (ven_abono == null || ven_abono.Rows.Count == 0)
            //{
            //    MessageBox.Show("No puede facturar un Abono de una Venta no facturada (" + subs[0] + "/" + subs[1].Substring(0, 2) + ")");
            //    System.Windows.Forms.Application.Exit();
            //}

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_mensaje = ? WHERE " +
                                        "VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'F' " +
                                        "AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "'";
            sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE " +
                                        "VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'F' " +
                                        "AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "'";

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

            ImportTotalString = Conection.SelectOnce(sql_getVenImportTotal).Replace(".", ",");

            

            ImporTotalCheck = Convert.ToDecimal(ImportTotalString);

            vendaTable = Conection.SelectRows(sql_getVentas);

            //MessageBox.Show("OK: " + vendaTable, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);


            //================DATOS DEL CLIENTE================

            if (vendaTable.Rows[0].Field<string>("TipoDoc").ToString() != null)
            {
                if (vendaTable.Rows[0].Field<string>("TipoDoc").ToString() == "02") //IrsIdType
                {

                }
                else
                {
                    //apiPartnerOther.irsId = vendaTable.Rows[0].Field<string>("TipoDoc").ToString(); //IrsIdType
                    apiPartnerOther.irsId = vendaTable.Rows[0].Field<string>("TipoDoc");
                }

            }
            else
            {
                MessageBox.Show("Error: Falta el País del Cliente");
                System.Windows.Forms.Application.Exit();
            }



            return "Factura creada correctamente";
        }

        public string facturaAbono()
        {
            serie = "FA";
            rectificactiva = "S";
            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                       "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                       "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                       "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                       "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                       "AND v.VEN_NumFactura = '" + txtFactura.Text + "' " +
                                       "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                       "AND YEAR(v.VEN_DataFactura) = '" + txtYear.Text + "' " +
                                       "AND v.VEN_Referencia = 'ABONO' " +
                                       "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";

            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='FA' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_NumFactura = '" + txtFactura.Text + "'";

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='FA' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='FA' AND VEN_NumFactura = '" + txtFactura.Text + "' AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_mensaje = ? WHERE " +
                                        "VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'FA' " +
                                        "AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "'";

            sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE " +
                                        "VEN_NumFactura = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'FA' " +
                                        "AND YEAR(VEN_DataFactura) = '" + txtYear.Text + "'";

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

            return "Factura de abono creada correctamente";
        }

        public string ticket()
        {
            serie = "T";
            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                      "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                      "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                      "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                      "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                      "AND v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                      "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                      "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                      "AND (v.VEN_Referencia <> 'ABONO' OR v.VEN_Referencia is null) " +
                                      "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";

            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='T' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_Ticket = '" + txtFactura.Text + "'";

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='T' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='T' AND VEN_Ticket = '" + txtFactura.Text + "' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_mensaje = ? WHERE " +
                                        "VEN_Ticket = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'T' " +
                                        "AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "'";

            sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE " +
                                        "VEN_Ticket = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'T' " +
                                        "AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "'";

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

            return "Ticket creado correctamente";
        }

        public string ticketAbono()
        {
            serie = "TA";
            rectificactiva = "S";
            sql_getVentas = "select v.VEN_ImportTotal, v.VEN_Codi, v.VEN_Tienda, lv.LVN_Codi, c.CLI_Dni, '02' AS Tipo, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_Nom + ' ' + c.CLI_Cognom1 + ' ' + c.CLI_Cognom2 as Nom, c.CLI_DefAuditiu AS Pais, ('0' + c.CLI_EstatCivil) as TipoDoc, c.CLI_CodiPostal," +
                                      "(case when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Puerta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera when C.CLI_NumEdif is not null and C.CLI_Piso is not null and C.CLI_Puerta is not null and C.CLI_Escalera is not null then C.CLI_Domicili + ' Nº ' + C.CLI_NumEdif + ', Piso ' + C.CLI_Piso + ', Pta ' + C.CLI_Puerta + ', Esc. ' + C.CLI_Escalera end) AS Domicili, " +
                                      "lv.PRD_Item, cast(sum((LV.LVN_SUBTOTAL - (LV.LVN_SUBTOTAL * V.VEN_Descompte) / 100)) as numeric (10, 2)) as LVN_PreuDtoLineaVenta,LV.LVN_SUBTOTAL as LVN_PreuDtoLinea, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA " +
                                      "from CLIENT c inner join VENDA v on c.TIE_Codigo = v.VEN_CliFactTie and c.CLI_Codi = v.VEN_CliFact " +
                                      "inner join LIN_VENDA lv on v.VEN_Tienda = lv.VEN_Tienda and v.VEN_Codi = lv.VEN_Codi " +
                                      "AND v.VEN_Ticket = '" + txtFactura.Text + "' " +
                                      "AND v.VEN_Tienda = '" + txtTienda.Text + "' " +
                                      "AND YEAR(v.VEN_DataTicket) = '" + txtYear.Text + "' " +
                                      "AND v.VEN_Referencia = 'ABONO' " +
                                      "GROUP BY v.VEN_ImportTotal,v.VEN_Codi,v.VEN_Tienda,lv.LVN_Codi,LV.LVN_SUBTOTAL,c.CLI_Dni, c.CLI_Nom,c.CLI_Cognom1,c.CLI_Cognom2, c.CLI_DefAuditiu, c.CLI_CodiPostal,C.CLI_NumEdif, C.CLI_Piso,C.CLI_Puerta,C.CLI_Escalera, C.CLI_Domicili, lv.PRD_Item, lv.LVN_PreuUnitat , lv.LVN_Uds, lv.LVN_Desconte, lv.LVN_IVA, v.VEN_Data, v.VEN_Hora, VEN_DataFactura, Ven_DataTicket, c.CLI_DefAuditiu, c.CLI_EstatCivil ORDER BY LVN_Codi";

            sql_checkTbaiCodi = "SELECT TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='TA' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "' AND VEN_Ticket = '" + txtFactura.Text + "'";

            sql_getTbaiCodi = "SELECT MAX(TBAI_Codi) + 1 as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='TA' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_getTbaiCodiReenvio = "SELECT TBAI_Codi as TBAI_Codi FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo='TA' AND VEN_Ticket = '" + txtFactura.Text + "' AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "' AND FACT_Tienda = '" + txtTienda.Text + "'";

            sql_updateFactura = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Codi = ?, TBAI_Id = ?, TBAI_Qr = ?, TBAI_Url = ?, TBAI_Queue = ?, TBAI_Xml = ?, TBAI_ImgQr = ?, TBAI_Data = ?, TBAI_mensaje = ? WHERE " +
                                       "VEN_Ticket = '" + txtFactura.Text + "' " +
                                       "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                       "AND FACT_Tipo = 'TA' " +
                                       "AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "'";

            sql_updateFactura_KO = "UPDATE FACTURAS_HACIENDA_AUTO SET TBAI_Mensaje = ? WHERE " +
                                        "VEN_Ticket = '" + txtFactura.Text + "' " +
                                        "AND FACT_Tienda = '" + txtTienda.Text + "' " +
                                        "AND FACT_Tipo = 'TA' " +
                                        "AND YEAR(VEN_DataTicket) = '" + txtYear.Text + "'";

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

            return "Ticket abono creado correctamente";
        }

        public string deleteState()
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
        }

        //public string state()
        //{

        //    string baseQuery = "SELECT TBAI_Id FROM FACTURAS_HACIENDA_AUTO WHERE FACT_Tipo=";

        //    string factTipo = "";
        //    string fechaColumna = "";
        //    string numFacturaColumna = "";

        //    string texto = txtTipo.Text;

        //    if (texto.Length > 1)
        //    {
        //        factTipo = texto.Substring(1); // Ignora el primer carácter y toma el resto
        //    }

        //    if (txtTipo.Text == "SF" || txtTipo.Text == "SFA")
        //    {
        //        fechaColumna = "VEN_DataFactura";
        //        numFacturaColumna = "VEN_NumFactura";
        //    }
        //    else if (txtTipo.Text == "ST" || txtTipo.Text == "STA")
        //    {
        //        fechaColumna = "VEN_DataTicket";
        //        numFacturaColumna = "VEN_Ticket";
        //    }

        //    // Construcción final de la consulta SQL
        //    serie = "D";
        //    sql_checkTbaiCodi = $"{baseQuery}'{factTipo}' AND YEAR({fechaColumna}) = '{txtYear.Text}' " +
        //                         $"AND FACT_Tienda = '{txtTienda.Text}' AND {numFacturaColumna} = '{txtFactura.Text}'";

        //    return "pendiente de pasar el state";
        //}


        public void getVEN_Tipo(string value)
        {
            txtTipo.Text = value;
        }

        public void getVEN_Numfactura(string value)
        {
            txtFactura.Text = value;
        }

        public void getVEN_Tienda(string value)
        {
            txtTienda.Text = value;
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
