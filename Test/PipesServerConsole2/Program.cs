using NamedPipeWrapper;
using System;
using LauncherPipes.Protocol;

namespace PipesServerConsole2 {
    class Program {
        static void Main(string[] args) {
            var server = new NamedPipeServer<Message>("localhost");

            server.ClientConnected += delegate (NamedPipeConnection<Message, Message> conn){
                var messageConnected = new Message{
                    Text = string.Format("Client {0} is now connected!", conn.Id)
                };
                Console.WriteLine(messageConnected.Text);
                conn.PushMessage(new Message { Text = "Welcome!" });
            };

            server.ClientDisconnected += delegate (NamedPipeConnection<Message, Message> conn) {
                var messageDisconnected = new Message {
                    Text = string.Format("Client {0} is now disconnected!", conn.Id)
                };
                Console.WriteLine(messageDisconnected.Text, conn.Id);
            };

            server.ClientMessage += delegate (NamedPipeConnection<Message, Message> conn, Message msg){
                Console.WriteLine("Client {0} says: {1}", conn.Id, msg.Text);
            };

            // Start up the server asynchronously and begin listening for connections.
            // This method will return immediately while the server runs in a separate background thread.
            server.Start();
            Console.ReadKey();
        }
    }
}
