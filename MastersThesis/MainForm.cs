using System.Diagnostics;
using static MastersThesis.MainForm.PlanarObjectStore;
using static MastersThesis.Triangulation.MeshStore;

namespace MastersThesis {
    public partial class MainForm : Form {

        #region Private Class Variables
        /// <summary>
        /// Cостояния приложения
        /// </summary>
        private enum ApplicationStateType : int {
            /// <summary>
            /// Генерация узлов
            /// </summary>
            NodeGeneration = 0,
            /// <summary>
            /// Жадная триангуляция
            /// </summary>
            GreedyTriangulation = 1,
            /// <summary>
            /// Триангуляция Делоне
            /// </summary>
            DelaunayTriangulation = 2,
            /// <summary>
            /// Триангуляция методом измельчения
            /// </summary>
            MeshRefinement = 3,
            /// <summary>
            /// Триангуляция Делоне, по которой была построена триангуляция методом измельчения
            /// </summary>
            ParentDelaunayTriangulation = 4
        }
        /// <summary>
        /// Текущиее состояние приложения
        /// </summary>
        private ApplicationStateType _applicationState = ApplicationStateType.NodeGeneration;
        /// <summary>
        /// Размеры используемой области в pictureBox_mainPic
        /// </summary>
        private SizeF _canvasSize;
        /// <summary>
        /// Точка начала координат в pictureBox_mainPic
        /// </summary>
        private PointF _canvasOrgin;
        /// <summary>
        /// Начало диапазона по оси Ox
        /// </summary>
        private double _xAxisStart;
        /// <summary>
        /// Конец диапазона по оси Ox
        /// </summary>
        private double _xAxisEnd;
        /// <summary>
        /// Начало диапазона по оси Oy
        /// </summary>
        private double _yAxisStart;
        /// <summary>
        /// Конец диапазона по оси Oy
        /// </summary>
        private double _yAxisEnd;
        /// <summary>
        /// Коэффициэнт растяжения для ширины в pictureBox_mainPic
        /// </summary>
        private double _widthScalingCoeff;
        /// <summary>
        /// Коэффициэнт растяжения для высоты в pictureBox_mainPic
        /// </summary>
        private double _heightScalingCoeff;
        /// <summary>
        /// Счетчик для анимации
        /// </summary>
        private int _animationCounter;
        /// <summary>
        /// Коэффициент измельчения q
        /// </summary>
        private int _meshRefinementCoeff = 3;
        /// <summary>
        /// Количество знаков после запятой при вычислении координат внутренних узлов в методе измельчения
        /// </summary>
        private readonly int _decimalPlaces = 3;
        /// <summary>
        /// Интервал для таймера анимации (мс)
        /// </summary>
        private readonly int _timerInterval = 1000;
        /// <summary>
        /// Текущая триангуляция (PlanarObjectStore)
        /// </summary>
        private PlanarObjectStore _triangulation = null!;
        /// <summary>
        /// Триангуляция Делоне, по которой была построена триангуляция методом измельчения (PlanarObjectStore)
        /// </summary>
        private PlanarObjectStore _parentTriangulation = null!;
        /// <summary>
        /// Список узлов (NodeStore)
        /// </summary>
        private List<NodeStore> _nodeStoreList = null!;
        /// <summary>
        /// Список ребер (EdgeStore)
        /// </summary>
        private List<EdgeStore> _edgeStoreList = null!;
        /// <summary>
        /// Список треугольников (TriangleStore)
        /// </summary>
        private List<TriangleStore> _triangleStoreList = null!;
        #endregion

        #region Exact Solution
        private double ExactSolution(double x, double y) {
            return Math.Sin(x) * y + x * Math.Cos(y) + x + y;
        }
        #endregion

        #region Constructor & Event Handlers
        public MainForm() {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            GenerateRandomNodes();
        }
        private void timer_animationTimer_Tick(object sender, EventArgs e) {
            if (_animationCounter == _triangulation.AnimationList.Count - 1) {
                _animationCounter = 0;
            } else {
                _animationCounter++;
            }
            pictureBox_mainPic.Refresh();
        }
        private void pictureBox_mainPic_Paint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.TranslateTransform(_canvasOrgin.X, _canvasOrgin.Y);

