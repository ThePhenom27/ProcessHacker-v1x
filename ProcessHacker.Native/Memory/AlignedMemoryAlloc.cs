﻿using System;
using ProcessHacker.Common;

namespace ProcessHacker.Native
{
    public sealed class AlignedMemoryAlloc : MemoryAlloc
    {
        private readonly IntPtr _realMemory;

        public AlignedMemoryAlloc(int size, int alignment)
        {
            // Make sure the alignment is positive and a power of two.
            if (alignment <= 0 || alignment.CountBits() != 1)
                throw new ArgumentOutOfRangeException("alignment");

            // Since we are going to align our pointer, we need to account for 
            // any padding at the beginning.
            _realMemory = PrivateHeap.Allocate(size + alignment - 1);

            // aligned memory = (memory + alignment - 1) & ~(alignment - 1)
            this.Memory = _realMemory.Align(alignment);
            this.Size = size;
        }

        protected override void Free()
        {
            PrivateHeap.Free(_realMemory);
        }

        public override void Resize(int newSize)
        {
            throw new NotSupportedException();
        }
    }
}
