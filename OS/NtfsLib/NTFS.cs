using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    /// <summary>
    /// Объектное представление файловой системы
    /// </summary>
    public class NTFS
    {
        /// <summary>
        /// Представление диска как файла для прямого чтения
        /// </summary>
        public SafeFileHandle Drive { get; set; }

        /// <summary>
        /// Объектное представление блока параметров биос
        /// </summary>
        public BPB BPB { get; set; }

        /// <summary>
        /// Запись МФТ, описывающая саму МФТ
        /// </summary>
        public MFT MFT { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="drive">Имя логического диска в формате C:</param>
        public NTFS(string drive)
        {
            drive = "\\\\.\\" + drive;

            // Создаем файл для чтения с диска
            Drive = HD_API.CreateFile(
                drive,
                HD_API.GENERIC_READ,
                HD_API.FILE_SHARE_READ | HD_API.FILE_SHARE_WRITE,
                IntPtr.Zero,
                HD_API.OPEN_EXISTING,
                0,
                IntPtr.Zero
                );

            BPB = new BPB(Drive); // считываем блок параметров БИОС
            MFT = GetFirstMFT();// считываем первую запись МФТ
        }


        /// <summary>
        /// Чтение произвольного сектора с диска
        /// </summary>
        /// <param name="sectorNum">Номер сектора</param>
        /// <returns>Массив байт размером BPB.BYTE_IN_SECTOR</returns>
        private unsafe byte[] _ReadSector(int sectorNum)
        {
            byte[] bytes = new byte[BPB.BYTE_IN_SECTOR]; // сектор в виде одномерного массива байтов
            IntPtr BytesRead = IntPtr.Zero;
            ulong pointer = (ulong)sectorNum * (ulong)BPB.BYTE_IN_SECTOR;
            int hight = (int)(pointer >> 32);
            HD_API.SetFilePointer(Drive, (int)(pointer & 0xffffffff), out hight, HD_API.EMoveMethod.Begin);

            fixed (byte* ptr = bytes)
            {
                HD_API.ReadFile(Drive, ptr, BPB.BYTE_IN_SECTOR, BytesRead, IntPtr.Zero); // Считывание сектора
            };

            HD_API.SetFilePointer(Drive, 0, out hight, HD_API.EMoveMethod.Begin);

            return bytes;
        }

        /// <summary>
        /// Чтение произвольного сектора диска с проверкой
        /// </summary>
        /// <param name="num">Номер сектора</param>
        /// <returns>Сектор диска в виде массива байт</returns>
        public byte[] ReadSector(int num)
        {
            if ((ulong)num >= BPB.SecCount)
                throw new ArgumentException($"Слишком большой номер сектора {num}. Максимальный номер сектора {BPB.SecCount}");

            return _ReadSector(num);
        }

        /// <summary>
        /// Преобразование массива байт в HEX форму
        /// </summary>
        /// <param name="sector">Массив байт</param>
        /// <returns>Сектор диска в виде массива строк</returns>
        public string[] SectorToHex(byte[] sector)
        {
            string[] SectorHex = new string[sector.Length];

            for (int i = 0; i < sector.Length; i++)
                SectorHex[i] = BitConverter.ToString(new byte[] { sector[i] }); // Преобразование к шестнадцатиричному виду

            return SectorHex;
        }

        /// <summary>
        /// Чтение сектора по номеру кластера
        /// </summary>
        /// <param name="num">Номер кластера</param>
        /// <returns>Сектор диска в виде массива байт</returns>
        public byte[] ReadSectorByClusterNum(int num)
        {
            int sectorNum = num * BPB.SectorPerCluster;//* 8;
            if ((ulong)sectorNum >= BPB.SecCount)
            {
                throw new ArgumentException($"Слишком большой номер кластера {num}. Максимальный номер сектора {BPB.SecCount}");
            }

            return _ReadSector(sectorNum);
        }

        /// <summary>
        /// Чтение произвольного кластера диска
        /// </summary>
        /// <param name="cluster">Номер кластера</param>
        /// <returns>Кластер диска в виде массива байт</returns>
        public byte[] ReadCluster(int cluster)
        {
            byte[] ByteRecord = new byte[BPB.SectorPerCluster * BPB.BytePerSec];
            for (int i = 0; i < ByteRecord.Length / 512; i++)
            {
                Array.Copy(ReadSector((int)(cluster * BPB.SectorPerCluster) + i), 0, ByteRecord, i * BPB.BytePerSec, BPB.BytePerSec); // Так как кластер состоит из нескольких секторов, чтение производится
            }                                                                                                                         // посекторно

            return ByteRecord;
        }


        /// <summary>
        /// Чтение первой записи МФТ
        /// </summary>
        /// <returns>Возвращает объектное представление первой записи МФТ</returns>
        public MFT GetFirstMFT()
        {
            byte[] ByteRecord = new byte[(int)Math.Pow(2, BPB.ClustersPerMFT * -1)]; // массив байт, совпадающий размером с записью в МФТ, вычисляется с помощью BPB.ClustersPerMFT
            for (int i = 0; i < ByteRecord.Length / 512; i++)
            {
                Array.Copy(ReadSector((int)(BPB.FirstClusterMFT * BPB.SectorPerCluster) + i), 0, ByteRecord, i * BPB.BytePerSec, BPB.BytePerSec);
            }

            return new MFT(ByteRecord, this);
        }

        /// <summary>
        /// Чтение произвольной записи МФТ
        /// </summary>
        /// <param name="indexMFT">Номер записи МФТ</param>
        /// <returns>Возвращает объектное представление записи МФТ</returns>
        public MFT GetMftRecord(int indexMFT)
        {
            int mftSize = (int)Math.Pow(2, BPB.ClustersPerMFT * -1);
            int recordInCluster = BPB.SectorPerCluster * BPB.BytePerSec / mftSize;
            int sector = FindSector(indexMFT); // Поиск номера первого сектора записи МФТ, необходимо потому, что сама МФТ может быть фрагментирована

            byte[] ByteRecord = new byte[mftSize];
            for (int i = 0; i < ByteRecord.Length / 512; i++)
            {
                Array.Copy(ReadSector(sector + i), 0, ByteRecord, i * BPB.BytePerSec, BPB.BytePerSec); // считываем запись
            }

            return new MFT(ByteRecord, this); // Создаем и возвращаем объектное представление записи МФТ
        }


        /// <summary>
        /// Поиск номера первого сектора записи МФТ
        /// </summary>
        /// <param name="indexMFT">Номер записи МФТ</param>
        /// <returns>Номер первого сектора записи МФТ</returns>
        private int FindSector(int indexMFT)
        {
            int mftSize = (int)Math.Pow(2, BPB.ClustersPerMFT * -1); // Размер записи МФТ
            int recordInCluster = BPB.SectorPerCluster * BPB.BytePerSec / mftSize; // Подсчет количества записей МФТ в одном кластере
            Attribute data = MFT.Attributes.Where(n => n.Type == AttributeTypes.AT_DATA).FirstOrDefault(); // из аттрибутов первой записи МФТ выбираем аттрибут DATA
            int prevMin = 0;
            int prevMax = 0;
            int maxRec = 0;
            int minRec = 0;
            int run = 0;
            for (int i = 0; i < data.NonResident.Clusters.Count; i++) // Для всех отрезков, в которых хранится нерезидентный атрибут DATA
            {
                minRec = prevMax; // Максимальный номер записи МФТ в предыдущем отрезке (0 для первого отрезка), является минимальным номером записи в текущем отрезке
                maxRec = (int)(data.NonResident.Clusters[i].End - data.NonResident.Clusters[i].Start) * recordInCluster + prevMax;  // Максимальный номер записи МФТ в текущем отрезке
                if (indexMFT < maxRec) // Если запись номер записи МФТ попадает в текущий отрезок, то запоминаем номер отрезка
                {
                    run = i;
                    break;
                }

                prevMin = minRec;
                prevMax = maxRec;
            }

            int startSectorOfCluster = (int)data.NonResident.Clusters[run].Start * BPB.SectorPerCluster; // Вычисляем начальный сектор отрезка, в котором хранится искомая запись
            int recordInRun = indexMFT - minRec; // вычисляем номер записи внутри отрезка
            int number = startSectorOfCluster + recordInRun * (mftSize / BPB.BytePerSec); // вычисляем первый сектор записи МФТ

            return number;
        }
    }
}
