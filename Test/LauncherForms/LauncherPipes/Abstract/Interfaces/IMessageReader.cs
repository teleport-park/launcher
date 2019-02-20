using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace LauncherPipes.Abstract.Interfaces {
    public interface IMessageReader {
        Task<byte[]> Read(PipeStream pipe);
        string MessageAsString(byte[] message);
    }
}
