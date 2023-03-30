namespace MastersThesis {
    internal static class Program {
        /// <summary>
        /// Главная точка входа для приложения
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitor);
            Application.Run(new MainForm());
        }
    }
}