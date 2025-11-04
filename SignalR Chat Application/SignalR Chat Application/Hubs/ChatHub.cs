namespace SignalR_Chat_Application.Hub
{
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Concurrent;

    public class ChatHub : Hub
    {

        //store connected users and their conndction ids
        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();


        //send message to all connected clients
        public async Task SendMessage(string user, string messasge)
        {
            //brodcast message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", user, messasge);
        }

        //send private message to specific user
        public async Task SendPrivateMessage(string recipientUsername, string message)
        {
            var senderUsername = Context.Items["Username"]?.ToString();

            if (string.IsNullOrEmpty(senderUsername))
            {
                throw new HubException("Sender username not found.");
            }

            if (ConnectedUsers.TryGetValue(recipientUsername, out var recipientConnectionId))
            {
                // Send to recipient
                await Clients.Client(recipientConnectionId).SendAsync("ReceivePrivateMessage", senderUsername, message);

                //send conformation back to sender
                await Clients.Caller.SendAsync("RecievedPrivateMessage", senderUsername, message, recipientUsername);
            }
            else
            {
                throw new HubException($"User '{recipientUsername}' is not connected.");
            }
        }

        //join a group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var username = Context.Items["Username"]?.ToString() ?? "Anonymous";
            await Clients.Group(groupName).SendAsync("ReceiveMessage", groupName, "System", $"{username} has joined the group");
        }

        //leave a group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            var username = Context.Items["Username"]?.ToString() ?? "Anonymous";
            await Clients.Group(groupName).SendAsync("ReceiveMessage", groupName, "System", $"{username} has left the group");

        }

        //send message to a group
        public async Task SendMessageToGroup(string groupName, string message)
        {
            var username = Context.Items["Username"]?.ToString() ?? "Anonymous";
            await Clients.Group(groupName).SendAsync("ReceiveMessage", groupName, username, message);
        }

        public async Task SetUsernaem(string username)
        {
            //remove the old username if exists
            var oldUsername = Context.Items["Username"]?.ToString();
            if (!string.IsNullOrEmpty(oldUsername))
            {
                ConnectedUsers.TryRemove(oldUsername, out _);
            }

            //now that he old username is removed we can set the new one
            Context.Items["Username"] = username;
            ConnectedUsers[username] = Context.ConnectionId;

            //notify all clients about online users
            await Clients.All.SendAsync("UpdateUserList", ConnectedUsers.Keys.ToList());
            await Clients.Caller.SendAsync("UsernameSet", username);
        }

        //when a client disconnects
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.Items["Username"]?.ToString();
            if (!string.IsNullOrEmpty(username))
            {
                ConnectedUsers.TryRemove(username, out _);
                //notify all clients about online users
                await Clients.All.SendAsync("UpdateUserList", ConnectedUsers.Keys.ToList());
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
