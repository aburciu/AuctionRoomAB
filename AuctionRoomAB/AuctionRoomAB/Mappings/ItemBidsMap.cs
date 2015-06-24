using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuctionRoomAB.Models;
using FluentNHibernate.Mapping;

namespace AuctionRoomAB.Mappings
{
    public class ItemBidsMap : ClassMap<ItemBids>
    {
        public ItemBidsMap()
        {
            Table("ItemBids");
            Id(x => x.ItemBidsId);
            Map(x => x.BidAmount);
            Map(x => x.Add_Dt);


            References(x => x.Item).Column("ItemId");
            References(x => x.User).Column("UserId");
        }
    }
}