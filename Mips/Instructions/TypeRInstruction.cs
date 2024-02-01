using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Mips.Instructions; 

public class TypeRInstruction : IInstruction {

    public uint Rs { get; init; }

    public uint Rt { get; init; }

    public uint Rd { get; init; }

    public uint Shamt { get; init; }

    public Function Function { get; init; }
}
