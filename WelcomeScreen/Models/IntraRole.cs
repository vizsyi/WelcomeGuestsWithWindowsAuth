using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WelcomeScreen.Models
{
    public class IntraRole
    {
        [Key]
        public int Role_ID { get; set; }
        [Required]
        [Display(Name = "Felhasználói csoport")]
        public string Role { get; set; } = "";
        #region Jogosultság property-k
        [Required]
        [Display(Name = "Adminisztrátor")]
        public bool Admin { get; set; }
        [Required]
        [Display(Name = "Home Page")]//todo: törölni
        public bool HomePage { get; set; }
        [Required]
        [Display(Name = "Usefull links")]
        public bool UsefullLimks { get; set; }
        [Required]
        [Display(Name = "Can view visits")]
        public bool WS_ViewVisit { get; set; }
        [Required]
        [Display(Name = "Can create visits")]
        public bool WS_CreateVisit { get; set; }
        [Required]
        [Display(Name = "Can delete companies", ShortName = "WS_DeleteCompany")]
        public bool WS_DelCompany { get; set; }
        [Required]
        [Display(Name = "Can set WelcomeScreen")]
        public bool WS_Settings { get; set; }
        [Required]
        [Display(Name = "Gyártásindítási lapot lejelenthet")]
        public bool T2rT_RecordChecklist { get; set; }
        [Required]
        [Display(Name = "Gyártásindítási feladatlistát szerkeszthet")]
        public bool T2rT_EditChecklist { get; set; }

        #endregion

        #region ViewModel rész
        [NotMapped]
        public string UserName;
        //{
        //    get { return this.UserName; }
        //    set { this.UserName = value.Substring(0, 4) == "LAN\\" ? value.Substring(4) : value; }
        //}
        [NotMapped]
        [Display(Name = "Vendégüdvözlő")]
        public string WelcomeScreen
        {
            get { return
                     WS_ViewVisit && WS_CreateVisit && WS_DelCompany ?
                        (WS_Settings ? "Teljes" : "Adatkezelő") :
                        (WS_ViewVisit || WS_CreateVisit || WS_DelCompany || WS_Settings ? "Részleges" : "Semmi");
            }
        }
        [NotMapped]
        [Display(Name = "Gyártásindítási lapok")]
        public string T2reTool
        {
            get
            {
                return
                   T2rT_RecordChecklist ?
                      (T2rT_EditChecklist ? "Teljes" : "Lejelentő") :
                      (T2rT_EditChecklist ? "Feladatszerkesztő" : "Semmi");
            }
        }
        #endregion

        #region methods
        public IntraRole(){}
        public IntraRole(string name)
        {
            this.UserName = name.Substring(0, 4) == "LAN\\" ? name.Substring(4) : name;
        }
        /// <summary>
        /// Átveszi a paraméterként megadott IntraRole objektum jogosultságait
        /// </summary>
        /// <param name="dbrole"></param> Az objektum, mely jogosultságait le kell származtatni.
        public void Add (IntraRole dbrole)
        {
            this.Role = this.Role == "" ? dbrole.Role : this.Role + "," + dbrole.Role;
            if (!this.Admin && dbrole.Admin) { this.Admin = true; }
            if (!this.HomePage && dbrole.HomePage) { this.HomePage = true; }
            if (!this.UsefullLimks && dbrole.UsefullLimks) { this.UsefullLimks = true; }
            if (!this.WS_ViewVisit && dbrole.WS_ViewVisit) { this.WS_ViewVisit = true; }
            if (!this.WS_CreateVisit && dbrole.WS_CreateVisit) { this.WS_CreateVisit = true; }
            if (!this.WS_DelCompany && dbrole.WS_DelCompany) { this.WS_DelCompany = true; }
            if (!this.WS_Settings && dbrole.WS_Settings) { this.WS_Settings = true; }
            if (!this.T2rT_RecordChecklist && dbrole.T2rT_RecordChecklist) { this.T2rT_RecordChecklist = true; }
            if (!this.T2rT_EditChecklist && dbrole.T2rT_EditChecklist) { this.T2rT_EditChecklist = true; }
        }

        internal void Assign(IntraRole role)
        {
            this.Role = role.Role;
            this.Admin = role.Admin;
            this.UsefullLimks = role.UsefullLimks;
            this.WS_ViewVisit = role.WS_ViewVisit;
            this.WS_CreateVisit = role.WS_CreateVisit;
            this.WS_DelCompany = role.WS_DelCompany;
            this.WS_Settings = role.WS_Settings;
            this.T2rT_RecordChecklist = role.T2rT_RecordChecklist;
            this.T2rT_EditChecklist = role.T2rT_EditChecklist;
        }
        #endregion

    }
}