using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuctionRoomAB.Models;
using FluentNHibernate.Mapping;

namespace AuctionRoomAB.Mappings
{
    public class ItemMap : ClassMap<Item>
    {
        public ItemMap()
        {
            Table("Item");
            Id(x => x.ItemId);
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.Image);
            Map(x => x.StartPrice);
            Map(x => x.Inactive);
        }
    }
}