namespace PhotoContest.Web.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    
    public class BaseHub : Hub
    {
        public override Task OnConnected()
        {
            if (this.Context.User != null)
            {
                var userName = this.Context.User.Identity.GetUserName();
                var connectionId = this.Context.ConnectionId;
                var id = this.Context.User.Identity.GetUserId();

                var user = ConnectionManager.Users.GetOrAdd(
                    userName,
                    _ => new ConnectedUser { Name = userName, ConnectionsIds = connectionId, Id = id });

                lock (user.ConnectionsIds)
                {
                    user.ConnectionsIds = connectionId;

                    // // broadcast this to all clients other than the caller
                    // Clients.AllExcept(user.ConnectionIds.ToArray()).userConnected(userName);

                    // Or you might want to only broadcast this info if this 
                    // is the first connection of the user
                    if (user.ConnectionsIds != null || user.ConnectionsIds != string.Empty)
                    {
                        this.Clients.Others.userConnected(userName);
                    }
                }
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (this.Context.User != null)
            {
                var userName = this.Context.User.Identity.Name;
                var connectionId = this.Context.ConnectionId;

                ConnectedUser user;
                ConnectionManager.Users.TryGetValue(userName, out user);

                if (user != null)
                {
                    lock (user.ConnectionsIds)
                    {
                        user.ConnectionsIds = null;

                        if (string.IsNullOrEmpty(user.ConnectionsIds))
                        {
                            ConnectedUser removedUser;
                            ConnectionManager.Users.TryRemove(userName, out removedUser);

                            // You might want to only broadcast this info if this 
                            // is the last connection of the user and the user actual is 
                            // now disconnected from all connections.
                            // Clients.Others.userDisconnected(userName);
                            this.Clients.All.userDisconnected(userName);
                        }
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}