            switch (_applicationState) {
                case ApplicationStateType.NodeGeneration:
                    _triangulation.DrawNodeList(ref graphics, checkBox_labelVisibility.Checked, _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationStateType.GreedyTriangulation:
                    _triangulation.DrawTriangulation(ref graphics, checkBox_labelVisibility.Checked, checkBox_tweenAnimation.Checked, ref _animationCounter,
                                                         _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationStateType.DelaunayTriangulation:
                    _triangulation.DrawTriangulation(ref graphics, checkBox_labelVisibility.Checked, checkBox_tweenAnimation.Checked, ref _animationCounter,
                                                         checkBox_innerTriangleVisibility.Checked, checkBox_circumcircleVisibility.Checked, _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationStateType.MeshRefinement:
                    _triangulation.DrawTriangulation(ref graphics, checkBox_labelVisibility.Checked, checkBox_tweenAnimation.Checked, ref _animationCounter,
                                                         checkBox_innerTriangleVisibility.Checked, _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationStateType.ParentDelaunayTriangulation:
                    break;
            }
        }

        #region CheckBoxs
        private void checkBox_labelVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        private void checkBox_circumcircleVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        private void checkBox_tweenAnimation_CheckedChanged(object sender, EventArgs e) {
            if (checkBox_tweenAnimation.Checked) {
                _animationCounter = 0;
                timer_animationTimer.Interval = _timerInterval;
                timer_animationTimer.Enabled = true;
                timer_animationTimer.Start();
            } else {
                timer_animationTimer.Stop();
                timer_animationTimer.Enabled = false;
            }
            pictureBox_mainPic.Refresh();
        }
        private void checkBox_innerTriangleVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        #endregion

        #region Buttons
        private void button_generateNodes_Click(object sender, EventArgs e) {
            checkBox_innerTriangleVisibility.Checked = false;
            checkBox_innerTriangleVisibility.Enabled = true;
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = true;
            GenerateRandomNodes();
        }
        private void button_greedyTriangulation_Click(object sender, EventArgs e) {
            checkBox_innerTriangleVisibility.Checked = false;
            checkBox_innerTriangleVisibility.Enabled = false;
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = false;
            GreedyTriangulation();
        }
        private void button_delaunayTriangulation_Click(object sender, EventArgs e) {
            checkBox_innerTriangleVisibility.Enabled = true;
            checkBox_circumcircleVisibility.Enabled = true;
            DelaunayTriangulation();
        }
        private void button_meshRefinement_Click(object sender, EventArgs e) {
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = false;
            MeshRefinement();
        }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Инициализация области определения и коэффициэнтов растяжения ширины и высоты в pictureBox_mainPic
        /// </summary>
        private void InitializeDomainOfDefinition() {
            if (checkBox_setDomainOfDefinition.Checked) {
                _xAxisStart = (double)numericUpDown_xAxisStart.Value;
                _xAxisEnd = (double)numericUpDown_xAxisEnd.Value;

                _yAxisStart = (double)numericUpDown_yAxisStart.Value;
                _yAxisEnd = (double)numericUpDown_yAxisEnd.Value;

                _widthScalingCoeff = Math.Round(_canvasSize.Width / (_xAxisEnd - _xAxisStart), 2, MidpointRounding.AwayFromZero);
                _heightScalingCoeff = Math.Round(_canvasSize.Height / (_yAxisEnd - _yAxisStart), 2, MidpointRounding.AwayFromZero);

            } else {
                _xAxisStart = -Math.Round(_canvasSize.Width * 0.5, 2, MidpointRounding.AwayFromZero);
                _xAxisEnd = Math.Round(_canvasSize.Width * 0.5, 2, MidpointRounding.AwayFromZero);

                _yAxisStart = -Math.Round(_canvasSize.Height * 0.5, 2, MidpointRounding.AwayFromZero);
                _yAxisEnd = Math.Round(_canvasSize.Height * 0.5, 2, MidpointRounding.AwayFromZero);

                _widthScalingCoeff = 1;
                _heightScalingCoeff = 1;
            }
        }
        /// <summary>
        /// Инициализация используемой области в pictureBox_mainPic и точки начала координат 
        /// </summary>
        private void InitializeCanvasSize() {
            _canvasSize = new SizeF((float)(pictureBox_mainPic.Width * 0.9), (float)(pictureBox_mainPic.Height * 0.9));
            InitializeDomainOfDefinition();
            if (checkBox_setDomainOfDefinition.Checked) {
                _canvasOrgin = new PointF((float)(pictureBox_mainPic.Width * 0.5 - (_xAxisStart + _xAxisEnd) * 0.5 * _widthScalingCoeff),
                                          (float)(pictureBox_mainPic.Height * 0.5 + (_yAxisStart + _yAxisEnd) * 0.5 * _heightScalingCoeff));
            } else {
                _canvasOrgin = new PointF((float)(pictureBox_mainPic.Width * 0.5), (float)(pictureBox_mainPic.Height * 0.5));
            }
            pictureBox_mainPic.Refresh();
        }
        /// <summary>
        /// Random-генерация узлов в  используемой области
        /// </summary>
        private void GenerateRandomNodes() {
            InitializeCanvasSize();
            _applicationState = ApplicationStateType.NodeGeneration;
            int nodeCount = (int)numericUpDown_numberOfNodes.Value;

            _nodeStoreList = new List<NodeStore>();
            _edgeStoreList = new List<EdgeStore>();
            _triangleStoreList = new List<TriangleStore>();

            _triangulation = new PlanarObjectStore();
            List<Node> tempNodeList = new List<Node>();

            Random random = new Random();

            tempNodeList.Add(new Node(0, _xAxisStart, _yAxisStart));
            tempNodeList.Add(new Node(1, _xAxisStart, _yAxisEnd));
            tempNodeList.Add(new Node(2, _xAxisEnd, _yAxisStart));
            tempNodeList.Add(new Node(3, _xAxisEnd, _yAxisEnd));

            if (nodeCount > 4) {
                for (int j = 4; j < nodeCount; j++) {
                    double tmpXCoord = Math.Round(random.Next(Convert.ToInt32(Math.Ceiling(_xAxisStart)) * 100, Convert.ToInt32(Math.Floor(_xAxisEnd)) * 100) / 100.0,
                                                  2, MidpointRounding.AwayFromZero);
                    double tmpYCoord = Math.Round(random.Next(Convert.ToInt32(Math.Ceiling(_yAxisStart)) * 100, Convert.ToInt32(Math.Floor(_yAxisEnd)) * 100) / 100.0,
                                                  2, MidpointRounding.AwayFromZero);

                    if (tempNodeList.Exists(obj => obj.XCoordinate == tmpXCoord && obj.YCoordinate == tmpYCoord)) {
                        j--;
                        continue;
                    }
                    tempNodeList.Add(new Node(j, tmpXCoord, tmpYCoord));
                }
            }
            _triangulation.NodeList = tempNodeList;
            pictureBox_mainPic.Refresh();
        }
        #endregion

        


        private void GreedyTriangulation() {
            if (this._applicationState == ApplicationStateType.MeshRefinement) {
                MessageBox.Show("Perform node generation before Greedy triangulation.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._applicationState = ApplicationStateType.GreedyTriangulation;
            this._animationCounter = 0;
            this._triangulation.EdgeList = new List<PlanarObjectStore.Edge>();
            this._triangulation.AnimationList = new List<PlanarObjectStore.TweenAnimation>();

            List<PlanarObjectStore.Edge> tempEdgeList = new List<PlanarObjectStore.Edge>();
            List<PlanarObjectStore.TweenAnimation> tempTrackerList = new List<PlanarObjectStore.TweenAnimation>();

            (new Triangulation()).GreedyTriangulationStart(this._triangulation.NodeList, ref tempEdgeList, ref tempTrackerList);

            this._triangulation.EdgeList = tempEdgeList;
            this._triangulation.AnimationList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
#warning w1: remove logger            
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            Debug.WriteLine($"GreedyTriangulation:\tnodeListCount: {nLC}\tedgeListCount: {eLC}\ttriangleListCount: {tLC}\tidentity (n-e+t=1): {nLC - eLC + tLC == 1}");
        }
        private void DelaunayTriangulation() {
            if (this._applicationState == ApplicationStateType.MeshRefinement) {
                MessageBox.Show("Perform node generation before Delaunay triangulation.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._applicationState = ApplicationStateType.DelaunayTriangulation;
            this._animationCounter = 0;
            this._triangulation.EdgeList = new List<PlanarObjectStore.Edge>();
            this._triangulation.AnimationList = new List<PlanarObjectStore.TweenAnimation>();

            List<PlanarObjectStore.Edge> tempEdgeList = new List<PlanarObjectStore.Edge>();
            List<PlanarObjectStore.Triangle> tempTriangleList = new List<PlanarObjectStore.Triangle>();
            List<PlanarObjectStore.TweenAnimation> tempTrackerList = new List<PlanarObjectStore.TweenAnimation>();

            (new Triangulation()).DelaunayTriangulationStart(this._triangulation.NodeList, ref tempEdgeList, ref tempTriangleList, ref tempTrackerList,
                                                             ref this._nodeStoreList, ref this._edgeStoreList, ref this._triangleStoreList);

            this._triangulation.EdgeList = tempEdgeList;
            this._triangulation.TriangleList = tempTriangleList;
            this._triangulation.AnimationList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
#warning w1: remove logger            
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            Debug.WriteLine($"DelaunayTriangulation:\tnodeListCount: {nLC}\tedgeListCount: {eLC}\ttriangleListCount: {tLC}\tidentity (n-e+t=1): {nLC - eLC + tLC == 1}");
        }
        private void MeshRefinement() {
            if (this._applicationState != ApplicationStateType.DelaunayTriangulation) {
                MessageBox.Show("Perform Delaunay triangulation before the mesh refinement method.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._applicationState = ApplicationStateType.MeshRefinement;
            this._meshRefinementCoeff = (int)this.numericUpDown_meshRefinementCoeff.Value;
            this._animationCounter = 0;

            List<PlanarObjectStore.Node> tempNodeList = new List<PlanarObjectStore.Node>();
            List<PlanarObjectStore.Edge> tempEdgeList = new List<PlanarObjectStore.Edge>();
            List<PlanarObjectStore.Triangle> tempTriangleList = new List<PlanarObjectStore.Triangle>();
            List<PlanarObjectStore.TweenAnimation> tempTrackerList = new List<PlanarObjectStore.TweenAnimation>();
            PlanarObjectStore.TweenAnimation tempTracker = this._triangulation.AnimationList.Last();

            (new Triangulation()).MeshRefinementStart(ref tempNodeList, ref tempEdgeList, ref tempTriangleList, ref tempTrackerList, tempTracker, this._decimalPlaces,
                                                      this._meshRefinementCoeff, this._nodeStoreList, this._edgeStoreList, this._triangleStoreList);

            this._triangulation.NodeList = tempNodeList;
            this._triangulation.EdgeList = tempEdgeList;
            this._triangulation.TriangleList = tempTriangleList;
            this._triangulation.AnimationList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
#warning w1: remove logger            
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            Debug.WriteLine($"MeshRefinement:\tnodeListCount: {nLC}\tedgeListCount: {eLC}\ttriangleListCount: {tLC}\tidentity (n-e+t=1): {nLC - eLC + tLC == 1}");
        }




        #region Test Buttons & Debug
#warning To do: remove from the final version with control (button_test visible = false)
        internal static void DebugLog(string type, string message) {
            File.AppendAllText(@"D:\mechmath\.Master's Thesis\logger.log", $"{DateTime.Now} {type}: {message}\n");
        }

        private void button_test_Click(object sender, EventArgs e) {

        }
        #endregion


        #region new methods
        /// <summary>
        /// Вычисление лямбда функции
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <param name="thirdNode"></param>
        /// <returns></returns>
        private double[,] LambdaFunction(PlanarObjectStore.Node firstNode, PlanarObjectStore.Node secondNode, PlanarObjectStore.Node thirdNode) {
            double[,] lambda = new double[3, 3];

            lambda[0, 0] = 1;
            lambda[0, 1] = firstNode.XCoordinate;
            lambda[0, 2] = firstNode.YCoordinate;

            lambda[1, 0] = 1;
            lambda[1, 1] = secondNode.XCoordinate;
            lambda[1, 2] = secondNode.YCoordinate;

            lambda[2, 0] = 1;
            lambda[2, 1] = thirdNode.XCoordinate;
            lambda[2, 2] = thirdNode.YCoordinate;

            return lambda;
        }

        /// <summary>
        /// Вычисление обратной лямбда функции
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <param name="thirdNode"></param>
        /// <returns></returns>
        private double[,] InverseLambdaFunction(Node firstNode, Node secondNode, Node thirdNode) {
            double[,] inverseLambda = new double[3, 3];

            double x1 = firstNode.XCoordinate;
            double y1 = firstNode.YCoordinate;

            double x2 = secondNode.XCoordinate;
            double y2 = secondNode.YCoordinate;

            double x3 = thirdNode.XCoordinate;
            double y3 = thirdNode.YCoordinate;

            double det = x2 * y3 + x1 * y2 + y1 * x3 - y1 * x2 - y2 * x3 - x1 * y3;

            inverseLambda[0, 0] = (x2 * y3 - y2 * x3) / det;
            inverseLambda[0, 1] = (y1 * x3 - x1 * y3) / det;
            inverseLambda[0, 2] = (x1 * y2 - y1 * x2) / det;

            inverseLambda[1, 0] = (y2 - y3) / det;
            inverseLambda[1, 1] = (y3 - y1) / det;
            inverseLambda[1, 2] = (y1 - y2) / det;

            inverseLambda[2, 0] = (x3 - x2) / det;
            inverseLambda[2, 1] = (x1 - x3) / det;
            inverseLambda[2, 2] = (x2 - x1) / det;

            return inverseLambda;
        }



        /// <summary>
        ///                 data (1,x,y)   -> 1x3
        ///                 inverseLambda  -> 3x3
        ///                 functionValues -> 3x1
        ///                 
        ///                 tmp = data x inverseLambda -> 1x3
        /// </summary>
        /// <param name="data"></param>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <param name="thirdNode"></param>
        /// <param name="functionValues"></param>
        /// <returns></returns>
        private double GJFunction(double[] data, Node firstNode, Node secondNode, Node thirdNode, double[] functionValues) {
            double result = 0;
            double[] tmp = new double[3];

            double[,] inverseLambda = InverseLambdaFunction(firstNode, secondNode, thirdNode);

            for (int j = 0; j < 3; j++) {
                tmp[j] = 0;
                for (int k = 0; k < 3; k++) {
                    tmp[j] += data[k] * inverseLambda[k, j];
                }
            }
            for (int j = 0; j < 3; j++) {
                result += tmp[j] * functionValues[j];
            }
            return result;
        }

        private double GJFunctionTwo(double[] data, Node firstNode, Node secondNode, Node thirdNode, double[] functionValues) {
            double result = 0;
            double[] tmp = new double[3];

            double[,] inverseLambda = InverseLambdaFunction(firstNode, secondNode, thirdNode);

            for (int j = 0; j < 3; j++) {
                tmp[j] = 0;
                for (int k = 0; k < 3; k++) {
                    tmp[j] += functionValues[k] * inverseLambda[k, j];
                }
            }
            for (int j = 0; j < 3; j++) {
                result += tmp[j] * data[j];
            }
            return result;
        }

        /// <summary>
        /// Вычисление взвешенных расстояний
        /// </summary>
        /// <param name="curXCoord"></param>
        /// <param name="curYCoord"></param>
        /// <param name="midXCoord"></param>
        /// <param name="midYCoord"></param>
        /// <returns></returns>
        private double WJFunction(double curXCoord, double curYCoord, double midXCoord, double midYCoord) {
            return 1 / ((curXCoord - midXCoord) * (curXCoord - midXCoord) + (curYCoord - midYCoord) * (curYCoord - midYCoord));
        }

        private double GFunction(double curX, double curY, List<Triangle> triangleList) {

            double result = 0;
            double w_cur = 0;
            double w_sum = 0;
            double[] data = new double[] { 1, curX, curY };
#warning добавить массив длиной 8 для координат узлов и центра и переписать убрав кастомные классы из параметоров сзявных функций GJFunction
            for (int j = 0; j < triangleList.Count; j++) {
                w_cur = WJFunction(curX, curY, triangleList[j].GeometricCenter.XCoordinate, triangleList[j].GeometricCenter.YCoordinate);

                double[] functionValues = new double[] {
                    ExactSolution(triangleList[j].FirstNode.XCoordinate,triangleList[j].FirstNode.YCoordinate),
                    ExactSolution(triangleList[j].SecondNode.XCoordinate,triangleList[j].SecondNode.YCoordinate),
                    ExactSolution(triangleList[j].ThirdNode.XCoordinate,triangleList[j].ThirdNode.YCoordinate)
                };
                w_sum += w_cur;
                result += w_cur * GJFunction(data, triangleList[j].FirstNode, triangleList[j].SecondNode, triangleList[j].ThirdNode, functionValues);
            }

            return result /= w_sum;
        }

        private void ApproxFunction(int nodeNum_oX, int nodeNum_oY, PlanarObjectStore planarObjectStore) {

            double h_oX = (_xAxisEnd - _xAxisStart) / (nodeNum_oX - 1);
            double h_oY = (_yAxisEnd - _yAxisStart) / (nodeNum_oY - 1);

            double[] xVal = new double[nodeNum_oX * nodeNum_oY];
            double[] yVal = new double[nodeNum_oX * nodeNum_oY];
            double[] zVal = new double[nodeNum_oX * nodeNum_oY];
            double[] exactVal = new double[nodeNum_oX * nodeNum_oY];
            int index = 0;

            for (double j = 0; j < nodeNum_oX; j++) {
                for (double k = 0; k < nodeNum_oY; k++) {
                    xVal[index] = _xAxisStart + j * h_oX;
                    yVal[index] = _yAxisStart + k * h_oY;
                    zVal[index] = GFunction(xVal[index], yVal[index], planarObjectStore.TriangleList);
                    exactVal[index] = ExactSolution(xVal[index], yVal[index]);
                    index++;
                }
            }

            int numC = planarObjectStore.NodeList.Count;
            double[] xValQ = new double[numC];
            double[] yValQ = new double[numC];
            double[] fValQ = new double[numC];
            for (int indexx = 0; indexx < numC; indexx++) {
                xValQ[indexx] = planarObjectStore.NodeList[indexx].XCoordinate;
                yValQ[indexx] = planarObjectStore.NodeList[indexx].YCoordinate;
                fValQ[indexx] = ExactSolution(xValQ[indexx], yValQ[indexx]);
            }



            GnuPlot.Set("dgrid3d 50,50, qnorm 2");
            GnuPlot.Set("title \"Approximation\"", "xlabel \"X-Axis\"", "ylabel \"Y-Axis\"", "zlabel \"Z-Axis\"");

            //GnuPlot.Set("contour base");
            //GnuPlot.Set("hidden3d");
            //GnuPlot.Set("palette grey");

            //GnuPlot.WriteLine($"set xrange[{_xAxisStart}:{_xAxisEnd}]");
            //GnuPlot.WriteLine($"set yrange[{_yAxisStart}:{_yAxisEnd}]");
            //GnuPlot.WriteLine("pause -1 'Please press any key'");

            GnuPlot.HoldOn();

            // точная 
            GnuPlot.SPlot(xVal, yVal, exactVal, "title \"f(x,y)\" lc rgb \"purple\"");

            GnuPlot.SPlot(xVal, yVal, exactVal, "with pm3d title \"f(x,y)\"");

            // по изначальным узлам
            GnuPlot.SPlot(xValQ, yValQ, fValQ, "title \"set of points\" lc rgb \"blue\"");

            // интеполяция
            GnuPlot.SPlot(xVal, yVal, zVal, "with pm3d title \"G(A,x,y)\"");

            //GnuPlot.SPlot(xVal, yVal, zVal, $"title \"G(A,x,y)\" with linespoints pt 7");

            GnuPlot.SPlot(xVal, yVal, zVal, "title \"G(A,x,y)\" lc rgb  \"red\"");

        }


        #endregion






        #region Interpolation



        #endregion

        private void button_interpolation_Click(object sender, EventArgs e) {
            ApproxFunction((int)numericUpDown_xAxisNum.Value, (int)numericUpDown_yAxisNum.Value, _triangulation);
        }
        
        private void checkBox_setDomainOfDefinition_CheckedChanged(object sender, EventArgs e) {

        }

        #region Useless
#warning Useless
        private void pictureBox_mainPic_SizeChanged(object sender, EventArgs e) {
            //InitializeCanvasSize();
        }
        private void numericUpDown_xAxisStart_ValueChanged(object sender, EventArgs e) {

        }

        private void numericUpDown_xAxisEnd_ValueChanged(object sender, EventArgs e) {

        }

        private void numericUpDown_yAxisStart_ValueChanged(object sender, EventArgs e) {

        }

        private void numericUpDown_yAxisEnd_ValueChanged(object sender, EventArgs e) {

        }
        #endregion


    }
}
