namespace MipsSimulator.Mips.Runtime.Instructions;

public enum Opcode : byte
{
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

    /// <summary>
    /// Sets the 16 least significant bits of a register to a 16bit immediate value
    /// </summary>
    Llo = 0b011_000,
    /// <summary>
    /// Sets the 16 most significant bits of a register to a 16bit immediate value
    /// </summary>
    Lhi = 0b011_001,
    Trap = 0b011_010,

    /// <summary>
    /// Load byte
    /// </summary>
    Lb = 0b100_000,
    /// <summary>
    /// Load halfword
    /// </summary>
    Lh = 0b100_001,
    /// <summary>
    /// Load word
    /// </summary>
    Lw = 0b100_011,
    /// <summary>
    /// Load byte unsigned
    /// </summary>
    Lbu = 0b100_100,
    /// <summary>
    /// Load halfword unsigned
    /// </summary>
    Lhu = 0b100_101,

    /// <summary>
    /// Store byte
    /// </summary>
    Sb = 0b101_000,
    /// <summary>
    /// Store halfword
    /// </summary>
    Sh = 0b101_001,
    /// <summary>
    /// Store word
    /// </summary>
    Sw = 0b101_011
}
