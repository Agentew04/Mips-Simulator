using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Mips.Instructions;

public class TypeJInstruction : IInstruction {
    public Opcode Opcode { get; init; }

    public uint Address { get; init; }
}
