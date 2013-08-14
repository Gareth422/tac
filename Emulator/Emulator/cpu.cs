using System;
using System.IO;

namespace CPUEm
{
	public class Registers
	{
		public const uint ZERO = 0;
		public const uint G1 = 1;
		public const uint G2 = 2;
		public const uint G3 = 3;
		public const uint G4 = 4;
		public const uint G5 = 5;
		public const uint G6 = 6;
		public const uint G7 = 7;
		public const uint G8 = 8;
		public const uint SP = 9;
		public const uint PC = 10;
	}
	
	public class Opcodes
	{
		public const uint NOP = 0x00;
		public const uint SET = 0x01;
		public const uint COP = 0x02;
		public const uint LDW = 0x03;
		public const uint LDB = 0x04;
		public const uint STW = 0x05;
		public const uint STB = 0x06;
		public const uint IFE = 0x07;
		public const uint IFN = 0x08;
		public const uint IFL = 0x09;
		public const uint IFG = 0x0A;
		
		public const uint ADD = 0x10;
		public const uint SUB = 0x11;
		public const uint MUL = 0x12;
		public const uint DIV = 0x13;


	}
	
	public class CPU
	{
		public short[] registers = new short[32];
		public byte[] ram = new byte[65536];

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

		public void Emulate()
		{
			byte[] instruction = { ram[registers[Registers.PC]], ram[registers[Registers.PC] + 1], ram[registers[Registers.PC] + 2], ram[registers[Registers.PC] + 3] };
			if(BitConverter.IsLittleEndian) Array.Reverse(instruction);
			RunOp(BitConverter.ToUInt32(instruction, 0));
		}

		public void RunOp(uint opcode)
		{
			uint op = opcode >> 26;
			Console.WriteLine("Running Op:" + op);
			uint reg_t;
			uint reg_a;
			uint reg_b;
			switch(op)
			{
			case Opcodes.NOP:
				break;
			case Opcodes.COP:
				reg_t = GetBits(opcode, 23, 5);
				Console.WriteLine("REG_T " + reg_t);
				reg_a = GetBits(opcode, 28, 5);
				Console.WriteLine("REG_A " + reg_a);
				SetRegister(reg_t, GetRegister(reg_a));
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
			case Opcodes.MUL:
				reg_t = GetBits(opcode, 18, 5);
				reg_a = GetBits(opcode, 23, 5);
				reg_b = GetBits(opcode, 28, 5);

				SetRegister(reg_t, (short)(GetRegister(reg_a) * GetRegister(reg_b)));
				break;
			case Opcodes.DIV:
				reg_t = GetBits(opcode, 18, 5);
				reg_a = GetBits(opcode, 23, 5);
				reg_b = GetBits(opcode, 28, 5);

				SetRegister(reg_t, (short)(GetRegister(reg_a) / GetRegister(reg_b)));
				break;
			//picase Opcodes.IFE

			}

			registers[Registers.PC] += 4;
		}
	}

	public class Emulator
	{

		public static void Main ()
		{
			Console.WriteLine ("CPUEm");
			CPU cpu = new CPU ();
			BinaryReader br = new BinaryReader (new FileStream (@"prog.bin", FileMode.Open));
			int i = 0;
			while (br.PeekChar() >= 0)
			{
				cpu.ram[i] = br.ReadByte();
				i++;
			}
			//cpu.RunOp(Convert.ToUInt32("04010006", 16));
			cpu.Emulate();
			cpu.Emulate();
			cpu.Emulate();
			cpu.Emulate();
			Console.WriteLine(cpu.registers[Registers.G4]);
			Console.ReadLine();
		}
	}
}