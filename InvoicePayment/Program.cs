using System;
using System.IO;
using InvoicePayment;

namespace InvoicePayment
{
    class Program
    {
        static void Main(string[] args)
        {
            InvoicePayment invoicePayment = new InvoicePayment(5555);
            InvoicePayment.SeriAndDesri = false;
            InvoicePayment.SAD(invoicePayment);
        }
    }
}
