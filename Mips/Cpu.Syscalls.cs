using System.Text;

namespace MipsSimulator.Mips;
public partial class Cpu {
    /// <summary>
    /// SPIM simulator compatible syscalls
    /// </summary>
    private void Syscall() {
        uint v0 = Registers[Register.V0];

        switch (v0) {
            case 1:
                PrintInteger();
                break;
            case 2:
                PrintFloat();
                break;
            case 3:
                PrintDouble();
                break;
            case 4:
                PrintString();
                break;
            case 5:
                ReadInteger();
                break;
            case 6:
                ReadFloat();
                break;
            case 7:
                ReadDouble();
                break;
            case 8:
                ReadString();
                break;
            case 9:
                AllocateHeap();
                break;
            case 10:
                Exit();
                break;
            case 11:
                PrintCharacter();
                break;
            case 12:
                ReadCharacter();
                break;
            case 13:
                OpenFile();
                break;
            case 14:
                ReadFile();
                break;
            case 15:
                WriteFile();
                break;
            case 16:
                CloseFile();
                break;
            case 17:
                ExitWithValue();
                break;
        }
    }

    private void PrintInteger() {
        int a0 = (int)Registers[Register.A0];

        outStream?.Write(a0.ToString());
    }

    private void PrintFloat() {
        throw new NotImplementedException();
    }

    private void PrintDouble() {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Print a null-terminated string
    /// </summary>
    private void PrintString() {
        uint strAddress = Registers[Register.A0];

        StringBuilder sb = new();
        char c;
        while ((c = (char)Memory.ReadByte(strAddress++)) != 0) {
            sb.Append(c);
        }
        outStream?.Write(sb.ToString());
    }

    private void ReadInteger() {
        string input = inStream?.ReadLine() ?? "";
        if (int.TryParse(input, out int result)) {
            Registers[Register.V0] = (uint)result;
        } else {
            Registers[Register.V0] = 0;
        }

    }

    private void ReadFloat() {
        throw new NotImplementedException();
    }

    private void ReadDouble() {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Reads a null terminated string from the input stream. Follow fgets semantics
    /// </summary>
    private void ReadString() {
        uint bufferAddr = Registers[Register.A0];
        int maxLen = (int)Registers[Register.A1];

        string line = inStream?.ReadLine() ?? ""; // ignore the input

        // if less than maxLen append \n else pad with \0
        if (line.Length < maxLen) {
            line += '\n';
        } else {
            line = line.Substring(0, maxLen - 1);
        }

        Span<byte> bytes = stackalloc byte[maxLen];
        Encoding.ASCII.GetBytes(line, bytes);

        Memory.WriteBytes(bufferAddr, bytes);
    }

    private void AllocateHeap() {
        throw new NotImplementedException();
    }

    private void Exit() {
        executing = false;
    }

    private void PrintCharacter() {
        uint a0 = Registers[Register.A0];
        byte c = (byte)a0;
        char[] chars = Encoding.ASCII.GetChars([c]);
        outStream?.Write(chars[0]);
    }

    private void ReadCharacter() {
        string line = inStream?.ReadLine() ?? "";

        if (line.Length > 0) {
            Span<byte> bytes = stackalloc byte[1];
            Encoding.ASCII.GetBytes(line[0].ToString(), bytes);
            Registers[Register.V0] = bytes[0];
        } else {
            Registers[Register.V0] = 0;
        }
    }

    private void OpenFile() {
        throw new NotImplementedException();
    }

    private void ReadFile() {
        throw new NotImplementedException();
    }

    private void WriteFile() {
        throw new NotImplementedException();
    }

    private void CloseFile() {
        throw new NotImplementedException();
    }

    private void ExitWithValue() {
        throw new NotImplementedException();
    }
}
