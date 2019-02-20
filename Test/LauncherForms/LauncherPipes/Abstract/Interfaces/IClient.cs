using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LauncherPipes.Abstract.Interfaces {
    interface IClient {
        void Prepare(string serverName, string name, int timeout);
        Task StartListening();
        void StopListening();
        string GetStringMessage();
        void SendStringMessage(string message);
    }
}
