using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin;

namespace PushSystem
{
    public class MyHub : Hub
    {
        public MyHub()
        {

        }
        public void Send(string name, string message)
        {            
            Clients.All.addNewMessage(name, message);
        }
    }

}