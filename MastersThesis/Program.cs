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
                #warning Set up a color scheme:
                    DrawNode(ref Graphics graphics, bool labelVisibility);
                    DrawEdge(ref Graphics graphics);
                    DrawTriangle(ref Graphics graphics, bool labelVisibility, bool meshVisibility);
                    DrawTriangleAndCircumcircle(ref Graphics graphics, bool labelVisibility, bool meshVisibility, bool circumcircleVisibility);

                #warning Rename some var:
                    SetIncircle();
                    GetAngleBetween(Edge2DStore withEdge, Edge2DStore edge);
            */
        }
    }
}