using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    /// <summary>
    /// Объектное представление записи МФТ
    /// Содержит заголовок МФТ, заголовки всех атрибутов, имя файла, индексные элементы для каталогов
    /// </summary>
    public class MFT
    {
        public string Signature { get; set; }
        public ushort UsaOffset { get; set; }
        public ushort UsaCount { get; set; }
        public int LSN { get; set; }
        public ushort SequenceNumber { get; set; }
        public ushort LinkCount { get; set; }
        public ushort AttrsOffset { get; set; }
        public ushort Flags { get; set; }
        public uint BytesInUse { get; set; }
        public uint BytesAllocated { get; set; }
        public ulong BaseMFTRecord { get; set; }
        public ushort NextAttrInstance { get; set; }
        public ushort Reserved { get; set; }
        public uint MFTRecordNumber { get; set; }
        public uint IndexBlockSize { get; set; }

        public byte[] Inner { get; private set; }
        public string FileName { get; private set; }

        public ulong ParentDir { get; set; }

        public List<Attribute> Attributes { get; set; }
        public List<IndexHeaderDir> Indexes { get; set; }
        public NTFS NTFS { get; set; }

        public override string ToString()
        {
            string result = "";

            result += "Сигнатура:" + Signature + "\n";
            result += "Смещение массива данных:" + UsaOffset.ToString() + "\n";
            result += "Размер в словах номера последовательности обновления:" + UsaCount.ToString() + "\n";
            result += "Номер последовательности файла транзакций:" + LSN.ToString() + "\n";
            result += "Номер последовательности:" + SequenceNumber.ToString() + "\n";
            result += "Счетчик жестких ссылок:" + LinkCount.ToString() + "\n";
            result += "Смещение первого атрибута:" + AttrsOffset.ToString() + "\n";
            result += "Флаги записи:" + Flags.ToString() + "\n";
            result += "Реальный размер файловой записи в байтах :" + BytesInUse.ToString() + "\n";
            result += "Выделенный размер файловой записи:" + BytesAllocated.ToString() + "\n";
            result += "Ссылка на базовую файловую запись:" + BaseMFTRecord.ToString() + "\n";
            result += "Идентификатор следующего атрибута:" + NextAttrInstance.ToString() + "\n";
            result += "Индекс данной файловой записи:" + MFTRecordNumber.ToString() + "\n";

            return result;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sector">Запись МФТ в виде массива байт</param>
        /// <param name="ntfs">Объектное представление файловой системы</param>
        public MFT(byte[] sector, NTFS ntfs)
        {
            NTFS = ntfs;
            Indexes = new List<IndexHeaderDir>(); // пустой список индексных элементов
            Inner = sector;
            FillData(sector); // заполяем заголовок МФТ
            LoadAttributes(sector); // Считываем аттрибуты
            ParentDir = 0;
        }

        /// <summary>
        /// Чтение заголовком атрибутов
        /// </summary>
        /// <param name="sector">Запись МФТ в виде массива байт</param>
        private void LoadAttributes(byte[] sector)
        {
            Attributes = new List<Attribute>(); // создаем пустой список аттрибутов
            int offset = AttrsOffset; // смещение до первого аттрибута
            Attribute Attribute = new Attribute(sector, offset); // считываем первый аттрибут
            Attributes.Add(Attribute); //сохраняем в списке аттрибутов
            if (Signature != "FILE") // если текущая запись МФТ не файл, то прекращаем чтение
                return;
            while (Attribute.Type != AttributeTypes.AT_END) // пока не дошли до последнего аттрибута
            {
                if (Attribute.NonResidentFlg == 1) // Если аттрибут нерезидентный, то считываем его список отрезков и высчитываем адреса занимаемых кластеров
                {
                    int runListStart = offset + Attribute.NonResident.MappingPairOffset; // начало списка отрезков
                    int currenrRunList = runListStart;
                    byte RunListCurrentByte = sector[currenrRunList];
                    byte NumByteInRunOffset = (byte)(RunListCurrentByte >> 4); // сколько байт отводится под адрес начального кластера
                    byte NumByteInRunLen = (byte)(RunListCurrentByte & 0x0F); // сколько байт отводится под длину отрезка
                    currenrRunList++;
                    int currentSeg = 0;
                    do
                    {
                        LineSegment seg = new LineSegment(); // создаем новый отрезок
                        int segmentLength = 0;
                        for (int i = 0; i < NumByteInRunLen; i++)
                        {
                            segmentLength += sector[currenrRunList] << (i * 8);
                            currenrRunList++;
                        }

                        for (int i = 0; i < NumByteInRunOffset; i++)
                        {
                            seg.Start += (ulong)sector[currenrRunList] << (i * 8);
                            currenrRunList++;
                        }

                        if(currentSeg != 0)
                        {
                            seg.Start += Attribute.NonResident.Clusters[currentSeg - 1].Start; // высчитываем LCN начального кластера
                        }

                        seg.End = seg.Start + (ulong)segmentLength; // высчитываем адрес конечного кластера отрезка

                        Attribute.NonResident.Clusters.Add(seg);
                        RunListCurrentByte = sector[currenrRunList];
                        // переходим к следующему отрезку
                        NumByteInRunOffset = (byte)(RunListCurrentByte >> 4);
                        NumByteInRunLen = (byte)(RunListCurrentByte & 0x0F);
                        currenrRunList++;
                        currentSeg++;
                    } while (RunListCurrentByte != 0); // подка не закончится список отрезков
                }

                if(Attribute.Type == AttributeTypes.AT_INDEX_ROOT && FileName != "$Secure") // если аттрибут INDEX_ROOT, то считываем индексные элементы
                {
                    Indexes.AddRange(IndexElements(sector, offset, Attribute));
                }

                if(Attribute.Type == AttributeTypes.AT_INDEX_ALLOCATION && FileName != "$Secure") // если аттрибут INDEX_ALLOCATIOB, то считываем индексные записи из списка отрезков, и читаем из них индексные элементы
                {
                    Indexes.AddRange(IndexAllocationElements(this, Attribute));
                }

                if (Attribute.Type == AttributeTypes.AT_FILE_NAME) // если аттрибут FILE_NAME, то читаем имя файла
                {
                    ParentDir = 0;
                    for (int i = 0; i < 6; i++)
                        ParentDir += (ulong)sector[offset + Attribute.Resident.ValueOffset + i] << (i * 8);

                    byte[] chars = new byte[sector[offset + 0x58] * 2];
                    for (int i = 0; i < chars.Length; i += 2)
                        chars[i] = (byte)(sector[offset + 0x5A + i] + (sector[offset + 0x5A + i + 1] << 8));

                    char[] unicode = new char[sector[offset + 0x58]];
                    Encoding.Unicode.GetChars(chars, 0, chars.Length, unicode, 0);
                    FileName = new string(unicode);
                }


                offset += Attribute.Length;
                Attribute = new Attribute(sector, offset); // считываем следующий аттрибут
                Attributes.Add(Attribute);
            }
        }

        /// <summary>
        /// Чтение индексных элементов из аттрибута INDEX_ALLOCATION
        /// </summary>
        /// <param name="mft">Запись МФТ, которой принадлежит аттрибут</param>
        /// <param name="attr">Аттрибут INDEX_ALLOCATION</param>
        /// <returns></returns>
        public List<IndexHeaderDir> IndexAllocationElements(MFT mft, Attribute attr)
        {
            if (attr.Type != AttributeTypes.AT_INDEX_ALLOCATION)
                throw new ArgumentException("Incorrect type of attribute");

            var bpb = NTFS.BPB;
            int bytePerCluster = bpb.BytePerSec * bpb.SectorPerCluster;

            List<IndexHeaderDir> indexes = new List<IndexHeaderDir>(); // список индексных элементов
            for (int i = 0; i < attr.NonResident.Clusters.Count; i++) // перебираем все отрезки нерезидентного аттрибута
            {
                int curClus = (int)attr.NonResident.Clusters[i].Start; // текущий кластер
                int clusters = (int)(attr.NonResident.Clusters[i].End - attr.NonResident.Clusters[i].Start); // количество кластеров в отрезке
                // читаем весь отрезок
                byte[] run = new byte[clusters * (uint)bytePerCluster]; 
                List<byte> list = new List<byte>();
                for (int j = 0; j < clusters; j++)
                {
                    list.AddRange(NTFS.ReadCluster(curClus));
                    curClus++;
                }

                run = list.ToArray();

                int count = (int)(attr.NonResident.DataSize / mft.IndexBlockSize); // количество индексных
                for (int k = 0; k < count; k++) // перебираем индексные записи
                {
                    int offset = (int)(0 + IndexBlockSize * k);
                    IndexAllocation ia = new IndexAllocation(); // читаем заголовок INDEX_ALLOCATION
                    ia.Signature = new byte[4];
                    for (int j = 0; j < 4; j++)
                        ia.Signature[j] = run[offset + j];

                    for (int j = 0; j < 2; j++)
                        ia.UsaOffset += (ushort)(run[offset + 0x04 + j] << (j * 8));

                    for (int j = 0; j < 2; j++)
                        ia.UsaCount += (ushort)(run[offset + 0x06 + j] << (j * 8));

                    for (int j = 0; j < 8; j++)
                        ia.Lsn += (ulong)run[offset + 0x08 + j] << (j * 8);

                    for (int j = 0; j < 8; j++)
                        ia.IndexBlockVCN += (ulong)run[offset + 0x10 + j] << (j * 8);

                    IndexHeader ih = new IndexHeader(); // читаем заголовок индексной записи
                    offset += 0x18;
                    for (int j = 0; j < 4; j++)
                        ih.EntriesOffset += (uint)run[offset + j + 0x00] << (j * 8);

                    for (int j = 0; j < 4; j++)
                        ih.IndexLength += (uint)run[offset + j + 0x04] << (j * 8);

                    for (int j = 0; j < 4; j++)
                        ih.AllocatedSize += (uint)run[offset + j + 0x08] << (j * 8);

                    for (int j = 0; j < 4; j++)
                        ih.Flags += (uint)run[offset + j + 0x0C] << (j * 8);

                    offset += (int)ih.EntriesOffset;



                    IndexHeaderDir ind; // читаем индексные элементы
                    do
                    {
                        ind = new IndexHeaderDir();
                        for (int j = 0; j < 6; j++)
                            ind.IndexedFile += (ulong)run[(uint)offset + 0x00 + (ulong)j] << (j * 8);

                        for (int j = 0; j < 2; j++)
                            ind.Length += (ushort)(run[(uint)offset + 0x08 + (ulong)j] << (j * 8));

                        for (int j = 0; j < 2; j++)
                            ind.KeyLength += (ushort)(run[(uint)offset + 0x0A + (ulong)j] << (j * 8));

                        for (int j = 0; j < 4; j++)
                            ind.Flags += run[(uint)offset + 0x0C + (ulong)j] << (j * 8);

                        if ((ind.Flags & 2) == 2)
                            break;

                        ind.FileName = new byte[ind.KeyLength];
                        for (int j = 0; j < ind.KeyLength; j++)
                            ind.FileName[j] = run[(uint)offset + 0x10 + (ulong)j];

                        string fn = "";
                        if (ind.KeyLength > 0)
                        {
                            int length = ind.FileName[0x40];
                            for (int g = 0; g < length * 2; g += 2)
                                fn += (char)(ind.FileName[0x42 + g] + (ind.FileName[0x42 + g + 1] << 8));
                        }

                        ind.FileNameString = fn;

                        if (ind.Flags != 2)
                            indexes.Add(ind);

                        offset += ind.Length;
                    } while (ind.Flags != 2); // пока не встретим элемент с флагом 2 (т.е. последний)
                }
            }

            return indexes;
        }

        /// <summary>
        /// Считываем индексные элементы из аттрибута INDEX_ROOT
        /// </summary>
        /// <param name="sector">Запись МФТ в виде массива байт</param>
        /// <param name="offset">Смещение до начала аттрибута</param>
        /// <param name="attr">Аттрибут</param>
        /// <returns></returns>
        private List<IndexHeaderDir> IndexElements(byte[] sector, int offset, Attribute attr)
        {
            List<IndexHeaderDir> indexes = new List<IndexHeaderDir>();
            int bodyOffset = attr.Resident.ValueOffset; // смещение до тела аттрибута
            IndexRoot indexRoot = new IndexRoot(); // заголовок INDEX_ROOT
            for (int i = 0; i < 4; i++)
                indexRoot.Type += (uint)sector[offset + bodyOffset + i] << (i * 8);

            for (int i = 0; i < 4; i++)
                indexRoot.CollarationRule += (uint)sector[offset + bodyOffset + i + 0x04] << (i * 8);

            for (int i = 0; i < 4; i++)
                indexRoot.IndexBlockSize += (uint)sector[offset + bodyOffset + i + 0x08] << (i * 8);

            indexRoot.ClustersPerIndexBlock += sector[offset + bodyOffset + 0x0C];

            indexRoot.Reserved = new byte[3];
            for (int i = 0; i < 3; i++)
                indexRoot.Reserved[i] = sector[offset + bodyOffset + 0x0D + i];

            IndexBlockSize = indexRoot.IndexBlockSize;

            IndexHeader indexHeader = new IndexHeader(); // Заголовок индексной записи
            int indexHeaderOffset = 0x10 + offset + bodyOffset;
            for (int i = 0; i < 4; i++)
                indexHeader.EntriesOffset += (uint)sector[indexHeaderOffset + i + 0x00] << (i * 8);

            for (int i = 0; i < 4; i++)
                indexHeader.IndexLength += (uint)sector[indexHeaderOffset + i + 0x04] << (i * 8);

            for (int i = 0; i < 4; i++)
                indexHeader.AllocatedSize += (uint)sector[indexHeaderOffset + i + 0x08] << (i * 8);

            for (int i = 0; i < 4; i++)
                indexHeader.Flags += (uint)sector[indexHeaderOffset + i + 0x0C] << (i * 8);

            ulong firstIndexElement = 0x10 + indexHeader.EntriesOffset + (ulong)bodyOffset + (ulong)offset;
            ulong current = firstIndexElement;

            IndexHeaderDir ind;
            do//считываем индексные элементы
            {
                ind = new IndexHeaderDir();
                for (int i = 0; i < 6; i++)
                    ind.IndexedFile += (ulong)sector[current + 0x00 + (ulong)i] << (i * 8);

                for (int i = 0; i < 2; i++)
                    ind.Length += (ushort)(sector[current + 0x08 + (ulong)i] << (i * 8));

                for (int i = 0; i < 2; i++)
                    ind.KeyLength += (ushort)(sector[current + 0x0A + (ulong)i] << (i * 8));

                for (int i = 0; i < 4; i++)
                    ind.Flags += sector[current + 0x0C + (ulong)i] << (i * 8);

                if ((ind.Flags & 2) == 2)
                    break;

                ind.FileName = new byte[ind.KeyLength];
                for (int i = 0; i < ind.KeyLength; i++)
                    ind.FileName[i] = sector[current + 0x10 + (ulong)i];

                string fn = "";
                if (ind.KeyLength > 0)
                {
                    int length = ind.FileName[0x40];
                    for (int g = 0; g < length * 2; g += 2)
                        fn += (char)(ind.FileName[0x42 + g] + (ind.FileName[0x42 + g + 1] << 8));
                }

                ind.FileNameString = fn;

                if (ind.Flags != 2)
                    indexes.Add(ind);

                current += ind.Length;
            } while (ind.Flags != 2);// пока не встретим элемент с флагом 2 (т.е. последний)

            return indexes;
        }

        /// <summary>
        /// Чтение заголовка МФТ
        /// </summary>
        /// <param name="sector">Запись МФТ в виде массива байт</param>
        private void FillData(byte[] sector)
        {
            Signature = "";
            for (int i = 0; i < 4; i++)
                Signature += (char)sector[0x00 + i];

            UsaOffset = 0;
            for (int i = 0; i < 2; i++)
                UsaOffset += (ushort)(sector[0x04 + i] << (i * 8));

            UsaCount = 0;
            for (int i = 0; i < 2; i++)
                UsaCount += (ushort)(sector[0x06 + i] << (i * 8));

            LSN = 0;
            for (int i = 0; i < 8; i++)
                LSN += sector[0x08 + i] << (i * 8);

            SequenceNumber = 0;
            for (int i = 0; i < 2; i++)
                SequenceNumber += (ushort)(sector[0x10 + i] << (i * 8));

            LinkCount = 0;
            for (int i = 0; i < 2; i++)
                LinkCount += (ushort)(sector[0x12 + i] << (i * 8));

            AttrsOffset = 0;
            for (int i = 0; i < 2; i++)
                AttrsOffset += (ushort)(sector[0x14 + i] << (i * 8));

            Flags = 0;
            for (int i = 0; i < 2; i++)
                Flags += (ushort)(sector[0x16 + i] << (i * 8));

            BytesInUse = 0;
            for (int i = 0; i < 4; i++)
                BytesInUse += (uint)sector[0x18 + i] << (i * 8);

            BytesAllocated = 0;
            for (int i = 0; i < 4; i++)
                BytesAllocated += (uint)sector[0x1C + i] << (i * 8);

            BaseMFTRecord = 0;
            for (int i = 0; i < 8; i++)
                BaseMFTRecord += (ulong)sector[0x20 + i] << (i * 8);

            NextAttrInstance = 0;
            for (int i = 0; i < 2; i++)
                NextAttrInstance += (ushort)(sector[0x28 + i] << (i * 8));

            Reserved = 0;
            for (int i = 0; i < 2; i++)
                Reserved += (ushort)(sector[0x2A + i] << (i * 8));

            MFTRecordNumber = 0;
            for (int i = 0; i < 4; i++)
                MFTRecordNumber += (uint)sector[0x2C + i] << (i * 8);
        }
    }
}
