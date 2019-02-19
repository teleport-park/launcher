using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherForms.AppExecuter {
    public class AppExecuter : IAppExecuter {
        public void RunApp(IExecutableApp app) {
            app.Prepare();
            app.Run();
        }
    }
}
