using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    public class Attribute
    {
        /*0x00*/
        public AttributeTypes Type { get; private set; } //тип атрибута
        /*0x04*/
        public ushort Length { get; private set; } //длина заголовка используется для перехода к следующему   атрибуту
        /*0x06*/
        public ushort Reserved { get; private set; }
        /*0x08*/
        public byte NonResidentFlg { get; private set; } //1 если атрибут нерезидентный, 0 - резидентный
        /*0x09*/
        public byte NameLength { get; private set; } //длина имени атрибута, в символах
        /*0x0A*/
        public ushort NameOffset { get; private set; } //смещение имени атрибута, относительно заголовка
                                                       //атрибута
                                                       /*0x0C*/
        public ushort Flags { get; private set; } //флаги, перечислены в ATTR_FLAGS
        /*0x0E*/
        public ushort Instance { get; private set; }

        public Resident Resident { get; private set; }
        public NonResident NonResident { get; private set; }

        public Attribute(byte[] sector, int offset)
        {
            int t = 0;
            for (int i = 0; i < 4; i++)
                t += sector[offset + i] << (i * 8);

            Type = (AttributeTypes)t;
            if (Type != AttributeTypes.AT_END)
            {
                Length = 0;
                for (int i = 0; i < 2; i++)
                    Length += (ushort)(sector[offset + 0x04 + i] << (i * 8));

                Reserved = 0;
                for (int i = 0; i < 2; i++)
                    Reserved += (ushort)(sector[offset + 0x06 + i] << (i * 8));

                NonResidentFlg = sector[offset + 0x08];

                NameLength = sector[offset + 0x09];

                NameOffset = 0;
                for (int i = 0; i < 2; i++)
                    NameOffset += (ushort)(sector[offset + 0x0A + i] << (i * 8));

                Flags = 0;
                for (int i = 0; i < 2; i++)
                    Flags += (ushort)(sector[offset + 0x0C + i] << (i * 8));

                Instance = 0;
                for (int i = 0; i < 2; i++)
                    Instance += (ushort)(sector[offset + 0x0E + i] << (i * 8));

                if (NonResidentFlg == 1)
                    NonResident = new NonResident(sector, offset);
                else
                    Resident = new Resident(sector, offset);
            }
        }

        public override string ToString()
        {
            string result = "";
            result += "Тип атрибута:" + Type.ToString() + "\n";
            result += "Длина заголовка:" + Length.ToString() + "\n";
            if (NonResidentFlg == 1)
                result += "Атрибут нерезидентный\n";
            else
                result += "Атрибут резидентный\n";
            result += "Длина имени атрибута:" + NameLength.ToString() + "\n";
            result += "Смещение имени атрибута:" + NameOffset.ToString() + "\n";
            result += "Флаги:" + Flags.ToString() + "\n";
            result += "Идентификатор атрибута:" + Instance.ToString() + "\n";
            if (NonResidentFlg == 1)
                result += NonResident.ToString();
            else
                result += Resident.ToString();

            return result;
        }
    }
}
