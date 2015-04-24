using System;

namespace TED.Dashboard.Dataflow.Models
{
    public class Receipt
    {
        public string HarmonyNumber { get; set; }
        public long ChangeId { get; set; }
        public string ReceiptType { get; set; }
        public string Action { get; set; }
        public DateTime ExtractDate { get; set; }
    }
}
