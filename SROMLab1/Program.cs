using System;


namespace SROMLab1
{

    public class Calculate
    {
        public static ulong[] Converting(string a)
        {
            string temp = a;
            while (temp.Length % 8 != 0)
            {
                temp = "0" + temp;
            }
            var cheker = temp.Length;
            var dm = cheker / 8;
            var arr = new ulong[temp.Length / 8];
            for (int i = 0; i < temp.Length; i += 8)
            {
                arr[i / 8] = Convert.ToUInt32(temp.Substring(i, 8), 16);
            }
            Array.Reverse(arr);
            return arr;
        }

        public static string ReConv(ulong[] a)
        {


            string g, G = "";
            for (int i = 0; i < a.Length; i++)
            {
                g = a[i].ToString("X");
                if (g.Length < 8)
                    do g = "0" + g;
                    while (g.Length < 8);
                G = g + G;
            }
            G = G.TrimStart('0');
            if (G == "")
                G = "0";
            return (G);
        }

        public static ulong[] LongAddInternal(ulong[] a, ulong[] b)
        {
            var lenght = Math.Max(a.Length, b.Length);
            ulong[] C = new ulong[lenght + 1];

            ulong carry = 0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                ulong temp = a[i] + b[i] + carry;
                C[i] = temp & 0xffffffff;
                carry = temp >> 32;
            }
            C[a.Length] = carry;

            if (a.Length > b.Length)
                for (int i = Math.Min(a.Length, b.Length); i < a.Length; i++)
                    C[i] = a[i];
            else
                for (int i = Math.Min(a.Length, b.Length); i < b.Length; i++)
                    C[i] = b[i];

            return C;
        }

        public static int FindZeroIndx(ulong[] a)
        {
            var j = 0;

            int i = a.Length - 1;
            while ((a[i] == 0))
            {
                i--;
                
                j++;
                if (i == -1)
                {
                    break;
                }
               
            }
            return (j );
        }

        public static int LongCmp(ulong[] a, ulong[] b)
         {


            var realLenghA = a.Length - FindZeroIndx(a);
            var realLenghB = b.Length - FindZeroIndx(b);
            if (realLenghA < realLenghB) return -1;
            if (realLenghA > realLenghB) return 1;

             var start = Math.Max(realLenghA, realLenghB);


             for (int i = start-1; i > -1; i--)
             {
                 if (a[i] < b[i]) return -1;
                 if (a[i] > b[i]) return 1;
             }
             return 0;
        }
    
            

        public static ulong[] LongSub(ulong[] a, ulong[] b)

