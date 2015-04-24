using System;
using Omu.ValueInjecter;
using TED.Dashboard.Dataflow.Models;

namespace TED.Dashboard.Api.Models
{
    public class ReceiptDataModel
    {
        public string HarmonyNumber { get; set; }
        public long ChangeId { get; set; }
        public string ReceiptType { get; set; }
        public string Action { get; set; }
        public DateTime ExtractDate { get; set; }
        public string DisplayExtractDate 
        { 
            get { return ExtractDate.ToShortDateString(); }
        }

        public static ReceiptDataModel BuildFromLogEntry(Receipt receipt)
        {
            var receiptVM = new ReceiptDataModel();
            receiptVM.InjectFrom(receipt);

            return receiptVM;
        }
    }
}