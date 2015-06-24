Auction Room
============

Auction Room is an online platform where you can bid *live* on items from a real-world Auction Room. 
The updates are real-time and it supports user accounts and concurrent connections, which are mapped to each user account.

The tech stack behind Auction Room includes Bootstrap for a modern design, SignlaR for real-time updates and NHibernate as an ORM.


Motivation for tech
-------------------
SignlaR was chosen for its ability to use Websocket transport, if availabe, and fallback to older transports if necessary. 
The order in which the transport is chosen is the following:

HTML 5 transports:
	Websocket
	Server Sent Events
	
If the client browser doesn't support HTML 5:
	Forever Frame(IE Explorer only)
	Ajax long polling


Business logic
--------------
The application will require users to sign-up before being able to bid. Once signed-up, user accounts are mapped to connectionIDs. 
This will allow a user to open multiple browser windows to the Auction Room, and receive updates in all of them on wether he's the highest user.
Once the highest bidding user, the user will not be allowed to bid further. The bid price will increase on a bid comming from a different user.

	
Running
-------
After opening the solution you will need to install the required packages. 
Once done, you can open the application in your browser and sign-up for an account.


License
-------
MIT licensed. See the bundled [LICENSE](LICENSE.md) file for more details.