        {
            var lenght = Math.Max(a.Length, b.Length);
           // Array.Resize(ref a, lenght);
           // Array.Resize(ref b, lenght);
            ulong[] C = new ulong[a.Length]; //ulong ?
            ulong temp, borrow = 0;
            ulong Zero = 0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                temp = a[i] - b[i] - borrow;
                C[i] = (temp & 0xFFFFFFFF);
                borrow = temp <= a[i] ? Zero : 1;
            }
            if (a.Length > b.Length)
                for (int i = Math.Min(a.Length, b.Length); i < a.Length; i++)
                    C[i] = a[i];
           /* else
                for (int i = Math.Min(a.Length, b.Length); i < b.Length; i++)
                    C[i] = b[i];
*/
            return C;
        }

        public static ulong[] LongMulOneDigit(ulong[] a, ulong b)
        {
            ulong temp, carry = 0;
            ulong[] c = new ulong[a.Length + 1];
            for (int i = 0; i < a.Length; i++)
            {
                temp = a[i] * b + carry;
                carry = temp >> 32;
                c[i] = temp & 0xffffffff;
            }
            c[a.Length] = carry;
            return c;
        }

        public static ulong[] LongShiftDigitsToHigh(ulong[] a, int ind)
        {
            ulong[] c = new ulong[a.Length + ind];
            for (int i = 0; i < a.Length; i++)
            {
                c[i + ind] = a[i];
            }
            return c;
        }

        public static ulong[] MulUlong(ulong[] a, ulong[] b)
        {
            var maxlenght = Math.Max(a.Length, b.Length);
            Array.Resize(ref a, maxlenght);
            Array.Resize(ref b, maxlenght);
            ulong[] answer = new ulong[(a.Length) * 2];
            ulong[] temp;
            for (int i = 0; i < a.Length; i++)
            {
                temp = LongMulOneDigit(a, b[i]);
                temp = LongShiftDigitsToHigh(temp, i);
                answer = LongAddInternal(answer, temp);
            }
            answer = RemoveHighZeros(answer); //Trim ?
            return answer;
        }

        static ulong[] RemoveHighZeros(ulong[] c)
        {
            int i = c.Length - 1;
            while (c[i] == 0)
            {
                i--;
                if (i == -1)
                {
                    break;
                }
            }
            ulong[] result = new ulong[i + 1];
            Array.Copy(c, result, i + 1);
            return result;
        } 



        public static int BitLength(ulong[] a)
        {
            int bit = 0;
            int i = a.Length - 1;
            while (a[i] == 0)
            {
                if (i < 0)
                    return 0;
                i--;
            }

            var ai = a[i];

            while (ai > 0)
            {
                bit++;
                ai = ai >> 1;
            }
            bit = bit + 32 * i;
            return bit;
        }

        public static ulong[] LongShiftBitsToHigh(ulong[] a, int b)
        {
            int t = b / 32;
            int s = b - t * 32;
            ulong n, carry = 0;
            ulong[] C = new ulong[a.Length + t + 1];
            for (int i = 0; i < a.Length; i++)
            {
                n = a[i];
                n = n << s;
                C[i + t] = (n & 0xFFFFFFFF) + carry;
                carry = (n & 0xFFFFFFFF00000000) >> 32;
            }
            C[C.Length - 1] = carry;
            return C;
        }

        public static ulong[] LongShiftBitsToLow(ulong[] a, int b)
        {
            int t = b / 32;
            int s = b - t * 32;
            ulong n, nn, carry = 0;
            ulong[] C = new ulong[a.Length - t];
            for (int i = t; i < a.Length - 1; i++)
            {
                n = a[i];
                nn = a[i + 1];
                n = n >> s;
                nn = nn << (64 - s);
                nn = nn >> (64 - s);
                carry = nn;
                C[i - t] = n | (carry << 32 - s);
            }
            C[a.Length - 1 - t] = a[a.Length - 1] >> s;
            return C;
        }



        public static ulong[] LongDiv(ulong[] a, ulong[] b)
        {
            var k = BitLength(b);
            var R = a;
            ulong[] Q = new ulong[a.Length];
            ulong[] T = new ulong[a.Length];
            ulong[] C = new ulong[a.Length];
            T[0] = 0x1;

            while (LongCmp(R, b) >= 0)
            {
                var t = BitLength(R);
                C = LongShiftBitsToHigh(b, t - k);
                if (LongCmp(R, C) == -1)
                {
                    t = t - 1;
                    C = LongShiftBitsToHigh(b, t - k);
                }
                R = LongSub(R, C);
                Q = LongAddInternal(Q, LongShiftBitsToHigh(T, t - k));
            }
            Q = RemoveHighZeros(Q);
            return Q;
        }

        public static ulong[] LongPower(ulong[] a, ulong[] b)
        {
            string Pow_b = ReConv(b);
            ulong[] C = new ulong[1];
            C[0] = 0x1;
            ulong[][] D = new ulong[16][];
            D[0] = new ulong[1] { 1 };
            D[1] = a;
            for (int i = 2; i < 16; i++)
            {
                D[i] = MulUlong(D[i - 1], a);
                D[i] = RemoveHighZeros(D[i]);
            }

            for (int i = 0; i < Pow_b.Length; i++)
            {
                C = MulUlong(C, D[Convert.ToInt32(Pow_b[i].ToString(), 16)]);
                if (i != Pow_b.Length - 1)
                {
                    for (int k = 1; k <= 4; k++)
                    {
                        C = MulUlong(C, C);
                        C = RemoveHighZeros(C);
                    }
                }
            }
            return C;
        }

        public static ulong[] BinaryGCD(ulong[] a, ulong[] b)
        {
            ulong[] d = new ulong[Math.Min(a.Length, b.Length)];
            d[0] = 0x1;
            
            /* GCD(0, b) == b; GCD(a, 0) == a, 
               GCD(0, 0) == 0 */
            string As = ReConv(a);
            string Bs = ReConv(b);
            if (As == "0")
                return b;
            if (Bs == "0")
                return a;


            while (((a[0] & 1) == 0) & ((b[0] & 1) == 0))
            {
                a = LongDiv(a, Converting("2"));
                b = LongDiv(b, Converting("2"));
                d = MulUlong(d, Converting("2"));
            }

            while ((a[0] & 1) == 0)
            {
                a = LongDiv(a, Converting("2"));
            }

            while (b[0] != 0)
            {
                while ((b[0] & 1) == 0)
                {
                    b = LongDiv(b, Converting("2"));
                    if (LongCmp(a, b) == 1)
                        a = b;
                }
                b = LongSub(a, b);

            }
            d = MulUlong(d, a);
            return d;
        }

        public static ulong[] usearch(ulong[] b)
        {
            ulong[] c = new ulong[] { 0x01 };

            return (LongDiv(LongShiftBitsToHigh(c, (2 * BitLength(b))), b));
        }

        public static ulong[] BarrettReduction(ulong[] a, ulong[] b)
        {
            if (LongCmp(a, b) == -1)
                return a;
            ulong[] u = usearch(b);

            ulong[] q = LongShiftBitsToLow(a, BitLength(b)-1);
            q = MulUlong(q, u);
            q = LongShiftBitsToLow(q, BitLength(b) + 1);
            ulong[] r = LongSub(a, MulUlong(q, b));

            while (LongCmp(r, b) != -1)
            {
                r = LongSub(r, b);
            }
            return r;
        }
        public static ulong[] LongModPowerBarrett(ulong[] a, ulong[] b, ulong[] n)
        {
            ulong[] c = new ulong[] { 0x01 };
            ulong[] u = usearch(n);
            var length = BitLength(b);
            for (int j = 0; j < b.Length; j++)
            {
                for (int i = 0; i < 32; i++)
                {
                    var test1 = (b[j] >> i) & 1;
                   

                    if (test1 == 1)
                    {
                        c = BarrettReduction(MulUlong(c, a), n);
                    }

                    a = BarrettReduction(MulUlong(a, a), n);
                }
             
            }

                return c;
        }

        public static ulong[] LongModAdd(ulong[] a, ulong[] b, ulong[] n)
        {
            ulong[] sum = LongAddInternal(a, b);
            return (BarrettReduction(sum, n));
        }

        public static ulong[] LongModSub(ulong[] a, ulong[] b, ulong[] n)
        {
            ulong[] sum = LongSub(a, b);
            return (BarrettReduction(sum, n));
        }

        static void Main(string[] args)
        {
            var a = Converting("1131A0939A911173");
            var b = Converting("1800000000000000");
            var c = Converting("18D95542377F163795BB3CC5065F56A9E9386F4F580F076A745B01E1A51D6A9F");
           Console.WriteLine(LongCmp(a, b));
            // Console.WriteLine(ReConv(LongAddInternal(a, b)));
            //Console.WriteLine(ReConv(LongSub(a, b)));
            //Console.WriteLine(ReConv(LongMulOneDigit(a,5)));
            //Console.WriteLine(ReConv(MulUlong(a, b)));
          //  Console.WriteLine(ReConv(LongDiv(a, b)));
            //Console.WriteLine(ReConv(LongShiftBitsToHigh(Converting("48C1B463F2782F60D01"), 64)));
            //Console.WriteLine(ReConv(BarrettReduction(a, b))); 
           //Console.WriteLine(ReConv(LongModPowerBarrett(a, b, c)));
            //Console.WriteLine(ReConv(LongModAdd(a, b, c)));
            Console.ReadKey();
        }
    }
}

        
    
