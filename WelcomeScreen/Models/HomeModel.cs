using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WelcomeScreen.Models
{
    public class HomeModel
    {
        public IEnumerable<WelcomeGuest> welcomeGuests;
        public IEnumerable<OnLineReport> onLineReports;

        public int GuestCount;
    }
}