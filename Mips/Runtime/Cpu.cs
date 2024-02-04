using MipsSimulator.Mips.Runtime;
using MipsSimulator.Mips.Runtime.Instructions;
using System.Numerics;

namespace MipsSimulator.Mips;

/// <summary>
/// implements a monocycle MIPS CPU
/// </summary>
public partial class Cpu : IResettable {

    private bool executing = false;

    /// <summary>
    /// A class containing the registers of the CPU
    /// </summary>
    public CpuRegisters Registers { get; } = new CpuRegisters();

    /// <summary>
    /// The memory of the computer. basically big array with some helper methods
    /// </summary>
    public Memory Memory { get; } = new Memory();

    private ConsoleIO? stdio = null;

    /// <summary>
    /// If the CPU is currently processing one instruction. Useful if waiting in input to 
    /// block other commands
    /// </summary>
    public bool IsProcessing { get; private set; } = false;

    /// <summary>
    /// If the CPU is under continuous execution via the Continue Program.
    /// </summary>
    public bool IsExecuting => executing;
    
    public void SetStdIO(ConsoleIO io) {
        stdio = io;
    }

    public async Task Step() {
        // prevent race conditions
        await Extensions.WaitWhile(() => IsProcessing);


        IsProcessing = true;
        // fetch
        uint instructionRaw = Memory.ReadWord(Registers.GetRegister(Register.Pc));

        // decode
        IInstruction instruction = Decode(instructionRaw);

        // as this is a monocycle and not a real hardware
        // we can just do everything the instruction should
        // do in the execution step

        // execute
        await Execute(instruction);

        // pc+4 in the 'end' of the cycle (low border)
        Registers.SetRegister(Register.Pc, Registers.GetRegister(Register.Pc) + 4);
        IsProcessing = false;
    }

    public async Task Continue() {
        executing = true;
        while (executing) {
            Console.WriteLine("Step");
            await Step();
            Console.WriteLine("Yield");
            await Task.Delay(1000);
            await Task.Yield();
        }
    }

    public void Stop() {
        executing = false;
    }



    public void Reset() {
        executing = false;
        IsProcessing = false;
        Registers.Reset();
        Memory.Reset();
    }

    // TODO create a Program class to hold each segment
    public void LoadProgram(string textpath, string datapath) {
        // load text segment
        Span<byte> wordbuffer = stackalloc byte[4]; // one word
        using (FileStream fs = new(textpath, FileMode.Open)) {
            uint position = (uint)Memory.SegmentType.Text;
            while (fs.Read(wordbuffer) > 0) {
                uint word = BitConverter.ToUInt32(wordbuffer);
                Memory.WriteWord(position, word);
                position += 4;
            }
            Registers[Register.Pc] = (uint)Memory.SegmentType.Text;
        }

        // load data segment
        using (FileStream fs = new(datapath, FileMode.Open)) {
            uint position = (uint)Memory.SegmentType.Data;
            while (fs.Read(wordbuffer) > 0) {
                uint word = BitConverter.ToUInt32(wordbuffer);
                Memory.WriteWord(position, word);
                position += 4;
            }
        }
    }
}
