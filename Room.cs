using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Room
    {

        public static int counter = File.ReadLines(Form2.instance.roomsFilePath).Count();

        public int beds, cost;
        public string type;

        public List<Rezervation> Rezervations;

        public int id;

        public Room(int inpBeds, string inpType, int inpCost)
        {

            int freeID = Form2.instance.checkFreeRoomID(counter);
            counter++;

            id = freeID;

            beds = inpBeds;
            type = inpType;
            cost = inpCost;

            Rezervations = new List<Rezervation>();

        }

        public Room(int inpID, int inpBeds, string inpType, int inpCost) //Load Room
        {
            id = inpID;
            beds = inpBeds;
            type = inpType;
            cost = inpCost;

            Rezervations = new List<Rezervation>();

        }

        public override string ToString()
        {
            return id.ToString() + " \tBeds: " + beds.ToString() + " Type: " + type + " Cost: " + cost.ToString();
        }

        public string save()
        {
            return id.ToString() + " " + beds.ToString() + " " + type + " " + cost.ToString();
        }

    }
}
