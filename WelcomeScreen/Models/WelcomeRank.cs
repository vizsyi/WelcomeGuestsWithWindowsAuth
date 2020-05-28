using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WelcomeScreen.Models
{
    public class WelcomeRank
    {
        [Key]
        public int Rank_ID { get; set; }
        [Required]
        [Display(Name = "Pozíció")]
        public string Rank { get; set; }
        [Required]
        [Display(Name = "Fontosság")]
        public int Importance { get; set; }

        #region ViewModel rész
        [NotMapped]
        public List<WelcomeGuest> guests;
        #endregion
    }
}