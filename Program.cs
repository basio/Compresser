using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compresser
{
    class Program
    {
        static double[] readfile(string filename)
        {
            List<double> l = new List<double>();
            StreamReader sr = new StreamReader(filename);
            while (!sr.EndOfStream)
            {
                l.Add(Double.Parse(sr.ReadLine()));
            }

            sr.Close();
            return l.ToArray();
        }

        static BitArray compress(int[] data)
        {
            ICompress c = new Huffman();
            BitArray bits=c.compress(data);
            return bits;
        }
        static int[] uncompress(BitArray bits)
        {
            ICompress c = new Huffman();
            return c.uncompress(bits);            
        }
        static void Main(string[] args)
        {
           BitArray ba= compress(Array.ConvertAll( readfile(@"test.txt"),x => (int)x ));
           Console.WriteLine(ba.Length);
           int[] b = uncompress(ba);
            
        }
    }
}
