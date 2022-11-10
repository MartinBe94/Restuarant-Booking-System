using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Labb_3
{
    internal class BookReserve:BookingSystem
    {
        public List<BookReserve> Bookedlist = new List<BookReserve>(); //Object list
        public string filename = "BookedTimeAndTable.json"; // string filename that have have the value from a json file with the name "BookedTimeAndTable.json"
        public BookReserve(string name, int tablenumber, DateTime date) //Construct that inherit from the abstract class BookingSystem
            : base(name, tablenumber, date)
        {
            Name = name;
            Tablenumber = tablenumber;
            Date = date;
        }
        public override async void SerializeWithAsync() //A async method
        {
            using FileStream createStream = File.Create(filename); // Using the FileStream to create a file from the string filename
            await JsonSerializer.SerializeAsync(createStream, Bookedlist); //JsonSerializer serializeAsync and create the stream with the object list Bookedlist value
            await createStream.DisposeAsync(); // await the Createstream and after DiposeAsync
        }
    }
}
