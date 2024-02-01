namespace MipsSimulator.Mips.Instructions; 

public class TypeIInstruction : IInstruction {

    public uint Rs { get; init; }

    public uint Rt { get; init; }

    public short Immediate { get; init; }

    public Opcode Opcode { get; init; }

}
