using System.Collections.Generic;

namespace TED.Dashboard.Services.Models
{
    public class PageableItem<T>
    {
        public int ItemCount { get; set; }
        public IEnumerable<T> Items { get; set; } 
    }
}
