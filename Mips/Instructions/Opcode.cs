namespace MipsSimulator.Mips.Instructions;

public enum Opcode : byte {
    RType = 0,
    J = 0b000_010,
    Jal = 0b000_011,
    Beq = 0b000_100,
    Bne = 0b000_101,
    Blez = 0b000_110,
    Bgtz = 0b000_111,

    Addi = 0b001_000,
    Addiu = 0b001_001,
    Slti = 0b001_010,
    Sltiu = 0b001_011,
    Andi = 0b001_100,
    Ori = 0b001_101,
    Xori = 0b001_110,

    Llo = 0b011_000,
    Lhi = 0b011_001,
    Trap = 0b011_010,

    Lb = 0b100_000,
    Lh = 0b100_001,
    Lw = 0b100_011,
    Lbu = 0b100_100,
    Lhu = 0b100_101,

    Sb = 0b101_000,
    Sh = 0b101_001,
    Sw = 0b101_011
}
