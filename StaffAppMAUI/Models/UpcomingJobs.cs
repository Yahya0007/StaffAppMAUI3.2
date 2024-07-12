using System;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace StaffApp.StaffDataService
{
    public class UpcomingJobs : INotifyPropertyChanged
    {
        public int EventID { get; set; }
        public int StaffID { get; set; }
        public string StaffIDSt { get; set; }
        public int EventStaffRequiredID { get; set; }
        public DateTime EventDate { get; set; }
        public int  Weekday { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string Client { get; set; }
        public string EventType { get; set; }
        public string EventStatus { get; set; }
        public string Venue { get; set; }
        public string VenueFirst { get; set; }
        public string VenuePostcode { get; set; }
        public string VenueDetails { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string JobType { get; set; }
        public string AppRole { get; set; }
        public string StaffJobBlob { get; set; }
        public string JobsUniforms { get; set; }
        public string MapLink { get; set; }
        public string MeetingPoint { get; set; }
        public string NearestStation { get; set; }
        public string Location { get; set; }
        public string AppInfo { get; set; }
        public int StaffRequired { get; set; }
        public int StaffBooked { get; set; }
        public int StaffBalance { get; set; }
        public bool IsNew { get; set; }
        public string StaffNotes { get; set; }
        public string Username { get; set; }
        public int ItemColourRed { get; set; }
        public int ItemColourGreen { get; set; }
        public int ItemColourBlue { get; set; }
        public int ItemBackgroundColourRed { get; set; }
        public int ItemBackgroundColourGreen { get; set; }
        public int ItemBackgroundColourBlue { get; set; }
        public bool isAllowSelfJobBooking { get; set; }
        public bool isAllowRequestforWork { get; set; }
        public Microsoft.Maui.Graphics.Color ItemColour { get; set; }
        public Microsoft.Maui.Graphics.Color ItemBackgroundColour { get; set; }
        bool isfavorite = false;
        public bool IsFavorite
        {
            get => isfavorite;
            set
            {
                if (isfavorite != value)
                {
                    isfavorite = value;
                    OnPropertyChanged("IsFavorite");
                }
            }
        }

        bool isjobenabled = false;
        public bool IsJobDisabled
        {
            get => isjobenabled;
            set
            {
                if (isjobenabled != value)
                {
                    isjobenabled = value;
                    OnPropertyChanged("IsJobDisabled");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public UpcomingJobs()
        {
            EventID = 0;
            StaffID = 0;
            StaffIDSt = string.Empty;
            EventStaffRequiredID = 0;
            EventDate = DateTime.MinValue;
            Weekday = 0;
            FromTime = DateTime.MinValue;
            ToTime = DateTime.MinValue;
            Client = string.Empty;
            EventType = string.Empty;
            EventStatus = string.Empty;
            Venue = string.Empty;
            VenueFirst = string.Empty;
            VenuePostcode = string.Empty;
            VenueDetails = string.Empty;
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            JobType = string.Empty;
            AppRole = string.Empty;
            StaffJobBlob = string.Empty;
            JobsUniforms = string.Empty;
            MapLink = string.Empty;
            MeetingPoint = string.Empty;
            NearestStation = string.Empty;
            Location = string.Empty;
            AppInfo = string.Empty;
            StaffRequired = 0;
            StaffBooked = 0;
            StaffBalance = 0;
            IsNew = false;
            StaffNotes = string.Empty;
            Username = string.Empty;
            ItemColourRed = 0;
            ItemColourGreen = 0;
            ItemColourBlue = 0;
            ItemBackgroundColourRed = 0;
            ItemBackgroundColourGreen = 0;
            ItemBackgroundColourBlue = 0;
            isAllowSelfJobBooking = false;
            isAllowRequestforWork = false;
            ItemColour = Colors.Transparent;
            ItemBackgroundColour = Colors.Transparent;
            isfavorite = false;
            isjobenabled = false;
        }
    }
}
