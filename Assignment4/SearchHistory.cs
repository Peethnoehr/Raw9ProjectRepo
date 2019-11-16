using System;

namespace Assignment4
{
    public class SearchHistory
    {
        public int SearchHistoryId { get; set; }
        public DateTime SearchDate { get; set; }
        public string SearchText { get; set; }
        public int UserId { get; set; }
    }
}