namespace MipsSimulator.Mips.Runtime.Instructions;

public class TypeIInstruction : IInstruction
{

    public Register Rs { get; init; }

    public Register Rt { get; init; }

    public short Immediate { get; init; }

    public Opcode Opcode { get; init; }

}
