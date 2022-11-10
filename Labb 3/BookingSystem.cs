using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3
{
    abstract class BookingSystem
    {
        public List<string> time = new List<string>(); //List with string for TimeComboBox
        public List<int> tables = new List<int>(); //List with int for TableComboBox
        protected string Name; //Field with variables
        protected int Tablenumber;
        protected DateTime Date;
        protected int Tablecount;
        public BookingSystem(string name, int tablenumber, DateTime date) //Constructs
        {
            Name = name;
            Tablenumber = tablenumber;
            Date = date;
        }
        public string name //Property string name that gets the string Name and return and after set the value
        {
            get { return Name; }
            set { Name = value; }
        }
        public int tablenumber //Property int tablenumber that gets the int tablenumber and return and after set the value
        {
            get { return Tablenumber; }
            set { Tablenumber = value; }
        }
        public DateTime date //Property DateTime that gets the DateTime date and return and after set the value
        {
            get { return Date; }
            set { Date = value; }
        }
        public abstract void SerializeWithAsync(); //A abstract void method
    }
}
