using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    class IndexRoot
    {
        /*0x00*/
        public uint Type { get; set; }//тип индексируемого атрибута
        /*0x04*/
        public uint CollarationRule { get; set; } //правило упорядочения в дереве
        /*0x08*/
        public uint IndexBlockSize { get; set; } //размер индексной записи в байтах 
        /*0x0C*/
        public byte ClustersPerIndexBlock { get; set; }//size of each index block (record) in clusters 
                                                       //либо логарифм размера
                                                       /*0x0D*/
        public byte[] Reserved { get; set; } //unused
        /*0x10*/
        public IndexHeader Index { get; set; } //заголовок индексного узла
    }
}
