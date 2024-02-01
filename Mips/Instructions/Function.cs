namespace MipsSimulator.Mips.Instructions;

public enum Function : byte {
    Sll = 0b000_000,
    Srl = 0b000_010,
    Sra = 0b000_011,
    Sllv = 0b000_100,
    Srlv = 0b000_110,
    Srav = 0b000_111,

    /// <summary>
    /// PC = RS. Jump register
    /// </summary>
    Jr = 0b001_000,
    /// <summary>
    /// $31 = PC; PC = RS
    /// </summary>
    Jalr = 0b001_001,
    /// <summary>
    /// Executes a syscall
    /// </summary>
    Syscall = 0b001_100,

    /// <summary>
    /// RD = HI
    /// </summary>
    Mfhi = 0b010_000,
    /// <summary>
    /// HI = RS
    /// </summary>
    Mthi = 0b010_001,
    /// <summary>
    /// RD = LO
    /// </summary>
    Mflo = 0b010_010,
    /// <summary>
    /// LO = RS
    /// </summary>
    Mtlo = 0b010_011,

    /// <summary>
    /// HI:LO = RS * RT
    /// </summary>
    Mult = 0b011_000,
    /// <summary>
    /// HI:LO = RS * RT
    /// </summary>
    Multu = 0b011_001,
    /// <summary>
    /// LO = RS / RT, HI = RS % RT
    /// </summary>
    Div = 0b011_010,
    /// <summary>
    /// LO = RS / RT, HI = RS % RT
    /// </summary>
    Divu = 0b011_011,

    /// <summary>
    /// RD = RS + RT. Throws on overflow
    /// </summary>
    Add = 0b100_000,
    /// <summary>
    /// RD = RS + RT
    /// </summary>
    Addu = 0b100_001,
    /// <summary>
    /// RD = RS - RT. Throws on overflow
    /// </summary>
    Sub = 0b100_010,
    /// <summary>
    /// RD = RS - RT
    /// </summary>
    Subu = 0b100_011,
    /// <summary>
    /// RD = RS & RT
    /// </summary>
    And = 0b100_100,
    /// <summary>
    /// RD = RS | RT
    /// </summary>
    Or = 0b100_101,
    /// <summary>
    /// RD = RS ^ RT
    /// </summary>
    Xor = 0b100_110,
    /// <summary>
    /// RD = ~(RS | RT)
    /// </summary>
    Nor = 0b100_111,
    /// <summary>
    /// RD = RS -lt RT. Will use signed subtraction
    /// </summary>
    Slt = 0b101_010,
    /// <summary>
    /// RD = RS -lt RT. Will use unsigned subtraction
    /// </summary>
    Sltu = 0b101_011
}
