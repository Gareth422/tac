using System;
using System.IO;

namespace ToBin
{
	public class EndianBinaryWriter
    {
        /* ===[ Notes ]====================================================== */
 
        /* If there is a discrepency between the system's endianness and the 
         * requested operation, reverse the bytes where needed. In cases where
         * the system's endianness is the requested mode, take no action. The
         * following table illustrates when byte reversals are made: 
         * 
         * +-----------------------------+-------------+---------+
         * | BitConverter.IsLittleEndian | this.bigEnd | Action  |
         * +-----------------------------+-------------+---------+
         * | True                        | True        | Reverse |
         * | True                        | False       | None    |
         * | False                       | True        | None    |
         * | False                       | False       | Reverse |
         * +-----------------------------+-------------+---------+
         *
         * Therefore, use XOR logic to determine whether to reverse bytes.
         *
         */
 
        /* ===[ Constants ]================================================== */
 
        internal const string DEFAULT_ENCODING   = "UTF8";
        internal const bool   DEFAULT_BIG_ENDIAN = 
 
        /* ===[ Constructors ]=============================================== */
 
        public EndianBinaryWriter(Stream output)
            : this(output, Encoding.GetEncoding(DEFAULT_ENCODING)) { }
 
        public EndianBinaryWriter(Stream output, Encoding encoding)
            : this(output, encoding, DEFAULT_BIG_ENDIAN) { }
 
        public EndianBinaryWriter(Stream output, Encoding encoding, bool bigEndian)
        {
            this.output   = output;
            this.encoding = encoding;
            this.writer   = new BinaryWriter(output, encoding);
            this.bigEnd   = bigEndian;
        }
 
    }
	class ToBin
	{
		public static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Incorrect Usage");
				return;
			}
			StreamReader sr = new StreamReader (args [0]);
			StreamWriter sw = new StreamWriter (args [1]);

			while (sr.Peek() >= 0)
			{
				sw.Write((string)Convert.ToUInt32(sr.ReadLine(), 2));
			}
		}
	}
}
