using MipsSimulator.Mips.Instructions;

namespace MipsSimulator.Mips; 

public partial class Cpu {
    private void Execute(IInstruction instruction) {
        if (instruction is TypeRInstruction r) {
            ExecuteTypeR(r);
        } else if (instruction is TypeIInstruction i) {
            ExecuteTypeI(i);
        } else if (instruction is TypeJInstruction j) {
            ExecuteTypeJ(j);
        }
    }

    private void ExecuteTypeR(TypeRInstruction instruction) {
        switch (instruction.Function) {
            case Function.Sll:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rt) << (int)instruction.Shamt);
                break;
            case Function.Sllv:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rt) << (int)Registers.GetRegister(instruction.Rs));
                break;
            case Function.Sra:
                bool sign = (Registers.GetRegister(instruction.Rt) & 0x80000000) != 0;
                uint shiftMask = 0xFFFFFFFF << (32 - (int)instruction.Shamt);
                uint shiftedValue = Registers.GetRegister(instruction.Rt) >> (int)instruction.Shamt;
                if (sign) {
                    shiftedValue |= shiftMask;
                }
                Registers.SetRegister(instruction.Rd, shiftedValue);
                break;
            case Function.Srav:
                sign = (Registers.GetRegister(instruction.Rt) & 0x80000000) != 0;
                shiftMask = 0xFFFFFFFF << (32 - (int)Registers.GetRegister(instruction.Rs));
                shiftedValue = Registers.GetRegister(instruction.Rt) >> (int)Registers.GetRegister(instruction.Rs);
                if (sign) {
                    shiftedValue |= shiftMask;
                }
                Registers.SetRegister(instruction.Rd, shiftedValue);
                break;
            case Function.Srl:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rt) >> (int)instruction.Shamt);
                break;
            case Function.Srlv:
                Registers.SetRegister(instruction.Rd, Registers.GetRegister(instruction.Rt) >> (int)Registers.GetRegister(instruction.Rs));
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
        uint branch(uint addr, int offset) {
            uint newAddr;
            if (offset > 0) {
                newAddr = addr + (uint)offset;
            } else {
                newAddr = addr - (uint)(-offset);
            }
            return newAddr;
        }

        switch (instruction.Opcode) {
            case Opcode.Beq:
                if (Registers.GetRegister(instruction.Rs) == Registers.GetRegister(instruction.Rt)) {
                    Registers.SetRegister(Register.Pc, branch(Registers.GetRegister(Register.Pc), instruction.Immediate << 2));
                }
                break;
            case Opcode.Bne:
                if (Registers.GetRegister(instruction.Rs) != Registers.GetRegister(instruction.Rt)) {
                    Registers.SetRegister(Register.Pc, branch(Registers.GetRegister(Register.Pc), instruction.Immediate << 2));
                }
                break;
            case Opcode.Blez:
                if ((int)Registers.GetRegister(instruction.Rs) <= 0) {
                    Registers.SetRegister(Register.Pc, branch(Registers.GetRegister(Register.Pc), instruction.Immediate << 2));
                }
                break;
            case Opcode.Bgtz:
                if ((int)Registers.GetRegister(instruction.Rs) > 0) {
                    Registers.SetRegister(Register.Pc, branch(Registers.GetRegister(Register.Pc), instruction.Immediate << 2));
                }
                break;
            case Opcode.Addi:
                Registers.SetRegister(instruction.Rt, Registers.GetRegister(instruction.Rs) + (uint)instruction.Immediate);
                break;
            case Opcode.Addiu:
                Registers.SetRegister(instruction.Rt, Registers.GetRegister(instruction.Rs) + (uint)instruction.Immediate);
                break;
            case Opcode.Slti:
                break;
            case Opcode.Sltiu:
                break;
            case Opcode.Andi:
                break;
            case Opcode.Ori:
                break;
            case Opcode.Xori:
                break;
            case Opcode.Llo:
                break;
            case Opcode.Lhi:
                break;
            case Opcode.Trap:
                break;
            case Opcode.Lb:
                break;
            case Opcode.Lh:
                break;
            case Opcode.Lw:
                break;
            case Opcode.Lbu:
                break;
            case Opcode.Lhu:
                break;
            case Opcode.Sb:
                break;
            case Opcode.Sh:
                break;
            case Opcode.Sw:
                break;
        }
    }

    private void ExecuteTypeJ(TypeJInstruction instruction) {
        switch (instruction.Opcode) {
            case Opcode.J:
                Registers.SetRegister(Register.Pc, (Registers.GetRegister(Register.Pc) & 0xF0000000) | (instruction.Address << 2));
                break;
            case Opcode.Jal:
                Registers.SetRegister(Register.Ra, Registers.GetRegister(Register.Pc));
                Registers.SetRegister(Register.Pc, (Registers.GetRegister(Register.Pc) & 0xF0000000) | (instruction.Address << 2));
                break;
        }
    }

    private void Syscall() {

    }
}
