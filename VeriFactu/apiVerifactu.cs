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

        public class PartnerOther
        {
            public string irsId {  get; set; }//numero de identidad
            public string idType {  get; set; }//tipo de documento
            public string name {  get; set; }
            public string country {  get; set; }
        }

        public class Recipient
        {
            public string irsId { get; set; }
            public string name { get; set; }
            public string country { get; set; }
        }

        public class InvoiceIdJson
        {
            public string number { get; set; }
            public string issuedTime { get; set; }
        }
        
        public class InvoiceDescriptionJson //revisar valor de Date con david (porque hay dos ejemplos)
        {
            public string text { get; set; }
            public string operationDate { get; set; }
        }
        
        public class VatLine 
        {
            //pendiente vatOperation
            public int @base { get; set; }
            public int rate { get; set; }
            public int amount { get; set; }
            public int rate2 { get; set; }
            public int amount2 { get; set; }
            //pendiente hacer el tipo VatType que es string y VatExemptReason que también es string de este componente vatOperation
            public string vatOperation { get; set; }
            //pendiente vatKey de tipo vatKey
            public bool isUsingSimplifiedRegime { get; set; }
        }

        //=================FACTURAS RECTIFICATIVAS===============

        public class CreditNoteType
        {
            public string style { get; set; }//EL STYLE ES 'S' O 'I'
            public ArrayList ids { get; set; }
            public string reason { get; set; }
        }

    }
}
