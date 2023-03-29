using System;

namespace GuardTerminal.Models
{
    public class RequestArrival
    {

        public RequestArrival(int app,DateTime date) 
        {
            this.app = app;
            this.date = date;
        }
        public int app { get; set; }
        public DateTime date { get; set; }
    }
}
