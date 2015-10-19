using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabow.Steg.Core
{
    public class Helpers
    {
        public static string SmallDecimal(string input, int numberOfDecimals)
        {
            int i;
            for (i = input.Length - 1; i > 0; i--)
                if (input[i] == '.')
                    break;
            try
            {
                return input.Substring(0, i + numberOfDecimals + 1);
            }
            catch
            {
                return input;
            }
        }

        public static void ByteToBool(byte input, ref bool[] output)
        {
            if (input >= 0 && input <= 255)
                for (short i = 7; i >= 0; i--)
                {
                    if (input % 2 == 1)
                        output[i] = true;
                    else
                        output[i] = false;
                    input /= 2;
                }
            else
                throw new Exception("Input number is illegal.");
        }

        public static byte BoolToByte(bool[] input)
        {
            byte output = 0;
            for (short i = 7; i >= 0; i--)
            {
                if (input[i])
                    output += (byte)Math.Pow(2.0, (double)(7 - i));
            }
            return output;
        }
    }
}
