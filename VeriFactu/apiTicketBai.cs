using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VeriFactu.apiVerifactu;

namespace VeriFactu
{
    internal class apiTicketBai
    {

        public class Factura
        {
            public string qrcode { get; set; }
            public chainInfo chainInfo { get; set; }
            public string tbaiXml { get; set; }
            public string tbaiId { get; set; }
            public string tbaiUrl { get; set; }
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
            public string serie { get; set; }
        }

        public class FacturaState
        {
            public string state { get; set; }
        }
        public class FacturaDelete
        {
            public string tbaiXml { get; set; }
            public string tbaiId { get; set; }
            public string queueId { get; set; }

        }

        public class Invoice_id
        {
            public string number { get; set; }
            public string serie { get; set; }
            public string issuedTime { get; set; }

        }
        public class Description
        {
            public string text { get; set; }
            public string operationDate { get; set; }

        }
        public class CreditNote
        {
            public string reason { get; set; }
            public string style { get; set; }
            public List<CreditNote_ids> ids { get; set; }
        }
        public class CreditNote_ids
        {
            public string number { get; set; }
            public string serie { get; set; }
            public string issuedTime { get; set; }
        }//id
        public class Recipient
        {
            public string irsId { get; set; }
            public string id { get; set; }//ojo extra
            public string idType { get; set; }//ojo extra
            public string name { get; set; }
            public string country { get; set; }
            public string postal { get; set; }
            public string address { get; set; }
            public bool vat2 { get; set; }
        }//recipient

        public class Invoice
        {
            public Issuer issuer { get; set; }
            public Invoice_id id;
            public CreditNote creditNote;
            public Description description;
            public bool simple { get; set; }
            public Recipient recipient;
            public List<Line> lines { get; set; }
            public List<VatLine> vatLines { get; set; }
            public Decimal total { get; set; }

            public string[] vatKeys = new string[1];
        }

        public class Issuer//datos del emisor de la factura
        {
            public string irsId { get; set; }
            public string name { get; set; }
        }

        public class Lines
        {
            public ArrayList lines { get; set; }
        }
        public class VatLines
        {
            public string vatLines { get; set; }
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

            public string merchandiseOrService { get; set; }

        }//lines
        public class VatLine
        {
            public Int16 rate { get; set; }
            public Decimal amount { get; set; }
            public Decimal rate2 { get; set; }
            public Decimal amount2 { get; set; }
            public Decimal @base { get; set; }
            public string exemptionReason { get; set; }
            public bool isUsingSimplifiedRegime { get; set; }
            public string merchandiseOrService { get; set; }

        }//vatLines

        public class Json_Oddo
        {
            public Invoice invoice;
        }

    }
}
