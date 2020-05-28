using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WelcomeScreen.Models
{
    public class OnLineReport
    {
        [Key]
        public int Report_ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }

        public bool IsInformatic { get; set; }
        [Required]
        public int Order { get; set; } = 0;

    }
}