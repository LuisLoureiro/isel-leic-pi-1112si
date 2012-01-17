using System;

namespace mvc.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (ItemsPerPage == 0 || TotalItems == 0) ? 1 : (int)Math.Ceiling( (float)TotalItems / ItemsPerPage); }
        }
    }
}