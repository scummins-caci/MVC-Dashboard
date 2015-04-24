using System;

namespace TED.Dashboard.Dataflow.Models
{
    public abstract class MetaInfoBase
    {
        public string Id { get; set; }
        public string HarmonyNumber { get; set; }
        public string MetaId { get; set; }
        public long ChangeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime RoutedOn { get; set; }
        public string Network { get; set; }
    }
}
