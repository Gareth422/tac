$mnems = {
	"nop" => "000000",
	"set" => "000001",
	"cop" => "000010",
	"ldw" => "000011",
	"ldb" => "000100",
	"stw" => "000101",
	"stb" => "000110",
	"ife" => "000111",
	"ifn" => "001000",
	"ifl" => "001001",
	"ifg" => "001010",
	"add" => "010000",
	"sub" => "010001",
	"mul" => "010010",
	"div" => "010011"

}

$regs = {
	"zero" => "00000",
	"g1" => "00001",
	"g2" => "00010",
	"g3" => "00011",
	"g4" => "00100",
	"g5" => "00101",
	"g6" => "00110",
	"g7" => "00111",
	"g8" => "01000",
	"sp" => "01001",
	"pc" => "01010"
}

$opfmt = {
	"nop" => [],
	"set" => [:reg, :val],
	"add" => [:reg, :reg, :reg],
	"sub" => [:reg, :reg, :reg]
}

def getWords(line)
	return line.split(/\W+/)
end

def to16(num)
	return num.to_s(2).rjust(16, "0")
end

def toBin(num)
	return num.to_i(2)
end

def assembleOp(mnem, a, b, c)
	if(mnem == "nop")
		return "00000000000000000000000000000000"
	elsif(mnem == "set")
		return $mnems["set"] + ("0" * 5) + $regs[a] + to16(b.to_i())
	elsif(mnem == "cop")
		return $mnems["cop"] + ("0" * 16) + $regs[a] + $regs[b]
	elsif(mnem == "add")
		return $mnems["add"] + ("0" * 11) + $regs[a] + $regs[b] + $regs[c]
	elsif(mnem == "sub")
		return $mnems["sub"] + ("0" * 11) + $regs[a] + $regs[b] + $regs[c]
	elsif(mnem == "mul")
		return $mnems["mul"] + ("0" * 11) + $regs[a] + $regs[b] + $regs[c]
	elsif(mnem == "div")
		return $mnems["div"] + ("0" * 11) + $regs[a] + $regs[b] + $regs[c]
	end
end

def assembleLine(line)
	words = getWords(line)
	op = words[0]
	p1 = words[1]
	p2 = words[2]
	p3 = words[3]

	return assembleOp(op, p1, p2, p3)
end

prog = File.open(ARGV[1], "w")
prog_bin = ""

File.open(ARGV[0], "r").each_line do |line|
	puts line
	prog_bin += [toBin(assembleLine(line))].pack("N")
end

prog << prog_bin

prog.close()
