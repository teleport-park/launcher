using LauncherPipes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LauncherUI {
    public partial class MainForm : Form {
        private bool _spaceKeyPressed;
        public MainForm() {
            InitializeComponent();
            _spaceKeyPressed = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !(e.CloseReason == CloseReason.WindowsShutDown || _spaceKeyPressed);
        }

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            // pipes server test
            if (e.Shift) {
                using(var pipesServer = new Server()) {
                    MessageBox.Show("Starting pipes server");
                    Task.Run(async () => {
                        pipesServer.Prepare("localhost");
                        await pipesServer.Start();
                    });
                    MessageBox.Show("Pipes server has been started");
                }
                return;
            }
            // close app on space
            if (e.KeyCode == Keys.Space) {
                if (MessageBox.Show("Close application?", "Launcher message", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    CloseForm();   
                }
            }
        }

        private void CloseForm() {
            _spaceKeyPressed = true;
            Close();
        }
    }
}
