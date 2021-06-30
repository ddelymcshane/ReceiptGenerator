using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ReceiptGenerator
{
    class ReceiptItem
    {
        public string item { get; set; }
        public double price { get; set; }

        public Boolean salesExempt { get; set; }
        public int qty { get; set; }
        public double salesTax { get; set; }
        public double importTax { get; set; }
        public double totalPrice { get; set; }
        public Boolean isImport { get; set; }

        public ReceiptItem(string item, int qty, double price, Boolean isImport, Boolean salesExempt)
        {
            this.item = item;
            this.qty = qty;
            this.price = price;
            this.salesExempt = salesExempt;
            this.isImport = isImport;
            importTax = 0;
            CalculateTotalPrice();
        }

        public void CalculateTotalPrice()
        {
            CalculateSalesTax();
            CalculateImportTax();
            totalPrice = (qty * (price + importTax + salesTax));
            salesTax *= qty;
            salesTax = RoundToNickel(salesTax);
        }

        public double RoundToNickel(double val)
        {
            if (val % .05 != 0)
            {
                val += (.05 - Math.Round(val % .05, 2));

            }
            return val;
        }
        private void CalculateSalesTax()
        {
            if (!salesExempt)
            {
                salesTax = price * .1;
                salesTax = Utility.GetValueToTwoDecimalsAsDouble(salesTax);
                if (salesTax % .05 != 0)
                {
                    salesTax *= qty;
                    salesTax += (.05 - Math.Round(salesTax % .05, 2));  
                }
            }
            else
            {
                salesTax = 0;
            }
        }

        private void CalculateImportTax()
        {
            if (isImport)
            {
                importTax = RoundToNickel(price * .05);
                importTax = Utility.GetValueToTwoDecimalsAsDouble(importTax);
            }
            else
            {
                importTax = 0;
            }
        }

        public string GenerateOutput()
        {
            if (importTax != 0)
            {
                item = " Imported" + item;
            }
            if (qty > 1)
            {
                return item + ": " + Utility.GetValueToTwoDecimals(totalPrice) + "( " + qty.ToString() + " @ " + price.ToString() + " )";
            }
            else
            {
                return item + ": " + Utility.GetValueToTwoDecimals(totalPrice);
            }
        }

    }
}
