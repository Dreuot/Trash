using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    class IndexHeader
    {
        /*0x00*/
        public uint EntriesOffset { get; set; } //байтовое смещение первого индексного элемента, 
                                                //относительно заголовка узла
                                                /*0x04*/
        public uint IndexLength { get; set; } //размер узла в байтах
        /*0x08*/
        public uint AllocatedSize { get; set; } //выделенный размер узла
        /*0x0C*/
        public uint Flags { get; set; }
    }
}
