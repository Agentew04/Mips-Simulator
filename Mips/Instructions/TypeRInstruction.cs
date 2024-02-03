namespace MipsSimulator.Mips.Instructions;

public class TypeRInstruction : IInstruction {

    public Register Rs { get; init; }

    public Register Rt { get; init; }

    public Register Rd { get; init; }

    public uint Shamt { get; init; }

    public Function Function { get; init; }
}
