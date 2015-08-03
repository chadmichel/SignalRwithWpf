using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection = new HubConnection("http://localhost:51255/signalr");
        IHubProxy proxy;
        string username;
        string group = "wpf";

        public MainWindow()
        {
            InitializeComponent();

            InitSignalR();
        }

        private async void InitSignalR()
        {
            proxy = connection.CreateHubProxy("MyHub");
            proxy.On<string, string>("addNewMessage", (name, message) =>
                {
                    Console.WriteLine(message);

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            Results.Text = Results.Text + "\r\n" + name + " : " + message;
                        }));
                });
            Console.WriteLine(connection.ConnectionId);
            await connection.Start();   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private async void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                username = UserName.Text;
                UserName.IsEnabled = false;

                await proxy.Invoke("Register", username, group);
            }

            await proxy.Invoke("Send", username, SendTo.Text, Message.Text);   
        }
    }
}
