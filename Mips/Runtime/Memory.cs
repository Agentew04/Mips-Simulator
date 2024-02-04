namespace MipsSimulator.Mips.Runtime;

public class Memory : IResettable
{
    private readonly LazyArray<byte> text;
    private readonly LazyArray<byte> intern;
    private readonly LazyArray<byte> data;
    private readonly LazyArray<byte> heap;
    private readonly LazyArray<byte> kdata;

    public Memory()
    {
        text = new LazyArray<byte>(TextSize, 8192);
        intern = new LazyArray<byte>(InternSize, 8192);
        data = new LazyArray<byte>(DataSize, 8192);
        heap = new LazyArray<byte>(HeapSize, 8192);
        kdata = new LazyArray<byte>(KDataSize, 1); // kdata is small, 2Kb

        segmentTable = new() {
            { SegmentType.Text, text },
            { SegmentType.Intern, intern },
            { SegmentType.Data, data },
            { SegmentType.Heap, heap },
            { SegmentType.KData, kdata }
        };
    }

    #region Read/Write

    public byte this[uint index]
    {
        get => ReadByte(index);
        set => WriteByte(index, value);
    }

    public byte[] ReadBytes(uint index, int length)
    {
        byte[] result = new byte[length];
        ReadBytes(index, result);
        return result;
    }

    public void ReadBytes(uint index, Span<byte> buffer)
    {
        // size divisible by 4 (call to ReadWord)
        if (buffer.Length % 4 == 0) {
            for (int i = 0; i < buffer.Length; i += 4) {
                uint word = ReadWord(index + (uint)i);
                BitConverter.GetBytes(word).CopyTo(buffer.Slice(i, 4));
            }
        }else if (buffer.Length % 2 == 0) { // size even, use ReadHalfWord
            for (int i = 0; i < buffer.Length; i += 2) {
                ushort halfword = ReadHalfWord(index + (uint)i);
                BitConverter.GetBytes(halfword).CopyTo(buffer.Slice(i, 2));
            }
        } else {
            // the size is odd, so we read all of it with read halfword
            // but the last with read byte
            for (int i = 0; i < buffer.Length - 1; i += 2) {
                ushort halfword = ReadHalfWord(index + (uint)i);
                BitConverter.GetBytes(halfword).CopyTo(buffer.Slice(i, 2));
            }
            buffer[^1] = ReadByte(index + (uint)buffer.Length - 1);
        }
    }

    public void WriteBytes(uint index, byte[] bytes)
    {
        WriteBytes(index, bytes.AsSpan());
    }

    public void WriteBytes(uint index, ReadOnlySpan<byte> bytes)
    {
        // size divisible by 4 (call to WriteWord)
        if (bytes.Length % 4 == 0) {
            for (int i = 0; i < bytes.Length; i += 4) {
                uint word = BitConverter.ToUInt32(bytes.Slice(i, 4));
                WriteWord(index + (uint)i, word);
            }
        } else if (bytes.Length % 2 == 0) { // size even, use WriteHalfWord
            for (int i = 0; i < bytes.Length; i += 2) {
                ushort halfword = BitConverter.ToUInt16(bytes.Slice(i, 2));
                WriteHalfWord(index + (uint)i, halfword);
            }
        } else {
            // the size is odd, so we write all of it with write halfword
            // but the last with write byte
            for (int i = 0; i < bytes.Length - 1; i += 2) {
                ushort halfword = BitConverter.ToUInt16(bytes.Slice(i, 2));
                WriteHalfWord(index + (uint)i, halfword);
            }
            WriteByte(index + (uint)bytes.Length - 1, bytes[^1]);
        }
    }

    public void Reset()
    {
        foreach(var segment in segmentTable.Values)
            segment.Clear();
    }

    public void WriteByte(uint index, byte value)
    {
        var(segment, type) = GetSegment(index);
        uint localIndex = index - (uint)type;
        segment[localIndex] = value;
    }

    public void WriteHalfWord(uint index, ushort value)
    {
        var (segment, type) = GetSegment(index);
        uint localIndex = index - (uint)type;

        segment[localIndex] = (byte)(value >> 8);
        segment[localIndex + 1] = (byte)value;
    }

    public void WriteWord(uint index, uint value)
    {
        var (segment, type) = GetSegment(index);
        uint localIndex = index - (uint)type;

        segment[localIndex] = (byte)(value >> 24);
        segment[localIndex + 1] = (byte)(value >> 16);
        segment[localIndex + 2] = (byte)(value >> 8);
        segment[localIndex + 3] = (byte)value;
    }

    public void WriteDoubleWord(uint index, ulong value)
    {
        WriteWord(index, (uint)(value >> 32));
        WriteWord(index + 4, (uint)value);
    }

    public byte ReadByte(uint index)
    {
        var (segment, type) = GetSegment(index);
        uint localIndex = index - (uint)type;
        return segment[localIndex];
    }

    public ushort ReadHalfWord(uint index)
    {
        var (segment, type) = GetSegment(index);
        uint localIndex = index - (uint)type;

        return (ushort)((segment[localIndex] << 8) | segment[localIndex + 1]);
    }

    public uint ReadWord(uint index)
    {
        var (segment, type) = GetSegment(index);
        uint localIndex = index - (uint)type;

        return (uint)((segment[localIndex] << 24) | (segment[localIndex + 1] << 16) | (segment[localIndex + 2] << 8) | segment[localIndex + 3]);
    }

    public ulong ReadDoubleWord(uint index)
    {
        uint low = ReadWord(index);
        uint high = ReadWord(index + 4);
        return ((ulong)high << 32) | low;
    }

    #endregion

    #region Segment Management

    public enum SegmentType : uint{
        Text = 0x00400000,
        Intern = 0x10000000,
        Data = 0x10010000,
        Heap = 0x10040000,
        KData = 0x90000000
    }

    // fast segment access
    private readonly Dictionary<SegmentType, LazyArray<byte>> segmentTable;

    private const uint TextSize = SegmentType.Intern - SegmentType.Text;
    private const uint InternSize = SegmentType.Data - SegmentType.Intern;
    private const uint DataSize = SegmentType.Heap - SegmentType.Data;
    private const uint HeapSize = SegmentType.KData - SegmentType.Heap;
    private const uint KDataSize = 0x2048;

    private (LazyArray<byte> segment, SegmentType type) GetSegment(uint index) {
        return index switch {
            >= (uint)SegmentType.Text and < (uint)SegmentType.Intern => (text, SegmentType.Text),
            >= (uint)SegmentType.Intern and < (uint)SegmentType.Data => (intern, SegmentType.Intern),
            >= (uint)SegmentType.Data and < (uint)SegmentType.Heap => (data, SegmentType.Data),
            >= (uint)SegmentType.Heap and < (uint)SegmentType.KData => (heap, SegmentType.Heap),
            >= (uint)SegmentType.KData => (kdata, SegmentType.KData),
            _ => throw new IndexOutOfRangeException()
        };
    }

    #endregion
}
