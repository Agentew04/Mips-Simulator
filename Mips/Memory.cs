namespace MipsSimulator.Mips; 

public class Memory : IResettable {
    // just a big array of bytes?

    private readonly int memorySize;
    private readonly byte[] memory;

    public const uint InternOffset = 0x10000000;
    public const uint DataOffset = 0x10010000;
    public const uint HeapOffset = 0x10040000;
    public const uint TextOffset = 0x00400000;
    public const uint KDataOffset = 0x90000000;

    public Memory() : this(1024*1024*4){
    }

    public Memory(int capacity) {
        memorySize = capacity;
        memory = new byte[capacity];
    }

    public byte this[uint index] {
        get {
            if(index >= memorySize) {
                throw new IndexOutOfRangeException();
            }
            return memory[index];
        }
        set {
            if(index >= memorySize) {
                throw new IndexOutOfRangeException();
            }
            memory[index] = value;
        }
    }

    public byte[] GetBytes(uint index, int length) {
        byte[] result = new byte[length];
        Array.Copy(memory, index, result, 0, length);
        return result;
    }

    public void GetBytes(uint index, Span<byte> buffer) {
        memory.AsSpan((int)index, buffer.Length).CopyTo(buffer);
    }

    public void SetBytes(uint index, byte[] bytes) {
        Array.Copy(bytes, 0, memory, index, bytes.Length);
    }

    public void SetBytes(uint index, ReadOnlySpan<byte> bytes) {
        bytes.CopyTo(memory.AsSpan((int)index, bytes.Length));
    }

    public void Reset() {
        Array.Clear(memory, 0, memory.Length);
    }

    public void WriteByte(uint index, byte value) {
        memory[index] = value;
    }

    public void WriteHalfWord(uint index, ushort value) {
        memory[index] = (byte)(value >> 8);
        memory[index + 1] = (byte)value;
    }

    public void WriteWord(uint index, uint value) {
        memory[index] = (byte)(value >> 24);
        memory[index + 1] = (byte)(value >> 16);
        memory[index + 2] = (byte)(value >> 8);
        memory[index + 3] = (byte)value;
    }

    public void WriteDoubleWord(uint index, ulong value) {
        memory[index] = (byte)(value >> 56);
        memory[index + 1] = (byte)(value >> 48);
        memory[index + 2] = (byte)(value >> 40);
        memory[index + 3] = (byte)(value >> 32);
        memory[index + 4] = (byte)(value >> 24);
        memory[index + 5] = (byte)(value >> 16);
        memory[index + 6] = (byte)(value >> 8);
        memory[index + 7] = (byte)value;
    }

    public byte ReadByte(uint index) {
        return memory[index];
    }

    public ushort ReadHalfWord(uint index) {
        return (ushort)(memory[index] << 8 | memory[index + 1]);
    }

    public uint ReadWord(uint index) {
        return (uint)(memory[index] << 24 | memory[index + 1] << 16 | memory[index + 2] << 8 | memory[index + 3]);
    }

    public ulong ReadDoubleWord(uint index) {
        return (ulong)(memory[index] << 56 | memory[index + 1] << 48 | memory[index + 2] << 40 | memory[index + 3] << 32 | memory[index + 4] << 24 | memory[index + 5] << 16 | memory[index + 6] << 8 | memory[index + 7]);
    }

    public void GetAll(Span<byte> membuf) {
        memory.AsSpan().CopyTo(membuf);
    }

    public void GetAll(byte[] membuf) {
        memory.CopyTo(membuf, 0);
    }
}
