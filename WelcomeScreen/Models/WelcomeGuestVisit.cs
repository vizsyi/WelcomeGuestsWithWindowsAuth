using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WelcomeScreen.Models
{
    public class WelcomeGuestVisit
    {
        [Key]
        public int GV_ID { get; set; }
        [Required]
        virtual public WelcomeVisit Visit { get; set; }
        [Required]
        [Display(Name = "Résztvevő")]
        virtual public WelcomeGuest Guest { get; set; }

        #region ViewModel rész
        [NotMapped]
        public List<SelectListItem> AssignableGuests { get; set; } = new List<SelectListItem>();
        [NotMapped]
        public int Visit_ID { get; set; }
        [NotMapped]
        public int Sel_Guest_ID { get; set; }
        [NotMapped]
        public bool hasguest;
        #endregion
    }
}