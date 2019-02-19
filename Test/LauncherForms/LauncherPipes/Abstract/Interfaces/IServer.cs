using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LauncherPipes.Abstract.Interfaces {
    public interface IServer {
        void Prepare(string name);
        Task Start();
        Task Stop();
    }
}
