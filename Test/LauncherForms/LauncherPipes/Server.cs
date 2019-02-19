using LauncherPipes.Abstract.Interfaces;
using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace LauncherPipes {
    public class Server: IDisposable, IServer {
        private NamedPipeServerStream _pipe;
        private string _name;
        private bool _prepared;
        private CancellationTokenSource _cancellationToken;
        public void Prepare(string name) {
            if (_prepared) {
                return;
            }
            _name = name;
             _pipe = new NamedPipeServerStream(name, PipeDirection.InOut, 1, PipeTransmissionMode.Message);
            _cancellationToken = new CancellationTokenSource();
            _prepared = true;
        }
        public async Task Start() {
            if (!_prepared) {
                return;
            }
            await _pipe.WaitForConnectionAsync(_cancellationToken.Token);
            while (!_cancellationToken.IsCancellationRequested) {
                // read messages from client
            }
        }

        // TODO
        public async Task Stop() {
            if (!_prepared) {
                return;
            }
            _cancellationToken.Cancel();
            if (_pipe.IsConnected) {
                _pipe.Disconnect();
            }
            _prepared = false;
            // TODO
        }

        private bool disposedValue = false; //
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (_pipe.IsConnected) {
                        _pipe.Disconnect();
                    }
                    _pipe.Dispose();
                    _cancellationToken.Dispose();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }
        public void Dispose() {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }

        
    }
}
