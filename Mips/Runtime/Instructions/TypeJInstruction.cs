namespace MipsSimulator.Mips.Runtime.Instructions;

public class TypeJInstruction : IInstruction
{
    public Opcode Opcode { get; init; }

    public uint Address { get; init; }
}
