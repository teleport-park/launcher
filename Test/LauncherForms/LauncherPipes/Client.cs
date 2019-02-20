using LauncherPipes.Abstract.Interfaces;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LauncherPipes {
    class Client: IDisposable, IClient, IMessageReader, IMessageWriter {

        private NamedPipeClientStream _pipe;
        private string _serverName;
        private string _name;
        private bool _prepared;
        private CancellationTokenSource _cancellationToken;
        private string _message;
        public Client() {
            _cancellationToken = new CancellationTokenSource();
            _pipe = new NamedPipeClientStream("localhost_client");
        }

        // disposable pattern
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (_pipe.IsConnected) {
                        _pipe.Close();
                    }
                    _pipe.Dispose();
                    _cancellationToken.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Prepare(string serverName, string name, int timeout) {
            if (_prepared) {
                return;
            }
            _serverName = serverName;
            _name = name;
            _pipe = new NamedPipeClientStream(serverName, name, PipeDirection.InOut) {
                ReadMode = PipeTransmissionMode.Message,
                ReadTimeout = timeout
            };
            _prepared = true;
        }
        public void SendStringMessage(string message) {
            if (!_prepared) {
                return;
            }
            if (string.IsNullOrWhiteSpace(message)) {
                return;
            }
            var msg = StringAsMessage(message);
            Write(_pipe, msg);
        }
        public async Task StartListening() {
            if (!_prepared) {
                return;
            }
            while (!_cancellationToken.IsCancellationRequested) {
                // read messages from server
                var messageBytes = await Read(_pipe);
                _message = MessageAsString(messageBytes);
            }
        }
        public void StopListening() {
            if (!_prepared) {
                return;
            }
            _cancellationToken.Cancel();
            if (_pipe.IsConnected) {
                _pipe.Close();
            }
            _prepared = false;
        }
        public async Task<byte[]> Read(PipeStream pipe) {
            byte[] buffer = new byte[1024];
            using (var ms = new MemoryStream()) {
                do {
                    var readBytes = await pipe.ReadAsync(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, readBytes);
                }
                while (!pipe.IsMessageComplete);

                return ms.ToArray();
            }
        }
        public string MessageAsString(byte[] message) {
            return Encoding.UTF8.GetString(message);
        }
        public string GetStringMessage() {
            return _message;
        }
        public void Write(PipeStream pipe, byte[] message) {
            if (message == null) {
                return;
            }
            pipe.Write(message, 0, message.Length);  
        }
        public byte[] StringAsMessage(string message) {
            return Encoding.UTF8.GetBytes(message);
        }
    }
}
