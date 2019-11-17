using System;

namespace Assignment4
{
    public class QAUser
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public Nullable<int> Age { get; set; }
        public string UserLocation { get; set; }
        public Nullable<DateTime> CreationDate { get; set; }
    }
}