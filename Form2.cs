using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {

        public static Form2 instance;

        public Form2()
        {
            InitializeComponent();
            instance = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            loadCusomers();

            setRooms();
            setRezervation();
            loadRezervations();

            textBoxTotalCost.ReadOnly = true;

        }

        public string customerFilePath = @"D:\Skola\TVP\WindowsFormsApp1\WindowsFormsApp1\Customers.txt";
        public string roomsFilePath = @"D:\Skola\TVP\WindowsFormsApp1\WindowsFormsApp1\Rooms.txt";
        public string rezervationFilePath = @"D:\Skola\TVP\WindowsFormsApp1\WindowsFormsApp1\Rezervations.txt";

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveFiles(customerFilePath, customers);
            saveRooms(roomsFilePath, rooms);
            saveRezervations();
            Application.Exit();
        }

        List<Guest> customers = new List<Guest>();
        List<Room> rooms = new List<Room>();

        List<Room> activeRoooms;

        //Push guest
        private void ButtonAddGuest_Click(object sender, EventArgs e)
        {

            checkField(textName, false);
            checkField(textLastName, false);
            checkField(textNumber, true);

            Guest newGuest = new Guest(textName.Text, textLastName.Text, int.Parse(textNumber.Text));
            listBox1.Items.Add(newGuest.ToString());

            customers.Add(newGuest);
            textSelectedGuest.Text = newGuest.id.ToString();

        }

        private void checkField(TextBox textBox, bool isNumber)
        {

            if (isNumber == true)
            {

                var onlyNumber = new StringBuilder();

                for (int i = 0; i < textBox.Text.Length; i++)
                {
                    if (textBox.Text[i] >= '0' && textBox.Text[i] <= '9')
                    {
                        onlyNumber.Append(textBox.Text[i]);
                    }
                }

                textBox.Text = onlyNumber.ToString();

            }

            if (textBox.Text.Length == 0)
            {
                if (isNumber == true)
                {
                    textBox.Text = "0";
                }
                else
                {
                    textBox.Text = "empty";
                }
            }
        }

        string cutChars(string stringToCut)
        {

            var onlyNumber = new StringBuilder();

            for (int i = 0; i < stringToCut.Length; i++)
            {
                if (stringToCut[i] >= '0' && stringToCut[i] <= '9')
                {
                    onlyNumber.Append(stringToCut[i]);
                }
            }

            stringToCut = onlyNumber.ToString();
            return stringToCut;
        }

        //Delete guest
        private void ButtonDeleteGuest_Click(object sender, EventArgs e)
        {

            textSelectedGuest.Text = cutChars(textSelectedGuest.Text);

            if (textSelectedGuest.Text.Length == 0)
            {
                return;
            }

            string textField = textSelectedGuest.Text.ToString();
            int toDelite = int.Parse(textField);

            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].id == toDelite)
                {
                    customers.RemoveAt(i);
                    Guest.counter--;
                    break;
                }
            }

            textDebug.Text = "Deleted ID: " + textSelectedGuest.Text;

            textSelectedGuest.Clear();
            updateCustomers();

        }

        private void updateCustomers()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(customers.ToArray());
        }

        public int checkFreeID(int counter)
        {

            textDebug.Text = counter.ToString();

            for (int i = 0; i < customers.Count; i++)
            {
                for (int k = 0; k < customers.Count; k++)
                {
                    if (i == customers[k].id) //Same slot
                    {
                        break;
                    }
                    else if ((k + 1) == customers.Count) //Free slot
                    {
                        return i;
                    }
                }
            }

            return counter;

        }


        private void saveFiles(string filePath, List<Guest> list)
        {

            List<string> lines = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                lines.Add(list[i].ToString());
            }

            File.WriteAllLines(filePath, lines);

        }

        private void loadCusomers()
        {

            List<string> decode = new List<string>();
            List<string> allLines = new List<string>();

            allLines = File.ReadAllLines(customerFilePath).ToList();

            if (allLines.Count == 0)
            {
                return;
            }

            for (int i = 0; i < allLines.Count; i++)
            {

                var separateWord = new StringBuilder();

                for (int k = 0; k < allLines[i].Length; k++)
                {
                    if (allLines[i][k] == ' ' || allLines[i][k] == '\n') //Add word - Fresh slot
                    {
                        decode.Add(separateWord.ToString());
                        separateWord.Clear();
                    }
                    else
                    {
                        separateWord.Append(allLines[i][k]);
                    }
                }

                decode.Add(separateWord.ToString());

                Guest newGuest = new Guest(int.Parse(decode[0].ToString()), decode[1].ToString(), decode[2].ToString(), int.Parse(decode[3].ToString()));
                decode.Clear();

                listBox1.Items.Add(newGuest.ToString());
                customers.Add(newGuest);

            }

        }

        //Select Unit

        private void selectUnit(object sender, TextBox output)
        {
            output.Clear();

            var separateID = new StringBuilder();
            string inputString = ((ListBox)sender).SelectedItem.ToString();

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == ' ' || inputString[i] == '\n')
                {
                    break;
                }
                else
                {
                    separateID.Append(inputString[i]);
                }
            }

            output.Text = separateID.ToString();

            textDebug.Text = inputString;

        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectUnit(sender, textSelectedGuest);
        }

        //  -   -   -   Rooms   -   -   -   

        private void setRooms()
        {

            loadRooms();

            //Number of beds 
            comboBoxBeds.Items.Add("x1");
            comboBoxBeds.Items.Add("x2");
            comboBoxBeds.Items.Add("x3");
            comboBoxBeds.Items.Add("x4");
            comboBoxBeds.Items.Add("x5");
            comboBoxBeds.Items.Add("x6");

            comboBoxBeds.Text = comboBoxBeds.Items[0].ToString();
            comboBoxBeds.DropDownStyle = ComboBoxStyle.DropDownList;

            //Room Type
            comboBoxType.Items.Add("Standard");
            comboBoxType.Items.Add("Premium");

            comboBoxType.Text = comboBoxType.Items[0].ToString();
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        //Push Room
        private void ButtonAddRoom_Click(object sender, EventArgs e)
        {

            checkField(textCost, true);

            int nBeds = int.Parse(comboBoxBeds.Text.Remove(0, 1));

            Room newRoom = new Room(nBeds, comboBoxType.Text, int.Parse(textCost.Text));
            listBox2.Items.Add(newRoom.ToString());

            rooms.Add(newRoom);
            textSelectedRoom.Text = newRoom.id.ToString();

        }

        //Delete Room
        private void ButtonDeliteRoom_Click(object sender, EventArgs e)
        {


            textSelectedRoom.Text = cutChars(textSelectedRoom.Text);

            if (textSelectedRoom.Text.Length == 0)
            {
                return;
            }

            string textField = textSelectedRoom.Text.ToString();
            int toDelite = int.Parse(textField);

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].id == toDelite)
                {
                    rooms.RemoveAt(i);
                    Room.counter--;

                    updateRezervationList();

                    break;
                }
            }

            textDebug.Text = "Deleted ID: " + textSelectedRoom.Text;

            textSelectedRoom.Clear();
            updateRooms();

        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectUnit(sender, textSelectedRoom);
        }

        private void updateRooms()
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(rooms.ToArray());
        }


        public int checkFreeRoomID(int counter)
        {

            textDebug.Text = counter.ToString();

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int k = 0; k < rooms.Count; k++)
                {
                    if (i == rooms[k].id) //Same slot
                    {
                        break;
                    }
                    else if ((k + 1) == rooms.Count) //Free slot
                    {
                        return i;
                    }
                }
            }

            return counter;

        }

        private void saveRooms(string filePath, List<Room> list)
        {

            List<string> lines = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                lines.Add(list[i].save());
            }

            File.WriteAllLines(filePath, lines);

        }

        private void loadRooms()
        {

            List<string> decode = new List<string>();
            List<string> allLines = new List<string>();

            allLines = File.ReadAllLines(roomsFilePath).ToList();

            if (allLines.Count == 0)
            {
                return;
            }

            for (int i = 0; i < allLines.Count; i++)
            {

                var separateWord = new StringBuilder();

                for (int k = 0; k < allLines[i].Length; k++)
                {
                    if (allLines[i][k] == ' ' || allLines[i][k] == '\n') //Add word - Fresh slot
                    {
                        decode.Add(separateWord.ToString());
                        separateWord.Clear();
                    }
                    else
                    {
                        separateWord.Append(allLines[i][k]);
                    }
                }

                decode.Add(separateWord.ToString());

                Room newRoom = new Room(int.Parse(decode[0].ToString()), int.Parse(decode[1].ToString()), decode[2].ToString(), int.Parse(decode[3].ToString()));
                decode.Clear();

                listBox2.Items.Add(newRoom.ToString());
                rooms.Add(newRoom);

            }

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            sortRooms();
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            sortRooms();
        }

        private void ComboBoxBeds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                sortRooms();
            }
        }

        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                sortRooms();
            }
        }

        private void sortRooms()
        {

            activeRoooms = new List<Room>();

            for (int i = 0; i < rooms.Count; i++)
            {
                activeRoooms.Add(rooms[i]);
            }


            if (checkBox1.Checked)   //Sort by number of beds
            {

                int nBeds = int.Parse(comboBoxBeds.Text.Remove(0, 1));

                for (int i = 0; i < activeRoooms.Count; i++)
                {
                    if (activeRoooms[i].beds != nBeds)
                    {
                        activeRoooms.RemoveAt(i);
                        i--;
                    }
                }

            }

            if (checkBox2.Checked)   //Sort by type of room
            {

                for (int i = 0; i < activeRoooms.Count; i++)
                {
                    if (activeRoooms[i].type != comboBoxType.Text)
                    {
                        activeRoooms.RemoveAt(i);
                        i--;
                    }
                }

            }


            //By date
            if (checkBoxDateRoom.Checked)
            {

                Rezervation currentRezervation;

                for (int i = 0; i < activeRoooms.Count; i++)
                {
                    for (int k = 0; k < activeRoooms[i].Rezervations.Count; k++)
                    {

                        currentRezervation = activeRoooms[i].Rezervations[k];

                        if (currentRezervation.inDate >= dateTimePicker1.Value.AddHours(-2) && currentRezervation.inDate <= dateTimePicker2.Value.AddHours(2))
                        {
                            activeRoooms.RemoveAt(i);
                            i--;

                            break;
                        }

                        if (currentRezervation.outDate >= dateTimePicker1.Value.AddHours(-2) && currentRezervation.outDate <= dateTimePicker2.Value.AddHours(2))
                        {
                            activeRoooms.RemoveAt(i);
                            i--;

                            break;
                        }

                    }
                }

            }

            //Update List
            listBox2.Items.Clear();
            listBox2.Items.AddRange(activeRoooms.ToArray());

        }

        public int roomCost(int roomIndex)
        {

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].id == roomIndex)
                {
                    return rooms[i].cost;
                }
            }

            return 0;
        }

        //  -   -   -   Rezervation   -   -   -   

        private int minimumRezervationDays = 2;

        private void setRezervation()
        {

            updateTimePicker();

            //Rezervation Typs:
            comboBox3.Items.Add("NA");  //Najam
            comboBox3.Items.Add("ND");  //Samo dorucak
            comboBox3.Items.Add("PP");  //Polupansion
            comboBox3.Items.Add("P");   //Pansion
            comboBox3.Items.Add("AL");  //All Inclusive


            comboBox3.Text = comboBox3.Items[0].ToString();
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            updateTimePicker();
            sortRooms();
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            updateTimePicker();
            sortRooms();
        }

        private void updateTimePicker()
        {

            DateTime currentDate = DateTime.UtcNow.Date;

            if (dateTimePicker1.Value < currentDate)
            {
                dateTimePicker1.Value = currentDate;
            }

            if (dateTimePicker2.Value < dateTimePicker1.Value.AddDays(minimumRezervationDays))
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(minimumRezervationDays);
            }

        }

        //Push
        private void ButtonAddRezervation_Click(object sender, EventArgs e)
        {
            addRezervation();
        }

        private void addRezervation()
        {

            if (checkInputs() != true)
            {
                return;
            }

            Rezervation newRezervation = new Rezervation(int.Parse(textSelectedGuest.Text), int.Parse(textSelectedRoom.Text), dateTimePicker1.Value, dateTimePicker2.Value, comboBox3.Text);

            Room selectedRoom;

            for (int i = 0; i < rooms.Count; i++)
            {
                if (int.Parse(textSelectedRoom.Text) == rooms[i].id)
                {
                    selectedRoom = rooms[i];
                    selectedRoom.Rezervations.Add(newRezervation);
                    break;
                }
            }

            listBox3.Items.Add(newRezervation.ToString());

            sortRooms();

        }

        //Delete
        private void ButtonDeleteRezervation_Click(object sender, EventArgs e)
        {
            deleteRezervation();
        }

        private void deleteRezervation()
        {

            Rezervation.counter--;

            textSelectedRezervation.Text = cutChars(textSelectedRezervation.Text);

            if (textSelectedRezervation.Text.Length == 0)
            {
                return;
            }

            int selectedIndex = int.Parse(textSelectedRezervation.Text);
            bool found = false;

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int k = 0; k < rooms[i].Rezervations.Count; k++)
                {
                    if (rooms[i].Rezervations[k].id == selectedIndex)
                    {

                        rooms[i].Rezervations.RemoveAt(k);
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }

            }

            updateRezervationList();

        }

        //Change
        private void ButtonChangeRezervation_Click(object sender, EventArgs e)
        {

            if (textSelectedGuest.Text.Length == 0)
            {
                massage("Not entered ID for guest. Cancel operation.");
                return;
            }

            if (textSelectedRoom.Text.Length == 0)
            {
                massage("Not entered ID for room. Cancel operation.");
                return;
            }

            deleteRezervation();
            addRezervation();
        }

        private void updateRezervationList()
        {

            listBox3.Items.Clear();

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int k = 0; k < rooms[i].Rezervations.Count; k++)
                {
                    listBox3.Items.Add(rooms[i].Rezervations[k].ToString());
                }
            }

        }


        private bool checkInputs()
        {

            textSelectedGuest.Text = cutChars(textSelectedGuest.Text);
            textSelectedRoom.Text = cutChars(textSelectedRoom.Text);

            bool foundCustomer = false;
            bool foundRoom = false;

            //Try to find guest
            if (textSelectedGuest.Text.Length == 0)
            {
                massage("Not entered ID for guest. Cancel operation.");
                return false;
            }

            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].id == int.Parse(textSelectedGuest.Text))
                {
                    foundCustomer = true;
                    break;
                }
            }

            if (foundCustomer != true)
            {
                massage("Can not find customer with ID-" + textSelectedGuest.Text +  ". Cancel operation.");
                return false;
            }

            //Try to find room
            if (textSelectedRoom.Text.Length == 0)
            {
                massage("Not entered ID for room. Cancel operation.");
                return false;
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].id == int.Parse(textSelectedRoom.Text))
                {
                    foundRoom = true;
                    break;
                }
            }

            if (foundRoom != true)
            {
                massage("Can not find room with ID-" + textSelectedRoom.Text + ". Cancel operation.");
                return false;
            }

            return true; //All inputs are correct

        }

        private void massage(string inpMassage)
        {
            string message = inpMassage;
            string caption = "Massage";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            MessageBox.Show(message, caption, buttons);

        }

        private void saveRezervations()
        {

            List<string> lines = new List<string>();

            for (int i = 0; i < rooms.Count; i++)
            {

                for (int k = 0; k < rooms[i].Rezervations.Count; k++)
                {

                    lines.Add(rooms[i].Rezervations[k].save());

                }
                
            }

            File.WriteAllLines(rezervationFilePath, lines);

        }

        private void loadRezervations()
        {

            List<string> decode = new List<string>();
            List<string> allLines = new List<string>();

            allLines = File.ReadAllLines(rezervationFilePath).ToList();

            if (allLines.Count == 0)
            {
                return;
            }

            for (int i = 0; i < allLines.Count; i++)
            {

                var separateWord = new StringBuilder();

                for (int k = 0; k < allLines[i].Length; k++)
                {
                    if (allLines[i][k] == ' ' || allLines[i][k] == '\n') //Add word - Fresh slot
                    {
                        decode.Add(separateWord.ToString());
                        separateWord.Clear();
                    }
                    else
                    {
                        separateWord.Append(allLines[i][k]);
                    }
                }

                decode.Add(separateWord.ToString());

                int id = int.Parse(decode[0]);
                int guestID = int.Parse(decode[1]);
                int roomID = int.Parse(decode[2]);
                DateTime inDate = toDate(decode[3]);
                DateTime outDate = toDate(decode[4]);
                string type = decode[5];

                Rezervation newRezervation = new Rezervation(id, guestID, roomID, inDate, outDate, type);
                decode.Clear();

                listBox3.Items.Add(newRezervation);

                for (int j = 0; j < rooms.Count; j++)
                {
                    if (rooms[j].id == roomID)
                    {
                        rooms[j].Rezervations.Add(newRezervation);
                        break;
                    }
                }

            }

        }

        private DateTime toDate(string line)
        {

            List<string> decode = new List<string>();

            var separateWord = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '-')
                {
                    decode.Add(separateWord.ToString());
                    separateWord.Clear();
                }
                else
                {
                    separateWord.Append(line[i]);
                }
            }

            decode.Add(separateWord.ToString());

            int year = int.Parse(decode[2]);
            int mounh = int.Parse(decode[1]);
            int day = int.Parse(decode[0]);

            return new DateTime(year, mounh, day);

        }

        public int checkFreeRezervationID(int counter)
        {

            List<Rezervation> list = new List<Rezervation>();

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int k = 0; k < rooms[i].Rezervations.Count; k++)
                {
                    list.Add(rooms[i].Rezervations[k]);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (i == list[k].id) //Same slot
                    {
                        break;
                    }
                    else if ((k + 1) == list.Count) //Free slot
                    {
                        return i;
                    }
                }
            }

            return counter;

        }

        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectUnit(sender, textSelectedRezervation);

            Rezervation selectedRezervation;

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int k = 0; k < rooms[i].Rezervations.Count; k++)
                {
                    if (rooms[i].Rezervations[k].id == int.Parse(textSelectedRezervation.Text))
                    {

                        selectedRezervation = rooms[i].Rezervations[k];

                        textSelectedGuest.Text = selectedRezervation.guestID.ToString();
                        textSelectedRoom.Text = selectedRezervation.roomID.ToString();

                        dateTimePicker1.Value = selectedRezervation.inDate;
                        dateTimePicker2.Value = selectedRezervation.outDate;

                        comboBox3.Text = selectedRezervation.type;

                        totalCost = selectedRezervation.totalCost;

                        textBoxTotalCost.Text = (totalCost - totalCost * trackBar1.Value / 100).ToString()+"$";

                        return;

                    }
                }

            }

        }

        private int totalCost = 0;

        private void CheckBoxDateRoom_CheckedChanged(object sender, EventArgs e)
        {
            sortRooms();
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            Discound.Text = "Discound: " + trackBar1.Value.ToString() + "%";

            textBoxTotalCost.Text = (totalCost - totalCost * trackBar1.Value / 100).ToString() + "$";
        }

    }

}
