using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuctionRoomAB.Models;
using FluentNHibernate.Mapping;

namespace AuctionRoomAB.Mappings
{
    public class UserProfileMap : ClassMap<UserProfile>
    {
        public UserProfileMap()
        {
            Id(x => x.UserId);
            Map(x => x.UserName);
        }
    }
}