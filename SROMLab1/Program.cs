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
            return G.TrimStart('0');
        }

            public static ulong[] LongAddInternal(ulong[] a, ulong[] b)
            {
                var lenght = Math.Max(a.Length, b.Length);
                Array.Resize(ref a, lenght);
                Array.Resize(ref b, lenght);
                ulong[] C = new ulong[a.Length + 1];

                ulong carry = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    ulong temp = a[i] + b[i] + carry;
                    C[i] = temp & 0xffffffff; // какого хуя это работает 
                    carry = temp >> 32;
                }
                C[a.Length] = carry;
                return C;
            }

            public static int LongCmp(ulong[] a, ulong[] b)
            {
                var maxlenght = Math.Max(a.Length, b.Length);
                Array.Resize(ref a, maxlenght);
                Array.Resize(ref b, maxlenght);
                for (int i = a.Length - 1; i > -1; i--)
                {
                    if (a[i] < b[i]) return -1;
                    if (a[i] > b[i]) return 1;
                }
                return 0;
            }

            public static ulong[] LongSub(ulong[] a, ulong[] b)

            {
                var lenght = Math.Max(a.Length, b.Length);
                Array.Resize(ref a, lenght);
                Array.Resize(ref b, lenght);
                ulong[] C = new ulong[a.Length]; //ulong ?
                ulong temp, borrow = 0;
                ulong Zero = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    temp = a[i] - b[i] - borrow;
                    C[i] = (temp & 0xFFFFFFFF);
                    borrow = temp <= a[i] ? Zero : 1;
                }
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
                answer = RemoveHighZeros(answer);
                return answer;
            }

            static ulong[] RemoveHighZeros(ulong[] c)
            {
                int i = c.Length - 1;
                while (c[i] == 0)
                {
                    i--;
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
                //r = R;
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


        static void Main(string[] args)
            {
                var a = Converting("982FD67737A55C8D2FDD3C35685EBCFF8B57BC515EC1C214587489A605DFEC9D");
                var b = Converting("B5106AE3D824F9CAC3335890B7512DCF27F26F69379115E92596D9367C6FAE3E");

                Console.WriteLine(LongCmp(a, b));
                Console.WriteLine(ReConv(LongAddInternal(a, b)));
                Console.WriteLine(ReConv(LongSub(a, b)));
                Console.WriteLine(ReConv(LongMulOneDigit(a,5)));
                Console.WriteLine(ReConv(MulUlong(a, b)));
                Console.WriteLine(ReConv(LongDiv(a, b)));
           
               
                Console.ReadKey();
            }

        }
    }
