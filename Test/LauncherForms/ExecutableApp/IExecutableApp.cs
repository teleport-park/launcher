namespace LauncherForms.AppExecuter {
    public interface IExecutableApp {
        string Path { get; set; }
        void Run();
        void Prepare();
    }
}