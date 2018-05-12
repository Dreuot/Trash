using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phonebook
{
    [Serializable]
    public class Contact
    {
        public string Phone { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public Bitmap Photo { get; set; } = null;

        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public string House { get; set; } = "";
        public string Flat { get; set; } = "";

        public string FullName => $"{LastName} {FirstName} {MiddleName}";
        public string Adress => $"{Country} г.{City} ул.{Street} д.{House}" + ((Flat == null || Flat == "") ? "" : $"кв.{Flat}");

        public Contact()
        {
        }

        public Contact(string phone, string firstName, string lastName, string middleName = "", string photo = "")
        {
            Phone = phone;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Photo = photo != "" ? new Bitmap(photo) : new Bitmap("default.png");
        }

        public void SetAddress(string address)
        {
            const string courntyPattern = @"(?<country>^([A-Za-zА-Яа-я]+))";
            const string cityPattern = @"[Гг].(?<city>([A-Za-zА-Яа-я]+))";
            const string streetPattern = @"[Уу]{1}[Лл]{1}.(?<street>([A-Za-zА-Яа-я]+))";
            const string homePattern = @"[Дд].(?<home>([A-Za-zА-Яа-я0-9]+))";
            const string flatPattern = @"[Кк]{1}[Вв]{1}.(?<flat>([A-Za-zА-Яа-я0-9]+))";

            try
            {
                Country = Regex.Match(address, courntyPattern).Groups["country"].Value;
                City = Regex.Match(address, cityPattern).Groups["city"].Value;
                Street = Regex.Match(address, streetPattern).Groups["street"].Value;
                House = Regex.Match(address, homePattern).Groups["home"].Value;
                Flat = Regex.Match(address, flatPattern).Groups["flat"].Value;
            }
            catch (Exception)
            {   
                throw new Exception("Адрес не распознан");
            }
            
        }
    }
}
