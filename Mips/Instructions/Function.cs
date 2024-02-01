namespace MipsSimulator.Mips.Instructions;

public enum Function : byte {
    Sll = 0b000_000,
    Srl = 0b000_010,
    Sra = 0b000_011,
    Sllv = 0b000_100,
    Srlv = 0b000_110,
    Srav = 0b000_111,

    Jr = 0b001_000,
    Jalr = 0b001_001,
    Syscall = 0b001_100,

    Mfhi = 0b010_000,
    Mthi = 0b010_001,
    Mflo = 0b010_010,
    Mtlo = 0b010_011,

    Mult = 0b011_000,
    Multu = 0b011_001,
    Div = 0b011_010,
    Divu = 0b011_011,

    Add = 0b100_000,
    Addu = 0b100_001,
    Sub = 0b100_010,
    Subu = 0b100_011,
    And = 0b100_100,
    Or = 0b100_101,
    Xor = 0b100_110,
    Nor = 0b100_111,

    Slt = 0b101_010,
    Sltu = 0b101_011
}
