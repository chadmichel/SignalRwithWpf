# SignalRwithWpf
Sample application of using SignalR with WPF

Based off of: http://www.asp.net/signalr/overview/getting-started/tutorial-getting-started-with-signalr-and-mvc

Very basic hub hosted in asp.net
```C#
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
```

Very basic html client for sending an receiving hub messages (see link above)

```JavaScript
    $(function () {
        // Reference the auto-generated proxy for the hub.
        var chat = $.connection.myHub;
        // Create a function that the hub can call back to display messages.
        chat.client.addNewMessage = function (name, message) {
            // Add the message to the page.
            $('#discussion').append('<li><strong>' + htmlEncode(name)
                + '</strong>: ' + htmlEncode(message) + '</li>');
        };
        // Get the user name and store it to prepend to messages.
        $('#displayname').val(prompt('Enter your name:', ''));
        // Set initial focus to message input box.
        $('#message').focus();
        // Start the connection.
        $.connection.hub.start().done(function () {
            $('#sendmessage').click(function () {
                // Call the Send method on the hub.
                chat.server.send($('#displayname').val(), $('#message').val());
                // Clear text box and reset focus for next comment.
                $('#message').val('').focus();
            });
        });
    });
    // This optional function html-encodes messages for display in the page.
    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }
    
```

Receive messages in WPF

```C#
HubConnection connection = new HubConnection("http://localhost:51255/signalr");
proxy = connection.CreateHubProxy("MyHub");
proxy.On<string, string>("addNewMessage", (name, message) =>
    {
        Console.WriteLine(message);
    });
await connection.Start();    
```

Send messages in WPF

```C#
await proxy.Invoke("Send", "wpf", "hello");   
```
