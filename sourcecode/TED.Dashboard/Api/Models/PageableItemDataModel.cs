using System.Collections.Generic;

namespace TED.Dashboard.Api.Models
{
    public class PageableItemDataModel<T>
    {
        public int ItemCount { get; set; }
        public IEnumerable<T> Items { get; set; } 
    }
}