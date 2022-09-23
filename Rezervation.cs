using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{

    class Rezervation
    {
        public static int counter = File.ReadLines(Form2.instance.rezervationFilePath).Count();

        public string type;
        public int totalCost;

        public DateTime inDate, outDate;

        public int id, guestID, roomID;

        public Rezervation(int guestID, int roomID, DateTime inDate, DateTime outDate, string type)
        {

            int freeID = Form2.instance.checkFreeRezervationID(counter);
            counter++;

            id = freeID;


            this.guestID = guestID;
            this.roomID = roomID;

            this.inDate = inDate;
            this.outDate = outDate;

            this.type = type;

            //Calculate cost
            int days = (outDate.AddHours(12) - inDate).Days;
            int cost = Form2.instance.roomCost(roomID);
            totalCost = days * cost;

        }

        public Rezervation(int id,int guestID, int roomID, DateTime inDate, DateTime outDate, string type) //Load
        {

            this.id = id;

            this.guestID = guestID;
            this.roomID = roomID;

            this.inDate = inDate;
            this.outDate = outDate;

            this.type = type;

            //Calculate cost
            int days = (outDate - inDate).Days;
            int cost = Form2.instance.roomCost(roomID);
            totalCost = days * cost;

        }

        public override string ToString()
        {

            TimeSpan duration = outDate - inDate;

            //return id.ToString() + " GuestID: " + guestID + " RoomID: " + roomID + " Days: " + duration.Days + " " + type;

            return id.ToString() + " G:" + guestID + " R: " + roomID + " in: " + inDate.ToString("dd.MM") + " out: " + outDate.ToString("dd.MM") + " " + type;

        }

        public string save()
        {
            return id.ToString() + " " + guestID + " " + roomID + " " + inDate.ToString("dd-MM-yyyy") + " " + outDate.ToString("dd-MM-yyyy") + " " + type;
        }

    }

}
