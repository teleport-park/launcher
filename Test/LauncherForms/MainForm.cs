using NamedPipeWrapper;
using System;
using System.Threading;
using System.Windows.Forms;
using Message = LauncherPipes.Protocol.Message;

namespace LauncherUI {
    public partial class MainForm : Form {
        private bool _spaceKeyPressed;
        private bool _clientStarted;
        private SynchronizationContext _context;
        private NamedPipeClient<Message> _client;
        public MainForm() {
            InitializeComponent();
            _spaceKeyPressed = false;
            _clientStarted = false;
            _client = new NamedPipeClient<Message>("localhost");
            _client.ServerMessage += delegate (NamedPipeConnection<Message, Message> conn, Message message) {
                _context.Post((obj) => {
                    HandleMessage(message);
                }, null);
            };

        }

        private void HandleMessage(Message message) {
            if (!string.IsNullOrEmpty(message?.Text)) {
                if (message.Text.Contains("Welcome")) {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !(e.CloseReason == CloseReason.WindowsShutDown || _spaceKeyPressed);
        }

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            // pipes server test
            if (e.Shift) {
                if (!_clientStarted) {
                    _client.Start();
                    _clientStarted = true;
                    return;
                }
                MessageBox.Show("Client is already running");
            }
            // close app on space
            if (e.KeyCode == Keys.Space) {
                if (MessageBox.Show("Close application?", "Launcher message", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    if (_clientStarted) {
                        _client.Stop();
                    }
                    CloseForm();   
                }
            }
        }

        private void CloseForm() {
            _spaceKeyPressed = true;
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            _context = SynchronizationContext.Current;
        }
    }
}
