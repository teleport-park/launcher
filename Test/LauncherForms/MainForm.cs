using LauncherForms.AppExecuter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LauncherForms {
    public partial class MainForm : Form {
        private IAppExecuter _appExecuter;
        public MainForm() {
            InitializeComponent();
            _appExecuter = new AppExecuter.AppExecuter();
        }
    }
}
