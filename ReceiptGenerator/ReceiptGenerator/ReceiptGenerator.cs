using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ReceiptGenerator
{
    
    class ReceiptGenerator
    {
        readonly string[] defaultExemptList = { "BOOK", "BOX OF CHOCOLATES", "PACKET OF HEADACHE PILLS", "CHOCOLATE BAR" };
        List<ReceiptItem> Receipts;
        List<string> SalesExemptList;


        public ReceiptGenerator(List<string> SalesExemptList = null) //cli input method. Could supply exempt list 
        {
            Receipts = new List<ReceiptItem>();
            if(SalesExemptList == null)
            {
                this.SalesExemptList = new List<string>(defaultExemptList);
            }
            else
            {
                this.SalesExemptList = SalesExemptList;
            }
        }

        public void ParseLineToReceiptValues(string line)
        {
            Boolean isImported = line.ToUpper().Contains("IMPORTED");
            int qty = Int32.Parse(ParseToChar(ref line, ' ')); //parse qty from line and remove
            string item = line.Substring(0, line.IndexOf(" at ")); //isolate item name from line
            line = line.Trim();
            line = line.Substring(line.IndexOf(" at ") + 4); //remove remaining 'at' from line to isolate price
            double price = Double.Parse(line);
            if (isImported)
            {
                item = RemoveImportedFromItem(item).Trim(); //remove import from item name. this allows us to check if item is in exempt list
            }

            if (ItemAlreadyExists(item, price)) //if we already have an identical item in our receipt list, increase qty
            {
                AddToExistingReceiptItem(item, qty);
            }
            else
            {
                try
                {
                    Receipts.Add(new ReceiptItem(item, qty, price, isImported, SalesExemptList.Contains(item.ToUpper())));
                }catch(ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid format entered. Item ignored.");
                }
            }
        }

        private string RemoveImportedFromItem(string item)
        {
            return Regex.Replace(item, "imported", "", RegexOptions.IgnoreCase);
        }

        public Boolean ItemAlreadyExists(string itemName, double itemPrice)
        {
            foreach(ReceiptItem item in Receipts)
            {
                if ((itemName.ToUpper() == item.item.ToUpper()) && (itemPrice == item.price))
                {
                    return true;
                }
            }

            return false;
        }

        public Boolean AddToExistingReceiptItem(string itemName, int qty)
        {
            foreach (ReceiptItem item in Receipts)
            {
                if (itemName.ToUpper() == item.item.ToUpper())
                {
                    item.qty += qty;
                    item.CalculateTotalPrice();
                    return true;
                }
            }

            return false;
        }

        public void GenerateLineOutput()
        {
            double salesTaxTotal = 0;
            double receiptTotal = 0;
            Console.WriteLine("");
            foreach(ReceiptItem item in Receipts)
            {
                Console.WriteLine(item.GenerateOutput());
                salesTaxTotal += item.salesTax + item.importTax;
                receiptTotal += item.totalPrice;
            }

            Console.WriteLine("Sales Taxes: " + Utility.GetValueToTwoDecimals(salesTaxTotal));
            Console.WriteLine("Total: " + Utility.GetValueToTwoDecimals(receiptTotal));
        }

        public static string ParseToChar(ref string str, char val) 
        {
            string outString;
            int index = str.IndexOf(val);
            if (index == -1)
            {
                outString = str;
                str = "";
                return outString;
            }
            outString = str.Substring(0, index);
            str = str.Remove(0, index + 1);
            return outString;
        }
    }
}
