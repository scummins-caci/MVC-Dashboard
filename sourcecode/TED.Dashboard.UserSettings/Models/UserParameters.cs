using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TED.Dashboard.UserSettings.Models
{
    public class UserParameters
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        // model links
        public virtual ICollection<CustomDashboard> Dashboards { get; set; }
    }
}
