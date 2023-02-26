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
             *      MainFormExtension.cs
             *                          ExactSolution
             *                          warnings "w3: rename region"
             *                          
            */




            /*
             
             
                ����������  ��������� ����, �������� ������� ��� ������ ���������� ������� ������������ � �������� �� ��� ������� �����
                    ���������� ������, �������� ����������� ����� � ���� ��������� ��� ������ �������� ������������ � ������������, 
                    ��������� �����, ����� gnuplot �� ���������� ���������� �����.
             
              
                #warning Set up a color scheme:
                    DrawNode(ref Graphics graphics, bool labelVisibility);
                    DrawEdge(ref Graphics graphics);
                    DrawInnerTriangle(ref Graphics graphics, bool labelVisibility, bool meshVisibility);
                    DrawCircumcircle(ref Graphics graphics, bool labelVisibility, bool meshVisibility, bool circumcircleVisibility);

                #warning Rename some var:
                    GetAngleBetween(EdgeStore withEdge, EdgeStore edge);
            */
        }
    }
}