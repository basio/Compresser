using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Compresser
{
    class Huffman : ICompress
    {
        public BitArray compress(int[] data)
        {
            HuffmanTree ht = new HuffmanTree();
            ht.Frequencies = new Dictionary<int, int>();
            foreach (int x in data)
            {
                if (ht.Frequencies.ContainsKey(x))
                    ht.Frequencies[x]++;
                else
                    ht.Frequencies.Add(x, 1);
            }
            ht.Build();
            //encode
            BitArray bits = ht.Encode(data);
            return  bits;
        }

        public int[] uncompress(System.Collections.BitArray bits)
        {
            throw new NotImplementedException();
        }
    }
    class Node : PriorityQueueNode
    {
        public int Symbol { get; set; }
        public int frequency { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }

        public List<bool> Traverse(int symbol, List<bool> data)
        {
            // Leaf
            if (Right == null && Left == null)
            {
                if (symbol==Symbol)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Traverse(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Traverse(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
        public long Frequency()
        {
            if (Right == null) return frequency;
            else return frequency + Right.Frequency() + Left.Frequency();
        }
    }
    class HuffmanTree
    {
        private HeapPriorityQueue<Node> nodes = null;
        public Node Root { get; set; }
        public Dictionary<int, int> Frequencies = new Dictionary<Int32, int>();
        public Dictionary<int, List<bool>> codes = new Dictionary<int, List<bool>>();
        public long Size()
        {
            int size = 0;
            //  List<Node> ls = new List<Node>();

            //foreach (string x in Frequencies.Keys)
            //{
            //    List<bool> l = Root.Traverse(x, new List<bool>());
            //    size += l.Count() * Frequencies[x];

            //}

            return Root.Frequency();
        }
        public void Build()
        {
            nodes = new HeapPriorityQueue<Node>(Frequencies.Count);
            foreach (KeyValuePair<Int32, int> symbol in Frequencies)
            {
                nodes.Enqueue(new Node() { Symbol = symbol.Key, frequency = symbol.Value }, symbol.Value);
            }

            while (nodes.Count >= 2)
            {
                //get the lowest two frequencies
                Node a = nodes.Dequeue();
                Node b = nodes.Dequeue();
                // Create a parent node by summing the frequencies
                Node parent = new Node()
                {
                    frequency = a.frequency + b.frequency,
                    Left = a,
                    Right = b
                };

                nodes.Enqueue(parent, parent.frequency);
            }
            this.Root = nodes.Dequeue();
        }

        public BitArray Encode(int[] source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                int s=source[i];
                List<bool> encodedSymbol;
                if(codes.ContainsKey(s)) 
                encodedSymbol=codes[s];
                
                else {
                    encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                    codes.Add(s, encodedSymbol);
                }
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());
            return bits;
        }

        public string Decode(BitArray bits)
        {
            Node current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }

            return decoded;
        }

        public bool IsLeaf(Node node)
        {
            return (node.Left == null && node.Right == null);
        }

    }

}
