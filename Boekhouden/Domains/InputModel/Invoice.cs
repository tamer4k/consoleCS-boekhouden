﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Boekhouden
{
    public class Invoice
    {
        public int ID { get; set; }
        public List<TransactionRow> TransactionRows { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string TableNumber { get; set; }
        public double SubTotal { get; set; }
        public CustomerDiscount CustomerDiscount { get; set; }
        public double Total { get; set; }
        public DateTime DateCreated { get; set; }   
    }
}
