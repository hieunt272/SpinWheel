using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpinWheel.Models
{
    public class ListClientAward
    {
        public int Id { get; set; }
        public int AwardId { get; set; }
        public int ClientId { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Award Award { get; set; }
        public virtual Client Client { get; set; }
        public ListClientAward()
        {
            CreateDate = DateTime.Now;
        }
    }
}