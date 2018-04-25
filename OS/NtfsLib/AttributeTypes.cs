using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    public enum AttributeTypes : uint
    {
        AT_STANDARD_INFORMATION = 0x10,
        AT_ATTRIBUTE_LIST = 0x20,
        AT_FILE_NAME = 0x30,
        AT_OBJECT_ID = 0x40,
        AT_SECURITY_DESCRIPTOR = 0x50,
        AT_VOLUME_NAME = 0x60,
        AT_VOLUME_INFORMATION = 0x70,
        AT_DATA = 0x80,
        AT_INDEX_ROOT = 0x90,
        AT_INDEX_ALLOCATION = 0xa0,
        AT_BITMAP = 0xb0,
        AT_REPARSE_POINT = 0xc0,
        AT_END = 0xffffffff
    }
}
