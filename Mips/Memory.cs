using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace MipsSimulator.Mips {
    public class Memory {
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

        public void SetByte(uint index, byte value) {
            memory[index] = value;
        }

        public byte GetByte(uint index) {
            return memory[index];
        }

        public void GetAll(Span<byte> membuf) {
            memory.AsSpan().CopyTo(membuf);
        }

        public void GetAll(byte[] membuf) {
            memory.CopyTo(membuf, 0);
        }
    }
}
