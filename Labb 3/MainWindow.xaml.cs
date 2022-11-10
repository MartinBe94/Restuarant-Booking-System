using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Labb_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BookReserve book = new BookReserve(null, 5, DateTime.MinValue);
        public MainWindow ()
        {
            InitializeComponent(); //Objects thats added into the Bookedlist that is in class BookReserve
            book.Bookedlist.Add(new BookReserve("Adam", 5, new DateTime(2022, 11, 12, 19, 00, 00)));
            book.Bookedlist.Add(new BookReserve("Sally", 4, new DateTime(2022, 11, 12, 18, 00, 00)));
            book.Bookedlist.Add(new BookReserve("Daniel", 3, new DateTime(2022, 11, 12, 19, 00, 00)));
            book.Bookedlist.Add(new BookReserve("Stella", 2, new DateTime(2022, 11, 12, 18, 00, 00)));
            for (DateTime timestart = DateTime.Parse("11:00:00"); timestart <= DateTime.Parse("22:30:00");) //For loop that set times for TimeComboBox
            {
                timestart = timestart.AddMinutes(15); // Add 15 minutes each loop
                book.time.Add(timestart.AddMinutes(15).ToShortTimeString()); // Add string 15 minutes into list time 
            }
            TimeComboBox.ItemsSource = book.time; // Add all string elements from List time into TimeComboBox

            for (int i = 1; i <= book.tablenumber; i++) //For that continues until it reach tablenumbers value that is 5
            {
                book.tables.Add(i); //Add 1 for each time the loop goes on
            }
            TableNumberBox.ItemsSource = book.tables; // Add all int elements from List tables into TableNumberBox

            foreach (var i in book.Bookedlist) //foreach that loops through the Bookedlist
            {
                BookedList.Items.Add(i.name + " // " + i.date + " // Table: " + i.tablenumber); // Add string,DateTime,Int value from Object List Bookedlist into Listbox BookedList
            }
            this.Datepicker.SelectedDate=DateTime.Now;// Set the Datepicker start selection to todays date
        }
        private async void Book_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text; // Give string name string value from NameBox
            DateTime datetimes = DateTime.Parse(Datepicker.Text + " " + TimeComboBox.Text); // datetimes get string value from Datepicker and TimeComboBox with DateTime.Parse
            int table = int.Parse(TableNumberBox.Text); // Give int table string value from TableNumberBox by int.Parse
            if (NameBox.Text.Equals("")) //If NameBox don't contain anything when Book button is clicked a Messagebox will appear and tell the user to fill in a name
            {
                book.SerializeWithAsync(); // Runs the SerializeWithSync method thats in BookReserve that contains a Json file with list items from List Bookedlist
                MessageBox.Show("Please fill in a name!");
            }
            else if (!book.Bookedlist.Any(x => x.date == datetimes && x.tablenumber == table))
            { // else if bookedlist date and tablenumber isn't equal to datetimes and table Bookedlist will add a new object and a new with new variables
                book.Bookedlist.Add(new BookReserve(name, table, datetimes));
                BookedList.Items.Add(name + " // " + datetimes + " // Table: " + table);
                book.SerializeWithAsync(); 
                MessageBox.Show("New booking registered!");
                NameBox.Clear();
            }
            else //if the time and tablenumber is equal a messagebox will show "The time and tablenumber is unavailable!"
            {
                book.SerializeWithAsync();
                MessageBox.Show("The time and tablenumber is unavailable!");
            }
        }
        private async void Cancel_Click(object sender, RoutedEventArgs e)
        {
            try // A try catch to check if wrong input or selection may occur
            {
                book.Bookedlist.RemoveAt(BookedList.SelectedIndex); //Remove the selectedIndex in object List Bookedlist
                BookedList.Items.RemoveAt(BookedList.SelectedIndex); //Remove the selectedIndex in Listbox BookedList
                File.Delete(book.filename); // Delete the Json file
                book.SerializeWithAsync(); //Method that recreates the Json file with new value from object List Bookedlist

                MessageBox.Show("Selected booking cancelled!"); //Messagebox shows "Selected booking cancelled!" if it did remove the item from list box
            }
            catch
            {
                MessageBox.Show("Please select a booking you want to cancel!"); //If index is not selected in Listbox BookedList Messagebox will show "Please select a booking you want to cancel!"
            }
        }
        private async void ShowBookings_Click(object sender, RoutedEventArgs e)
        {
            BookedList.Items.Clear(); // Clear the listbox from items so it is empty for new items to be added
            book.Bookedlist.Clear(); // Clear object list from objects so it wont be duplicate object in the object list when new items need to be added to the listbox BookedList
            using FileStream openStream = File.OpenRead(book.filename); //FileStream open up filename that is "BookedTimeAndTable.json" in Class BookReserve
            List<BookReserve>? getBookedlist =
                await JsonSerializer.DeserializeAsync<List<BookReserve>>(openStream); //And Deserialize the Json file
            getBookedlist.ForEach(i => book.Bookedlist.Add(new BookReserve(i.name, i.tablenumber, i.date))); // A ForEach that Add objects elements from the Json file itno object list Bookedlist
            getBookedlist.ForEach(i => BookedList.Items.Add((i.name + " // " + i.date + " // Table: " + i.tablenumber))); // A ForEach that Add items from object list Bookedlist into Listbox BookedList
        }
    }
}
