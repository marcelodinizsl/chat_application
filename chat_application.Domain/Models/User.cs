using System;

namespace chat_application.Domain.Models
{
    public class User
    {
        public Int64 key { get; set; }
        public string name { get; set; }
        public DateTime dtConnection { get; set; }
        public string connectionHost { get; set; }
    }
}
