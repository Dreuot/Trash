using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook
{
    class ContactDisplay
    {
        private Contact inner;

        [DisplayName("Полное имя")]
        public string FullName => inner.FullName;

        [DisplayName("Телефон")]
        public string Phone => inner.Phone;

        [DisplayName("Адрес")]
        public string Address => inner.Address;

        public ContactDisplay(Contact contact)
        {
            inner = contact;
        }
    }
}
