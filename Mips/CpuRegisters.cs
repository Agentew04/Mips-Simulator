namespace MipsSimulator.Mips;

/// <summary>
/// Structure to hold the values of all main registers of the CPU.
/// </summary>
public class CpuRegisters : IResettable {

    private uint[] registers = new uint[32];
    private uint pc = 0x0;
    private uint hi = 0x0;
    private uint lo = 0x0;

    public uint this[int index] {
        get => registers[index];
        set => registers[index] = value;
    }

    public uint this[Register r] {
        get => GetRegister(r);
        set => SetRegister(r, value);
    }

    public void SetRegister(Register r, uint value) {
        if (r == Register.Zero) {
            return;
        }

        if (r is >= Register.Zero and <= Register.Ra) {
            registers[(int)r] = value;
            return;
        }

        switch (r) {
            case Register.Pc:
                pc = value;
                break;
            case Register.Hi:
                hi = value;
                break;
            case Register.Lo:
                lo = value;
                break;
        }
    }

    public uint GetRegister(Register r) {
        if (r is >= Register.Zero and <= Register.Ra) {
            return registers[(int)r];
        }
        return r switch {
            Register.Pc => pc,
            Register.Hi => hi,
            Register.Lo => lo,
            _ => 0,
        };
    }

    /// <summary>
    /// Reset the state of the CPU registers.
    /// </summary>
    public void Reset() {
        registers = new uint[32];
        pc = 0x0;
        hi = 0x0;
        lo = 0x0;
    }
}

public enum Register {
    Zero = 0,
    At,
    V0,
    V1,
    A0,
    A1,
    A2,
    A3,
    T0,
    T1,
    T2,
    T3,
    T4,
    T5,
    T6,
    T7,
    S0,
    S1,
    S2,
    S3,
    S4,
    S5,
    S6,
    S7,
    T8,
    T9,
    K0,
    K1,
    Gp,
    Sp,
    Fp,
    Ra,
    Pc,
    Hi,
    Lo
}
