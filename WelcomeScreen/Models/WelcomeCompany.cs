using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WelcomeScreen.Models
{
    public class WelcomeCompany
    {
        [Key]
        public int Comp_ID { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Egy cég megnevezésének legalább 3 karakter hoszúnak kell lennie.")]
        [Display(Name = "Partnercég")]
        public string Company { get; set; }
        [Required]
        [Display(Name = "Cégnév megjelenjen")]
        public bool ShowComp { get; set; }
        [Required]
        public bool Deleted { get; set; }

        virtual public List<WelcomeGuest> Guests { get; set; }

        #region ViewModel rész
        [NotMapped]
        public string CompShow
        {
            get { return ShowComp ? Company : ""; }
        }
        #endregion
    }
}