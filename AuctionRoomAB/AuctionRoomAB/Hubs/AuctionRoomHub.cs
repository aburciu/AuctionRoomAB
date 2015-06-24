using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Web;
using Microsoft.AspNet.SignalR;
using AuctionRoomAB.Entities;
using AuctionRoomAB.Models;

namespace AuctionRoomAB.Hubs
{
    public class AuctionRoomHub : Hub
    {
        private readonly AuctionBroadcast auctionBroadcast;

        private readonly object _lock = new object();


        public AuctionRoomHub()
            : this(AuctionBroadcast.Instance)
        {
        }

        public AuctionRoomHub(AuctionBroadcast auctionRoom)
        {
            this.auctionBroadcast = auctionRoom;
        }


        // Called to get current price or "highest" message on client's page load
        public void Init()
        {
            string username = Context.User.Identity.Name;
            auctionBroadcast.Init(username);
        }


        // Called on Bid Button click
        public void Bid()
        {
            string username = Context.User.Identity.Name;
            auctionBroadcast.Bid(username);
        }


        // When client connects, store his connectionIDs in a HashSet
        // Used to map usernames to connectionsIds (a user can have more than one connectionID)
        public override Task OnConnected()
        {
            string username = Context.User.Identity.Name;

            Groups.Add(Context.ConnectionId, username);

            lock (_lock)
            {
                if (auctionBroadcast.userGroups.ContainsKey(username))
                {
                    auctionBroadcast.userGroups[username].Add(Context.ConnectionId);
                }
                else
                {
                    auctionBroadcast.userGroups.Add(username, new HashSet<string> { Context.ConnectionId });
                }
            }

            return base.OnConnected();
        }


        // When client disconnects, remove his connectionIDs from the HashSet
        public override Task OnDisconnected(bool stopCalled)
        {
            string username = Context.User.Identity.Name;

            Groups.Remove(Context.ConnectionId, username);

            lock (_lock)
            {
                if (auctionBroadcast.userGroups.ContainsKey(username))
                {
                    auctionBroadcast.userGroups[username].Remove(Context.ConnectionId);
                }
            }

            return base.OnDisconnected(true);
        }

    }
}