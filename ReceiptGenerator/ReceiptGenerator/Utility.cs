using System;
using System.Collections.Generic;
using System.Text;

namespace ReceiptGenerator
{
    class Utility
    {
        public static string GetValueToTwoDecimals(double val)
        {
            return (Math.Truncate(100 * val) / 100).ToString();
        }

        public static double GetValueToTwoDecimalsAsDouble(double val)
        {
            return Math.Truncate(100 * val) / 100;
        }
    }
}
