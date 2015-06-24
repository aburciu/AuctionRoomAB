using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using AuctionRoomAB.Hubs;
using AuctionRoomAB.Models;
using NHibernate;

namespace AuctionRoomAB.Entities
{
    public class AuctionBroadcast
    {

        // Singleton instance
        private readonly static Lazy<AuctionBroadcast> _instance = new Lazy<AuctionBroadcast>(() => new AuctionBroadcast());

        // User Groups (Contain sets of connectionIDs for each user logged in)
        public Dictionary<string, HashSet<string>> userGroups = new Dictionary<string, HashSet<string>>();

        private Item auctionItem;
        private readonly IHubContext hubContext;
        private readonly string highestMessage = "You're the highest bidder!";
        private string HighestUserName { get; set; }

        private object _bidLock = new object();

        private volatile bool _placingBid = false;

        public static AuctionBroadcast Instance { get { return _instance.Value; } }


        public AuctionBroadcast()
        {
            hubContext = GlobalHost.ConnectionManager.GetHubContext<AuctionRoomHub>();

            ISession session = NHibernateHelper.CurrentSessionFactory.OpenSession();
            auctionItem = session.QueryOver<Item>().SingleOrDefault();

            // Set price at which bidding starts
            auctionItem.CurrentPrice = auctionItem.StartPrice;
        }


        public void Bid(string username)
        {
            lock (_bidLock)
            {
                if (!_placingBid)
                {
                    _placingBid = true;

                    // Log bid to database.
                    // If successful increase bid price + broadcast highest/new prices
                    if (InsertBid(username))
                    {
                        IncreaseBidPrice();
                        BroadcastBid(username);
                    }

                    _placingBid = false;
                }
            }
        }


        public void BroadcastBid(string username)
        {
            // Get connectionIDs belonging to calling client
            var callerConnectionIds = userGroups.Where(x => x.Key == username).SelectMany(x => x.Value);

            // Broadcast the new price to all others
            hubContext.Clients.AllExcept(callerConnectionIds.ToArray()).updateBidButton(auctionItem.CurrentPrice);

            // Broadcast "highest" message to caller
            hubContext.Clients.Group(username).updateBidButton(highestMessage, true);
            HighestUserName = username;
        }


        // Check if client is highest and give message, when re-loading the page
        public void Init(string username)
        {
            if (HighestUserName == username)
            {
                hubContext.Clients.Group(username).updateBidButton(highestMessage, true);
            }
            else
            {
                hubContext.Clients.Group(username).updateBidButton(auctionItem.CurrentPrice);
            }
        }


        public void IncreaseBidPrice()
        {
            auctionItem.CurrentPrice += 100;
        }


        // Log bid info to database
        public bool InsertBid(string username)
        {
            ISession session = NHibernateHelper.CurrentSessionFactory.OpenSession();
            UserProfile user = session.QueryOver<UserProfile>().Where(x => x.UserName == username).SingleOrDefault();

            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    ItemBids bid = new ItemBids
                    {
                        Item = auctionItem,
                        User = user,
                        BidAmount = auctionItem.CurrentPrice,
                        Add_Dt = DateTime.Now
                    };

                    session.Save(bid);
                    transaction.Commit();
                    
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

    }
}