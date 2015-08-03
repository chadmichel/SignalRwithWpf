using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin;

namespace PushSystem
{
    /// <summary>
    /// Very basic SignalR server hub
    /// </summary>
    public class MyHub : Hub
    {
        public MyHub()
        {

        }

        public void SendAndRegister(string name, string group, string sendTo, string message)
        {
            Register(name, group);
            Send(name, sendTo, message);
        }

        public void Send(string name, string sendTo, string message)
        {
            if (string.IsNullOrWhiteSpace(sendTo) || sendTo.ToLower() == "all")
            {
                Clients.All.addNewMessage(name, message);
            }
            else
            {
                Clients.Group(("USER:" + sendTo).ToLower()).addNewMessage(name, message);
                Clients.Group(("GROUP:" + sendTo).ToLower()).addNewMessage(name, message);
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public void Register(string username, string group)
        {
            var connectionId = this.Context.ConnectionId;

            this.Groups.Add(connectionId, ("USER:" + username).ToLower());
            this.Groups.Add(connectionId, ("GROUP:" + group).ToLower());
        }
    }

}