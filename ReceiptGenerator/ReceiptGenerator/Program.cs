using System;

namespace ReceiptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            ReceiptGenerator rg = new ReceiptGenerator();

            string val = "";
            while (val != "done")
            {
                val = Console.ReadLine();
                if(val != "done")
                {
                    rg.ParseLineToReceiptValues(val);
                }
                
            }


            rg.GenerateLineOutput();
            
        }
    }
}
