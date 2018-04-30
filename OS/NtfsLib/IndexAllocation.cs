using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    class IndexAllocation
    {
        public byte[] Signature { get; set; }
        public ushort UsaOffset { get; set; }
        public ushort UsaCount { get; set; }
        public ulong Lsn { get; set; }
        public ulong IndexBlockVCN { get; set; }
        IndexHeader header { get; set; }
    }
}
