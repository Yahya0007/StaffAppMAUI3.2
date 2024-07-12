using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApp.StaffDataService
{
    public class Notification
    {
        [PrimaryKey]
        public string GUID { get; set; }
        //[AutoIncrement]
        //public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Arrived { get; set; }
        public DateTime Opened { get; set; }
        public string Username { get; set; }
        public int StaffID { get; set; }
        public int EventID { get; set; }
        public int StaffBookingID { get; set; }
        public string Comments { get; set; }
        public string Action { get; set; }
        public string ActionPayload { get; set; }
        //public Microsoft.Maui.Graphics.Color ItemColour { get; set; } = Colors.Azure;
    }
}
