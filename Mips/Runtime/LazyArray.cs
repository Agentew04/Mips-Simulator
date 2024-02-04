namespace MipsSimulator.Mips.Runtime; 

// TODO add support to optionally lazy load files
public class LazyArray<T> {

    private readonly Dictionary<uint, T?[]> segments;

    private readonly uint totalSize;
    private readonly uint divisions;
    private readonly uint segmentSize;

    public LazyArray(uint size, uint subdivisions) {
        segments = [];
        segmentSize = size / divisions;
        totalSize = size;
        divisions = subdivisions;
    }

    public T? this[uint index] {
        get => Get(index);
        set => Set(index, value);
    }

    public T? Get(uint index) {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, totalSize);
        
        uint segment = index / divisions;
        uint offset = index % divisions;

        if (segments.TryGetValue(segment, out T?[]? seg)) {
            return seg[offset];
        }

        // here the segment does not exist
        // se we create it
        T[] newSegment = new T[segmentSize];
        segments.Add(segment, newSegment);
        return default;
    }

    public void Set(uint index, T? value) {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, totalSize);

        uint segment = index / divisions;
        uint offset = index % divisions;

        if (segments.TryGetValue(segment, out T?[]? seg)) {
            seg[offset] = value;
            return;
        }

        // here the segment does not exist
        // se we create it
        T?[] newSegment = new T[segmentSize];
        newSegment[offset] = value;
        segments.Add(segment, newSegment);
    }

    /// <summary>
    /// Deletes all internal subdivisions
    /// </summary>
    public void Clear() {
        segments.Clear();
    }
}
