using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    public class IndexHeaderDir
    {
        /*0x00*/
        public ulong IndexedFile { get; set; } //адрес MFT файла
        /*0x08*/
        public ushort Length { get; set; } //смещение следующего элемента, относительно текущего
        /*0x0A*/
        public ushort KeyLength { get; set; } //длина атрибута $FILE_NAME
        /*0x0C*/
        public int Flags { get; set; } //флаги
        /*0x10*/
        public byte[] FileName { get; set; }//сам атрибут $FILE_NAME, если key_length 
                                     //больше нуля.

        public string FileNameString
        {
            get
            {
                char[] chars = new char[FileName.Length];
                Encoding.Unicode.GetChars(FileName, 0, chars.Length, chars, 0);

                return new string(chars);
            }
        }
    }
}
