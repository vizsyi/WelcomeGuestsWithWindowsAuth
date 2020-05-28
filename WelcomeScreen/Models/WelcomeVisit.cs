using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WelcomeScreen.Models
{
    public class WelcomeVisit
    {
        [Key]
        public int Visit_ID { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "A látogatás címének legalább 3 karakter hoszúnak kell lennie.")]
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

        virtual public List<WelcomeGuestVisit> GuestVisits { get; set; }

        #region ViewModel rész
        [NotMapped]
        public List<SelectListItem> AssignableCompanies { get; set; } = new List<SelectListItem>();
        [NotMapped]
        public int Sel_Comp_ID { get; set; }

        [NotMapped]
        [Display(Name = "Kezdet")]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd.}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [NotMapped]
        [Display(Name = "Idő")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan FromTime { get; set; }

        [NotMapped]
        [Display(Name = "Vég")]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd.}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        [NotMapped]
        [Display(Name = "Idő")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ToTime { get; set; }

        [NotMapped]
        public bool recent;

        [NotMapped]
        [Display(Name = "Időtartam")]
        public string Period
        {
            get { return FromDT.ToString("yyyy.MM.dd. H:mm") + " - " + ToDT.ToString("yyyy.MM.dd. H:mm"); }
        }
        
        public void SetParts()
        {
            FromDate = FromDT.Date;
            FromTime = FromDT.TimeOfDay;
            ToDate = ToDT.Date;
            ToTime = ToDT.TimeOfDay;
        }
        public void SetDT()
        {
            FromDT = (FromDate.Date + FromTime).AddDays(-FromTime.Days);
            ToDT = (ToDate.Date + ToTime).AddDays(-ToTime.Days);
            //todo: letesztelni a napértéket a time-ban
        }
        #endregion
    }
}