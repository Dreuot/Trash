using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook
{
    class ContactDisplay
    {
        private Contact inner;

        public string FullName => inner.FullName;
        public string Phone => inner.Phone;
        public string Address => inner.Adress;

        public ContactDisplay(Contact contact)
        {
            inner = contact;
        }
    }
}
