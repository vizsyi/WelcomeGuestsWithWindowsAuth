using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WelcomeScreen.Models
{
    public class IntranetContext : DbContext
    {
        public IntranetContext()
            //: base("DefaultConnection") //, throwIfV1Schema: false)
        {
            //foreach (var comp in WelcomeCompanies)
            //{
            //    comps += comp.Company + ",";
            //}
        }
        /// <summary>
        /// Saját adattáblák
        /// </summary>
        public DbSet<WelcomeCompany> WelcomeCompanies { get; set; }
        public DbSet<WelcomeRank> WelcomeRanks { get; set; }
        public DbSet<WelcomeGuest> WelcomeGuests { get; set; }
        public DbSet<WelcomeVisit> WelcomeVisits { get; set; }
        public DbSet<WelcomeGuestVisit> WelcomeGuestVisits { get; set; }

        public DbSet<WelcomeVisitHist> WelcomeVisitHists { get; set; }
        public DbSet<WelcomeGuestVisitHist> WelcomeGuestVisitHists { get; set; }

        public DbSet<OnLineReport> OLReports { get; set; }

        public DbSet<IntraRole> IntraRoles { get; set; }

        //todo: törölni kell
        public string comps="";

    }

}