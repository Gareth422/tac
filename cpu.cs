using System;

namespace CPUEm
{
	public class Registers
	{
		public const uint G0 = 0;
		public const uint G1 = 1;
		public const uint G2 = 2;
		public const uint G3 = 3;
		public const uint G4 = 4;
		public const uint G5 = 5;
		public const uint G6 = 6;
		public const uint G7 = 7;
		public const uint HF = 8;
	}
	
	public class Opcodes
	{
		public const uint NOP = 0;
		public const uint SET = 1;
		public const uint ADD = 2;
		public const uint SUB = 3;

	}
	
	public class CPU
	{
		public short[] registers = new short[32];
		//public byte[] ram = new byte[65536];

		uint GetBits(uint value, int startbit, int bitcount)
		{
			uint result;
			result = (uint)(value << (startbit - 1));
			result = (uint)(result >> (32 - bitcount));
			return result;
		}



		public short GetRegister (uint reg)
		{
			if(reg == 0) return 0;
			else return registers[reg];
		}

		public void SetRegister (uint reg, short val)
		{
			if(reg == 0) return;
			else registers[reg] = val;
		}

		public void RunOp(uint opcode)
		{
			uint op = opcode >> 26;
			Console.WriteLine("Running Op:" + op);
			switch(op)
			{
			case Opcodes.NOP:
				// Define some variables that the other cases will use
				uint reg_t;
				uint reg_a;
				uint reg_b;
				break;
			case Opcodes.SET: // Switch to GetBits
				uint reg = opcode << 11; // Remove opcode and Extension bits
				reg >>= 27;

				uint val = opcode << 16; // Isolate the value
				val >>= 16; // Return it to its original state

				SetRegister(reg, (short)val);
				break;
			case Opcodes.ADD:
				reg_t = GetBits(opcode, 18, 5);
				reg_a = GetBits(opcode, 23, 5);
				reg_b = GetBits(opcode, 28, 5);

				SetRegister(reg_t, (short)(GetRegister(reg_a) + GetRegister(reg_b)));
				break;
			case Opcodes.SUB:
				reg_t = GetBits(opcode, 18, 5);
				reg_a = GetBits(opcode, 23, 5);
				reg_b = GetBits(opcode, 28, 5);

				SetRegister(reg_t, (short)(GetRegister(reg_a) - GetRegister(reg_b)));
				break;

			}
		}
	}

	public class Emulator
	{
		public static void Main()
		{
			Console.WriteLine("CPUEm");
			CPU cpu = new CPU();
			//uint op = (uint)Convert.ToInt32("00001000000000000101010000011111", 2);
			Console.WriteLine(cpu.GetRegister(1));
			cpu.RunOp((uint)Convert.ToInt32("00000100000000100000000000000110", 2));
			cpu.RunOp((uint)Convert.ToInt32("00000100000000110000000000000010", 2));
			cpu.RunOp((uint)Convert.ToInt32("00001000000000000000010001000011", 2));
			Console.WriteLine(cpu.GetRegister(1));
		}
	}
}
