using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;

namespace LauncherPipes.Abstract.Interfaces {
    public interface IMessageWriter {
        void Write(PipeStream pipe, byte[] message);
        byte[] StringAsMessage(string message);
    }
}
