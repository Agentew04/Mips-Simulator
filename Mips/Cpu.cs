using MipsSimulator.Mips.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Mips;

/// <summary>
/// implements a monocycle MIPS CPU
/// </summary>
public partial class Cpu : IResettable {

    /// <summary>
    /// A class containing the registers of the CPU
    /// </summary>
    public CpuRegisters Registers { get; } = new CpuRegisters();

    /// <summary>
    /// The memory of the computer. basically big array with some helper methods
    /// </summary>
    public Memory Memory { get; } = new Memory();
    

    public void Step() {
        // fetch
        uint instructionRaw = Memory.ReadWord(Registers.GetRegister(Register.Pc));

        // decode
        IInstruction instruction = Decode(instructionRaw);

        // as this is a monocycle and not a real hardware
        // we can just do everything the instruction should
        // do in the execution step

        // execute
        Execute(instruction);

        // pc+4 in the 'end' of the cycle (low border)
        Registers.SetRegister(Register.Pc, Registers.GetRegister(Register.Pc) + 4); 
    }

    public void Reset() {
        Registers.Reset();
        Memory.Reset();
    }


}
