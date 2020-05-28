namespace WelcomeScreen.Migrations
{
    using WelcomeScreen.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WelcomeScreen.Models.IntranetContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WelcomeScreen.Models.IntranetContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            #region Welcome táblák feltöltése
            var rank1 = new WelcomeRank() { Rank = "Beszállító képviseloje", Importance = 1 };
            var rank3 = new WelcomeRank() { Rank = "Beszállító vezetoje", Importance = 3 };
            var rank1011 = new WelcomeRank() { Rank = "AQ Group felsõ vezetõ", Importance = 1011 };
            var rank1013 = new WelcomeRank() { Rank = "AQ Group CEO", Importance = 1013 };
            var rank2001 = new WelcomeRank() { Rank = "Vevo képviseloje", Importance = 2001 };
            var rank2003 = new WelcomeRank() { Rank = "Vevo vezetoje", Importance = 2003 };
            var rank2101 = new WelcomeRank() { Rank = "Stratégiai vevo képviseloje", Importance = 2101 };
            var rank2103 = new WelcomeRank() { Rank = "Stratégiai vevo vezetoje", Importance = 2103 };
            var rank2105 = new WelcomeRank() { Rank = "Stratégiai vevõ felsõ vezetõje", Importance = 2105 };
            var rank2107 = new WelcomeRank() { Rank = "Stratégiai vevõ CEO", Importance = 2107 };

            context.WelcomeRanks.AddOrUpdate(x => x.Rank
                , rank1, rank3, rank1011, rank1013, rank2001, rank2003, rank2101, rank2103, rank2105, rank2107);

            var compAQ = new WelcomeCompany() { Company = "AQ Group", ShowComp=true };

            context.WelcomeCompanies.AddOrUpdate(x => x.Company, compAQ);

            var guestAqCEO = new WelcomeGuest() { Fullname = "Anders Carlsson", Company = compAQ, Rank = rank1013 };
            var guestAqCEF = new WelcomeGuest() { Fullname = "Mia Tomczak", Company = compAQ, Rank = rank1011 };
            var guestAqBD = new WelcomeGuest() { Fullname = "Per Lindblad", Company = compAQ, Rank = rank1011 };
            var guestAqMS = new WelcomeGuest() { Fullname = "James Ahrgren", Company = compAQ, Rank = rank1011 };
            context.WelcomeGuests.AddOrUpdate(x => x.Fullname
                , guestAqCEO, guestAqCEF, guestAqBD, guestAqMS);

            var compBosch = new WelcomeCompany() { Company = "Bosch", ShowComp = true };
            context.WelcomeCompanies.AddOrUpdate(x => x.Company, compBosch);

            var guestBoschDVD = new WelcomeGuest() { Fullname = "Dr.Volkmar Denner", Company = compBosch, Rank = rank2107 };
            var guestBoschCK = new WelcomeGuest() { Fullname = "Christoph Kübel", Company = compBosch, Rank = rank2105 };

            context.WelcomeGuests.AddOrUpdate(x => x.Fullname
                , guestBoschDVD
                , new WelcomeGuest() { Fullname = "Dr.Michael Bolle", Company = compBosch, Rank = rank2105 }
                , guestBoschCK
                , new WelcomeGuest() { Fullname = "Uwe RaschkeL", Company = compBosch, Rank = rank2105 }
                );

            var comp = new WelcomeCompany() { Company = "Würth Szereléstechnika Kft.", ShowComp = true };
            context.WelcomeCompanies.AddOrUpdate(x => x.Company, comp);
            context.WelcomeGuests.AddOrUpdate(x => x.Fullname
                , new WelcomeGuest() { Fullname = "Kránitz Elemér", Company = comp, Rank = rank1 }
                );

            context.WelcomeCompanies.AddOrUpdate(x => x.Company
                , new WelcomeCompany() { Company = "Egyéni vállalkozók", ShowComp = false });

            var event1 = new WelcomeVisit
            {
                Visit = "Éves AQ látogatás"
                ,FromDT = new DateTime(2019, 5, 20, 7, 40, 0)
                ,ToDT = new DateTime(2019, 5, 23, 20, 30, 0)
                ,Company = compAQ
                ,CreatingDT = new DateTime(2019, 4, 15, 13, 30, 0)
            };
            var event2 = new WelcomeVisit
            {
                Visit = "Bosch vevõi látogatás"
                ,FromDT = new DateTime(2019, 6, 18, 7, 40, 0)
                ,ToDT = new DateTime(2019, 6, 20, 18, 10, 0)
                ,Company = compBosch
                ,CreatingDT = new DateTime(2019, 4, 16, 11, 28, 11)

            };
            context.WelcomeVisits.AddOrUpdate(x => x.Visit, event1, event2);

            if (context.WelcomeGuestVisits.Count() == 0)
            {
                context.WelcomeGuestVisits.Add(new WelcomeGuestVisit() { Visit = event1, Guest = guestAqCEO });
                context.WelcomeGuestVisits.Add(new WelcomeGuestVisit() { Visit = event1, Guest = guestAqCEF });
                context.WelcomeGuestVisits.Add(new WelcomeGuestVisit() { Visit = event1, Guest = guestAqBD });
                context.WelcomeGuestVisits.Add(new WelcomeGuestVisit() { Visit = event2, Guest = guestAqCEO });
                context.WelcomeGuestVisits.Add(new WelcomeGuestVisit() { Visit = event2, Guest = guestAqMS });
                context.WelcomeGuestVisits.Add(new WelcomeGuestVisit() { Visit = event2, Guest = guestBoschDVD });
                context.WelcomeGuestVisits.Add(new WelcomeGuestVisit() { Visit = event2, Guest = guestBoschCK });
            }
            #endregion

            #region On-line reports tábla feltöltése
            context.OLReports.AddOrUpdate(x => x.Name
                , new OnLineReport { Name = "Nyilvános riportok", Order = 1, Link = "http://riportok.lan.local/report_list.html?path=Anton-Public&header=Nyilv%C3%A1nos%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Értékesítés", Order = 2, Link = "http://riportok.lan.local/report_list.html?path=Sales-Public&header=%C3%89rt%C3%A9kes%C3%ADt%C3%A9si%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Logisztika", Order = 3, Link = "http://riportok.lan.local/report_list.html?path=Logistics-Public&header=Logisztikai%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Pénzügy", Order = 4, Link = "http://riportok.lan.local/report_list.html?path=Financial-Public&header=P%C3%A9nz%C3%BCgyes%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Karbantartás", Order = 5, Link = "http://riportok.lan.local/report_list.html?path=Maintenance-Public&header=Karbantart%C3%A1s%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Emberi erõforrás", Order = 6, Link = "http://riportok.lan.local/report_list.html?path=HR-Public&header=Emberi%20er%C5%91forr%C3%A1s%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Informatika", Order = 7, Link = "http://riportok.lan.local/report_list.html?path=IT-Public&header=Informatikai%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Történetiségi riportok", Order = 8, Link = "http://riportok.lan.local/report_list.html?path=Histories-Public&header=T%C3%B6rt%C3%A9netis%C3%A9gi%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Szerszámüzem", Order = 21, Link = "http://riportok.lan.local/report_list.html?path=T1-Public&header=Szersz%C3%A1m%C3%BCzemi%20riportok", IsInformatic = false }
                , new OnLineReport { Name = "Mûanyagüzem", Order = 22, Link = "http://riportok.lan.local/report_list.html?path=T2-Public&header=M%C5%B1anyag%C3%Bczem", IsInformatic = false }
                , new OnLineReport { Name = "SMD", Order = 23, Link = "http://riportok.lan.local/report_list.html?path=T3-Public&header=SMD", IsInformatic = false }
                , new OnLineReport { Name = "CIM stopped", Order = 41, Link = "http://rs/Reports/Pages/ReportViewer.aspx?%2fIT-Public%2fCIM%20stopped&rs:Command=Render", IsInformatic = true }
                , new OnLineReport { Name = "QAD adatbázis lockok", Order = 42, Link = "http://rs/Reports/Pages/ReportViewer.aspx?%2fIT-Public%2fQAD%20adatb%C3%A1zis%20lockok&rs:Command=Render", IsInformatic = true }
                );
            #endregion

            #region Jogosultságok feltöltése
            context.IntraRoles.AddOrUpdate(x => x.Role
                , new IntraRole { Role= "Informatikai vezetõk"
                    , Admin=true
                    , HomePage = true
                    , UsefullLimks = true
                    , WS_ViewVisit = true
                    , WS_CreateVisit = true
                    , WS_DelCompany = true
                    , WS_Settings = true
                    , T2rT_RecordChecklist = true
                    , T2rT_EditChecklist = false }
                );

            #endregion
        }
    }
}
