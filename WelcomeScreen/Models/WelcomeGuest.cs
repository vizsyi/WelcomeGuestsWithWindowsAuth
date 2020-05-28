using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WelcomeScreen.Models
{
    public class WelcomeGuest
    {
        [Key]
        public int Guest_ID { get; set; }
        [Required]
        [Display(Name = "Teljes név")]
        public string Fullname { get; set; }
        [Required]
        [Display(Name = "Cég")]
        virtual public WelcomeCompany Company { get; set; }
        [Required]
        [Display(Name = "Pozíció")]
        virtual public WelcomeRank Rank { get; set; }
        [Required]
        [Display(Name = "Törölt")]
        public bool Deleted { get; set; }

        #region ViewModel rész
        [NotMapped]
        public List<SelectListItem> AssignableCompanies { get; set; } = new List<SelectListItem>();
        [NotMapped]
        public List<SelectListItem> AssignableRanks { get; set; } = new List<SelectListItem>();
        [NotMapped]
        public int Sel_Comp_ID { get; set; }
        [NotMapped]
        public int Sel_Rank_ID { get; set; }

        [NotMapped]
        [Display(Name = "Pozíció")]
        public string RankOrDel
        {
            get { return Deleted ? "* Törölt *" : Rank.Rank; }
        }
        #endregion
    }
}