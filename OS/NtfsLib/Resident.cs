using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    //Резидентный атрибут
    public class Resident
    {
        /*0x10*/
        public uint ValueLength { get; private set; } //размер, в байтах, тела атрибута
                                                      /*0x14*/
        public ushort ValueOffset { get; private set; } //байтовое смещение тела, относительно заголовка 
                                                        //атрибута
                                                        /*0x16*/
        public byte ResidentFlags { get; private set; } //флаги, перечислены в RESIDENT_ATTR_FLAGS
                                                        /*0x17*/
        public byte Reserved { get; private set; }

        public byte[] Value { get; private set; }

        public Resident(byte[] sector, int offset)
        {
            ValueLength = 0;
            for (int i = 0; i < 4; i++)
                ValueLength += (uint)sector[offset + 0x10 + i] << (i * 8);

            ValueOffset = 0;
            for (int i = 0; i < 4; i++)
                ValueOffset += (ushort)(sector[offset + 0x14 + i] << (i * 8));

            Value = new byte[ValueLength];
            for (int i = 0; i < Value.Length; i++)
                Value[i] = sector[ValueOffset + i];

            ResidentFlags = sector[offset + 0x16];
            Reserved = sector[offset + 0x17];
        }

        public override string ToString()
        {
            string result = "";

            result += "Размер тела атрибута:" + ValueLength.ToString() + "\n";
            result += "Смещение тела относительно заголовка:" + ValueOffset.ToString() + "\n";
            result += "Резидентные флаги:" + ResidentFlags.ToString() + "\n";

            return result;
        }
    }
}
