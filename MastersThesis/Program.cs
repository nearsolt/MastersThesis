namespace MastersThesis {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitor);
            Application.Run(new MainForm());

#warning  To do: warnings handling, regrouping classes/methods, writing summaries and comments.

            /* 
             * ToDo: 
             *      PlanarObjectStore.cs - complit
             *                          
             *                          
             *      MainForm.cs                   
             *                          ExactSolution
             *                          #warnings
             *                          
             *      Triangulation.cs
             *      
             *      MeshStore.cs
             *      
             *      GnuPlot.cs
            */
        }
    }
}