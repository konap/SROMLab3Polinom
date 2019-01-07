using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SROMLab3
{

    public class Calculate
    {
        public static string PolGenerator (int highdem, int dem1, int dem2, int dem3) //359, 18, 4, 2
            {
            string str = "0";
            str = str.Insert(1, new string('0', highdem));
            StringBuilder GenPolinom = new StringBuilder(str);
            GenPolinom[0] = '1';
            GenPolinom[highdem - dem1] = '1';
            GenPolinom[highdem - dem2] = '1';
            GenPolinom[highdem - dem3] = '1';
            GenPolinom[highdem] = '1';

            string answer = GenPolinom.ToString();

            return answer;


        }
    public static string FindZero(string polinom)
        {
            int powerp_olynomial = polinom.Length;
            string str = "0";
            str = str.Insert(1, new string('0', powerp_olynomial));
            return str;
        }

        public static string FindOne(string polinom)

        {

            int powerp_olynomial = polinom.Length;
            string str = "";
            str = str.Insert(0, new string('0', powerp_olynomial));
            str = str.Insert(powerp_olynomial, new string('1', 1));
            return str;
        }

        public static ulong[] BitSting_ToByte(string numb)
        {

            var lenth = numb.Length;
            ulong[] number = new ulong[lenth];

            for (var i = 0; i < lenth; i++)
            {
                number[i] = Convert.ToByte(numb.Substring(i, 1), 2);
            }
            return number;

        }

        public static ulong[] HexStringToBinary(string hexstring)
        {
            string binarystring = String.Join(String.Empty, hexstring.Select(
            c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            return BitSting_ToByte(binarystring);
        }

        public static string Bite_to_String(ulong[] numb)
        {

            string answer = "";
            for (int i = 0; i < numb.Length; i++)
            {
                answer = answer + Convert.ToString(numb[i]);
            }
            return answer;

        }


        public static ulong[] Add(ulong[] a, ulong[] b)
        {
            ulong[] C = new ulong[a.Length];
            for (int i = a.Length-1; i > -1; i--)
            {
                C[i] = a[i] ^ b[i];
            }
        
            return C;
        }

        public static ulong[] Mul(ulong[] a, ulong[] b)
        {
            ulong[] C = new ulong[a.Length];
            for (int i = a.Length - 1; i > -1; i--)
            {
                C[i] = a[i] ^ b[i];
            }

            return C;
        }
      

        static void Main(string[] args)
        {
            ulong[] a = BitSting_ToByte("11100000101100001011110110111011101111110101000110110001111001001001010110100101110010100010110111010110101101010010111111100111010110011001011101001111100110011000100101110011110000010110110000011111101110000101111010011001001101000111010100000101010111001010001110110001000010110101101011011101111000111100101101010011011111110100110010000100000101100000001");
            ulong[] b = BitSting_ToByte("01000001010101001001111110111000001001110011100001011000111101011101101000001100110001000101101101101000101001100100111100111001000110100100101100000111000110101001111111100001000101000100011100100110111110000010001011001101111011001111110100110111001111010001100000101110001000110101001001001101100010010000010001010111001111100110001101100001100100110100011");
            ulong[] c = Add(a, b);
            Console.WriteLine(PolGenerator(179,4,2,1));
            


            Console.WriteLine(Bite_to_String(c));
           
            Console.ReadKey();
        }
    }
}
 
        
    
