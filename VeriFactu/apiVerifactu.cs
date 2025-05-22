using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeriFactu
{
    internal class apiVerifactu
    {


        public class Invoice //datos del emisor de la factura
        {
            public string issuer { get; set; } // IDEmisorFactura
            public string recipient { get; set; } //? Destinatarios
            public string id { get; set; } // IDFactura
            public string type { get; set; } // TipoFactura
            public string replacesTicket { get; set; } //? boolean
            public string replacedTicketIds { get; set; } //? rray<InvoiceId>; FacturasSustituidas
            public string creditNote { get; set; } //? CreditNoteType; FacturasRectificadas
            public string simple { get; set; } //? boolean; FacturaSimplificadaArticulos7.2_7.3
            public string description { get; set; } //? InvoiceDescription; DescripcionOperacion
            public string vatLines { get; set; } //Array<VatLine>; Desglose
            public string amount { get; set; } //CuotaTotal
            public string issuedBy { get; set; } //? EmitidaPorTercerosODestinatario

        }

        public class Issuer//datos del emisor de la factura
        {
            public string irsId { get; set; }
            public string name { get; set; }
        }

        public class Recipient//destinatarios )(PartnerOther)
        {
            public string irsId {  get; set; }//numero de identidad
            public string idType {  get; set; }//tipo de documento
            public string name {  get; set; }
            public string country {  get; set; }

        }

        public class Id// IDFactura (InvoiceIdJson)
        {
            public string number { get; set; }
            public string serie { get; set; }
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
            public Decimal @base { get; set; }
            public int rate { get; set; }
            public Decimal amount { get; set; }
            public Decimal rate2 { get; set; }//DAVID NO USA
            public Decimal amount2 { get; set; }//DAVID NO USA
            public string vatOperation { get; set; }//pendiente hacer el tipo VatType que es string y VatExemptReason que también es string de este componente vatOperation
            public string vatKeys { get; set; }
            public bool isUsingSimplifiedRegime { get; set; }
        }

        public class Line
        {
            public string description { get; set; }
            public Int16 quantity { get; set; }
            public Decimal price { get; set; }
            //public Decimal discount { get; set; }
            //public Decimal amount { get; set; }
            public Decimal amountWithVat { get; set; }
            public Int16 vat { get; set; }
            //public Decimal discountAmount { get; set; }

            //public string merchandiseOrService { get; set; }

        }


        //=================FACTURAS RECTIFICATIVAS===============

        public class CreditNote//(CreditNoteType) este es el array que va despues de type que tiene los datos rectificativos
        {
            public string style { get; set; }//EL STYLE ES 'S' O 'I'
            public ArrayList ids { get; set; }
        }
        
        public class InvoiceId//(InvoiceIdJson) este es el array que va dentro de ids de (CreditNoteType)
        {
            public string serie { get; set; }//la serie es el número para la factura (lleva seri+tienda+factura_abono_year)
            public string number { get; set; }//número de copias
            public string issuedTime { get; set; }
        }





    }
}
