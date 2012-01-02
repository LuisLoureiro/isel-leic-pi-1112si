using System.Collections;

namespace mvc.Models
{
    public class TableViewModel
    {
        public IEnumerable Items { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}