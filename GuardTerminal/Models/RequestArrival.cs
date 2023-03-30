using System;

namespace GuardTerminal.Models
{
    public class RequestArrival
    {

        public RequestArrival(int id,DateTime date) 
        {
            this.id = id;
            this.date = date;
        }
        public int id { get; set; }
        public DateTime date { get; set; }
    }
}
