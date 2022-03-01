using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Boekhouden
{
    internal class TransactionRow
    {
        public int ID { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
        public double TransactionRowDiscount { get; set; }
        public int VatType { get; set; }
        public double VatAmount { get; set; }

    }
}
