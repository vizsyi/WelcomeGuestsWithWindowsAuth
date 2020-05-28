using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WelcomeScreen.Models
{
    public class WelcomeGuestVisitHist
    {
        [Key]
        public int GV_ID { get; set; }
        [Required]
        virtual public WelcomeVisitHist VisitHist { get; set; }
        [Required]
        [Display(Name = "Résztvevő")]
        virtual public WelcomeGuest Guest { get; set; }
    }
}