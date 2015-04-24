using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TED.Dashboard.UserSettings.Models
{
    public class CustomDashboard
    {
        public CustomDashboard()
        {
            Users = new HashSet<UserParameters>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DashboardID { get; set; }

        [MaxLength(200), Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public bool IsPublic { get; set; }

        // model links
        public virtual ICollection<Widget> Widgets { get; set; }
        public virtual ICollection<UserParameters> Users { get; set; }
    }
}
