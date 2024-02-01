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

    #region Decode

    private IInstruction Decode(uint instruction) {
        uint opcode = instruction >> 26;
        if(opcode == 0) {
            return DecodeRType(instruction);
        } else if(opcode == 1 || opcode == 2){
            return DecodeJType(instruction);
        }else {
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

    #endregion

    #region Execute

    private void Execute(IInstruction instruction) {
        if(instruction is TypeRInstruction r) {
            ExecuteTypeR(r);
        } else if(instruction is TypeIInstruction i) {
            ExecuteTypeI(i);
        } else if(instruction is TypeJInstruction j) {
            ExecuteTypeJ(j);
        }
    }

    private void ExecuteTypeR(TypeRInstruction instruction) {
        switch (instruction.Function) {
            case Function.Sll:
                break;
            case Function.Srl:
                break;
            case Function.Sra:
                break;
            case Function.Sllv:
                break;
            case Function.Srlv:
                break;
            case Function.Srav:
                break;
            case Function.Jr:
                Registers.SetRegister(Register.Pc, Registers.GetRegister(instruction.Rs));
                break;
            case Function.Jalr:
                Registers.SetRegister(Register.Ra, Registers.GetRegister(Register.Pc));
                Registers.SetRegister(Register.Pc, Registers.GetRegister(instruction.Rs));
                break;
            case Function.Syscall:
                Syscall();
                break;
            case Function.Mfhi:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(Register.Hi));
                break;
            case Function.Mthi:
                Registers.SetRegister(Register.Hi, Registers.GetRegister(instruction.Rs));
                break;
            case Function.Mflo:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(Register.Lo));
                break;
            case Function.Mtlo:
                Registers.SetRegister(Register.Lo, Registers.GetRegister(instruction.Rs));
                break;
            case Function.Mult:
            case Function.Multu:
                long result = Registers.GetRegister(instruction.Rs) * Registers.GetRegister(instruction.Rt);
                Registers.SetRegister(Register.Lo, (uint)result);
                Registers.SetRegister(Register.Hi, (uint)(result >> 32));

                break;
            case Function.Div:
            case Function.Divu:
                Registers.SetRegister(Register.Lo, Registers.GetRegister(instruction.Rs) / Registers.GetRegister(instruction.Rt));
                Registers.SetRegister(Register.Hi, Registers.GetRegister(instruction.Rs) % Registers.GetRegister(instruction.Rt));
                break;
            case Function.Add:
            case Function.Addu:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rs) + Registers.GetRegister(instruction.Rt));
                break;
            case Function.Sub:
            case Function.Subu:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rs) - Registers.GetRegister(instruction.Rt));
                break;
            case Function.And:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rs) & Registers.GetRegister(instruction.Rt));
                break;
            case Function.Or:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rs) | Registers.GetRegister(instruction.Rt));
                break;
            case Function.Xor:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rs) ^ Registers.GetRegister(instruction.Rt));
                break;
            case Function.Nor:
                Registers.SetRegister(instruction.Rd, ~(Registers.GetRegister(instruction.Rs) | Registers.GetRegister(instruction.Rt)));
                break;
            case Function.Slt:
            case Function.Sltu:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rs) < Registers.GetRegister(instruction.Rt) ? 1u : 0u);
                break;
        }
    }

    private void ExecuteTypeI(TypeIInstruction instruction) {

    }

    private void ExecuteTypeJ(TypeJInstruction instruction) {

    }

    private void Syscall() {

    }

    #endregion

    public void Reset() {
        Registers.Reset();
        Memory.Reset();
    }


}
