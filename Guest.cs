using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Guest
    {

        public static int counter = File.ReadLines(Form2.instance.customerFilePath).Count();

        private string name, lastName;
        private int number;

        public int id;

        public Guest(string inpName, string inpLastName, int inpNumber)
        {

            int freeID = Form2.instance.checkFreeID(counter);
            counter++;

            id = freeID;


            name = inpName;
            lastName = inpLastName;
            number = inpNumber;

        }

        public Guest(int inpID, string inpName, string inpLastName, int inpNumber) //Load Guest
        {
            id = inpID;
            name = inpName;
            lastName = inpLastName;
            number = inpNumber;
        }

        public override string ToString()
        {
            return id.ToString() + " " + name + " " + lastName + " " + number;
        }

    }

}
