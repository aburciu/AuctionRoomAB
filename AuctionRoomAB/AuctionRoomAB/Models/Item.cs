using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionRoomAB.Models
{
    public class Item
    {
        public virtual int ItemId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Image { get; set; }

        public virtual decimal StartPrice { get; set; }
        public virtual bool Inactive { get; set; }

        public virtual decimal CurrentPrice { get; set; }
    }
}