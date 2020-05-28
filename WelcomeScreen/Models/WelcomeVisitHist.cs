using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WelcomeScreen.Models
{
    public class WelcomeVisitHist
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Visit_ID { get; set; }
        [Required]
        [Display(Name = "Látogatás")]
        public string Visit { get; set; }
        [Required]
        [Display(Name = "Kezdet")]
        public DateTime FromDT { get; set; }
        [Required]
        [Display(Name = "Vég")]
        public DateTime ToDT { get; set; }
        [Display(Name = "Partnercég")]
        virtual public WelcomeCompany Company { get; set; }
        [Required]
        public DateTime CreatingDT { get; set; }

        [Required]
        [Display(Name = "Törölt")]
        public bool Deleted { get; set; }

        public List<WelcomeGuestVisitHist> GuestVisitHists { get; set; }

        #region ViewModel rész
        [NotMapped]
        [Display(Name = "Időtartam")]
        public string Period
        {
            get { return FromDT.ToString("yyyy.MM.dd. H:mm") + " - " + ToDT.ToString("yyyy.MM.dd. H:mm"); }
        }
        [NotMapped]
        [Display(Name = "Időtartam")]
        public string ShortPeriod
        {
            get { return FromDT.ToString("yyyy.MM.dd.") + 
                    (FromDT.Date == ToDT.Date ? "" : " - " + ToDT.ToString("yyyy.MM.dd.")); }
        }
        #endregion
    }
}