using MipsSimulator.Mips.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Mips; 
public partial class Cpu {

    private IInstruction Decode(uint instruction) {
        uint opcode = instruction >> 26;
        if (opcode == 0) {
            return DecodeRType(instruction);
        } else if (opcode == 1 || opcode == 2) {
            return DecodeJType(instruction);
        } else {
            return DecodeIType(instruction);
        }

    }

    private TypeRInstruction DecodeRType(uint instruction) {
        uint rs = (instruction >> 21) & 0x1F;
        uint rt = (instruction >> 16) & 0x1F;
        uint rd = (instruction >> 11) & 0x1F;

        uint shamt = (instruction >> 6) & 0x1F;
        Function funct = (Function)(instruction & 0x3F);

        return new TypeRInstruction {
            Rs = (Register)rs,
            Rt = (Register)rt,
            Rd = (Register)rd,
            Shamt = shamt,
            Function = funct
        };
    }

    private TypeIInstruction DecodeIType(uint instruction) {
        uint rs = (instruction >> 21) & 0x1F;
        uint rt = (instruction >> 16) & 0x1F;
        short immediate = (short)(instruction & 0xFFFF);
        Opcode opcode = (Opcode)((instruction >> 26) & 0x3F);

        return new TypeIInstruction {
            Rs = (Register)rs,
            Rt = (Register)rt,
            Immediate = immediate,
            Opcode = opcode
        };
    }

    private TypeJInstruction DecodeJType(uint instruction) {
        Opcode opcode = (Opcode)(instruction >> 26);
        uint address = instruction & 0x3FFFFFF;

        return new TypeJInstruction {
            Opcode = opcode,
            Address = address
        };
    }
}
