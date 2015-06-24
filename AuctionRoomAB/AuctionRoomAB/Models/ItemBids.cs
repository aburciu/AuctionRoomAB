using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionRoomAB.Models
{
    public class ItemBids
    {
        public virtual int ItemBidsId { get; set; }
        public virtual int ItemId { get; set; }
        public virtual int UserId { get; set; }
        public virtual decimal BidAmount { get; set; }

        public virtual DateTime Add_Dt { get; set; }

        public virtual Item Item { get; set; }
        public virtual UserProfile User { get; set; }
    }
}