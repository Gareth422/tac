$mnems = {
	"nop" => "000000",
	"set" => "000001",
	"add" => "000010",
	"sub" => "000011"
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
	"g8" => "01000"
}

$opfmt = {
	"nop" => [],
	"set" => [:reg, :val],
	"add" => [:reg, :reg, :reg],
	"sub" => [:reg, :reg, :reg]
}

def GetMnems(line)
	return line.split(/\W+/)
end

def to16(num)
	return num.to_s(2).rjust(16, "0")
end

def assembleOp(mnem, a, b, c)
	if(mnem == "nop")
		return "00000000000000000000000000000000"
	elsif(mnem == "set")
		puts "SET"
		return $mnems["set"] + "00000" + $regs[a] + to16(b)
	elsif(mnem == "add")
		puts "ADD"
		return $mnems["add"] + "00000000000" + $regs[a] + $regs[b] + $regs[c]
	end
end

puts assembleOp("set", "g2", 6, nil)
puts assembleOp("set", "g3", 2, nil)
puts assembleOp("add", "g1", "g2", "g3")