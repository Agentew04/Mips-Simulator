using System.Text;
using MipsSimulator.Mips.Runtime;

namespace MipsSimulator.Mips;
public partial class Cpu {
    /// <summary>
    /// SPIM simulator compatible syscalls
    /// </summary>
    private async Task Syscall() {
        if(stdio is null) {
            throw new InvalidOperationException("Stdio must be set before executing a syscall");
        }

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
                await ReadInteger();
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

        stdio?.Write(a0.ToString());
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

        stdio?.Write(sb.ToString());
    }

    private async Task ReadInteger() {
        string input = await stdio!.Read();
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
    private async Task ReadString() {
        uint bufferAddr = Registers[Register.A0];
        int maxLen = (int)Registers[Register.A1];

        string line = await stdio!.Read();

        // if less than maxLen append \n else pad with \0
        if (line.Length < maxLen) {
            line += '\n';
        } else {
            line = line.Substring(0, maxLen - 1);
        }

        byte[] bytes = new byte[maxLen];
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
        stdio!.Write(chars[0].ToString());
    }

    private async Task ReadCharacter() {
        string line = await stdio!.Read();

        if (line.Length > 0) {
            byte[] bytes = new byte[1];
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
