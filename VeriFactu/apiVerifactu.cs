using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VeriFactu.apiVerifactu;

namespace VeriFactu
{
    internal class apiVerifactu
    {
        public class jsonInvoice//datos del emisor de la factura
        {
            public Invoice invoice { get; set; }
        }

        public class Invoice //datos del emisor de la factura
        {
            public Issuer issuer { get; set; } // IDEmisorFactura
            public Recipient recipient { get; set; } //? Destinatarios
            public Id id { get; set; } // IDFactura
            public string type { get; set; } // TipoFactura
            public string replacesTicket { get; set; } //? boolean
            public string replacedTicketIds { get; set; } //? rray<InvoiceId>; FacturasSustituidas
            public CreditNote creditNote { get; set; } //? CreditNoteType; FacturasRectificadas
            public bool simple { get; set; } //? boolean; FacturaSimplificadaArticulos7.2_7.3
            public Description description { get; set; } //? InvoiceDescription; DescripcionOperacion
            public List<VatLine> vatLines { get; set; } //Array<VatLine>; Desglose
            public Decimal amount { get; set; } //CuotaTotal
            public Decimal total { get; set; } //CuotaTotal

        }

        public class Issuer//datos del emisor de la factura
        {
            public string irsId { get; set; }
            public string name { get; set; }
        }

        public class Recipient//destinatarios )(PartnerOther)
        {
            public string irsId {  get; set; }//numero de identidad
            public string id {  get; set; }//numero de identidad
            public string idType {  get; set; }//tipo de documento
            public string name {  get; set; }
            public string country {  get; set; }
            //public string postal { get; set; }
            //public string address { get; set; }

        }

        public class Id// IDFactura (InvoiceIdJson)
        {
            public string number { get; set; }
            public string issuedTime { get; set; }
        }
        
        public class Description //(InvoiceDescriptionJson) revisar valor de Date con david (porque hay dos ejemplos)
        {
            public string text { get; set; }
            public string operationDate { get; set; }
        }
        
        public class VatLine 
        {
            //pendiente vatOperation
            public string vatOperation { get; set; }
            public Decimal @base { get; set; }
            public int rate { get; set; }
            public Decimal amount { get; set; }
            //public Decimal rate2 { get; set; }//DAVID NO USA
            //public Decimal amount2 { get; set; }//DAVID NO USA
            public string vatKey { get; set; }
            public bool isUsingSimplifiedRegime { get; set; }
        }

        //=================FACTURAS RECTIFICATIVAS===============

        public class CreditNote//(CreditNoteType) este es el array que va despues de type que tiene los datos rectificativos
        {
            public string style { get; set; }//EL STYLE ES 'S' O 'I'
            public List<InvoiceId> ids { get; set; }
        }
        
        public class InvoiceId//(InvoiceIdJson) este es el array que va dentro de ids de (CreditNoteType)
        {
            //public string serie { get; set; }//la serie es el número para la factura (lleva seri+tienda+factura_abono_year)
            public string number { get; set; }//número de copias
            public string issuedTime { get; set; }
        }

        //=================RESPUESTA API===============

        public class apiResponse
        {
            public string qrcode { get; set; }
            public chainInfo chainInfo { get; set; }
            public string verifactuXml { get; set; }
            public string verifactuUrl { get; set; }
            public string queueId { get; set; }
            public string message { get; set; }
            public string state { get; set; }
        }

        public class chainInfo
        {
            public string issuerIrsId { get; set; }
            public string issuedTime { get; set; }
            public string number { get; set; }
            public string hash { get; set; }
        }




    }
}
