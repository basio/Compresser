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

        static long compress(int[] data)
        {
            ICompress c = new Huffman();
            BitArray bits=c.compress(data);
            return bits.Length;
        }
        static void Main(string[] args)
        {
           Console.WriteLine( compress(Array.ConvertAll( readfile(@"data.txt"),x => (int)x )));
        }
    }
}
