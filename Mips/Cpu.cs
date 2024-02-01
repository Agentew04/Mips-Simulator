using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Mips; 
public class Cpu {

    public CpuRegisters Registers { get; } = new CpuRegisters();

    public Memory Memory { get; } = new Memory();
}
